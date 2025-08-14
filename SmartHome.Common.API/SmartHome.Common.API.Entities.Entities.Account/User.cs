using System;
using System.Collections.Generic;
using JsonLite;

namespace SmartHome.Common.API.Entities.Entities.Account;

[Serializable]
public class User
{
	[JsonProperty("accountName")]
	public string AccountName { get; set; }

	[JsonProperty("password")]
	public string Password { get; set; }

	[JsonProperty("tenantId")]
	public string TenantId { get; set; }

	[JsonProperty("data")]
	public List<Property> Data { get; set; }

	public override string ToString()
	{
		return $"AccountName: {AccountName}, TenantId: {TenantId}";
	}
}
