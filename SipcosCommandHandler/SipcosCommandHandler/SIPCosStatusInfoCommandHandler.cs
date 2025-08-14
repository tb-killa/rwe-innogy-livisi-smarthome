using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPCosStatusInfoCommandHandler : SIPcosCommandHandler
{
	public delegate void ReceiveStatusFrame(SIPcosStatusFrame status);

	public event ReceiveStatusFrame ReceiveStatus;

	public SIPCosStatusInfoCommandHandler(SIPcosHandler handler)
		: base(handler, SIPcosFrameType.STATUSINFO)
	{
	}

	public override HandlingResult Handle(SIPcosHeader header, List<byte> message)
	{
		HandlingResult result = HandlingResult.NotHandled;
		if (this.ReceiveStatus != null && message.Count != 0)
		{
			if (header.FrameType == SIPcosFrameType.STATUSINFO)
			{
				SIPcosStatusFrame sIPcosStatusFrame = new SIPcosStatusFrame(header);
				sIPcosStatusFrame.parse(ref message);
				this.ReceiveStatus(sIPcosStatusFrame);
				result = HandlingResult.Handled;
			}
			else if (header.FrameType == SIPcosFrameType.TIMESLOT_CC)
			{
				SIPcosStatusFrame sIPcosStatusFrame2 = new SIPcosStatusFrame(header);
				sIPcosStatusFrame2.parseTimeSlot(ref message);
				sIPcosStatusFrame2.KeyChannelNumber = 2;
				if ((sIPcosStatusFrame2.TimeSlotMode & SIPcosStatusTimeSlotMode.MODE_AUTO) == SIPcosStatusTimeSlotMode.MODE_AUTO)
				{
					sIPcosStatusFrame2.IsLevel = 2;
				}
				else
				{
					sIPcosStatusFrame2.IsLevel = 1;
				}
				this.ReceiveStatus(sIPcosStatusFrame2);
				sIPcosStatusFrame2.KeyChannelNumber = 4;
				sIPcosStatusFrame2.IsLevel = sIPcosStatusFrame2.Setpoint;
				this.ReceiveStatus(sIPcosStatusFrame2);
				result = HandlingResult.Handled;
			}
		}
		return result;
	}

	public SIPCOSMessage GenerateRequestStatusInfo(SIPcosHeader header, byte Channel)
	{
		header.FrameType = SIPcosFrameType.STATUSINFO;
		header.BiDi = true;
		return new SIPCOSMessage(header, new byte[2] { 0, Channel });
	}
}
