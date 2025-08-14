namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public enum PacketSendState
{
	Open,
	WaitingForMacAck,
	WaitingForAppAck,
	Ack,
	Nak,
	Done
}
