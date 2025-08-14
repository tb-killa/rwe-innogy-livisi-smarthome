using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Function
{
	[JsonProperty("type")]
	public string Type { get; set; }

	[JsonProperty("params")]
	public List<Parameter> Parameters { get; set; }
}
