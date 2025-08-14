using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class EncryptedKeyResponseEventArgs
{
	public Guid DeviceId { get; set; }

	public EncryptedKeyResponseStatus Result { get; set; }

	public byte[] OneTimeKey { get; set; }

	public byte[] EncTwiceNetworkKey { get; set; }

	public byte[] KeyAuthentication { get; set; }
}
