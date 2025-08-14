using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface IConfigurationService
{
	void CommitDeviceConfiguration(DeviceIdentifier aDevice);

	void RollbackConfiguration(DeviceIdentifier aDevice);
}
