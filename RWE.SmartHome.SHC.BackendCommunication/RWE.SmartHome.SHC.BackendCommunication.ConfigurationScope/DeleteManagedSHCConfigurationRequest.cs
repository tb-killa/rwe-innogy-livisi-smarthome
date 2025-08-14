using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[XmlRoot(ElementName = "DeleteManagedSHCConfiguration", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class DeleteManagedSHCConfigurationRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string shcSerialNo;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string configDataVersion;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public ConfigurationType configType;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 3)]
	public string xPath;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 4)]
	public bool createRestorePointFirst;

	public DeleteManagedSHCConfigurationRequest()
	{
	}

	public DeleteManagedSHCConfigurationRequest(string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, bool createRestorePointFirst)
	{
		this.shcSerialNo = shcSerialNo;
		this.configDataVersion = configDataVersion;
		this.configType = configType;
		this.xPath = xPath;
		this.createRestorePointFirst = createRestorePointFirst;
	}
}
