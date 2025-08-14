namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public enum SequenceType
{
	Configuration,
	Inclusion,
	Exclusion,
	CollisionNotification,
	Icmp,
	DirectExecution,
	StatusRequest,
	FirmwareUpdate,
	Ack,
	Other
}
