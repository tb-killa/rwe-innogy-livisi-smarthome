using SerialAPI.BidCoSFrames;

namespace SerialAPI.BidCosLayer.DevicesSupport.Wsd;

internal class ReceiveFrameHandlerSwitch : ReceiveFrameHandler<WsdAdapter>
{
	public ReceiveFrameHandlerSwitch(WsdAdapter deviceAdapter)
		: base(deviceAdapter)
	{
	}

	public override bool CanHandle(ReceiveFrameData frameData)
	{
		return frameData.bidcosHeader.FrameType == BIDCOSFrameType.Switch;
	}

	public override BIDCOSMessage HandleFrame(ReceiveFrameData frameData)
	{
		if (base.DeviceAdapter.Included)
		{
			BIDCOSSwitchFrame bIDCOSSwitchFrame = new BIDCOSSwitchFrame(frameData.bidcosHeader);
			if (bIDCOSSwitchFrame.Parse(frameData.data))
			{
				return bIDCOSSwitchFrame;
			}
		}
		return null;
	}
}
