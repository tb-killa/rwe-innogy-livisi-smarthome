namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;

public enum DeviceInclusionState
{
	None,
	Found,
	InclusionPending,
	Included,
	FactoryReset,
	FactoryResetWithAddressCollision,
	FoundWithAddressCollision,
	ExclusionPending,
	Excluded
}
