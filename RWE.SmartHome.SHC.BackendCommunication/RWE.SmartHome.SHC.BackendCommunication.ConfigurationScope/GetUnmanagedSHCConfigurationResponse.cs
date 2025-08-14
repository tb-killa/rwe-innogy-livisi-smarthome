using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "GetUnmanagedSHCConfigurationResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
public class GetUnmanagedSHCConfigurationResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public ConfigurationResultCode GetUnmanagedSHCConfigurationResult;

	[XmlElement(DataType = "base64Binary", IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public byte[] data;

	public GetUnmanagedSHCConfigurationResponse()
	{
	}

	public GetUnmanagedSHCConfigurationResponse(ConfigurationResultCode GetUnmanagedSHCConfigurationResult, byte[] data)
	{
		this.GetUnmanagedSHCConfigurationResult = GetUnmanagedSHCConfigurationResult;
		this.data = data;
	}
}
