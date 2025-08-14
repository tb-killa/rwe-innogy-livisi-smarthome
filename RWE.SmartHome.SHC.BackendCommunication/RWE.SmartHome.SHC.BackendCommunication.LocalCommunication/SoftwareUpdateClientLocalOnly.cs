using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunication.LocalCommunication;

internal class SoftwareUpdateClientLocalOnly : ISoftwareUpdateClient
{
	public SwUpdateResultCode CheckForSoftwareUpdate(string certificateThumbprint, string shcSerial, ShcVersionInfo shcVersionInfo, out UpdateInfo updateInfo)
	{
		updateInfo = new UpdateInfo();
		return SwUpdateResultCode.AlreadyLatestVersion;
	}

	public ShcUpdateAnnouncementResultCode AnnounceShcUpdate(string certificateThumbprint, ShcVersionInfo shcVersionInfo)
	{
		return ShcUpdateAnnouncementResultCode.Success;
	}
}
