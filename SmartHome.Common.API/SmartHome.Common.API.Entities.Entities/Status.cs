using System;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Status
{
	[JsonProperty("gateway")]
	public GatewayStatus GatewayStatus { get; set; }
}
