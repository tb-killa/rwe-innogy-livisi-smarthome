using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "GetEmailRemainingQuotaResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class GetEmailRemainingQuotaResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public MessageAppResultCode GetEmailRemainingQuotaResult;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public int? remainingQuota;

	public GetEmailRemainingQuotaResponse()
	{
	}

	public GetEmailRemainingQuotaResponse(MessageAppResultCode GetEmailRemainingQuotaResult, int? remainingQuota)
	{
		this.GetEmailRemainingQuotaResult = GetEmailRemainingQuotaResult;
		this.remainingQuota = remainingQuota;
	}
}
