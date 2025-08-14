using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;

[DebuggerStepThrough]
[XmlRoot(ElementName = "GetEmailRemainingQuota", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class GetEmailRemainingQuotaRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string shcSerial;

	public GetEmailRemainingQuotaRequest()
	{
	}

	public GetEmailRemainingQuotaRequest(string shcSerial)
	{
		this.shcSerial = shcSerial;
	}
}
