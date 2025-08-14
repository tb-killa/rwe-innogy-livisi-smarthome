using RWE.SmartHome.SHC.Core.Configuration;

namespace RWE.SmartHome.SHC.BackendCommunication;

internal class Configuration : ConfigurationProperties
{
	public string DeviceManagementServiceUrl => base.ConfigurationSection.GetString("DeviceManagementServiceUrl");

	public string ConfigurationServiceUrl => base.ConfigurationSection.GetString("ConfigurationServiceUrl");

	public string SoftwareUpdateServiceUrl => base.ConfigurationSection.GetString("SoftwareUpdateServiceUrl");

	public string KeyExchangeServiceUrl => base.ConfigurationSection.GetString("KeyExchangeServiceUrl");

	public string ShcInitializationServiceUrl => base.ConfigurationSection.GetString("ShcInitializationServiceUrl");

	public string ShcMessagingServiceUrl => base.ConfigurationSection.GetString("ShcMessagingServiceUrl");

	public string ApplicationTokenServiceUrl => base.ConfigurationSection.GetString("ApplicationTokenServiceUrl");

	public string DataTrackingClientUrl => base.ConfigurationSection.GetString("DataTrackingClientUrl");

	public string DeviceUpdateServiceUrl => base.ConfigurationSection.GetString("DeviceUpdateServiceUrl");

	public string NotificationServiceUrl => base.ConfigurationSection.GetString("NotificationServiceUrl");

	public string SmsServiceUrl => base.ConfigurationSection.GetString("SmsServiceUrl");

	public Configuration(IConfigurationManager manager)
		: base(manager)
	{
	}
}
