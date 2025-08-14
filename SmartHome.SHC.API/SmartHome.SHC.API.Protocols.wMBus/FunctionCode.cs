namespace SmartHome.SHC.API.Protocols.wMBus;

public enum FunctionCode : byte
{
	InstantaneousValue = 0,
	MinimumValue = 2,
	MaximumValue = 1,
	ValueDuringErrorState = 3
}
