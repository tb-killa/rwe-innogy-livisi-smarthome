using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "SendSystemEmail", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
public class SendSystemEmailRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string receiverEmail;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string shcSerial;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public EmailTemplates template;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 3)]
	public string templateParameter;

	public SendSystemEmailRequest()
	{
	}

	public SendSystemEmailRequest(string receiverEmail, string shcSerial, EmailTemplates template, string templateParameter)
	{
		this.receiverEmail = receiverEmail;
		this.shcSerial = shcSerial;
		this.template = template;
		this.templateParameter = templateParameter;
	}
}
