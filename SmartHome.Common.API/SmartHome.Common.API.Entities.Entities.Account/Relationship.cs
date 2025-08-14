using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities.Account;

[Serializable]
public class Relationship
{
	[JsonProperty("accountName")]
	public string AccountName { get; set; }

	[JsonProperty("serialNumber")]
	public string SerialNumber { get; set; }

	[JsonProperty("config")]
	public List<Property> Config { get; set; }

	public override string ToString()
	{
		return $"AccountName: {AccountName}, SerialNumber: {SerialNumber}";
	}
}
