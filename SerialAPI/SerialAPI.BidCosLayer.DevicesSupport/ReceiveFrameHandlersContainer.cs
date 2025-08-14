using System.Linq;

namespace SerialAPI.BidCosLayer.DevicesSupport;

internal class ReceiveFrameHandlersContainer
{
	private readonly IReceiveFrameHandler[] sendFrameHandlerCollection;

	public ReceiveFrameHandlersContainer(IReceiveFrameHandler[] sendFrameHandlerCollection)
	{
		this.sendFrameHandlerCollection = sendFrameHandlerCollection;
	}

	public IReceiveFrameHandler GetSendFrameHandler(ReceiveFrameData frameData)
	{
		return sendFrameHandlerCollection.FirstOrDefault((IReceiveFrameHandler x) => x.CanHandle(frameData));
	}
}
