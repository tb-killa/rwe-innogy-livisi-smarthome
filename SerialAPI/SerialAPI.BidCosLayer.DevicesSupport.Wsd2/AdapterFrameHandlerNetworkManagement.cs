namespace SerialAPI.BidCosLayer.DevicesSupport.Wsd2;

internal class AdapterFrameHandlerNetworkManagement : AdapterFrameHandler<Wsd2Adapter>
{
	private readonly BidCosFrameHandlersContainer subHandlers;

	public AdapterFrameHandlerNetworkManagement(Wsd2Adapter deviceAdapter)
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
