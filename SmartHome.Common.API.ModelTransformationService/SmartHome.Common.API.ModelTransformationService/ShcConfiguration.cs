using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace SmartHome.Common.API.ModelTransformationService;

[Serializable]
public class ShcConfiguration
{
	public List<BaseDevice> BaseDevices { get; set; }

	public List<Location> Locations { get; set; }

	public List<LogicalDevice> LogicalDevices { get; set; }

	public List<Interaction> Interactions { get; set; }

	public List<Home> Homes { get; set; }

	public List<Member> Members { get; set; }

	public List<HomeSetup> HomeSetups { get; set; }

	public int RepositoryVersion { get; set; }

	public ShcConfiguration()
	{
		Locations = new List<Location>();
		BaseDevices = new List<BaseDevice>();
		LogicalDevices = new List<LogicalDevice>();
		Interactions = new List<Interaction>();
		Homes = new List<Home>();
		Members = new List<Member>();
		HomeSetups = new List<HomeSetup>();
	}
}
