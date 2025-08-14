using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;

[XmlRoot(ElementName = "SendSmokeDetectionNotification14Response", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class SendSmokeDetectionNotification14Response
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public SendSmokeDetectedNotificationResult SendSmokeDetectionNotification14Result;

	public SendSmokeDetectionNotification14Response()
	{
	}

	public SendSmokeDetectionNotification14Response(SendSmokeDetectedNotificationResult SendSmokeDetectionNotification14Result)
	{
		this.SendSmokeDetectionNotification14Result = SendSmokeDetectionNotification14Result;
	}
}
