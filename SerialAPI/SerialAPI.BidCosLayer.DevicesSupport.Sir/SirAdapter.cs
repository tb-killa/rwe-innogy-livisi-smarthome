using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using SerialAPI.BidCoSFrames;

namespace SerialAPI.BidCosLayer.DevicesSupport.Sir;

internal class SirAdapter : BidCosDeviceAdapter
{
	public readonly SirComm sirComm;

	private readonly IScheduler scheduler;

	public int answerFrameIgnore;

	private BidCosFrameHandlersContainer sendFrameHandlersContainer;

	private ReceiveFrameHandlersContainer receiveFrameHandlersContainer;

	public SirAdapter(SirComm sirComm, BidCosHandlerRef bidcosHandler, IScheduler scheduler)
		: base(bidcosHandler)
	{
		this.sirComm = sirComm;
		this.scheduler = scheduler;
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
		sendFrameHandlersContainer = new BidCosFrameHandlersContainer(new IAdapterFrameHandler[5]
		{
			new AdapterFrameHandlerNetworkManagement(this),
			new AdapterFrameHandlerConfiguration(this),
			new AdapterFrameHandlerStatusInfo(this),
			new AdapterFrameHandlerDirectExecution(this),
			new AdapterFrameHandlerVirtual(this)
		});
	}

	private void InitReceiveFrameHandlerCollection()
	{
		receiveFrameHandlersContainer = new ReceiveFrameHandlersContainer(new IReceiveFrameHandler[2]
		{
			new ReceiveFrameHandlerAnswer(this),
			new ReceiveFrameHandlerInfo(this, scheduler)
		});
	}

	public override bool EnsureCurrentNodeDefaultKey()
	{
		bidCosHandler.bidcosKeyRetriever.GetDeviceKey(SGTIN96.Create(base.Node.Sgtin), UpdateDefaultKey, null, 5000);
		return base.Node.DefaultKey != null;
	}

	private void UpdateDefaultKey(byte[] key)
	{
		base.Node.DefaultKey = key;
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
		return sirComm.FactoryReset(base.Node.address, GetBurstType());
	}

	public SendStatus TryInclusionForSir()
	{
		SendStatus sendStatus = SendStatus.BIDCOS_INCLUSION_FAILED;
		SirComm obj = sirComm;
		byte[] address = base.Node.address;
		byte[] partnerAddress = new byte[3];
		if (obj.ConfigBegin(address, 0, partnerAddress, 0, 0, GetBurstType()) && sirComm.ConfigData(base.Node.address, 0, sirComm.GetConfigurationData(), SendMode.Normal) && sirComm.ConfigEnd(base.Node.address, 0, SendMode.Normal))
		{
			base.Included = true;
			base.Node.UseDefaultKey = true;
			UpdateNode();
			sendStatus = SendStatus.ACK;
			sirComm.EnableAllChannels(base.Node.address);
			sirComm.DisableConfirmationSound(base.Node.address);
			if (sirComm.ConfigureNodeKey(base.Node.address, base.Node.DefaultKey, GetPrivateRandomKey()))
			{
				base.Node.UseDefaultKey = false;
				Log.Debug(Module.SerialCommunication, "Siren device is using the local key");
			}
		}
		else
		{
			sendStatus = SendStatus.BIDCOS_INCLUSION_FAILED;
		}
		return sendStatus;
	}

	internal void SendSysInfoToSipcosHandler()
	{
		SendToSipcosHandler(base.Node.Sysinfo);
	}

	internal void IncrementSequenceCounter()
	{
		bidCosHandler.NodesManager.IncreaseSequenceCounter();
	}

	public byte[] CurrentKey()
	{
		if (!base.Included || base.Node.UseDefaultKey)
		{
			return base.Node.DefaultKey;
		}
		return GetPrivateRandomKey();
	}

	private byte[] GetPrivateRandomKey()
	{
		return bidCosHandler.NodesManager.Wsd2LocalKey;
	}

	public override byte GetCounter()
	{
		return sirComm.GetBidCosCounter();
	}
}
