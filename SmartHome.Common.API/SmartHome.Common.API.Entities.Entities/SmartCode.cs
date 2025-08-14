using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class SmartCode
{
	[JsonProperty("id")]
	public int? Id { get; set; }

	[JsonProperty("createdBy")]
	public string CreatedBy { get; set; }

	[JsonProperty("pin")]
	public string Pin { get; set; }

	[JsonProperty("state")]
	public string State { get; set; }

	[JsonProperty("productPackage")]
	public string ProductPackage { get; set; }

	[JsonProperty("creationDate")]
	public DateTime? CreationDate { get; set; }

	[JsonProperty("blacklistDate")]
	public DateTime? BlacklistDate { get; set; }

	[JsonProperty("redeemDate")]
	public DateTime? RedeemDate { get; set; }

	[JsonProperty("properties")]
	public List<Property> Properties { get; set; }

	[JsonProperty("salesDetails")]
	public List<Property> SalesDetails { get; set; }

	[JsonProperty("items")]
	public List<ProductPackageItem> Items { get; set; }
}
