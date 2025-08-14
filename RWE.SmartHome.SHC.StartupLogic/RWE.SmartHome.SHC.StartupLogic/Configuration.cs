using RWE.SmartHome.SHC.Core.Configuration;

namespace RWE.SmartHome.SHC.StartupLogic;

public class Configuration : ConfigurationProperties
{
	public string CertificateSubjectName => base.ConfigurationSection.GetString("CertificateSubjectName");

	public string CertificateTemplateName => base.ConfigurationSection.GetString("CertificateTemplateName");

	public string CertificateUpnSuffix => base.ConfigurationSection.GetString("CertificateUpnSuffix");

	public string StopBackendRequestsDate => base.ConfigurationSection.GetString("StopBackendRequestsDate");

	public Configuration(IConfigurationManager manager)
		: base(manager)
	{
	}
}
