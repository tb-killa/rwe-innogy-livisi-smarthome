using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class ProductPackageItem
{
	[JsonProperty("productType")]
	public string ProductType { get; set; }

	[JsonProperty("params")]
	public List<ProductParameter> Parameters { get; set; }

	[JsonProperty("result")]
	public string Result { get; set; }
}
