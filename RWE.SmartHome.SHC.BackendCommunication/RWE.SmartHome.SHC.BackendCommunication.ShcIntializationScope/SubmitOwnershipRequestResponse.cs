using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "SubmitOwnershipRequestResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class SubmitOwnershipRequestResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public InitializationErrorCode SubmitOwnershipRequestResult;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string sessionToken;

	public SubmitOwnershipRequestResponse()
	{
	}

	public SubmitOwnershipRequestResponse(InitializationErrorCode SubmitOwnershipRequestResult, string sessionToken)
	{
		this.SubmitOwnershipRequestResult = SubmitOwnershipRequestResult;
		this.sessionToken = sessionToken;
	}
}
