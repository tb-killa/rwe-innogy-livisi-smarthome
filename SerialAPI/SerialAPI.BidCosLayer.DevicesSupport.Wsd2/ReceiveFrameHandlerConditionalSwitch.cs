using SerialAPI.BidCoSFrames;

namespace SerialAPI.BidCosLayer.DevicesSupport.Wsd2;

internal class ReceiveFrameHandlerConditionalSwitch : ReceiveFrameHandler<Wsd2Adapter>
{
	public ReceiveFrameHandlerConditionalSwitch(Wsd2Adapter deviceAdapter)
		: base(deviceAdapter)
	{
	}

	public override bool CanHandle(ReceiveFrameData frameData)
	{
		return frameData.bidcosHeader.FrameType == BIDCOSFrameType.ConditionalSwitch;
	}

	public override BIDCOSMessage HandleFrame(ReceiveFrameData frameData)
	{
		if (base.DeviceAdapter.Included)
		{
			BIDCOSCondSwitchFrame bIDCOSCondSwitchFrame = new BIDCOSCondSwitchFrame(frameData.bidcosHeader);
			if (bIDCOSCondSwitchFrame.Parse(frameData.data))
			{
				base.DeviceAdapter.RepeatConditionalSwitchFrame(bIDCOSCondSwitchFrame);
				return bIDCOSCondSwitchFrame;
			}
		}
		return null;
	}
}
