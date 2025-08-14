using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;

[XmlRoot(ElementName = "SendNotificationEmailResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
public class SendNotificationEmailResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public SendNotificationEmailResult SendNotificationEmailResult;

	public SendNotificationEmailResponse()
	{
	}

	public SendNotificationEmailResponse(SendNotificationEmailResult SendNotificationEmailResult)
	{
		this.SendNotificationEmailResult = SendNotificationEmailResult;
	}
}
