using System;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;

public interface IBidCosConfigurator
{
	Guid Configure(byte[] sourceAddress, byte[] destinationAddress, byte channel, byte[] parameters);

	void Flush();
}
