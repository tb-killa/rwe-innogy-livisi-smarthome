using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "SendEmail", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class SendEmailRequest
{
	[XmlArray(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	[XmlArrayItem(Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
	public string[] receivers;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string message;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public string shcSerial;

	public SendEmailRequest()
	{
	}

	public SendEmailRequest(string[] receivers, string message, string shcSerial)
	{
		this.receivers = receivers;
		this.message = message;
		this.shcSerial = shcSerial;
	}
}
