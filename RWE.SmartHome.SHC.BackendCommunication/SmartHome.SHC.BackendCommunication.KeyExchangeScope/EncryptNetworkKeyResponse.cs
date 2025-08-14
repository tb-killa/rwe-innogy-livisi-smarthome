using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace SmartHome.SHC.BackendCommunication.KeyExchangeScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "EncryptNetworkKeyResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class EncryptNetworkKeyResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public KeyExchangeResult EncryptNetworkKeyResult;

	[XmlElement(DataType = "base64Binary", IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public byte[] encTwiceNetworkKey;

	[XmlElement(DataType = "base64Binary", IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public byte[] keyAuthentication;

	public EncryptNetworkKeyResponse()
	{
	}

	public EncryptNetworkKeyResponse(KeyExchangeResult EncryptNetworkKeyResult, byte[] encTwiceNetworkKey, byte[] keyAuthentication)
	{
		this.EncryptNetworkKeyResult = EncryptNetworkKeyResult;
		this.encTwiceNetworkKey = encTwiceNetworkKey;
		this.keyAuthentication = keyAuthentication;
	}
}
