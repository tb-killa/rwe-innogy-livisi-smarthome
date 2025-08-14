using System;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Constant
{
	[JsonProperty("value")]
	public object Value { get; set; }
}
