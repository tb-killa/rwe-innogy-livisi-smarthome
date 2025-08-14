using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities.Account;

[Serializable]
public class OptInTextWithLinks
{
	[JsonProperty("lang")]
	public string Culture { get; set; }

	[JsonProperty("text")]
	public string Text { get; set; }

	[JsonProperty("links")]
	public List<OptInLink> Links { get; set; }
}
