using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using SerialAPI.BidCoSFrames;
using SerialAPI.BidCosLayer.DevicesSupport;

namespace SerialAPI.BidCosLayer;

public class BidCosHandler2 : SerialHandler
{
	private readonly SIPcosHandler sipCosHandler;

	private readonly BidCosHandlerRef bidcosHandlerRef;

	public BidCosHandler2(Core core, SIPcosHandler handler, IBidCoSKeyRetriever bidcosKeyRetriever, IBidCoSPersistence bidcosStorage, IEventManager eventManager, IScheduler scheduler)
		: base(SerialHandlerType.BIDCOS_HANDLER, core)
	{
		sipCosHandler = handler;
		eventManager.GetEvent<DeviceInclusionStartedEvent>().Subscribe(OnDeviceBeingIncluded, null, ThreadOption.PublisherThread, null);
		Func<byte[]> defaultIp = () => base.DefaultIP;
		bidcosHandlerRef = new BidCosHandlerRef(handler, defaultIp, base.BroadcastFrameToAir, bidcosStorage, bidcosKeyRetriever, scheduler);
	}

	private void OnDeviceBeingIncluded(DeviceInclusionStartedEventArgs args)
	{
		if (args != null)
		{
			bidcosHandlerRef.forceAcceptSysInfoFrame = true;
			bidcosHandlerRef.nodePendingInclusion = args.DeviceAddress;
		}
	}

	internal override void HandleInit()
	{
	}

	public void SetBidcosNodes(string bidcosNodes)
	{
		lock (bidcosHandlerRef.syncRoot)
		{
			bidcosHandlerRef.NodesManager.SetBidcosNodes(bidcosNodes);
		}
	}

	protected override void HandleData(List<byte> data, DateTime receiveTime)
	{
		byte[] me = ((data != null) ? data.ToArray() : new byte[0]);
		try
		{
			HandleFrameImplementation(data, receiveTime);
		}
		catch (Exception ex)
		{
			Log.Debug(Module.SerialCommunication, "Serial API: Error while handling BIDCOS frame: {0}", ex.ToString());
		}
		try
		{
			Log.Debug(Module.SerialCommunication, "Serial API: Handled BIDCOS Message: " + me.ToReadable());
		}
		catch (Exception arg)
		{
			Console.WriteLine("Error occured while logging BIDCOS message: {0}", arg);
		}
	}

