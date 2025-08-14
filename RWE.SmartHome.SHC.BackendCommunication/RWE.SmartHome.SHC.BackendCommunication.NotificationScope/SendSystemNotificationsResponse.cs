using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.NotificationScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "SendSystemNotificationsResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class SendSystemNotificationsResponse
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public NotificationResponse SendSystemNotificationsResult;

	public SendSystemNotificationsResponse()
	{
	}

	public SendSystemNotificationsResponse(NotificationResponse SendSystemNotificationsResult)
	{
		this.SendSystemNotificationsResult = SendSystemNotificationsResult;
	}
}
