using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Condition
{
	[JsonProperty("leqp")]
	public Parameter Leqp { get; set; }

	[JsonProperty("reqp")]
	public Parameter Reqp { get; set; }

	[JsonProperty("operator")]
	public Parameter Operator { get; set; }

	[JsonProperty("tags")]
	public List<Property> Tags { get; set; }
}
