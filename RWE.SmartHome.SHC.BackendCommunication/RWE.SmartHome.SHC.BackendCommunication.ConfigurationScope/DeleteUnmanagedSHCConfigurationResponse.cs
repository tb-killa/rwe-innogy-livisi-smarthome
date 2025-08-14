using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[XmlRoot(ElementName = "DeleteUnmanagedSHCConfigurationResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
public class DeleteUnmanagedSHCConfigurationResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public ConfigurationResultCode DeleteUnmanagedSHCConfigurationResult;

	public DeleteUnmanagedSHCConfigurationResponse()
	{
	}

	public DeleteUnmanagedSHCConfigurationResponse(ConfigurationResultCode DeleteUnmanagedSHCConfigurationResult)
	{
		this.DeleteUnmanagedSHCConfigurationResult = DeleteUnmanagedSHCConfigurationResult;
	}
}
