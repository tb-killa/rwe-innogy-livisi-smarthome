namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate;

public class DeviceUpdateInfo
{
	public string ImageUrl { get; set; }

	public DeviceUpdateType UpdateType { get; set; }

	public string ReleaseNotesLocation { get; set; }

	public string Version { get; set; }

	public string MD5Hash { get; set; }
}
