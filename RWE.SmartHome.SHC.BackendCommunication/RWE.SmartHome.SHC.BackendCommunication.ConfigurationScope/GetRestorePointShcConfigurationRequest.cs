using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
[XmlRoot(ElementName = "GetRestorePointShcConfiguration", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class GetRestorePointShcConfigurationRequest
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string restorePointId;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string configDataVersion;

	public GetRestorePointShcConfigurationRequest()
	{
	}

	public GetRestorePointShcConfigurationRequest(string restorePointId, string configDataVersion)
	{
		this.restorePointId = restorePointId;
		this.configDataVersion = configDataVersion;
	}
}
