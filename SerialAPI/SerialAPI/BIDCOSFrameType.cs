namespace SerialAPI;

internal enum BIDCOSFrameType
{
	Sysinfo = 0,
	Configuration = 1,
	Answer = 2,
	AnswerAesSolution = 3,
	AnswerAesContainer = 4,
	Info = 16,
	Switch = 64,
	ConditionalSwitch = 65
}
