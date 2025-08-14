using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;

[DebuggerStepThrough]
[XmlRoot(ElementName = "RetrieveInitializationData", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class RetrieveInitializationDataRequest
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string sessionToken;

	public RetrieveInitializationDataRequest()
	{
	}

	public RetrieveInitializationDataRequest(string sessionToken)
	{
		this.sessionToken = sessionToken;
	}
}
