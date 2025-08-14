using SerialAPI.BidCoSFrames;

namespace SerialAPI.BidCosLayer.DevicesSupport;

internal abstract class ReceiveFrameHandler<T> : IReceiveFrameHandler where T : BidCosDeviceAdapter
{
	protected T DeviceAdapter { get; private set; }

	public abstract bool CanHandle(ReceiveFrameData frameData);

	public abstract BIDCOSMessage HandleFrame(ReceiveFrameData frameData);

	protected ReceiveFrameHandler(T deviceAdapter)
	{
		DeviceAdapter = deviceAdapter;
	}
}
