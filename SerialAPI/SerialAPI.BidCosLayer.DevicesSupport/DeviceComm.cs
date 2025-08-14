using System.Collections.Generic;
using System.Threading;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace SerialAPI.BidCosLayer.DevicesSupport;

internal abstract class DeviceComm
{
	protected readonly BidCosHandlerRef bidcosHandler;

	public List<byte> PendingMessage { get; set; }

	protected DeviceComm(BidCosHandlerRef bidCosHandler)
	{
		bidcosHandler = bidCosHandler;
	}

	public virtual bool ConfigBegin(byte[] address, byte Channel, byte[] partnerAddress, byte partnerChannel, byte list, SendMode mode)
	{
		List<byte> list2 = new List<byte>();
		list2.Add(Channel);
		list2.Add(5);
		list2.AddRange(partnerAddress);
		list2.Add(partnerChannel);
		list2.Add(list);
		return SendBidcos(address, 1, list2.ToArray(), mode);
	}

	public virtual bool ConfigEnd(byte[] address, byte Channel, SendMode mode)
	{
		List<byte> list = new List<byte>();
		list.Add(Channel);
		list.Add(6);
		return SendBidcos(address, 1, list.ToArray(), mode);
	}

	public virtual bool ConfigData(byte[] address, byte Channel, byte[] data, SendMode mode)
	{
		List<byte> list = new List<byte>();
		list.Add(Channel);
		list.Add(8);
		list.AddRange(data);
		return SendBidcos(address, 1, list.ToArray(), mode);
	}

	public virtual byte[] GetConfigurationData()
	{
		byte[] defaultIP = bidcosHandler.DefaultIP;
		byte[] array = new byte[8] { 2, 1, 10, 0, 11, 0, 12, 0 };
		array[3] = defaultIP[0];
		array[5] = defaultIP[1];
		array[7] = defaultIP[2];
		return array;
	}

	public virtual bool ConfigOrderLinkList(byte[] address, byte Channel, SendMode mode)
	{
		List<byte> list = new List<byte>();
		list.Add(Channel);
		list.Add(3);
		return SendBidcos(address, 1, list.ToArray(), mode);
	}

	public virtual bool ConfigStatus(byte[] address, byte Channel, SendMode mode)
	{
		List<byte> list = new List<byte>();
		list.Add(Channel);
		list.Add(14);
		return SendBidcos(address, 1, list.ToArray(), mode);
	}

	public virtual bool FactoryReset(byte[] address, SendMode mode)
	{
		byte[] data = new byte[2] { 4, 0 };
		SendMode mode2 = mode;
		return SendBidcos(address, 17, data, mode2);
	}

	public virtual bool ConfigRegistration(byte[] address, byte Channel, byte[] partner, byte partnerChannel1, byte partnerChannel2, SendMode mode)
	{
		if (address != null && partner != null)
		{
			List<byte> list = new List<byte>();
			list.Add(Channel);
			list.Add(1);
			list.AddRange(partner);
			list.Add(partnerChannel1);
			list.Add(partnerChannel2);
			return SendBidcos(address, 1, list.ToArray(), mode);
		}
		return false;
	}

	public virtual bool ConfigDeregistration(byte[] address, byte Channel, byte[] partner, byte partnerChannel1, byte partnerChannel2, SendMode mode)
	{
		List<byte> list = new List<byte>();
		list.Add(Channel);
		list.Add(2);
		if (partner == null)
		{
			list.AddRange(address);
		}
		else
		{
			list.AddRange(partner);
		}
		list.Add(partnerChannel1);
		list.Add(partnerChannel2);
		return SendBidcos(address, 1, list.ToArray(), mode);
	}

	public byte GetBidCosCounter()
	{
		return bidcosHandler.m_frameCount;
	}

	public bool SendBidcos(byte header, byte[] sender, byte[] address, byte Command, byte[] data, SendMode mode)
	{
		return SendBidcos(header, sender, address, Command, data, mode, IncrementFrameType.Increment, null, useSpecialCounter: false, 0, 3);
	}

	protected bool SendBidcos(byte[] address, byte Command, byte[] data, SendMode mode)
	{
		return SendBidcos(bidcosHandler.GetDefaultHeader(), bidcosHandler.DefaultIP, address, Command, data, mode);
	}

	protected bool SendBidcos(byte header, byte[] sender, byte[] address, byte Command, byte[] data, SendMode mode, IncrementFrameType incrementFrameType, AesChallengeResponse aesChallengeResponse, bool useSpecialCounter, byte specialCounter, int retriesCount)
	{
		List<byte> list = (PendingMessage = BuildBidCosMessage(header, sender, address, Command, data, mode, incrementFrameType, useSpecialCounter, specialCounter));
		bidcosHandler.answerFrameReceivedEvent.Reset();
		int num = retriesCount;
		do
		{
			SendStatus sendStatus = SendStatus.ACK;
			for (int i = 0; i < 3; i++)
			{
				if (bidcosHandler.BroadcastFrameToAir(list, mode) == SendStatus.ACK)
				{
					break;
				}
				Thread.Sleep(11);
			}
			num--;
			if (bidcosHandler.answerFrameReceivedEvent.WaitOne(1200, exitContext: false))
			{
				return bidcosHandler.m_answer_ack;
			}
			mode = SendMode.Burst;
			list[1] = (byte)(header | 0x10 | 1);
			Log.Debug(Module.SerialCommunication, "No acknowledgement received. Retrying.");
		}
		while (num != 0);
		return false;
	}

	public List<byte> BuildBidCosMessage(byte header, byte[] sender, byte[] address, byte Command, byte[] data, SendMode mode, IncrementFrameType incrementFrameType, bool useSpecialCounter, byte specialCounter)
	{
		List<byte> list = new List<byte>();
		byte item = specialCounter;
		if (!useSpecialCounter)
		{
			lock (bidcosHandler.syncRoot)
			{
				if (incrementFrameType == IncrementFrameType.Increment)
				{
					bidcosHandler.m_frameCount++;
				}
				item = bidcosHandler.m_frameCount;
			}
		}
		list.Add(item);
		if (mode != SendMode.Normal)
		{
			header |= 0x11;
		}
		list.Add(header);
		list.Add(Command);
		list.AddRange(sender);
		list.AddRange(address);
		list.AddRange(data);
		return list;
	}
}
