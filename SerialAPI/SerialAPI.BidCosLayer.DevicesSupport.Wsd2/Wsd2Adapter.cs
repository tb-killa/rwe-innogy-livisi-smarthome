using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RWE.SmartHome.SHC.CommonFunctionality;
using SerialAPI.BidCoSFrames;

namespace SerialAPI.BidCosLayer.DevicesSupport.Wsd2;

internal class Wsd2Adapter : BidCosDeviceAdapter
{
	public const byte Wsd1AlarmOff = 1;

	public const byte Wsd1AlarmOn = 200;

	public const byte Wsd2AlarmOff = 0;

	public const byte Wsd2AlarmOn = 198;

	public readonly Wsd2Comm wsdComm;

	public readonly byte[] WsdAlarmOnCodes = new byte[2] { 200, 198 };

	public readonly byte[] WsdAlarmOffCodes = new byte[2] { 1, 0 };

	private BidCosFrameHandlersContainer sendFrameHandlersContainer;

	private ReceiveFrameHandlersContainer receiveFrameHandlersContainer;

	public byte[] GroupIp
	{
		get
		{
			return bidCosHandler.NodesManager.Wsd2GroupAddress;
		}
		set
		{
			bidCosHandler.NodesManager.Wsd2GroupAddress = value;
		}
	}

	public byte SequenceCounterWsd2HighByte => bidCosHandler.NodesManager.SequenceCounterWsd2HighByte;

	public byte SequenceCounterWsd2MidByte => bidCosHandler.NodesManager.SequenceCounterWsd2MidByte;

	public byte SequenceCounterWsd2LowByte => bidCosHandler.NodesManager.SequenceCounterWsd2LowByte;

	public byte[] Wsd2LocalKey => bidCosHandler.NodesManager.Wsd2LocalKey;

	public Wsd2Adapter(Wsd2Comm wsdComm, BidCosHandlerRef bidcosHandler)
		: base(bidcosHandler)
	{
		this.wsdComm = wsdComm;
		InitSendFrameHandlerCollection();
		InitReceiveFrameHandlerCollection();
	}

	public override SendStatus SendCosipMessage(BidCosMessageForSend message)
	{
		base.Node.UseAnswerCount = true;
		base.Node.AnswerSequenceCount = message.header.SequenceNumber;
		UpdateNode();
		SendStatus result = (base.Node.included ? SendStatus.ERROR : SendStatus.MODE_ERROR);
		IAdapterFrameHandler sendFrameHandler = sendFrameHandlersContainer.GetSendFrameHandler(message);
		if (sendFrameHandler != null)
		{
			result = sendFrameHandler.Handle(message);
		}
		return result;
	}

	public override BIDCOSMessage HandleFrame(ReceiveFrameData frameData)
	{
		return receiveFrameHandlersContainer.GetSendFrameHandler(frameData)?.HandleFrame(frameData);
	}

	private void InitSendFrameHandlerCollection()
	{
		sendFrameHandlersContainer = new BidCosFrameHandlersContainer(new IAdapterFrameHandler[4]
		{
			new AdapterFrameHandlerNetworkManagement(this),
			new AdapterFrameHandlerConfiguration(this),
			new AdapterFrameHandlerConditionalSwitch(this),
			new AdapterFrameHandlerStatusInfo(this)
		});
	}

	private void InitReceiveFrameHandlerCollection()
	{
		receiveFrameHandlersContainer = new ReceiveFrameHandlersContainer(new IReceiveFrameHandler[4]
		{
			new ReceiveFrameHandlerAnswer(this),
			new ReceiveFrameHandlerInfo(this),
			new ReceiveFrameHandlerConditionalSwitch(this),
			new ReceiveFrameHandlerSwitch(this)
		});
	}

	public override bool EnsureCurrentNodeDefaultKey()
	{
		bidCosHandler.bidcosKeyRetriever.GetDeviceKey(SGTIN96.Create(base.Node.Sgtin), delegate(byte[] key)
		{
			base.Node.DefaultKey = key;
		}, null, 5000);
		return base.Node.DefaultKey != null;
	}

	public override void UpdateNode()
	{
		bidCosHandler.NodesManager.UpdateNode(base.Node);
		bidCosHandler.NodesManager.Persist();
	}

	public override SendMode GetBurstType()
	{
		return SendMode.Burst;
	}

	public bool FactoryReset()
	{
		return wsdComm.FactoryReset(base.Node.address, GetBurstType());
	}

	public SendStatus TryInclusionForWsd()
	{
		SendStatus sendStatus = SendStatus.BIDCOS_INCLUSION_FAILED;
		Wsd2Comm wsd2Comm = wsdComm;
		byte[] address = base.Node.address;
		byte[] partnerAddress = new byte[3];
		if (wsd2Comm.ConfigBegin(address, 0, partnerAddress, 0, 0, GetBurstType()))
		{
			if (wsdComm.ConfigData(base.Node.address, 0, wsdComm.GetConfigurationData(), SendMode.Normal) && wsdComm.ConfigEnd(base.Node.address, 0, SendMode.Normal))
			{
				base.Included = true;
				base.Node.UseDefaultKey = true;
				UpdateNode();
				sendStatus = ConfigureNodeGroup();
				if (sendStatus == SendStatus.ACK && SetCurrentNodeKey())
				{
					base.Node.UseDefaultKey = false;
				}
				else
				{
					sendStatus = SendStatus.BIDCOS_INCLUSION_FAILED;
				}
			}
		}
		else if (IsNodeConfigured())
		{
			sendStatus = SendStatus.ACK;
			wsdComm.ConfigStatus(base.Node.address, 1, SendMode.Normal);
		}
		return sendStatus;
	}

