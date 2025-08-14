using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;

[XmlRoot(ElementName = "RetrieveOwnershipData", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class RetrieveOwnershipDataRequest
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string sessionToken;

	public RetrieveOwnershipDataRequest()
	{
	}

	public RetrieveOwnershipDataRequest(string sessionToken)
	{
		this.sessionToken = sessionToken;
	}
}
