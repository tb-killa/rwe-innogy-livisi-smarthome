using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class GetEncryptedKeyEventArgs
{
	public Guid DeviceId { get; set; }

	public byte[] OneTimeKey { get; set; }

	public byte[] Sgtin { get; set; }

	public byte[] SecNumber { get; set; }

	public byte[] EncOnceNetworkKey { get; set; }

	public string FirmwareVersion { get; set; }
}
