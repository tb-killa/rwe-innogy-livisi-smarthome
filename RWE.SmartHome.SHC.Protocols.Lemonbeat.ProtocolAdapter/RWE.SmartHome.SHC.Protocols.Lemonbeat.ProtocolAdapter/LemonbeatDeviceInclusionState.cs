namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;

public enum LemonbeatDeviceInclusionState
{
	None,
	Found,
	PublicKeyReceived,
	InclusionPending,
	InclusionInProgress,
	Included,
	ExclusionPending,
	FactoryReset,
	PublicKeyRetrievalPending,
	PublicKeyMissing
}
