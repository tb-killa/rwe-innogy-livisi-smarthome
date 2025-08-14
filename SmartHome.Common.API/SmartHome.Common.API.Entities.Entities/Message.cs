using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Message
{
	[JsonProperty("id")]
	public virtual string Id { get; set; }

	[JsonProperty("type")]
	public virtual string Type { get; set; }

	[JsonProperty("read")]
	public virtual bool Read { get; set; }

	[JsonProperty("namespace")]
	public string Namespace { get; set; }

	[JsonProperty("class")]
	public virtual string Class { get; set; }

	[JsonProperty("timestamp")]
	public virtual DateTime Timestamp { get; set; }

	[JsonProperty("devices")]
	public List<string> Devices { get; set; }

	[JsonProperty("capabilities")]
	public List<string> Capabilities { get; set; }

	[JsonProperty("properties")]
	public List<Property> Properties { get; set; }

	[JsonProperty("tags")]
	public List<Property> Tags { get; set; }
}
