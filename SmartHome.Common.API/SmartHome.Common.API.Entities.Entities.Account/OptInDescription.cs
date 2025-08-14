using System;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities.Account;

[Serializable]
public class OptInDescription
{
	[JsonProperty("culture")]
	public string Culture { get; set; }

	[JsonProperty("text")]
	public string Text { get; set; }

	[JsonProperty("linkUrl")]
	public string LinkUrl { get; set; }

	[JsonProperty("linkLabel")]
	public string LinkLabel { get; set; }
}
