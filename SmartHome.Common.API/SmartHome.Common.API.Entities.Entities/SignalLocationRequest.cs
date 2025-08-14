using System;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class SignalLocationRequest
{
	[JsonProperty("deviceId")]
	public string DeviceId { get; set; }

	[JsonProperty("location")]
	public string Location { get; set; }

	[JsonProperty("direction")]
	public string Direction { get; set; }
}
