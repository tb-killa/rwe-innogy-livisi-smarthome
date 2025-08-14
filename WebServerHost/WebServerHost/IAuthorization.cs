using System.Collections.Generic;
using SmartHome.Common.API.Entities.Login;
using WebServerHost.Web.Http;

namespace WebServerHost;

public interface IAuthorization
{
	TokenResponse Authenticate(TokenRequestData tokenRequest);

	void Authorize(ShcWebRequest request);

	void Authorize(string jwt);

	TokenResponse RefreshAuthentication(TokenRequestData tokenRequest);

	void ValidateBasicAuthorization(Dictionary<string, string> requestHeaders);

	void InvalidateTokens();
}
