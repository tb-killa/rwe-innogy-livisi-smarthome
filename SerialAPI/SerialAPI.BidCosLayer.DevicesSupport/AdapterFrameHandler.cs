namespace SerialAPI.BidCosLayer.DevicesSupport;

internal abstract class AdapterFrameHandler<T> : IAdapterFrameHandler where T : BidCosDeviceAdapter
{
	protected T DeviceAdapter { get; private set; }

	protected AdapterFrameHandler(T deviceAdapter)
	{
		DeviceAdapter = deviceAdapter;
	}

	public abstract bool CanHandle(BidCosMessageForSend message);

	public abstract SendStatus Handle(BidCosMessageForSend message);
}
