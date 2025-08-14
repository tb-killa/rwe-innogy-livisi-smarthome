using System;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Link
{
	[JsonProperty("value")]
	public string Value { get; set; }

	public override string ToString()
	{
		return $"[Value: {Value}]";
	}
}