	private void HandleFrameImplementation(List<byte> data, DateTime receiveTime)
	{
		if (data.Count <= 8)
		{
			Log.Debug(Module.SerialCommunication, $"Received packet too short: {data.Count}/8");
			return;
		}
		BIDCOSHeader bIDCOSHeader = new BIDCOSHeader();
		if (!bIDCOSHeader.Parse(ref data))
		{
			Log.Debug(Module.SerialCommunication, "Unable to parse BIDCOS header");
			return;
		}
		BIDCOSMessage bIDCOSMessage = null;
		BIDCOSNode bIDCOSNode = new BIDCOSNode(bIDCOSHeader.Sender);
		ReceiveFrameData receiveFrameData = new ReceiveFrameData();
		receiveFrameData.bidcosHeader = bIDCOSHeader;
		receiveFrameData.data = data;
		receiveFrameData.m_tmp_frame_count = 0;
		receiveFrameData.bidcosNodeAlreadyRegistered = false;
		receiveFrameData.receiveTime = receiveTime;
		ReceiveFrameData receiveFrameData2 = receiveFrameData;
		lock (bidcosHandlerRef.syncRoot)
		{
			receiveFrameData2.m_tmp_frame_count = bidcosHandlerRef.m_frameCount;
			BIDCOSNode node = bidcosHandlerRef.NodesManager.getNode(bIDCOSNode);
			if (node != null)
			{
				receiveFrameData2.bidcosNodeAlreadyRegistered = true;
				bIDCOSNode = node;
				if (bidcosHandlerRef.forceAcceptSysInfoFrame && bIDCOSHeader.FrameType == BIDCOSFrameType.Sysinfo && bidcosHandlerRef.nodePendingInclusion != null && bidcosHandlerRef.nodePendingInclusion.Take(3).SequenceEqual(bIDCOSNode.address.Take(3)))
				{
					bidcosHandlerRef.forceAcceptSysInfoFrame = false;
				}
				else if (bIDCOSNode.SequenceCount == bIDCOSHeader.FrameCounter && bIDCOSHeader.FrameType != BIDCOSFrameType.Answer && (!bidcosHandlerRef.acceptInfoAsResponse || bIDCOSHeader.FrameType != BIDCOSFrameType.Info))
				{
					Log.Debug(Module.SerialCommunication, $"Refusing handling - repeated message {bIDCOSNode.SequenceCount}/{bIDCOSHeader.FrameCounter}");
					return;
				}
				bIDCOSNode.SequenceCount = bIDCOSHeader.FrameCounter;
			}
		}
		if (bidcosHandlerRef.AddressesEqual(bIDCOSHeader.Receiver, base.DefaultIP))
		{
			BidCosHandlerRef bidCosHandlerRef = bidcosHandlerRef;
			byte[] receiver = bIDCOSHeader.Receiver;
			byte[] addr = new byte[3];
			if (!bidCosHandlerRef.AddressesEqual(receiver, addr) && bIDCOSHeader.FrameType != BIDCOSFrameType.Answer)
			{
				SendACK(bIDCOSHeader.Sender, bIDCOSHeader.FrameCounter);
				Log.Information(Module.SerialCommunication, "Sent ACK for frame " + bIDCOSHeader.FrameCounter);
			}
		}
		if (receiveFrameData2.bidcosHeader.FrameType == BIDCOSFrameType.Sysinfo)
		{
			BIDCOSSysinfoFrame bIDCOSSysinfoFrame = new BIDCOSSysinfoFrame(bIDCOSHeader);
			if (bIDCOSSysinfoFrame.Parse(data))
			{
				bIDCOSMessage = bIDCOSSysinfoFrame;
				bIDCOSNode.Sysinfo = bIDCOSSysinfoFrame;
				if (!receiveFrameData2.bidcosNodeAlreadyRegistered)
				{
					bidcosHandlerRef.NodesManager.AddNode(bIDCOSNode);
				}
				else
				{
					bidcosHandlerRef.NodesManager.UpdateNode(bIDCOSNode);
				}
				StoreDeviceKeyInCacheIfEncrypted(bIDCOSNode);
			}
		}
		else
		{
			BidCosDeviceAdapter adapter = bidcosHandlerRef.NodesManager.GetAdapter(bIDCOSNode);
			if (adapter != null)
			{
				bIDCOSMessage = adapter.HandleFrame(receiveFrameData2);
			}
		}
		if (bIDCOSNode.UseAnswerCount && bIDCOSMessage != null)
		{
			bIDCOSNode.UseAnswerCount = false;
			bIDCOSMessage.Header.FrameCounter = bIDCOSNode.AnswerSequenceCount;
			bidcosHandlerRef.NodesManager.UpdateNode(bIDCOSNode);
		}
		SendToSipcosHandler(bIDCOSMessage);
	}

	private void StoreDeviceKeyInCacheIfEncrypted(BIDCOSNode node)
	{
		if (node.DeviceType == BIDCOSDeviceType.Eq3EncryptedSiren || node.DeviceType == BIDCOSDeviceType.Eq3EncryptedSmokeDetector)
		{
			bidcosHandlerRef.bidcosKeyRetriever.StoreDeviceKeyInCache(SGTIN96.Create(node.Sgtin));
		}
	}

	private void SendToSipcosHandler(BIDCOSMessage message)
	{
		if (message != null)
		{
			CORESTACKHeader header = new CORESTACKHeader();
			List<byte> data = new List<byte>();
			message.GenerateSIPCOSMessage(ref header, ref data);
			header.MacSecurity = false;
			data[0] = (data[0] &= 127);
			BidCosDeviceAdapter adapterFromIp;
			lock (bidcosHandlerRef.syncRoot)
			{
				adapterFromIp = bidcosHandlerRef.NodesManager.GetAdapterFromIp(message.Header.Sender);
			}
			if (adapterFromIp != null)
			{
				header.MacSource = adapterFromIp.Address;
				header.IpSource = adapterFromIp.Address;
				sipCosHandler.Handle(header, data);
			}
		}
	}

	public byte? GetBidCosCounter(byte[] address)
	{
		List<BidCosDeviceAdapter> adaptersForIp = GetAdaptersForIp(address);
		if (adaptersForIp != null && adaptersForIp.Count > 0)
		{
			return adaptersForIp.First().GetCounter();
		}
		return null;
	}

	public SendStatus SendCosIpMessage(SIPcosHeader header, List<byte> message, SendMode mode)
	{
		return SendCosIpMessage(header, message.ToArray(), mode);
	}

