using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.SmsScope;

[XmlRoot(ElementName = "GetSmsRemainingQuota", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class GetSmsRemainingQuotaRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string shcSerial;

	public GetSmsRemainingQuotaRequest()
	{
	}

	public GetSmsRemainingQuotaRequest(string shcSerial)
	{
		this.shcSerial = shcSerial;
	}
}
