namespace RWE.SmartHome.SHC.wMBusProtocol;

public enum FunctionFieldCode : byte
{
	InstantaneousValue = 0,
	MinimumValue = 2,
	MaximumValue = 1,
	ValueDuringErrorState = 3
}