	public SendStatus SendCosIpMessage(SIPcosHeader header, byte[] message, SendMode mode)
	{
		List<BidCosDeviceAdapter> adaptersForMessageSending = GetAdaptersForMessageSending(header, message);
		if (adaptersForMessageSending.Count <= 0)
		{
			return SendStatus.MODE_ERROR;
		}
		SendStatus result = SendStatus.MULTI_CAST;
		foreach (BidCosDeviceAdapter item in adaptersForMessageSending)
		{
			Log.Debug(Module.SipCosProtocolAdapter, $"sending message: {header.FrameType.ToString()}, data={message.ToReadable()} to {item.Node.DeviceType.ToString()}");
			result = item.SendCosipMessage(new BidCosMessageForSend(header, message, mode));
		}
		return result;
	}

	private List<BidCosDeviceAdapter> GetAdaptersForMessageSending(SIPcosHeader header, byte[] message)
	{
		List<BidCosDeviceAdapter> list = GetAdaptersForIp(header.Destination);
		if (list.Count > 1 && IsExclusionMessage(header, message))
		{
			list = list.Where((BidCosDeviceAdapter a) => a.Address.SequenceEqual(header.Destination)).ToList();
		}
		return list;
	}

	private bool IsExclusionMessage(SIPcosHeader header, byte[] message)
	{
		if (header.FrameType == SIPcosFrameType.NETWORK_MANAGEMENT_FRAME)
		{
			return message[0] == 6;
		}
		return false;
	}

	private void SendACK(byte[] address, byte counter)
	{
		List<byte> list = new List<byte>();
		list.Add(counter);
		list.Add(128);
		list.Add(2);
		list.AddRange(base.DefaultIP);
		list.AddRange(address);
		list.Add(0);
		BroadcastFrameToAir(list, SendMode.Normal);
	}

	public byte GetDefaultHeader()
	{
		return 160;
	}

	internal bool IsSupportedEncryption(byte[] sgtin)
	{
		BIDCOSNode nodeFromSgtin;
		lock (bidcosHandlerRef.syncRoot)
		{
			nodeFromSgtin = bidcosHandlerRef.NodesManager.GetNodeFromSgtin(sgtin);
		}
		if (nodeFromSgtin != null)
		{
			if (nodeFromSgtin.DeviceType != BIDCOSDeviceType.Eq3EncryptedSmokeDetector)
			{
				return nodeFromSgtin.DeviceType == BIDCOSDeviceType.Eq3EncryptedSiren;
			}
			return true;
		}
		return false;
	}

	private List<BidCosDeviceAdapter> GetAdaptersForIp(byte[] ip)
	{
		BidCosDeviceAdapter bidCosDeviceAdapter = null;
		List<BidCosDeviceAdapter> list = new List<BidCosDeviceAdapter>();
		lock (bidcosHandlerRef.syncRoot)
		{
			bidCosDeviceAdapter = bidcosHandlerRef.NodesManager.GetAdapterFromIp(ip);
		}
		if (bidcosHandlerRef.LinkWSDGroups && bidCosDeviceAdapter != null && bidCosDeviceAdapter.Included && bidCosDeviceAdapter.Node != null && (bidCosDeviceAdapter.Node.DeviceType == BIDCOSDeviceType.Eq3BasicSmokeDetector || bidCosDeviceAdapter.Node.DeviceType == BIDCOSDeviceType.Eq3EncryptedSmokeDetector))
		{
			BidCosDeviceAdapter bidCosDeviceAdapter2 = ((bidCosDeviceAdapter.Node.DeviceType == BIDCOSDeviceType.Eq3BasicSmokeDetector) ? bidCosDeviceAdapter : bidcosHandlerRef.NodesManager.FindWsd1Device());
			BidCosDeviceAdapter bidCosDeviceAdapter3 = ((bidCosDeviceAdapter.Node.DeviceType == BIDCOSDeviceType.Eq3EncryptedSmokeDetector) ? bidCosDeviceAdapter : bidcosHandlerRef.NodesManager.FindWsd2Device());
			if (bidCosDeviceAdapter2 != null)
			{
				list.Add(bidCosDeviceAdapter2);
			}
			if (bidCosDeviceAdapter3 != null)
			{
				list.Add(bidCosDeviceAdapter3);
			}
		}
		else if (bidCosDeviceAdapter != null)
		{
			list.Add(bidCosDeviceAdapter);
		}
		return list;
	}
}
