using RWE.SmartHome.SHC.Core.Configuration;

namespace RWE.SmartHome.SHC.SHCRelayDriver;

public class Configuration : ConfigurationProperties
{
	public int? PollingInterval => base.ConfigurationSection.GetInt("RelayServerPollingInterval");

	public int? MaxRetrialsCount => base.ConfigurationSection.GetInt("MaxRetrialsCount");

	public string RelayServiceAddress => base.ConfigurationSection.GetString("RelayServerUrl");

	public string BackendAccountPassword => base.ConfigurationSection.GetString("BackendAccountPassword");

	public bool? UseCertificateAuthentication => base.ConfigurationSection.GetBool("UseCertificateAuthentication");

	public int? ZipThreshold => base.ConfigurationSection.GetInt("ZipThreshold");

	public int? NotificationSendDelay => base.ConfigurationSection.GetInt("NotificationSendDelay");

	public int? ReconnectionDelay => base.ConfigurationSection.GetInt("ReconnectionDelay");

	public Configuration(IConfigurationManager manager)
		: base(manager)
	{
	}
}