	internal void SendSysInfoToSipcosHandler()
	{
		SendToSipcosHandler(base.Node.Sysinfo);
	}

	public void SendConditionalSwitch(byte[] message)
	{
		if (GroupIp != null)
		{
			byte[] message2 = TranslateWsd1ToWsd2CondSwitchMessage(message);
			byte[] counter = new byte[3]
			{
				bidCosHandler.NodesManager.SequenceCounterWsd2HighByte,
				bidCosHandler.NodesManager.SequenceCounterWsd2MidByte,
				bidCosHandler.NodesManager.SequenceCounterWsd2LowByte
			};
			List<byte> data = wsdComm.BuildWsd2ConditionalSwitchMessage(GroupIp, bidCosHandler.DefaultIP, message2, counter, CurrentKey());
			for (int i = 0; i < 6; i++)
			{
				bidCosHandler.BroadcastFrameToAir(data, SendMode.Burst);
				Thread.Sleep(200);
			}
			bidCosHandler.NodesManager.IncreaseSequenceCounter();
		}
	}

	private byte[] TranslateWsd1ToWsd2CondSwitchMessage(byte[] message)
	{
		byte[] array = new byte[message.Length];
		message.CopyTo(array, 0);
		array[1] = 1;
		if (array[2] == 200)
		{
			array[2] = 198;
		}
		if (array[2] == 1)
		{
			array[2] = 0;
		}
		array[1] = bidCosHandler.NodesManager.SequenceCounterWsd2LowByte;
		return array;
	}

	private SendStatus ConfigureNodeGroup()
	{
		SendStatus result = SendStatus.BIDCOS_GROUP_ADDRESS_FAILED;
		if (wsdComm.ConfigOrderLinkList(base.Node.address, 1, SendMode.Normal) && bidCosHandler.answerFrameReceivedEvent.WaitOne(1000, exitContext: false))
		{
			byte[] array = null;
			lock (bidCosHandler.syncRoot)
			{
				byte[] array2 = bidCosHandler.m_partner.GetRow(0).Take(3).ToArray();
				byte[] array3 = new byte[3];
				byte[] addr = array3;
				if (bidCosHandler.m_partner == null || (GroupIp != null && !bidCosHandler.AddressesEqual(array2, GroupIp) && !bidCosHandler.AddressesEqual(array2, addr)))
				{
					array = array2;
				}
			}
			if (array != null && wsdComm.ConfigDeregistration(base.Node.address, 1, array, 1, 0, SendMode.Normal))
			{
				array = null;
			}
			if (array == null)
			{
				bidCosHandler.m_block_answer = false;
				if (GroupIp == null)
				{
					GroupIp = base.Node.address;
				}
				if (wsdComm.ConfigRegistration(base.Node.address, 1, GroupIp, 1, 0, SendMode.Normal))
				{
					result = SendStatus.ACK;
				}
				else if (GroupIp != null && bidCosHandler.AddressesEqual(GroupIp, base.Node.address))
				{
					GroupIp = null;
				}
			}
		}
		return result;
	}

	private bool IsNodeConfigured()
	{
		bool result = false;
		bool block_answer = bidCosHandler.m_block_answer;
		bidCosHandler.m_block_answer = true;
		bidCosHandler.acceptInfoAsResponse = true;
		if (wsdComm.ConfigOrderLinkList(base.Node.address, 1, SendMode.Normal) && bidCosHandler.answerFrameReceivedEvent.WaitOne(1000, exitContext: false))
		{
			byte[] array = new byte[3];
			byte[] addr = array;
			try
			{
				addr = bidCosHandler.m_partner.GetRow(0).Take(3).ToArray();
			}
			catch (Exception)
			{
			}
			byte[] groupIp = GroupIp;
			result = groupIp != null && bidCosHandler.AddressesEqual(addr, groupIp);
		}
		bidCosHandler.m_block_answer = block_answer;
		bidCosHandler.acceptInfoAsResponse = false;
		return result;
	}

	private bool SetCurrentNodeKey()
	{
		if (base.Node.DeviceType != BIDCOSDeviceType.Eq3EncryptedSmokeDetector)
		{
			throw new Exception($"Wrong device type {base.Node.DeviceType}");
		}
		return wsdComm.ConfigureNodeKey(base.Node.address, CurrentKey(), Wsd2LocalKey);
	}

	internal void IncrementSequenceCounter()
	{
		bidCosHandler.NodesManager.IncreaseSequenceCounter();
	}

	public byte[] CurrentKey()
	{
		byte[] array = new byte[6];
		if (base.Included)
		{
			return base.Node.UseDefaultKey ? base.Node.DefaultKey : Wsd2LocalKey;
		}
		return base.Node.DefaultKey;
	}

	public void RepeatConditionalSwitchFrame(BIDCOSCondSwitchFrame condSwitchFrame)
	{
		if (condSwitchFrame != null && condSwitchFrame.Data != null && condSwitchFrame.Data.Count >= 3)
		{
			byte smokeAlarmValue = condSwitchFrame.Data[2];
			bidCosHandler.RepeatConditionalSwitchFrame(condSwitchFrame.Header.Receiver, smokeAlarmValue);
		}
	}

	public override byte GetCounter()
	{
		return wsdComm.GetBidCosCounter();
	}
}
