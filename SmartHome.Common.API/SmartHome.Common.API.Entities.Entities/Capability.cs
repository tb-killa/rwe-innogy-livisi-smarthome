using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Capability
{
	[JsonProperty("id")]
	public virtual string Id { get; set; }

	[JsonProperty("type")]
	public virtual string Type { get; set; }

	[JsonProperty("class")]
	public virtual string Class { get; set; }

	[JsonProperty("traits")]
	public virtual List<string> Traits { get; set; }

	[JsonProperty("device")]
	public virtual string Device { get; set; }

	[JsonProperty("location")]
	public string Location { get; set; }

	[JsonProperty("config")]
	public List<Property> Config { get; set; }

	[JsonProperty("tags")]
	public List<Property> Tags { get; set; }

	public Capability()
	{
		Id = Guid.Empty.ToString("N");
		Type = "Unknown";
	}

	public override string ToString()
	{
		return $"Id: {Id}, Type: {Type}, Device: {Device}";
	}
}
