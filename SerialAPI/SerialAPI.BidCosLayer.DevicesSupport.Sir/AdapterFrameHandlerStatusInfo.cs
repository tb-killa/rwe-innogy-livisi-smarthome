namespace SerialAPI.BidCosLayer.DevicesSupport.Sir;

internal class AdapterFrameHandlerStatusInfo : AdapterFrameHandler<SirAdapter>
{
	public AdapterFrameHandlerStatusInfo(SirAdapter deviceAdapter)
		: base(deviceAdapter)
	{
	}

	public override bool CanHandle(BidCosMessageForSend message)
	{
		if (message.header.FrameType == SIPcosFrameType.STATUSINFO)
		{
			return base.DeviceAdapter.Included;
		}
		return false;
	}

	public override SendStatus Handle(BidCosMessageForSend message)
	{
		SendStatus result = SendStatus.NO_REPLY;
		if (message.message[0] == 0)
		{
			if (base.DeviceAdapter.sirComm.ConfigStatus(base.DeviceAdapter.Address, message.message[1], message.mode))
			{
				result = SendStatus.ACK;
			}
		}
		else
		{
			result = SendStatus.ERROR;
		}
		return result;
	}
}
