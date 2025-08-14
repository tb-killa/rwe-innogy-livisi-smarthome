using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;

[XmlRoot(ElementName = "SendSmokeDetectionNotification", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
public class SendSmokeDetectionNotificationRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string shcSerialNo;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string roomName;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public DateTime date;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 3)]
	public int shcTimeOffset;

	public SendSmokeDetectionNotificationRequest()
	{
	}

	public SendSmokeDetectionNotificationRequest(string shcSerialNo, string roomName, DateTime date, int shcTimeOffset)
	{
		this.shcSerialNo = shcSerialNo;
		this.roomName = roomName;
		this.date = date;
		this.shcTimeOffset = shcTimeOffset;
	}
}
