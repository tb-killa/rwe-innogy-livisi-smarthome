using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces;

public interface ISoftwareUpdateClient
{
	SwUpdateResultCode CheckForSoftwareUpdate(string certificateThumbprint, string shcSerial, ShcVersionInfo shcVersionInfo, out UpdateInfo updateInfo);

	ShcUpdateAnnouncementResultCode AnnounceShcUpdate(string certificateThumbprint, ShcVersionInfo shcVersionInfo);
}
