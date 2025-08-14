using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface IFirmwareUpdateService
{
	FirmwareInformation GetFirmwareInformation(DeviceIdentifier identifier);

	FirmwareUpdateStatus FirmwareUpdateInit(DeviceIdentifier identifier, uint firmwareID, byte[] firmwareChecksum, uint firmwareSize);

	FirmwareUpdateStatus TransferUpdate(DeviceIdentifier identifier, byte[] chunkData, uint desiredOffset);

	FirmwareUpdateStatus DoUpdate(DeviceIdentifier identifier);
}
