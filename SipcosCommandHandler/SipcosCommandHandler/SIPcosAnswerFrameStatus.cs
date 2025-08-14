namespace SipcosCommandHandler;

public enum SIPcosAnswerFrameStatus : byte
{
	ACK = 0,
	STATUSACK = 1,
	ACKAESPROBLEM = 4,
	NAK = 128,
	NAKINHIBIT = 129,
	NAKPEERNOTCONFIGURED = 130
}
