using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Configuration;

public class ConfigurationSaveProgressNotification : BaseNotification
{
	[XmlAttribute]
	public SaveConfigurationState State { get; set; }

	[XmlAttribute]
	public Guid TransactionId { get; set; }
}
