using System;
using System.Collections.Generic;
using System.Net;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.Entities.Extensions;
using WebServerHost.Web.Http;
using WebServerHost.Web.Routing;

namespace WebServerHost.Handlers;

public class ApiHandler : RequestHandler
{
	private const string passwordChange = "password/change";

	private const string passwordChangeActive = "password/change/active";

	private const string passwordReset = "password/reset";

	private readonly string endpointBase;

	private RequestRouter router;

	public ApiHandler(string endpointBase, Action<EndpointRoutes> endpoints)
	{
		this.endpointBase = endpointBase;
		router = new RequestRouter(endpointBase);
		endpoints(router.Endpoints);
	}

	public override ShcWebResponse HandleRequest(ShcWebRequest request)
	{
		try
		{
			if (request.RequestUri.StartsWith(endpointBase))
			{
				AuthorizeRequest(request);
				return router.RouteRequest(request);
			}
			return null;
		}
		catch (ApiException apiError)
		{
			return new RestApiErrorResponse(apiError);
		}
		catch (Exception ex)
		{
			if (ex is ArgumentException)
			{
				Error error = new Error();
				error.ErrorCode = 1005;
				error.Description = ErrorCode.InvalidArgument.GetDescription();
				error.Messages = new List<string> { ex.Message };
				return new RestApiErrorResponse(error);
			}
			if (ex is InvalidCastException || ex is FormatException)
			{
				Error error2 = new Error();
				error2.ErrorCode = 3001;
				error2.Description = ErrorCode.EntityMalformedContent.GetDescription();
				error2.Messages = new List<string> { ex.Message };
				return new RestApiErrorResponse(error2);
			}
			return new ShcErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
		}
	}

	private void AuthorizeRequest(ShcWebRequest request)
	{
		if (request.RequestUri.Contains("password/change") || request.RequestUri.Contains("password/change/active") || request.RequestUri.Contains("password/reset"))
		{
			base.Authorization.ValidateBasicAuthorization(request.Headers);
		}
		else
		{
			Authorize(request);
		}
	}
}
