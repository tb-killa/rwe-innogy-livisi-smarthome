using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Configuration;

public class EntitiesUpdateResponse : BaseResponse
{
	[XmlAttribute]
	public int ConfigurationVersion { get; set; }

	[XmlArray(ElementName = "LCs")]
	[XmlArrayItem(ElementName = "LC")]
	public List<Location> Locations { get; set; }

	[XmlArrayItem(ElementName = "BD")]
	[XmlArray(ElementName = "BDs")]
	public List<BaseDevice> BaseDevices { get; set; }

	[XmlArrayItem(ElementName = "LD")]
	[XmlArray(ElementName = "LDs")]
	public List<LogicalDevice> LogicalDevices { get; set; }

	public List<Interaction> Interactions { get; set; }

	[XmlArrayItem(ElementName = "Mmb")]
	[XmlArray(ElementName = "Mmbs")]
	public List<Member> Members { get; set; }

	[XmlArray(ElementName = "HMs")]
	[XmlArrayItem(ElementName = "HM")]
	public List<Home> Homes { get; set; }

	[XmlArray(ElementName = "HMSs")]
	[XmlArrayItem(ElementName = "HMS")]
	public List<HomeSetup> HomeSetups { get; set; }

	[XmlArray(ElementName = "DtEts")]
	[XmlArrayItem(ElementName = "EMd")]
	public List<EntityMetadata> DeletedEntities { get; set; }

	public EntitiesUpdateResponse()
	{
		Locations = new List<Location>();
		BaseDevices = new List<BaseDevice>();
		LogicalDevices = new List<LogicalDevice>();
		Interactions = new List<Interaction>();
		Members = new List<Member>();
		Homes = new List<Home>();
		HomeSetups = new List<HomeSetup>();
		DeletedEntities = new List<EntityMetadata>();
	}
}
