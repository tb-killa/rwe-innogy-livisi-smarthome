namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;

public class StoredDevice
{
	public byte[] Sgtin { get; set; }

	public string SerialNumber { get; set; }

	public byte[] Key { get; set; }
}
