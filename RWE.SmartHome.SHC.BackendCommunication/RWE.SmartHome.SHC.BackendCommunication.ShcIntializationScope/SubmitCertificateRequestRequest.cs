using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "SubmitCertificateRequest", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class SubmitCertificateRequestRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string shcSerial;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string pin;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public string certificateRequest;

	public SubmitCertificateRequestRequest()
	{
	}

	public SubmitCertificateRequestRequest(string shcSerial, string pin, string certificateRequest)
	{
		this.shcSerial = shcSerial;
		this.pin = pin;
		this.certificateRequest = certificateRequest;
	}
}
