using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.NotificationScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "SendNotifications", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class SendNotificationsRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public CustomNotification notification;

	public SendNotificationsRequest()
	{
	}

	public SendNotificationsRequest(CustomNotification notification)
	{
		this.notification = notification;
	}
}
