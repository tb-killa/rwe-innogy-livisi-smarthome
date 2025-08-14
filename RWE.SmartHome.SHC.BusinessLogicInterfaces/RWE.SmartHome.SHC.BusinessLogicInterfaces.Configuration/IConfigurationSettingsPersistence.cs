namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

public interface IConfigurationSettingsPersistence
{
	void SaveConfigurationVersion(int version);

	int LoadConfigurationVersion();

	void SaveConfigurationDirtyFlag(bool isDirty);

	bool LoadConfigurationDirtyFlag();
}
