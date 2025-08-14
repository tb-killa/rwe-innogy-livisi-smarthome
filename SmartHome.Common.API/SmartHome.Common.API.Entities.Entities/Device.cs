using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities;

[Serializable]
public class Device
{
	[JsonProperty("id")]
	public virtual string Id { get; set; }

	[JsonProperty("manufacturer")]
	public virtual string Manufacturer { get; set; }

	[JsonProperty("version")]
	public virtual string Version { get; set; }

	[JsonProperty("product")]
	public virtual string Product { get; set; }

	[JsonProperty("serialNumber")]
	public virtual string SerialNumber { get; set; }

	[JsonProperty("type")]
	public virtual string Type { get; set; }

	[JsonProperty("class")]
	public virtual string Class { get; set; }

	[JsonProperty("config")]
	public List<Property> Config { get; set; }

	[JsonProperty("tags")]
	public List<Property> Tags { get; set; }

	[JsonProperty("devices")]
	public List<string> Devices { get; set; }

	[JsonProperty("capabilities")]
	public List<string> Capabilities { get; set; }

	[JsonProperty("location")]
	public string Location { get; set; }

	[JsonProperty("volatile")]
	public List<Property> Volatile { get; set; }

	public Device()
	{
		Id = Guid.Empty.ToString("N");
		Manufacturer = "RWE";
		Version = "1.0";
		Product = "sh://Unknown.RWE";
		SerialNumber = "0000000000";
		Type = "Unknown";
	}

	public override string ToString()
	{
		return $"Id: {Id}, Manufacturer: {Manufacturer}, Version: {Version}, Product: {Product}, SerialNumber: {SerialNumber}, Type: {Type}";
	}
}
