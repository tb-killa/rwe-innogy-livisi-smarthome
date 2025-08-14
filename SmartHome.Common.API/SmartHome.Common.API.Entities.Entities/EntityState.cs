using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class EntityState
{
	[JsonProperty("id")]
	public string Id { get; set; }

	[JsonProperty("state")]
	public List<Property> State { get; set; }
}
