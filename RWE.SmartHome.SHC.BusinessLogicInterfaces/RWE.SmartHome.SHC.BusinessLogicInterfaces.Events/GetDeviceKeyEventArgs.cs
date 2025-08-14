using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class GetDeviceKeyEventArgs
{
	public SGTIN96 Sgtin { get; set; }

	public EncryptedKeyResponseStatus Result { get; set; }

	public byte[] DeviceKey { get; set; }
}
