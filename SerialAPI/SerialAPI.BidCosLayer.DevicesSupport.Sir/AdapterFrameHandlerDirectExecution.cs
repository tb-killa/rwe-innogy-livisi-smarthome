namespace SerialAPI.BidCosLayer.DevicesSupport.Sir;

internal class AdapterFrameHandlerDirectExecution : AdapterFrameHandler<SirAdapter>
{
	public AdapterFrameHandlerDirectExecution(SirAdapter deviceAdapter)
		: base(deviceAdapter)
	{
	}

	public override bool CanHandle(BidCosMessageForSend message)
	{
		return message.header.FrameType == SIPcosFrameType.DIRECT_EXECUTION;
	}

	public override SendStatus Handle(BidCosMessageForSend message)
	{
		if (!base.DeviceAdapter.sirComm.SendDecSiren(base.DeviceAdapter.Node.address, message.message, SendMode.Burst))
		{
			return SendStatus.MODE_ERROR;
		}
		return SendStatus.ACK;
	}
}
