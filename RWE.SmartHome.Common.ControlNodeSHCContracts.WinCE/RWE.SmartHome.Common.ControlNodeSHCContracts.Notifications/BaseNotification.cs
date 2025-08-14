using System;
using System.Globalization;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;

public abstract class BaseNotification
{
	[XmlAttribute]
	public string Version { get; set; }

	[XmlAttribute]
	public Guid NotificationId { get; set; }

	[XmlAttribute]
	public int SequenceNumber { get; set; }

	[XmlIgnore]
	public DateTime Timestamp { get; set; }

	[XmlAttribute]
	public string Namespace { get; set; }

	[XmlAttribute("time")]
	public string TimestampStr
	{
		get
		{
			return Timestamp.ToString("o", CultureInfo.InvariantCulture);
		}
		set
		{
			Timestamp = DateTime.Parse(value, CultureInfo.InvariantCulture);
		}
	}

	protected BaseNotification()
	{
		Version = "3.00";
		NotificationId = Guid.NewGuid();
		Timestamp = DateTime.UtcNow;
	}
}
