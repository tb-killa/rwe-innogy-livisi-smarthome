using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "SubmitCertificateRequestResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class SubmitCertificateRequestResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public InitializationErrorCode SubmitCertificateRequestResult;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string sessionToken;

	public SubmitCertificateRequestResponse()
	{
	}

	public SubmitCertificateRequestResponse(InitializationErrorCode SubmitCertificateRequestResult, string sessionToken)
	{
		this.SubmitCertificateRequestResult = SubmitCertificateRequestResult;
		this.sessionToken = sessionToken;
	}
}
