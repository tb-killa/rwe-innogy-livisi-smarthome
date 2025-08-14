using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WebServerHost.Web.Extensions;

namespace WebServerHost.Web.Routing;

public class EndpointRoutes
{
	private Dictionary<string, MethodInfo> getRoutes = new Dictionary<string, MethodInfo>();

	private Dictionary<string, MethodInfo> postRoutes = new Dictionary<string, MethodInfo>();

	private Dictionary<string, MethodInfo> putRoutes = new Dictionary<string, MethodInfo>();

	private Dictionary<string, MethodInfo> patchRoutes = new Dictionary<string, MethodInfo>();

	private Dictionary<string, MethodInfo> deleteRoutes = new Dictionary<string, MethodInfo>();

	public EndpointRoutes MapController<T>() where T : Controller
	{
		return MapController(typeof(T));
	}

	public EndpointRoutes MapControllers()
	{
		List<Type> list = (from t in Assembly.GetExecutingAssembly().GetTypes()
			where !t.IsAbstract && t.IsSubclassOf(typeof(Controller))
			select t).ToList();
		list.ForEach(delegate(Type c)
		{
			MapController(c);
		});
		return this;
	}

	public EndpointRoutes MapControllers(string @namespace)
	{
		List<Type> list = (from t in Assembly.GetExecutingAssembly().GetTypes()
			where !t.IsAbstract && t.IsSubclassOf(typeof(Controller)) && t.Namespace == @namespace
			select t).ToList();
		list.ForEach(delegate(Type c)
		{
			MapController(c);
		});
		return this;
	}

	public EndpointRoutes MapController(Type controllerType)
	{
		if (controllerType.IsSubclassOf(typeof(Controller)))
		{
			ICollection<RouteAttribute> allAttributes = controllerType.GetAllAttributes<RouteAttribute>();
			if (!allAttributes.Any())
			{
				allAttributes.Add(new RouteAttribute(GetControllerName(controllerType)));
			}
			{
				foreach (RouteAttribute item in allAttributes)
				{
					MapControllerActions(controllerType, item);
				}
				return this;
			}
		}
		throw new InvalidOperationException($"Type {controllerType.Name} is not a subclass for Controller");
	}

	private string GetControllerName(Type controllerType)
	{
		if (controllerType.Name.EndsWith("Controller"))
		{
			return controllerType.Name.Substring(0, controllerType.Name.Length - "Controller".Length).ToLower();
		}
		return controllerType.Name.ToLower();
	}

	private void MapControllerActions(Type controllerType, RouteAttribute controllerRoute)
	{
		IEnumerable<MethodInfo> enumerable = from m in controllerType.GetMethods()
			where m.GetCustomAttributes(typeof(RouteAttribute), inherit: false).Any()
			select m;
		foreach (MethodInfo action in enumerable)
		{
			IEnumerable<RouteAttribute> enumerable2 = action.GetCustomAttributes(inherit: false).OfType<RouteAttribute>();
			List<HttpMethodAttribute> list = action.GetAllAttributesOf<HttpMethodAttribute>().ToList();
			foreach (RouteAttribute item in enumerable2)
			{
				StringBuilder route = new StringBuilder(controllerRoute.template);
				if (item.template != string.Empty)
				{
					route.Append('/').Append(item.template);
				}
				if (list.Any())
				{
					list.ForEach(delegate(HttpMethodAttribute method)
					{
						MapEndpoint(route.ToString(), action, method.Method);
					});
				}
				else
				{
					MapEndpoint(route.ToString(), action);
				}
			}
		}
	}

	public EndpointRoutes MapEndpoint(string template, MethodInfo action)
	{
		return MapEndpoint(template, action, "GET").MapEndpoint(template, action, "POST").MapEndpoint(template, action, "PUT").MapEndpoint(template, action, "PATCH")
			.MapEndpoint(template, action, "DELETE");
	}

	public EndpointRoutes MapEndpoint(string template, MethodInfo action, string method)
	{
		Dictionary<string, MethodInfo> routesMapping = GetRoutesMapping(method);
		if (routesMapping.ContainsKey(template))
		{
			throw new InvalidOperationException($"Endpoint routes abiguity: {method}, {template}, cannot map more then one enpoint to same route");
		}
		routesMapping.Add(template, action);
		return this;
	}

	private Dictionary<string, MethodInfo> GetRoutesMapping(string method)
	{
		return method switch
		{
			"GET" => getRoutes, 
			"POST" => postRoutes, 
			"PUT" => putRoutes, 
			"PATCH" => patchRoutes, 
			"DELETE" => deleteRoutes, 
			_ => throw new ArgumentException($"Argument 'method' = \"{method}\", is not a valid or supported HTTP method"), 
		};
	}

	internal MethodInfo GetMappedAction(string route, out string matchedTemplateRoute)
	{
		MethodInfo mappedAction = GetMappedAction(route, "GET", out matchedTemplateRoute);
		if ((object)mappedAction != null)
		{
			return mappedAction;
		}
		mappedAction = GetMappedAction(route, "POST", out matchedTemplateRoute);
		if ((object)mappedAction != null)
		{
			return mappedAction;
		}
		mappedAction = GetMappedAction(route, "PUT", out matchedTemplateRoute);
		if ((object)mappedAction != null)
		{
			return mappedAction;
		}
		mappedAction = GetMappedAction(route, "PATCH", out matchedTemplateRoute);
		if ((object)mappedAction != null)
		{
			return mappedAction;
		}
		mappedAction = GetMappedAction(route, "DELETE", out matchedTemplateRoute);
		if ((object)mappedAction != null)
		{
			return mappedAction;
		}
		return null;
	}

	public MethodInfo GetMappedAction(string route, string httpMethod, out string matchedTemplateRoute)
	{
		MethodInfo result = null;
		matchedTemplateRoute = null;
		Dictionary<string, MethodInfo> routesMapping = GetRoutesMapping(httpMethod);
		string[] routeParts = route.SplitRouteParts();
		int num = 0;
		foreach (string key in routesMapping.Keys)
		{
			string[] templateParts = key.SplitRouteParts();
			int num2 = Matches(routeParts, templateParts);
			if (num2 > num)
			{
				matchedTemplateRoute = key;
				result = routesMapping[key];
				num = num2;
			}
		}
		return result;
	}

	private static int Matches(string[] routeParts, string[] templateParts)
	{
		int num = 0;
		if (templateParts.Length == routeParts.Length)
		{
			num++;
			for (int i = 0; i < templateParts.Length; i++)
			{
				if (!templateParts[i].IsParamTemplateRoutePart())
				{
					if (templateParts[i] != routeParts[i])
					{
						num = -1;
						break;
					}
					num++;
				}
			}
		}
		return num;
	}
}
