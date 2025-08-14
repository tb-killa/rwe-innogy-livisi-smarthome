using SerialAPI.BidCoSFrames;

namespace SerialAPI.BidCosLayer.DevicesSupport;

internal interface IReceiveFrameHandler
{
	bool CanHandle(ReceiveFrameData frameData);

	BIDCOSMessage HandleFrame(ReceiveFrameData frameData);
}
