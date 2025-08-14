using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "SetUnmanagedSHCConfigurationResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
public class SetUnmanagedSHCConfigurationResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public ConfigurationResultCode SetUnmanagedSHCConfigurationResult;

	public SetUnmanagedSHCConfigurationResponse()
	{
	}

	public SetUnmanagedSHCConfigurationResponse(ConfigurationResultCode SetUnmanagedSHCConfigurationResult)
	{
		this.SetUnmanagedSHCConfigurationResult = SetUnmanagedSHCConfigurationResult;
	}
}
