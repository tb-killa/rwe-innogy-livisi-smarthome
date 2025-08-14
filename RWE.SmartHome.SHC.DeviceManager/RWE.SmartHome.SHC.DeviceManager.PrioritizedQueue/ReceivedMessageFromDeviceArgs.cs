namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public class ReceivedMessageFromDeviceArgs
{
	public bool StayAwake { get; private set; }

	public bool Included { get; private set; }

	public byte[] Address { get; set; }

	public ReceivedMessageFromDeviceArgs(byte[] address, bool stayAwake, bool included)
	{
		StayAwake = stayAwake;
		Included = included;
		Address = address;
	}
}
