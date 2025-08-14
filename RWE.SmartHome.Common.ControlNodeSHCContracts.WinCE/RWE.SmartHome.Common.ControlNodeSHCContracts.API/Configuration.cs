using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.API;

[XmlRoot(ElementName = "UIConfigurationContainer")]
public class Configuration
{
	public const string ActuatorContainerNodeName = "AcCt";

	public const string LocationNodeName = "LC";

	public const string LogicalDeviceNodeName = "LD";

	public const string BaseDeviceNodeName = "BD";

	public const string InteractionNodeName = "INT";

	public const string HomeNodeName = "HM";

	public const string HomeSetupNodeName = "HMS";

	public const string MemberNodeName = "Mmb";

	[XmlArrayItem(ElementName = "HM")]
	[XmlArray(ElementName = "HMs")]
	public List<Home> Homes { get; set; }

	[XmlArrayItem(ElementName = "HMS")]
	[XmlArray(ElementName = "HMSs")]
	public List<HomeSetup> HomeSetups { get; set; }

	[XmlArrayItem(ElementName = "Mmb")]
	[XmlArray(ElementName = "Mmbs")]
	public List<Member> Members { get; set; }

	[XmlArray(ElementName = "LCs")]
	[XmlArrayItem(ElementName = "LC")]
	public List<Location> Locations { get; set; }

	[XmlArray(ElementName = "LDs")]
	[XmlArrayItem(ElementName = "LD")]
	public List<LogicalDevice> LogicalDevices { get; set; }

	[XmlArrayItem(ElementName = "BD")]
	[XmlArray(ElementName = "BDs")]
	public List<BaseDevice> BaseDevices { get; set; }

	[XmlArray(ElementName = "INTs")]
	[XmlArrayItem(ElementName = "INT")]
	public List<Interaction> Interactions { get; set; }

	public int RepositoryVersion { get; set; }

	public bool DalEnabled { get; set; }

	public Configuration()
	{
		Locations = new List<Location>();
		BaseDevices = new List<BaseDevice>();
		LogicalDevices = new List<LogicalDevice>();
		Interactions = new List<Interaction>();
		Homes = new List<Home>();
		Members = new List<Member>();
		HomeSetups = new List<HomeSetup>();
	}

	public List<Location> GetLocations()
	{
		return Locations;
	}

	public List<BaseDevice> GetBaseDevices()
	{
		return BaseDevices;
	}

	public List<LogicalDevice> GetLogicalDevices()
	{
		return LogicalDevices;
	}

	public List<Member> GetMembers()
	{
		return Members;
	}

	public List<Home> GetHomes()
	{
		return Homes;
	}

	public List<HomeSetup> GetHomeSetups()
	{
		return HomeSetups;
	}

	public Location GetLocation(Guid locationId)
	{
		return Locations.SingleOrDefault((Location l) => l.Id == locationId);
	}

	public LogicalDevice GetLogicalDevice(Guid id)
	{
		return LogicalDevices.SingleOrDefault((LogicalDevice d) => d.Id == id);
	}

	public BaseDevice GetBaseDevice(Guid id)
	{
		return BaseDevices.SingleOrDefault((BaseDevice d) => d.Id == id);
	}

	public Member GetMember(Guid id)
	{
		return Members.SingleOrDefault((Member m) => m.Id == id);
	}

	public Home GetHome(Guid id)
	{
		return Homes.SingleOrDefault((Home h) => h.Id == id);
	}

	public HomeSetup GetHomeSetup(Guid id)
	{
		return HomeSetups.SingleOrDefault((HomeSetup h) => h.Id == id);
	}

	public string Validate()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("validation started");
		foreach (LogicalDevice logicalDevice in LogicalDevices)
		{
			if (logicalDevice.BaseDevice == null)
			{
				stringBuilder.AppendLine($"logical : {logicalDevice.Id} missing base {logicalDevice.BaseDeviceId}");
			}
			if (logicalDevice.Location == null)
			{
				stringBuilder.AppendLine($"logical : {logicalDevice.Id} missing location {logicalDevice.LocationId}");
			}
		}
		stringBuilder.AppendLine("validation end");
		return stringBuilder.ToString();
	}
}
