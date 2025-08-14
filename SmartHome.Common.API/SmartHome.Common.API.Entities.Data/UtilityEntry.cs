using System;
using JsonLite;

namespace SmartHome.Common.API.Entities.Data;

public class UtilityEntry
{
	[JsonProperty("vl")]
	public int value { get; set; }

	[JsonProperty("cv")]
	public double? calculatedValue { get; set; }

	[JsonProperty("ti")]
	public DateTime timestamp { get; set; }

	[JsonProperty("t")]
	public int? tariff { get; set; }

	[JsonProperty("p")]
	public int? percentage { get; set; }
}
