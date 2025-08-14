using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities.Account;

[Serializable]
public class OptIn
{
	[JsonProperty("id")]
	public int? Id { get; set; }

	[JsonProperty("partner")]
	public string Partner { get; set; }

	[JsonProperty("accepted")]
	public bool Accepted { get; set; }

	[JsonProperty("descriptions")]
	public List<OptInDescription> Descriptions { get; set; }
}
