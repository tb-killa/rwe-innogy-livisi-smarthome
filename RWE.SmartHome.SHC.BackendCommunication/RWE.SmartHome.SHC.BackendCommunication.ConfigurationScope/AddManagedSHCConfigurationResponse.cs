using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "AddManagedSHCConfigurationResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class AddManagedSHCConfigurationResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public ConfigurationResultCode AddManagedSHCConfigurationResult;

	public AddManagedSHCConfigurationResponse()
	{
	}

	public AddManagedSHCConfigurationResponse(ConfigurationResultCode AddManagedSHCConfigurationResult)
	{
		this.AddManagedSHCConfigurationResult = AddManagedSHCConfigurationResult;
	}
}
