using RWE.SmartHome.SHC.Core.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogic;

public class Configuration : ConfigurationProperties
{
	public string SoftwareUpdateWindowStartTime => base.ConfigurationSection.GetString("SoftwareUpdateWindowStartTime");

	public string SoftwareUpdateWindowStopTime => base.ConfigurationSection.GetString("SoftwareUpdateWindowStopTime");

	public string SoftwareUpdateDownloadOnlyStartTime => base.ConfigurationSection.GetString("SoftwareUpdateDownloadOnlyStartTime");

	public string SoftwareUpdateDownloadOnlyEndTime => base.ConfigurationSection.GetString("SoftwareUpdateDownloadOnlyEndTime");

	public bool? CoprocessorUpdateEnabled => base.ConfigurationSection.GetBool("CoprocessorUpdateEnabled");

	public string TargetCoprocessorVersion => base.ConfigurationSection.GetString("TargetCoprocessorVersion");

	public int? CoprocessorUpdateTimeout => base.ConfigurationSection.GetInt("CoprocessorUpdateTimeout");

	public int? CoprocessorUpdateRetryCount => base.ConfigurationSection.GetInt("CoprocessorUpdateRetryCount");

	public string TargetCoprocessorChecksum => base.ConfigurationSection.GetString("TargetCoprocessorChecksum");

	public string ProtocolSpecificDataPersistenceStartTime => base.ConfigurationSection.GetString("ProtocolSpecificDataPersistenceStartTime") ?? "0:00";

	public string ProtocolSpecificDataPersistenceStopTime => base.ConfigurationSection.GetString("ProtocolSpecificDataPersistenceStopTime") ?? "5:00";

	public decimal? MemoryLoadThreshold => base.ConfigurationSection.GetDecimal("MemoryLoadThreshold");

	public int? DeviceFirmwareCheckTime => base.ConfigurationSection.GetInt("DeviceFirmwareCheckTime");

	public int? MaxDaysForNewMessages => base.ConfigurationSection.GetInt("MaxDaysForNewMessages");

	public int? MaxDaysForReadMessages => base.ConfigurationSection.GetInt("MaxDaysForReadMessages");

	public int? MaxNumberOfMessages => base.ConfigurationSection.GetInt("MaxNumberOfMessages");

	public int? MinimumMemoryNecessaryForUpdateOS => base.ConfigurationSection.GetInt("MinimumMemoryNecessaryForUpdateOS");

	public int? MinimumMemoryNecessaryForUpdateApplication => base.ConfigurationSection.GetInt("MinimumMemoryNecessaryForUpdateApplication");

	public int? MaximumNumberOfDevicesToDownloadOnce => base.ConfigurationSection.GetInt("MaximumNumberOfDevicesToDownloadOnce");

	public int? SendEmailReminderInNumberOfDays => base.ConfigurationSection.GetInt("SendEmailReminderInNumberOfDays");

	public Configuration(IConfigurationManager manager)
		: base(manager)
	{
	}
}
