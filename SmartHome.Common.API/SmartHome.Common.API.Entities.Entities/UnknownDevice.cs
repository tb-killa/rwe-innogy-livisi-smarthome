using System;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class UnknownDevice : Device
{
	[JsonProperty("id")]
	public override string Id => Guid.Empty.ToString("N");

	[JsonProperty("manufacturer")]
	public override string Manufacturer => "RWE";

	[JsonProperty("version")]
	public override string Version => "1.0";

	[JsonProperty("product")]
	public override string Product => "sh://Unknown.RWE";

	[JsonProperty("serialNumber")]
	public override string SerialNumber => "0000000000";

	[JsonProperty("type")]
	public override string Type => "Unknown";
}
