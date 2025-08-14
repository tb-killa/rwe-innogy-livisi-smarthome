namespace SerialAPI.BidCosLayer.DevicesSupport.Wsd;

internal class AdapterFrameHandlerConditionalSwitch : AdapterFrameHandler<WsdAdapter>
{
	public AdapterFrameHandlerConditionalSwitch(WsdAdapter deviceAdapter)
		: base(deviceAdapter)
	{
	}

	public override bool CanHandle(BidCosMessageForSend message)
	{
		return message.header.FrameType == SIPcosFrameType.CONDITIONAL_SWITCH_COMMAND;
	}

	public override SendStatus Handle(BidCosMessageForSend message)
	{
		if (base.DeviceAdapter.GroupIp == null)
		{
			return SendStatus.MULTI_CAST;
		}
		base.DeviceAdapter.wsdComm.SendBidcos(132, base.DeviceAdapter.GroupIp, base.DeviceAdapter.bidCosHandler.DefaultIP, 65, message.message, SendMode.Burst);
		return SendStatus.MULTI_CAST;
	}
}
