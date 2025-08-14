using System;
using System.Linq;
using System.Net;
using System.Reflection;
using JsonLite;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.Entities.Login;
using WebServerHost.Web;
using WebServerHost.Web.Extensions;
using WebServerHost.Web.Http;

namespace WebServerHost.Handlers;

public class AuthHandler : RequestHandler
{
	private readonly string authEndpointBase;

	public AuthHandler(string authEndpointBase, IAuthorization authorization)
	{
		this.authEndpointBase = authEndpointBase;
		base.Authorization = authorization;
	}

	public override ShcWebResponse HandleRequest(ShcWebRequest request)
	{
		ShcWebResponse result = null;
		if (request.RequestUri.StartsWith(authEndpointBase))
		{
			string text = request.RequestUri.Substring(authEndpointBase.Length, request.RequestUri.Length - authEndpointBase.Length).Trim('/');
			result = ((!(text == "token")) ? new ShcErrorResponse(HttpStatusCode.NotFound, "") : HandleTokenRequest(request));
		}
		return result;
	}

	private ShcWebResponse HandleTokenRequest(ShcWebRequest request)
	{
		try
		{
			TokenRequestData tokenFromRequest = GetTokenFromRequest(request);
			if (tokenFromRequest.Grant != null)
			{
				if (tokenFromRequest.Grant == GrantType.password.ToString())
				{
					ServiceProvider.Services.Get<IRegistrationService>();
					base.Authorization.ValidateBasicAuthorization(request.Headers);
					TokenResponse obj = base.Authorization.Authenticate(tokenFromRequest);
					return new ShcRestResponse(HttpStatusCode.OK, obj.ToJson());
				}
				if (tokenFromRequest.Grant == GrantType.refresh_token.ToString())
				{
					ServiceProvider.Services.Get<IRegistrationService>();
					base.Authorization.ValidateBasicAuthorization(request.Headers);
					TokenResponse obj = base.Authorization.RefreshAuthentication(tokenFromRequest);
					return new ShcRestResponse(HttpStatusCode.OK, obj.ToJson());
				}
				throw new ApiException(ErrorCode.ShcOperationError, "Invalid grant type");
			}
			throw new ApiException(ErrorCode.ShcOperationError, "Missing \"grant_type\"");
		}
		catch (NullReferenceException)
		{
			return new ShcRestResponse(HttpStatusCode.UnsupportedMediaType, "");
		}
		catch (FormatException ex2)
		{
			return new RestApiErrorResponse(new ApiException(ErrorCode.EntityMalformedContent, "Malformed Token entity", ex2.Message));
		}
		catch (ApiException apiError)
		{
			return new RestApiErrorResponse(apiError);
		}
	}

	private TokenRequestData GetTokenFromRequest(ShcWebRequest request)
	{
		string headerValue = request.GetHeaderValue("Content-Type");
		if (!string.IsNullOrEmpty(headerValue))
		{
			if (headerValue == "application/json")
			{
				return request.RequestContent.FromJson<TokenRequestData>();
			}
			if (headerValue == "application/x-www-form-urlencoded")
			{
				string[] source = request.RequestContent.Split('&');
				PropertyInfo[] properties = typeof(TokenRequestData).GetProperties();
				TokenRequestData tokenRequestData = new TokenRequestData();
				PropertyInfo[] array = properties;
				PropertyInfo property;
				for (int i = 0; i < array.Length; i++)
				{
					property = array[i];
					string text = source.FirstOrDefault((string kv) => kv.StartsWith(GetName(property)));
					if (!string.IsNullOrEmpty(text))
					{
						string[] array2 = Uri.UnescapeDataString(text).Split('=');
						if (array2.Length == 2)
						{
							string value = array2[1];
							property.SetValue(tokenRequestData, value, null);
						}
					}
				}
				return tokenRequestData;
			}
		}
		return null;
	}

	private string GetName(PropertyInfo property)
	{
		JsonPropertyAttribute attribute = property.GetAttribute<JsonPropertyAttribute>();
		if (attribute != null)
		{
			return attribute.Name;
		}
		return property.Name;
	}

	private void AddHeaders(ShcWebResponse response)
	{
		response.Headers["Content-Type"] = "application/json;charset=UTF-8";
		response.Headers["Cache-Control"] = "no-store";
		response.Headers["Pragma"] = "no-cache";
	}
}
