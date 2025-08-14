using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class HomeSetup
{
	[JsonProperty("id")]
	public virtual string Id { get; set; }

	[JsonIgnore]
	public virtual string HomeId { get; set; }

	[JsonProperty("config")]
	public List<Property> Config { get; set; }

	[JsonProperty("tags")]
	public List<Property> Tags { get; set; }

	public override string ToString()
	{
		return $"Id: {Id}";
	}
}
