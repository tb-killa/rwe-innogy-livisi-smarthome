using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPcosUnconditionalSwitchCommandHandler : SIPcosCommandHandler
{
	public delegate void ReceiveUnconditionalSwitchFrame(SIPcosUnconditionalSwitchFrame unconditionalSwitch);

	public event ReceiveUnconditionalSwitchFrame ReceiveUnconditionalSwitch;

	public SIPcosUnconditionalSwitchCommandHandler(SIPcosHandler handler)
		: base(handler, SIPcosFrameType.UNCONDITIONAL_SWITCH_COMMAND)
	{
	}

	public override HandlingResult Handle(SIPcosHeader header, List<byte> message)
	{
		if (message.Count < 2 || header.FrameType != SIPcosFrameType.UNCONDITIONAL_SWITCH_COMMAND)
		{
			return HandlingResult.NotHandled;
		}
		if (this.ReceiveUnconditionalSwitch != null)
		{
			SIPcosUnconditionalSwitchFrame sIPcosUnconditionalSwitchFrame = new SIPcosUnconditionalSwitchFrame(header);
			sIPcosUnconditionalSwitchFrame.parse(ref message);
			this.ReceiveUnconditionalSwitch(sIPcosUnconditionalSwitchFrame);
			return HandlingResult.Handled;
		}
		return HandlingResult.NotHandled;
	}

	public SIPCOSMessage GenerateSendUnconditionalSwitch(SIPcosHeader header, byte Channel, byte Count, bool LongPress)
	{
		header.FrameType = SIPcosFrameType.UNCONDITIONAL_SWITCH_COMMAND;
		byte[] array = new byte[2] { Channel, 0 };
		if (LongPress)
		{
			array[0] += 64;
		}
		array[1] = Count;
		return new SIPCOSMessage(header, array);
	}
}
