using System.CodeDom.Compiler;

namespace RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public interface ISoftwareUpdateService
{
	SwUpdateResultCode CheckForSoftwareUpdate(string shcSerial, ShcVersionInfo shcVersionInfo, out UpdateInfo updateInfo);

	ShcUpdateAnnouncementResultCode ShcSoftwareUpdated(ShcVersionInfo newShcVersion);
}
