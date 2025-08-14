using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace SmartHome.SHC.BackendCommunication.KeyExchangeScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
[XmlRoot(ElementName = "GetDevicesKeysResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class GetDevicesKeysResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public KeyExchangeResult GetDevicesKeysResult;

	[XmlArray(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	[XmlArrayItem("KeyValueOfbase64Binarybase64Binary", Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", IsNullable = false)]
	public ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary[] devicesKeys;

	public GetDevicesKeysResponse()
	{
	}

	public GetDevicesKeysResponse(KeyExchangeResult GetDevicesKeysResult, ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary[] devicesKeys)
	{
		this.GetDevicesKeysResult = GetDevicesKeysResult;
		this.devicesKeys = devicesKeys;
	}
}
