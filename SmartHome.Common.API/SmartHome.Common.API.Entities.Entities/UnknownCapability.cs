using System;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class UnknownCapability : Capability
{
	[JsonProperty("id")]
	public override string Id => Guid.Empty.ToString("N");

	[JsonProperty("type")]
	public override string Type => "Unknown";
}
