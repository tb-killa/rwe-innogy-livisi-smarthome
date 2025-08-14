using System;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Parameter
{
	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("type")]
	public string Type { get; set; }

	[JsonProperty("constant")]
	public Constant Constant { get; set; }

	[JsonProperty("function")]
	public Function Function { get; set; }
}
