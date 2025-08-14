using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.SmsScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "SendSmsResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
public class SendSmsResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public MessageAppResultCode SendSmsResult;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public int? remainingQuota;

	public SendSmsResponse()
	{
	}

	public SendSmsResponse(MessageAppResultCode SendSmsResult, int? remainingQuota)
	{
		this.SendSmsResult = SendSmsResult;
		this.remainingQuota = remainingQuota;
	}
}
