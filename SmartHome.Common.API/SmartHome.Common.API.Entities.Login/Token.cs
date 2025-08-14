using System;
using JsonLite;

namespace SmartHome.Common.API.Entities.Login;

public class Token
{
	[JsonProperty("aud")]
	public string audiance;

	[JsonProperty("exp")]
	public long expirationTime;

	[JsonProperty("iat")]
	public long issueTime;

	[JsonProperty("iss")]
	public string issuedBy;

	[JsonProperty("jti")]
	public Guid uniqueIdentifier;

	[JsonProperty("sub")]
	public string subject;

	[JsonProperty("user_permissions")]
	public string userPermisins;

	[JsonProperty("device")]
	public string device;
}
