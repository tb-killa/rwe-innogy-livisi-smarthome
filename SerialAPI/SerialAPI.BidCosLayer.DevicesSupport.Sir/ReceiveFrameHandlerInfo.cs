using RWE.SmartHome.SHC.Core.Scheduler;
using SerialAPI.BidCoSFrames;

namespace SerialAPI.BidCosLayer.DevicesSupport.Sir;

internal class ReceiveFrameHandlerInfo : ReceiveFrameHandler<SirAdapter>
{
	private readonly IScheduler scheduler;

	public ReceiveFrameHandlerInfo(SirAdapter deviceAdapter, IScheduler scheduler)
		: base(deviceAdapter)
	{
		this.scheduler = scheduler;
	}

	public override bool CanHandle(ReceiveFrameData frameData)
	{
		return frameData.bidcosHeader.FrameType == BIDCOSFrameType.Info;
	}

	public override BIDCOSMessage HandleFrame(ReceiveFrameData frameData)
	{
		BIDCOSInfoFrame bIDCOSInfoFrame = new BIDCOSInfoFrame(frameData.bidcosHeader);
		base.DeviceAdapter.bidCosHandler.answer = null;
		if (bIDCOSInfoFrame.Parse(frameData.data))
		{
			if ((bIDCOSInfoFrame.InfoType == 1 || bIDCOSInfoFrame.InfoType == 6) && frameData.m_tmp_frame_count == frameData.bidcosHeader.FrameCounter)
			{
				base.DeviceAdapter.bidCosHandler.m_answer_ack = true;
				base.DeviceAdapter.bidCosHandler.answerFrameReceivedEvent.Set();
			}
		}
		else
		{
			bIDCOSInfoFrame = null;
		}
		if (base.DeviceAdapter.bidCosHandler.m_block_answer)
		{
			lock (base.DeviceAdapter.bidCosHandler.syncRoot)
			{
				base.DeviceAdapter.bidCosHandler.m_partner = bIDCOSInfoFrame?.Partners;
			}
			base.DeviceAdapter.sirComm.LogIfVerbose("Expected info frame received");
			base.DeviceAdapter.bidCosHandler.answerFrameReceivedEvent.Set();
		}
		else if (base.DeviceAdapter.Included && bIDCOSInfoFrame != null)
		{
			return bIDCOSInfoFrame;
		}
		return null;
	}
}
