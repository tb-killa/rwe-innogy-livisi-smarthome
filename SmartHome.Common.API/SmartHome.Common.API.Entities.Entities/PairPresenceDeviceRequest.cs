using System;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class PairPresenceDeviceRequest
{
	[JsonProperty("deviceId")]
	public string DeviceId { get; set; }

	[JsonProperty("deviceInfo")]
	public string DeviceInfo { get; set; }
}
