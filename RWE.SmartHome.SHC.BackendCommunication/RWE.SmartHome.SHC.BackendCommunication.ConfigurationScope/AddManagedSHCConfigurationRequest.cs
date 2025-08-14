using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "AddManagedSHCConfiguration", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class AddManagedSHCConfigurationRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string shcSerialNo;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string configDataVersion;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public ConfigurationType configType;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 3)]
	public string xPath;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 4)]
	public string bulkXMLData;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 5)]
	public int currentPacketNumber;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 6)]
	public int nextPacketNumber;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 7)]
	public string correlationID;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 8)]
	public bool createRestorePointFirst;

	public AddManagedSHCConfigurationRequest()
	{
	}

	public AddManagedSHCConfigurationRequest(string shcSerialNo, string configDataVersion, ConfigurationType configType, string xPath, string bulkXMLData, int currentPacketNumber, int nextPacketNumber, string correlationID, bool createRestorePointFirst)
	{
		this.shcSerialNo = shcSerialNo;
		this.configDataVersion = configDataVersion;
		this.configType = configType;
		this.xPath = xPath;
		this.bulkXMLData = bulkXMLData;
		this.currentPacketNumber = currentPacketNumber;
		this.nextPacketNumber = nextPacketNumber;
		this.correlationID = correlationID;
		this.createRestorePointFirst = createRestorePointFirst;
	}
}
