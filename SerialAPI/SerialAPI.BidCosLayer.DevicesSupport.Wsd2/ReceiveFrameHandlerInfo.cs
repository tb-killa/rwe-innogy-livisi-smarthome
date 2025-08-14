using SerialAPI.BidCoSFrames;

namespace SerialAPI.BidCosLayer.DevicesSupport.Wsd2;

internal class ReceiveFrameHandlerInfo : ReceiveFrameHandler<Wsd2Adapter>
{
	public ReceiveFrameHandlerInfo(Wsd2Adapter deviceAdapter)
		: base(deviceAdapter)
	{
	}

	public override bool CanHandle(ReceiveFrameData frameData)
	{
		return frameData.bidcosHeader.FrameType == BIDCOSFrameType.Info;
	}

	public override BIDCOSMessage HandleFrame(ReceiveFrameData frameData)
	{
		base.DeviceAdapter.bidCosHandler.answer = null;
		BIDCOSInfoFrame bIDCOSInfoFrame = new BIDCOSInfoFrame(frameData.bidcosHeader);
		if (!bIDCOSInfoFrame.Parse(frameData.data))
		{
			bIDCOSInfoFrame = null;
		}
		else if ((bIDCOSInfoFrame.InfoType == 1 || bIDCOSInfoFrame.InfoType == 6) && frameData.m_tmp_frame_count == frameData.bidcosHeader.FrameCounter)
		{
			base.DeviceAdapter.bidCosHandler.m_answer_ack = true;
			base.DeviceAdapter.bidCosHandler.answerFrameReceivedEvent.Set();
		}
		if (base.DeviceAdapter.bidCosHandler.m_block_answer)
		{
			lock (base.DeviceAdapter.bidCosHandler.syncRoot)
			{
				base.DeviceAdapter.bidCosHandler.m_partner = bIDCOSInfoFrame?.Partners;
			}
			base.DeviceAdapter.bidCosHandler.answerFrameReceivedEvent.Set();
		}
		else if (base.DeviceAdapter.Included && bIDCOSInfoFrame != null)
		{
			return bIDCOSInfoFrame;
		}
		return null;
	}
}
