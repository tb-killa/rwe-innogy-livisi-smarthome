using System.Net;
using System.Reflection;
using WebServerHost.Web.Extensions;
using WebServerHost.Web.Http;

namespace WebServerHost.Web.Routing;

public class RequestRouter
{
	private readonly string endpointRouteBase;

	private EndpointRoutes endpoints;

	public EndpointRoutes Endpoints => endpoints;

	public RequestRouter(string endpointRouteBase)
	{
		this.endpointRouteBase = endpointRouteBase;
		endpoints = new EndpointRoutes();
	}

	internal ShcWebResponse RouteRequest(ShcWebRequest request)
	{
		ShcWebResponse result = null;
		if (request.RequestUri.StartsWith(endpointRouteBase))
		{
			string route = request.RequestUri.Substring(endpointRouteBase.Length, request.RequestUri.Length - endpointRouteBase.Length).Trim('/');
			string matchedTemplateRoute;
			MethodInfo mappedAction = endpoints.GetMappedAction(route, request.Method, out matchedTemplateRoute);
			if ((object)mappedAction != null)
			{
				object obj = null;
				ControllerActionInvoker controllerActionInvoker = new ControllerActionInvoker(mappedAction, request, route, matchedTemplateRoute);
				obj = controllerActionInvoker.Invoke();
				result = obj.AsIResult().ExecuteResult();
			}
			else
			{
				result = new ShcErrorResponse(HttpStatusCode.NotFound, string.Empty);
			}
		}
		return result;
	}
}
