using System;
using System.Collections.Generic;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.Entities.Cache;

[Serializable]
public class EntityCache
{
	public List<Home> Homes { get; set; }

	public List<Member> Members { get; set; }

	public List<HomeSetup> HomeSetups { get; set; }

	public List<Device> Devices { get; set; }

	public List<Capability> Capabilities { get; set; }

	public List<Interaction> Interactions { get; set; }

	public List<Location> Locations { get; set; }

	public int RepositoryVersion { get; set; }
}
