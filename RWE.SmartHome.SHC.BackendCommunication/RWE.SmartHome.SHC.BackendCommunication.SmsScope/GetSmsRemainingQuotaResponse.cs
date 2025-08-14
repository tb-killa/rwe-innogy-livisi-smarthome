using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.SmsScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "GetSmsRemainingQuotaResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
public class GetSmsRemainingQuotaResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public MessageAppResultCode GetSmsRemainingQuotaResult;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public int? remainingQuota;

	public GetSmsRemainingQuotaResponse()
	{
	}

	public GetSmsRemainingQuotaResponse(MessageAppResultCode GetSmsRemainingQuotaResult, int? remainingQuota)
	{
		this.GetSmsRemainingQuotaResult = GetSmsRemainingQuotaResult;
		this.remainingQuota = remainingQuota;
	}
}
