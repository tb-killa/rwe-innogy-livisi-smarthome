using System.Collections.Generic;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPCosAnswerCommandHandler : SIPcosCommandHandler
{
	public delegate void ReceiveAnswerFrame(SIPcosAnswerFrame answer);

	public event ReceiveAnswerFrame ReceiveAnswer;

	public SIPCosAnswerCommandHandler(SIPcosHandler handler)
		: base(handler, SIPcosFrameType.ANSWER)
	{
	}

	public override HandlingResult Handle(SIPcosHeader header, List<byte> message)
	{
		ReceiveAnswerFrame receiveAnswer = this.ReceiveAnswer;
		if (receiveAnswer != null && message.Count != 0 && header.FrameType == SIPcosFrameType.ANSWER)
		{
			SIPcosAnswerFrame sIPcosAnswerFrame = new SIPcosAnswerFrame(header);
			sIPcosAnswerFrame.parse(ref message);
			receiveAnswer(sIPcosAnswerFrame);
			return HandlingResult.Handled;
		}
		return HandlingResult.NotHandled;
	}

	public SendStatus SendAnswerCommand(SIPcosHeader header, SIPcosAnswerFrameStatus status, SendMode Mode)
	{
		SIPCOSMessage sIPCOSMessage = GenerateAnswerFrame(header, status);
		sIPCOSMessage.Mode = Mode;
		return Send(sIPCOSMessage);
	}

	public SIPCOSMessage GenerateAnswerFrame(SIPcosHeader header, SIPcosAnswerFrameStatus status)
	{
		header.FrameType = SIPcosFrameType.ANSWER;
		header.BiDi = false;
		List<byte> list = new List<byte>();
		list.Add((byte)status);
		return new SIPCOSMessage(header, list);
	}
}
