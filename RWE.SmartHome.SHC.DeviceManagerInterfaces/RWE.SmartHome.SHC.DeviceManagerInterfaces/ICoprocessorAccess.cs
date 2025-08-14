namespace RWE.SmartHome.SHC.DeviceManagerInterfaces;

public interface ICoprocessorAccess
{
	byte[] Version { get; }

	byte[] Checksum { get; }

	uint SecuritySequenceCounter { get; set; }

	void ResetCoprocessor();
}
