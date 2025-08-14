using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.NotificationScope;

[DebuggerStepThrough]
[XmlRoot(ElementName = "SendNotificationsResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class SendNotificationsResponse
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public NotificationResponse SendNotificationsResult;

	public SendNotificationsResponse()
	{
	}

	public SendNotificationsResponse(NotificationResponse SendNotificationsResult)
	{
		this.SendNotificationsResult = SendNotificationsResult;
	}
}
