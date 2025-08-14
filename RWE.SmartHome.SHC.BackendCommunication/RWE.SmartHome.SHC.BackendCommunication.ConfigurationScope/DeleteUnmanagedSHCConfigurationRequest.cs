using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[XmlRoot(ElementName = "DeleteUnmanagedSHCConfiguration", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
public class DeleteUnmanagedSHCConfigurationRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string shcSerialNo;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string configDataVersion;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public ConfigurationType configType;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 3)]
	public bool createRestorePointFirst;

	public DeleteUnmanagedSHCConfigurationRequest()
	{
	}

	public DeleteUnmanagedSHCConfigurationRequest(string shcSerialNo, string configDataVersion, ConfigurationType configType, bool createRestorePointFirst)
	{
		this.shcSerialNo = shcSerialNo;
		this.configDataVersion = configDataVersion;
		this.configType = configType;
		this.createRestorePointFirst = createRestorePointFirst;
	}
}
