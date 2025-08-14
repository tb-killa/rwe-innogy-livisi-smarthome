using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Home
{
	[JsonProperty("id")]
	public virtual string Id { get; set; }

	[JsonProperty("members")]
	public virtual List<string> Members { get; set; }

	[JsonProperty("config")]
	public List<Property> Config { get; set; }

	[JsonProperty("homeSetup")]
	public virtual string HomeSetup { get; set; }

	[JsonProperty("tags")]
	public List<Property> Tags { get; set; }

	public override string ToString()
	{
		return $"Id: {Id}";
	}
}
