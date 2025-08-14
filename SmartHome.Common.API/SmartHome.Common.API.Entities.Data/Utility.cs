using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Data;

public class Utility
{
	public string Type { get; set; }

	[JsonProperty("meterid")]
	public string MeterId { get; set; }

	public List<UtilityEntry> Data { get; set; }
}
