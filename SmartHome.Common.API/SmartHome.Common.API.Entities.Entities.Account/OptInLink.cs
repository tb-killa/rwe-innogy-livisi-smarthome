using System;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities.Account;

[Serializable]
public class OptInLink
{
	[JsonProperty("placeholder")]
	public string Placeholder { get; set; }

	[JsonProperty("url")]
	public string Url { get; set; }

	[JsonProperty("label")]
	public string Label { get; set; }
}
