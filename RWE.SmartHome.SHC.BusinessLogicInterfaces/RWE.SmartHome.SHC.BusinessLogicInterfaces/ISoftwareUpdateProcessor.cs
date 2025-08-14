using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces;

public interface ISoftwareUpdateProcessor : IService
{
	string AvailableUpdateVersion { get; }

	bool DoUpdate(SoftwareUpdateModifier updateModifier, bool usePersonalCertificate, bool saveDatabase, bool suppressErrorOutput, bool downloadOnly);

	UpdateCheckResult CheckForUpdate();

	bool AnnounceUpdate();
}
