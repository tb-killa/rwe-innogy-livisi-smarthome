using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace SmartHome.SHC.BackendCommunication.KeyExchangeScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "GetMasterKeyResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
public class GetMasterKeyResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public KeyExchangeResult GetMasterKeyResult;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string masterKey;

	public GetMasterKeyResponse()
	{
	}

	public GetMasterKeyResponse(KeyExchangeResult GetMasterKeyResult, string masterKey)
	{
		this.GetMasterKeyResult = GetMasterKeyResult;
		this.masterKey = masterKey;
	}
}
