using System;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class GatewayStatus
{
	[JsonProperty("serialNumber")]
	public string SerialNumber { get; set; }

	[JsonProperty("connected")]
	public bool? Connected { get; set; }

	[JsonProperty("appVersion")]
	public string AppVersion { get; set; }

	[JsonProperty("osVersion")]
	public string OsVersion { get; set; }

	[JsonProperty("configVersion")]
	public string ConfigVersion { get; set; }

	[JsonProperty("controllerType")]
	public string ControllerType { get; set; }
}
