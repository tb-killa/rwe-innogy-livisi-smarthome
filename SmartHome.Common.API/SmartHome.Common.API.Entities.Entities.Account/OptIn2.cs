using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities.Account;

[Serializable]
public class OptIn2
{
	[JsonProperty("id")]
	public int? Id { get; set; }

	[JsonProperty("accepted")]
	public bool Accepted { get; set; }

	[JsonProperty("type")]
	public string Type { get; set; }

	[JsonProperty("descriptions")]
	public List<OptInTextWithLinks> Descriptions { get; set; }
}
