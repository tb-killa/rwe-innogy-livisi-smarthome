namespace SipcosCommandHandler;

public enum FirmwareCommands : byte
{
	Start,
	UpdateData,
	End,
	DoUpdate,
	RequestFirmwareInfo,
	InfoReply,
	Answer
}
