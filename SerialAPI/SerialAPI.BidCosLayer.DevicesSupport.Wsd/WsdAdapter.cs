using System;
using System.Linq;
using RWE.SmartHome.SHC.CommonFunctionality;
using SerialAPI.BidCoSFrames;

namespace SerialAPI.BidCosLayer.DevicesSupport.Wsd;

internal class WsdAdapter : BidCosDeviceAdapter
{
	public readonly WsdComm wsdComm;

	private BidCosFrameHandlersContainer sendFrameHandlersContainer;

	private ReceiveFrameHandlersContainer receiveFrameHandlersContainer;

	public byte[] GroupIp
	{
		get
		{
			return bidCosHandler.NodesManager.Wsd1GroupAddress;
		}
		set
		{
			bidCosHandler.NodesManager.Wsd1GroupAddress = value;
		}
	}

	public WsdAdapter(WsdComm wsdComm, BidCosHandlerRef bidcosHandler)
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
		return true;
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
		WsdComm obj = wsdComm;
		byte[] address = base.Node.address;
		byte[] partnerAddress = new byte[3];
		if (obj.ConfigBegin(address, 0, partnerAddress, 0, 0, GetBurstType()))
		{
			if (wsdComm.ConfigData(base.Node.address, 0, wsdComm.GetConfigurationData(), SendMode.Normal) && wsdComm.ConfigEnd(base.Node.address, 0, SendMode.Normal))
			{
				base.Included = true;
				base.Node.UseDefaultKey = true;
				UpdateNode();
				sendStatus = ConfigureNodeGroup();
				if (sendStatus == SendStatus.ACK)
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
			byte[] array2 = null;
			array2 = GroupIp;
			result = array2 != null && bidCosHandler.AddressesEqual(addr, array2);
		}
		bidCosHandler.m_block_answer = block_answer;
		bidCosHandler.acceptInfoAsResponse = false;
		return result;
	}

	public void RepeatConditionalSwitchFrame(BIDCOSCondSwitchFrame condSwitchFrame)
	{
		if (condSwitchFrame != null && condSwitchFrame.Data != null && condSwitchFrame.Data.Count >= 3)
		{
			byte smokeAlarmValue = condSwitchFrame.Data[2];
			bidCosHandler.RepeatConditionalSwitchFrame(condSwitchFrame.Header.Receiver, smokeAlarmValue);
		}
	}

	public void SendConditionalSwitch(byte[] message)
	{
		if (GroupIp != null)
		{
			wsdComm.SendBidcos(132, GroupIp, bidCosHandler.DefaultIP, 65, message, SendMode.Burst);
		}
	}

	public override byte GetCounter()
	{
		return wsdComm.GetBidCosCounter();
	}
}
