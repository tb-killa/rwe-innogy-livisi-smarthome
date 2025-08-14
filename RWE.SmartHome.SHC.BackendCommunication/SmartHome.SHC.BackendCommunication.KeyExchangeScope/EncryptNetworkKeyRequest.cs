using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace SmartHome.SHC.BackendCommunication.KeyExchangeScope;

[XmlRoot(ElementName = "EncryptNetworkKey", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class EncryptNetworkKeyRequest
{
	[XmlElement(DataType = "base64Binary", IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public byte[] sgtin;

	[XmlElement(DataType = "base64Binary", IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public byte[] secNumber;

	[XmlElement(DataType = "base64Binary", IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public byte[] encOnceNetworkKey;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 3)]
	public string deviceFirmwareVersion;

	public EncryptNetworkKeyRequest()
	{
	}

	public EncryptNetworkKeyRequest(byte[] sgtin, byte[] secNumber, byte[] encOnceNetworkKey, string deviceFirmwareVersion)
	{
		this.sgtin = sgtin;
		this.secNumber = secNumber;
		this.encOnceNetworkKey = encOnceNetworkKey;
		this.deviceFirmwareVersion = deviceFirmwareVersion;
	}
}
