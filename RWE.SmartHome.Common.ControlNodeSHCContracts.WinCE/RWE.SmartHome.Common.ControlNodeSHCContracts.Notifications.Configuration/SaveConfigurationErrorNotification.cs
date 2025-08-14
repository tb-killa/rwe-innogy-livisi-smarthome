using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Configuration;

public class SaveConfigurationErrorNotification : BaseNotification
{
	[XmlAttribute]
	public int ConfigurationVersion { get; set; }

	[XmlAttribute]
	public SaveConfigurationError ErrorReason { get; set; }

	public List<ErrorEntry> ErrorEntries { get; set; }

	public SaveConfigurationErrorNotification()
	{
		ErrorEntries = new List<ErrorEntry>();
	}
}
