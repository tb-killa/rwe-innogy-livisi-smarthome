using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPcosConditionalSwitchCommandHandler : SIPcosCommandHandler
{
	public delegate void ReceiveConditionalSwitchFrame(SIPcosConditionalSwitchFrame conditionalSwitch);

	public event ReceiveConditionalSwitchFrame ReceiveConditionalSwitch;

	public SIPcosConditionalSwitchCommandHandler(SIPcosHandler handler)
		: base(handler, SIPcosFrameType.CONDITIONAL_SWITCH_COMMAND)
	{
	}

	public override HandlingResult Handle(SIPcosHeader header, List<byte> message)
	{
		if (message.Count < 2 || header.FrameType != SIPcosFrameType.CONDITIONAL_SWITCH_COMMAND)
		{
			return HandlingResult.NotHandled;
		}
		if (this.ReceiveConditionalSwitch != null)
		{
			SIPcosConditionalSwitchFrame sIPcosConditionalSwitchFrame = new SIPcosConditionalSwitchFrame(header);
			sIPcosConditionalSwitchFrame.parse(ref message);
			this.ReceiveConditionalSwitch(sIPcosConditionalSwitchFrame);
			return HandlingResult.Handled;
		}
		return HandlingResult.NotHandled;
	}

	public SIPCOSMessage GenerateSendConditionalSwitch(SIPcosHeader header, byte Channel, byte Count, byte Decision, bool LongPress)
	{
		header.FrameType = SIPcosFrameType.CONDITIONAL_SWITCH_COMMAND;
		List<byte> list = new List<byte>();
		list.Add(Channel);
		if (LongPress)
		{
			list[0] += 64;
		}
		list.Add(Count);
		list.Add(Decision);
		return new SIPCOSMessage(header, list);
	}
}
