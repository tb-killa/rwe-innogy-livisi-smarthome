using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;

namespace RWE.SmartHome.SHC.CommonFunctionality;

[XmlType("DeviceActivityLogEntry")]
public class DeviceActivityLogEntry
{
	[XmlElement(ElementName = "DeviceId")]
	public string DeviceId { get; set; }

	[XmlElement(ElementName = "ActivityType")]
	public ActivityType ActivityType { get; set; }

	[XmlElement(ElementName = "Timestamp")]
	public DateTime Timestamp { get; set; }

	[XmlElement(ElementName = "NewState")]
	public string NewState { get; set; }

	public EventType EventType { get; set; }

	[XmlIgnore]
	public int EntryId { get; set; }

	public override string ToString()
	{
		return $"{Timestamp} Device with {DeviceId} goes to new state {NewState} - activity type {ActivityType.ToString()}";
	}
}
