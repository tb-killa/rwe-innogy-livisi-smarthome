using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Configuration;

public class SaveConfigurationNotification : BaseNotification
{
	[XmlAttribute]
	public int ConfigurationVersion { get; set; }

	[XmlArrayItem(ElementName = "LC")]
	[XmlArray(ElementName = "LCs")]
	public List<Location> Locations { get; set; }

	[XmlArray(ElementName = "BDs")]
	[XmlArrayItem(ElementName = "BD")]
	public List<BaseDevice> BaseDevices { get; set; }

	[XmlArrayItem(ElementName = "LD")]
	[XmlArray(ElementName = "LDs")]
	public List<LogicalDevice> LogicalDevices { get; set; }

	public List<Interaction> Interactions { get; set; }

	public List<Home> Homes { get; set; }

	public List<HomeSetup> HomeSetups { get; set; }

	public List<Member> Members { get; set; }

	[XmlArray(ElementName = "DtEts")]
	[XmlArrayItem(ElementName = "EMd")]
	public List<EntityMetadata> DeletedEntities { get; set; }

	public SaveConfigurationNotification()
	{
		Locations = new List<Location>();
		BaseDevices = new List<BaseDevice>();
		LogicalDevices = new List<LogicalDevice>();
		Interactions = new List<Interaction>();
		Homes = new List<Home>();
		HomeSetups = new List<HomeSetup>();
		Members = new List<Member>();
		DeletedEntities = new List<EntityMetadata>();
	}
}
