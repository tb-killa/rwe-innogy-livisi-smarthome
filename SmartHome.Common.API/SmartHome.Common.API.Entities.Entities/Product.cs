using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Product
{
	[JsonProperty("type")]
	public string Type { get; set; }

	[JsonProperty("version")]
	public string Version { get; set; }

	[JsonProperty("provisioned")]
	public bool Provisioned { get; set; }

	[JsonProperty("config")]
	public List<Property> Config { get; set; }

	[JsonProperty("state")]
	public List<Property> State { get; set; }
}
