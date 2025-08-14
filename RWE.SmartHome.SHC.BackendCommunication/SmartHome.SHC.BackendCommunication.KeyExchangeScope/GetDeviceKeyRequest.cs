using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace SmartHome.SHC.BackendCommunication.KeyExchangeScope;

[XmlRoot(ElementName = "GetDeviceKey", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class GetDeviceKeyRequest
{
	[XmlElement(DataType = "base64Binary", IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public byte[] sgtin;

	public GetDeviceKeyRequest()
	{
	}

	public GetDeviceKeyRequest(byte[] sgtin)
	{
		this.sgtin = sgtin;
	}
}
