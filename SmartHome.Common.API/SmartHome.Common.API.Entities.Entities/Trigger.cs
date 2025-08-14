using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Trigger
{
	[JsonProperty("type")]
	public string Type { get; set; }

	[JsonProperty("eventType")]
	public string EventType { get; set; }

	[JsonProperty("source")]
	public string Link { get; set; }

	[JsonProperty("conditions")]
	public List<Condition> Conditions { get; set; }

	[JsonProperty("tags")]
	public List<Property> Tags { get; set; }
}
