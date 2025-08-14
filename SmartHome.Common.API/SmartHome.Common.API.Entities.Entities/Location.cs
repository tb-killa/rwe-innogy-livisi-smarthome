using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Location
{
	[JsonProperty("id")]
	public string Id { get; set; }

	[JsonProperty("config")]
	public List<Property> Config { get; set; }

	[JsonProperty("tags")]
	public List<Property> Tags { get; set; }
}
