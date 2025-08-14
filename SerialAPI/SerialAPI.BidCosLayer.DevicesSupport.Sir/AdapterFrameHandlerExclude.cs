namespace SerialAPI.BidCosLayer.DevicesSupport.Sir;

internal class AdapterFrameHandlerExclude : AdapterFrameHandler<SirAdapter>
{
	public AdapterFrameHandlerExclude(SirAdapter deviceAdapter)
		: base(deviceAdapter)
	{
	}

	public override bool CanHandle(BidCosMessageForSend message)
	{
		return message.message[0] == 6;
	}

	public override SendStatus Handle(BidCosMessageForSend message)
	{
		SendStatus result = SendStatus.MODE_ERROR;
		if (base.DeviceAdapter.Included)
		{
			if (base.DeviceAdapter.FactoryReset())
			{
				result = SendStatus.ACK;
				base.DeviceAdapter.Remove();
			}
			else
			{
				result = SendStatus.NO_REPLY;
			}
		}
		return result;
	}
}
