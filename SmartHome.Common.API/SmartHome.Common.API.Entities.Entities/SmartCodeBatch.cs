using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class SmartCodeBatch
{
	[JsonProperty("productPackage")]
	public string ProductPackage { get; set; }

	[JsonProperty("count")]
	public int Count { get; set; }

	[JsonProperty("salesDetails")]
	public List<Property> SalesDetails { get; set; }

	[JsonProperty("properties")]
	public List<Property> Properties { get; set; }

	[JsonProperty("codes")]
	public SmartCode[] SmartCodes { get; set; }
}
