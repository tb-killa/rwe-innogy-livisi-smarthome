using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class CustomTrigger
{
	[JsonProperty("type")]
	public string Type { get; set; }

	[JsonProperty("namespace")]
	public string Namespace { get; set; }

	[JsonProperty("source")]
	public string Link { get; set; }

	[JsonProperty("properties")]
	public List<Property> Properties { get; set; }

	[JsonProperty("tags")]
	public List<Property> Tags { get; set; }
}
