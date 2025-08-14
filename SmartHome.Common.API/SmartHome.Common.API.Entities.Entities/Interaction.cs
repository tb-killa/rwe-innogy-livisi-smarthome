using System;
using System.Collections.Generic;
using System.Linq;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Interaction
{
	[JsonProperty("id")]
	public string Id { get; set; }

	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("created")]
	public DateTime Created { get; set; }

	[JsonProperty("modified")]
	public DateTime Modified { get; set; }

	[JsonProperty("validFrom")]
	public DateTime? ValidFrom { get; set; }

	[JsonProperty("validTo")]
	public DateTime? ValidTo { get; set; }

	[JsonProperty("freezeTime")]
	public int? Freezetime { get; set; }

	[JsonProperty("isInternal")]
	public bool? IsInternal { get; set; }

	[JsonProperty("rules")]
	public List<Rule> Rules { get; set; }

	[JsonProperty("tags")]
	public List<Property> Tags { get; set; }

	public List<Action> ExtractActions()
	{
		if (Rules == null)
		{
			return new List<Action>();
		}
		return Rules.SelectMany((Rule r) => r.Actions).ToList();
	}
}
