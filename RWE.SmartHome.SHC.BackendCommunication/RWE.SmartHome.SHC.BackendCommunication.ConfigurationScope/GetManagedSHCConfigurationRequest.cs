using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[XmlRoot(ElementName = "GetManagedSHCConfiguration", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class GetManagedSHCConfigurationRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string shcSerialNo;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string configDataVersion;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public string configType;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 3)]
	public string xPath;

	public GetManagedSHCConfigurationRequest()
	{
	}

	public GetManagedSHCConfigurationRequest(string shcSerialNo, string configDataVersion, string configType, string xPath)
	{
		this.shcSerialNo = shcSerialNo;
		this.configDataVersion = configDataVersion;
		this.configType = configType;
		this.xPath = xPath;
	}
}
