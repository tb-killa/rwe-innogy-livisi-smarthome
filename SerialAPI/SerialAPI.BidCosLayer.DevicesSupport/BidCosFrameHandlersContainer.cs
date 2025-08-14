using System.Linq;

namespace SerialAPI.BidCosLayer.DevicesSupport;

internal class BidCosFrameHandlersContainer
{
	private readonly IAdapterFrameHandler[] sendFrameHandlerCollection;

	public BidCosFrameHandlersContainer(IAdapterFrameHandler[] sendFrameHandlerCollection)
	{
		this.sendFrameHandlerCollection = sendFrameHandlerCollection;
	}

	public IAdapterFrameHandler GetSendFrameHandler(BidCosMessageForSend message)
	{
		return sendFrameHandlerCollection.FirstOrDefault((IAdapterFrameHandler x) => x.CanHandle(message));
	}
}
