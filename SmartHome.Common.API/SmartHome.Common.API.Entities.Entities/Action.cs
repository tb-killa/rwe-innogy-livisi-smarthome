using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Action
{
	[JsonProperty("id")]
	public Guid? ActionId { get; set; }

	[JsonProperty("type")]
	public string Type { get; set; }

	[JsonProperty("namespace")]
	public string Namespace { get; set; }

	[JsonProperty("target")]
	public string Target { get; set; }

	[JsonProperty("params")]
	public List<Parameter> Data { get; set; }

	[JsonProperty("tags")]
	public List<Property> Tags { get; set; }

	public override string ToString()
	{
		return string.Format("Type: {0}, Target: {1}, Data count: {2}", Type, Target, (Data != null) ? Data.Count.ToString() : "null");
	}
}
