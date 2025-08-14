using System;
using JsonLite;

namespace SmartHome.Common.API.Entities.ClientData;

[Serializable]
public class ClientData
{
	[JsonProperty("partition")]
	public string Partition { get; set; }

	[JsonProperty("key")]
	public string Key { get; set; }

	[JsonProperty("cached")]
	public string Cached { get; set; }

	[JsonProperty("value")]
	public string Value { get; set; }

	[JsonProperty("modifiedTime")]
	public DateTime ModifiedTime { get; set; }
}
