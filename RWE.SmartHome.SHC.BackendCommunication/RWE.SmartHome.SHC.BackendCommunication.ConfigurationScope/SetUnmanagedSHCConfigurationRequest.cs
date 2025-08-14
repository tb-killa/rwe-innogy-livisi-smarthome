using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "SetUnmanagedSHCConfiguration", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
public class SetUnmanagedSHCConfigurationRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string shcSerialNo;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string configDataVersion;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public ConfigurationType configType;

	[XmlElement(DataType = "base64Binary", IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 3)]
	public byte[] data;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 4)]
	public int currentPacketNumber;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 5)]
	public int nextPacketNumber;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 6)]
	public string correlationID;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 7)]
	public bool createRestorePointFirst;

	public SetUnmanagedSHCConfigurationRequest()
	{
	}

	public SetUnmanagedSHCConfigurationRequest(string shcSerialNo, string configDataVersion, ConfigurationType configType, byte[] data, int currentPacketNumber, int nextPacketNumber, string correlationID, bool createRestorePointFirst)
	{
		this.shcSerialNo = shcSerialNo;
		this.configDataVersion = configDataVersion;
		this.configType = configType;
		this.data = data;
		this.currentPacketNumber = currentPacketNumber;
		this.nextPacketNumber = nextPacketNumber;
		this.correlationID = correlationID;
		this.createRestorePointFirst = createRestorePointFirst;
	}
}
