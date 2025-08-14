using System.Reflection;

namespace RWE.SmartHome.SHC.Core.Configuration;

public class ConfigurationProperties
{
	private readonly ConfigurationSection configurationSection;

	private static ConfigurationSection mainConfigurationSection;

	private readonly IConfigurationManager configurationManager;

	public string ShcFriendlyName => mainConfigurationSection.GetString("ShcFriendlyName");

	public bool? ForceRegistration => mainConfigurationSection.GetBool("ForceRegistration");

	public bool? AllowConfigurationWhileOffline => mainConfigurationSection.GetBool("AllowConfigurationWhileOffline");

	public string DefaultCertificateFile => mainConfigurationSection.GetString("DefaultCertificateFile");

	public string DefaultCertificatePassword => mainConfigurationSection.GetString("DefaultCertificatePassword");

	public string DefaultCertificateSearchString => ConfigurationSection.GetString("DefaultCertificateSearchString");

	public string RootCertificatesFile => mainConfigurationSection.GetString("RootCertificatesFile");

	public string IntermediateCertificatesFile => mainConfigurationSection.GetString("IntermediateCertificatesFile");

	public string ForcedHostname => mainConfigurationSection.GetString("ForcedHostname");

	public string HostnameFormatString => mainConfigurationSection.GetString("HostnameFormatString");

	public int? NumberOfPossibleHostnames => mainConfigurationSection.GetInt("NumberOfPossibleHostnames");

	public int? NameResolutionWait => mainConfigurationSection.GetInt("NameResolutionWait");

	public string NTPServers => mainConfigurationSection.GetString("NTPServers");

	public string TimeZone => mainConfigurationSection.GetString("TimeZone");

	public string V1CoprocessorVersion => ConfigurationSection.GetString("V1CoprocessorVersion");

	public bool DisableSetEntitiesRequestsValidation => ConfigurationSection.GetBool("DisableSetEntitiesRequestsValidation") == true;

	public int AddinLoadMemoryLimit => mainConfigurationSection.GetInt("AddinLoadMemoryLimit") ?? 100;

	public int AddinLoadMemoryLimitStartup => mainConfigurationSection.GetInt("AddinLoadMemoryLimitStartup") ?? 100;

	protected ConfigurationSection ConfigurationSection => configurationSection;

	public ConfigurationProperties(IConfigurationManager manager)
	{
		string name = Assembly.GetExecutingAssembly().GetName().Name;
		string name2 = Assembly.GetCallingAssembly().GetName().Name;
		configurationSection = manager[name2];
		if (mainConfigurationSection == null)
		{
			mainConfigurationSection = manager[name];
		}
		configurationManager = manager;
	}
}
