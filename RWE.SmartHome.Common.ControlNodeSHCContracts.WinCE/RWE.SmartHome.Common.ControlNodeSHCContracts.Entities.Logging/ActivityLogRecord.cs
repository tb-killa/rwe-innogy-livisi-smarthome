using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;

public class ActivityLogRecord
{
	[XmlAttribute(AttributeName = "Type")]
	public ActivityType ActivityType { get; set; }

	[XmlAttribute(AttributeName = "Time")]
	public DateTime TimeStamp { get; set; }

	[XmlElement(IsNullable = false)]
	public string Id { get; set; }

	[XmlElement(ElementName = "Value", IsNullable = false)]
	public string LogValue { get; set; }
}
