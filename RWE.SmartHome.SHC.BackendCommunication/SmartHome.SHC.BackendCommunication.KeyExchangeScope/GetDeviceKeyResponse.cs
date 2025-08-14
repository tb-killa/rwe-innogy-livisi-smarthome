using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace SmartHome.SHC.BackendCommunication.KeyExchangeScope;

[DebuggerStepThrough]
[XmlRoot(ElementName = "GetDeviceKeyResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class GetDeviceKeyResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public KeyExchangeResult GetDeviceKeyResult;

	[XmlElement(DataType = "base64Binary", IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public byte[] deviceKey;

	public GetDeviceKeyResponse()
	{
	}

	public GetDeviceKeyResponse(KeyExchangeResult GetDeviceKeyResult, byte[] deviceKey)
	{
		this.GetDeviceKeyResult = GetDeviceKeyResult;
		this.deviceKey = deviceKey;
	}
}
