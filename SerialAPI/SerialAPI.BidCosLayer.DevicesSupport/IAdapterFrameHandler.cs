namespace SerialAPI.BidCosLayer.DevicesSupport;

internal interface IAdapterFrameHandler
{
	bool CanHandle(BidCosMessageForSend message);

	SendStatus Handle(BidCosMessageForSend message);
}
