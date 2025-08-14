using JsonLite;

namespace SmartHome.Common.API.Entities.Login;

public class TokenResponse
{
	[JsonProperty("token_type")]
	public string TokenType
	{
		get
		{
			return "Bearer";
		}
		set
		{
		}
	}

	[JsonProperty("access_token")]
	public string AccessToken { get; set; }

	[JsonProperty("expires_in")]
	public long ExpiresIn { get; set; }

	[JsonProperty("refresh_token")]
	public string RefreshToken { get; set; }
}
