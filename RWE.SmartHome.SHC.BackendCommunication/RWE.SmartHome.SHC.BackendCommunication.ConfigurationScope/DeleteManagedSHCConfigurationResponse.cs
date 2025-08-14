using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "DeleteManagedSHCConfigurationResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
public class DeleteManagedSHCConfigurationResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public ConfigurationResultCode DeleteManagedSHCConfigurationResult;

	public DeleteManagedSHCConfigurationResponse()
	{
	}

	public DeleteManagedSHCConfigurationResponse(ConfigurationResultCode DeleteManagedSHCConfigurationResult)
	{
		this.DeleteManagedSHCConfigurationResult = DeleteManagedSHCConfigurationResult;
	}
}
