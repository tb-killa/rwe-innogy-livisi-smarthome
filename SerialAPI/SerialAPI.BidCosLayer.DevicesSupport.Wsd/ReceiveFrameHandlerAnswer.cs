using SerialAPI.BidCoSFrames;

namespace SerialAPI.BidCosLayer.DevicesSupport.Wsd;

internal class ReceiveFrameHandlerAnswer : ReceiveFrameHandler<WsdAdapter>
{
	public ReceiveFrameHandlerAnswer(WsdAdapter deviceAdapter)
		: base(deviceAdapter)
	{
	}

	public override bool CanHandle(ReceiveFrameData frameData)
	{
		return frameData.bidcosHeader.FrameType == BIDCOSFrameType.Answer;
	}

	public override BIDCOSMessage HandleFrame(ReceiveFrameData frameData)
	{
		BIDCOSAnswerFrame bIDCOSAnswerFrame = new BIDCOSAnswerFrame(frameData.bidcosHeader);
		if (!bIDCOSAnswerFrame.Parse(frameData.data))
		{
			bIDCOSAnswerFrame = null;
		}
		if (frameData.bidcosHeader.FrameCounter == base.DeviceAdapter.bidCosHandler.m_frameCount)
		{
			base.DeviceAdapter.bidCosHandler.m_answer_ack = bIDCOSAnswerFrame?.Ack ?? false;
			base.DeviceAdapter.bidCosHandler.answerFrameReceivedEvent.Set();
		}
		if (base.DeviceAdapter.Included && !base.DeviceAdapter.bidCosHandler.m_block_answer && bIDCOSAnswerFrame != null)
		{
			return bIDCOSAnswerFrame;
		}
		return null;
	}
}
