using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "SendSmokeDetectionNotification14", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class SendSmokeDetectionNotification14Request
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string shcSerialNo;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string roomName;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public DateTime date;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 3)]
	public string culture;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 4)]
	public int shcTimeOffset;

	public SendSmokeDetectionNotification14Request()
	{
	}

	public SendSmokeDetectionNotification14Request(string shcSerialNo, string roomName, DateTime date, string culture, int shcTimeOffset)
	{
		this.shcSerialNo = shcSerialNo;
		this.roomName = roomName;
		this.date = date;
		this.culture = culture;
		this.shcTimeOffset = shcTimeOffset;
	}
}
