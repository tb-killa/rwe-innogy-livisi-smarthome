using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface INetworkManagementService
{
	void SetInclusionData(DeviceIdentifier destination, string inclusionData);

	void SetInclusionData(DeviceIdentifier destination, string inclusionData, byte addressSize);
}
