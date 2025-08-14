using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "GetManagedSHCConfigurationResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class GetManagedSHCConfigurationResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public ConfigurationResultCode GetManagedSHCConfigurationResult;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string result;

	public GetManagedSHCConfigurationResponse()
	{
	}

	public GetManagedSHCConfigurationResponse(ConfigurationResultCode GetManagedSHCConfigurationResult, string result)
	{
		this.GetManagedSHCConfigurationResult = GetManagedSHCConfigurationResult;
		this.result = result;
	}
}
