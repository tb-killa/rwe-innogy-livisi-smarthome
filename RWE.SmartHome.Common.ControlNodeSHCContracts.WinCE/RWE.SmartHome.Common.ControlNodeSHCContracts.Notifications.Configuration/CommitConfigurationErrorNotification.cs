using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Configuration;

public class CommitConfigurationErrorNotification : BaseNotification
{
	[XmlAttribute]
	public int ConfigurationVersion { get; set; }

	[XmlAttribute]
	public Guid TransactionId { get; set; }

	[XmlAttribute]
	public string Message { get; set; }
}
