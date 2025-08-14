using System.Collections.Generic;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.DataAccessInterfaces.Applications;

public interface IApplicationsSettings : IService
{
	ConfigurationItem GetSetting(string appId, string name);

	void SetValue(ConfigurationItem setting);

	List<ConfigurationItem> GetAllSettings(string appId);

	bool Delete(string appId, string name);

	void RemoveAllApplicationSettings(string appId);

	List<ConfigurationItem> GetAllSettings();

	List<string> GetAllNames(string appId);

	List<ConfigurationItem> GetAllSettingsMetadata(out int totalSize);
}
