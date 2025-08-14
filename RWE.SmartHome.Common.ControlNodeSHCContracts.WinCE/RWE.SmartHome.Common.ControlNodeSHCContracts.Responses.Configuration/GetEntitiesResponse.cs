using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Configuration;

public class GetEntitiesResponse : BaseResponse
{
	[XmlAttribute]
	public int ConfigurationVersion { get; set; }

	[XmlArrayItem(ElementName = "LC")]
	[XmlArray(ElementName = "LCs")]
	public List<Location> Locations { get; set; }

	[XmlArray(ElementName = "BDs")]
	[XmlArrayItem(ElementName = "BD")]
	public List<BaseDevice> BaseDevices { get; set; }

	[XmlArray(ElementName = "LDs")]
	[XmlArrayItem(ElementName = "LD")]
	public List<LogicalDevice> LogicalDevices { get; set; }

	public List<Interaction> Interactions { get; set; }

	[XmlArray(ElementName = "HMs")]
	[XmlArrayItem(ElementName = "HM")]
	public List<Home> Homes { get; set; }

	[XmlArray(ElementName = "HMSs")]
	[XmlArrayItem(ElementName = "HMS")]
	public List<HomeSetup> HomeSetups { get; set; }

	[XmlArrayItem(ElementName = "Mmb")]
	[XmlArray(ElementName = "Mmbs")]
	public List<Member> Members { get; set; }

	public GetEntitiesResponse()
	{
		Locations = new List<Location>();
		BaseDevices = new List<BaseDevice>();
		LogicalDevices = new List<LogicalDevice>();
		Interactions = new List<Interaction>();
		Homes = new List<Home>();
		HomeSetups = new List<HomeSetup>();
		Members = new List<Member>();
	}
}
