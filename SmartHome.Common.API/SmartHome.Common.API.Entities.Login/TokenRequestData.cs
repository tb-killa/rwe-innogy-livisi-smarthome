using JsonLite;

namespace SmartHome.Common.API.Entities.Login;

public class TokenRequestData
{
	[JsonProperty("username")]
	public string Username { get; set; }

	[JsonProperty("password")]
	public string Password { get; set; }

	[JsonProperty("grant_type")]
	public string Grant { get; set; }

	[JsonProperty("scope")]
	public string Scope { get; set; }

	[JsonProperty("refresh_token")]
	public string RefreshToken { get; set; }
}
