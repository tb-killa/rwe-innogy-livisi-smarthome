using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class ActionResponse
{
	[JsonProperty("type")]
	public string Type { get; set; }

	[JsonProperty("resultCode")]
	public string ResultCode { get; set; }

	[JsonProperty("target")]
	public string Target { get; set; }

	[JsonProperty("namespace")]
	public string Namespace { get; set; }

	[JsonProperty("properties")]
	public List<Property> Data { get; set; }
}
