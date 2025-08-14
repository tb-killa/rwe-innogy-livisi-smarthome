using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class ApiShcConfiguration
{
	[JsonProperty("devices")]
	public List<Device> Devices { get; set; }

	[JsonProperty("locations")]
	public List<Location> Locations { get; set; }

	[JsonProperty("capabilities")]
	public List<Capability> Capabilities { get; set; }

	[JsonProperty("interactions")]
	public List<Interaction> Interactions { get; set; }

	[JsonProperty("homes")]
	public List<Home> Homes { get; set; }

	[JsonProperty("members")]
	public List<Member> Members { get; set; }

	[JsonProperty("homeSetups")]
	public List<HomeSetup> HomeSetups { get; set; }

	[JsonProperty("deleted")]
	public List<string> Deleted { get; set; }

	[JsonProperty("configVersion")]
	public int ConfigVersion { get; set; }
}
