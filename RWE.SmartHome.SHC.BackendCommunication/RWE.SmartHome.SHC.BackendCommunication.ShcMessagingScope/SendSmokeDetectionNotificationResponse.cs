using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "SendSmokeDetectionNotificationResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class SendSmokeDetectionNotificationResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public SendSmokeDetectedNotificationResult SendSmokeDetectionNotificationResult;

	public SendSmokeDetectionNotificationResponse()
	{
	}

	public SendSmokeDetectionNotificationResponse(SendSmokeDetectedNotificationResult SendSmokeDetectionNotificationResult)
	{
		this.SendSmokeDetectionNotificationResult = SendSmokeDetectionNotificationResult;
	}
}
