using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.SmsScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "SendSystemSms", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class SendSystemSmsRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string receiverPhoneNo;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string shcSerial;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public SmsTemplates template;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 3)]
	public string templateParameter;

	public SendSystemSmsRequest()
	{
	}

	public SendSystemSmsRequest(string receiverPhoneNo, string shcSerial, SmsTemplates template, string templateParameter)
	{
		this.receiverPhoneNo = receiverPhoneNo;
		this.shcSerial = shcSerial;
		this.template = template;
		this.templateParameter = templateParameter;
	}
}
