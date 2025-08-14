using JsonLite;

namespace SmartHome.Common.API.Entities.Entities.Account;

public class PasswordChange
{
	[JsonProperty("oldPassword")]
	public string OldPassword { get; set; }

	[JsonProperty("newPassword")]
	public string NewPassword { get; set; }
}
