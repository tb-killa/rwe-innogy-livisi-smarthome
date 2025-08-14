using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities.Account;

[Serializable]
public class Friend
{
	[JsonProperty("serialNumber")]
	public string SerialNumber { get; set; }

	[JsonProperty("accountName")]
	public string AccountName { get; set; }

	[JsonProperty("config")]
	public List<Property> Config { get; set; }

	public override string ToString()
	{
		return $"Friend [AccountName: {AccountName}]";
	}
}
