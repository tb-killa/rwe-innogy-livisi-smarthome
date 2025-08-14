using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "GetRestorePointShcConfigurationResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class GetRestorePointShcConfigurationResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public ConfigurationResultCode GetRestorePointShcConfigurationResult;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string result;

	public GetRestorePointShcConfigurationResponse()
	{
	}

	public GetRestorePointShcConfigurationResponse(ConfigurationResultCode GetRestorePointShcConfigurationResult, string result)
	{
		this.GetRestorePointShcConfigurationResult = GetRestorePointShcConfigurationResult;
		this.result = result;
	}
}
