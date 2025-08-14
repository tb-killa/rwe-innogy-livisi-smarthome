namespace SerialAPI.BidCosLayer.DevicesSupport.Wsd;

internal class AdapterFrameHandlerNetworkManagement : AdapterFrameHandler<WsdAdapter>
{
	private readonly BidCosFrameHandlersContainer subHandlers;

	public AdapterFrameHandlerNetworkManagement(WsdAdapter deviceAdapter)
		: base(deviceAdapter)
	{
		subHandlers = new BidCosFrameHandlersContainer(new IAdapterFrameHandler[3]
		{
			new AdapterFrameHandlerInclude(deviceAdapter),
			new AdapterFrameHandlerExclude(deviceAdapter),
			new AdapterFrameHandlerUpdateInfo(deviceAdapter)
		});
	}

	public override bool CanHandle(BidCosMessageForSend message)
	{
		return message.header.FrameType == SIPcosFrameType.NETWORK_MANAGEMENT_FRAME;
	}

	public override SendStatus Handle(BidCosMessageForSend message)
	{
		SendStatus result = SendStatus.MODE_ERROR;
		IAdapterFrameHandler sendFrameHandler = subHandlers.GetSendFrameHandler(message);
		if (sendFrameHandler != null)
		{
			result = sendFrameHandler.Handle(message);
		}
		return result;
	}
}
