using System;
using JsonLite;
using SmartHome.Common.API.Entities.Enumerations;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class ProductParameter
{
	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("value")]
	public string Value { get; set; }

	[JsonProperty("type")]
	public ParameterType? ParameterType { get; set; }
}
