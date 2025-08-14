using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace SmartHome.SHC.BackendCommunication.KeyExchangeScope;

[XmlRoot(ElementName = "GetDevicesKeys", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
public class GetDevicesKeysRequest
{
	[XmlArrayItem(Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", DataType = "base64Binary")]
	[XmlArray(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public byte[][] sgtins;

	public GetDevicesKeysRequest()
	{
	}

	public GetDevicesKeysRequest(byte[][] sgtins)
	{
		this.sgtins = sgtins;
	}
}
