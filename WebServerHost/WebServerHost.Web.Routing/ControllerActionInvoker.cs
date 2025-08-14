using System;
using System.Collections.Generic;
using System.Reflection;
using SmartHome.Common.API.Entities.Extensions;
using WebServerHost.Web.Extensions;
using WebServerHost.Web.Http;

namespace WebServerHost.Web.Routing;

internal class ControllerActionInvoker
{
	private MethodInfo action;

	private ShcWebRequest request;

	private string[] routeParts;

	private string[] machedTemplateRouteParts;

	private Controller controller;

	private object[] actionParameters;

	public ControllerActionInvoker(MethodInfo action, ShcWebRequest request, string route, string machedTemplateRoute)
	{
		this.action = action;
		this.request = request;
		routeParts = route.SplitRouteParts();
		machedTemplateRouteParts = machedTemplateRoute.SplitRouteParts();
	}

	private void CreateController()
	{
		Type declaringType = action.DeclaringType;
		ConstructorInfo[] constructors = declaringType.GetConstructors();
		if (constructors.Length > 1)
		{
			throw AmbigousControllerConstruction(declaringType, constructors);
		}
		ParameterInfo[] parameters = constructors[0].GetParameters();
		object[] array = new object[parameters.Length];
		if (parameters.Length > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				object obj = ServiceProvider.Services.Get(parameters[i].ParameterType);
				if (obj == null)
				{
					obj = parameters[i].ParameterType.GetDafaultValue();
				}
				array[i] = obj;
			}
		}
		controller = (Controller)constructors[0].Invoke(array);
		controller.Request = request;
	}

	private static Exception AmbigousControllerConstruction(Type type, ConstructorInfo[] constructor)
	{
		return new Exception($"Ambigous construction of {type} having {constructor.Length} constructors. Controllers must have only one constructor");
	}

	private void BindActionParameters()
	{
		ParameterInfo[] parameters = action.GetParameters();
		actionParameters = new object[parameters.Length];
		if (parameters.Length <= 0)
		{
			return;
		}
		GetRouteParamsIndexes(machedTemplateRouteParts);
		for (int i = 0; i < parameters.Length; i++)
		{
			actionParameters[i] = parameters[i].ParameterType.GetDafaultValue();
			if (parameters[i].HasAttributeOfType<FromRouteAttribute>())
			{
				BindFromRoute(parameters[i], ref actionParameters[i]);
			}
			else if (parameters[i].HasAttributeOfType<FromQueryAttribute>())
			{
				BindFromQuery(parameters[i], ref actionParameters[i]);
			}
			else if (parameters[i].HasAttributeOfType<FromBodyAttribute>())
			{
				BindFromBody(request, parameters[i], ref actionParameters[i]);
			}
			else if (!BindFromRoute(parameters[i], ref actionParameters[i]))
			{
				BindFromQuery(parameters[i], ref actionParameters[i]);
			}
		}
	}

	private List<int> GetRouteParamsIndexes(string[] routeParts)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < routeParts.Length; i++)
		{
			if (routeParts[i].IsParamTemplateRoutePart())
			{
				list.Add(i);
			}
		}
		return list;
	}

	private bool BindFromRoute(ParameterInfo parameter, ref object value)
	{
		List<int> routeParamsIndexes = GetRouteParamsIndexes(machedTemplateRouteParts);
		foreach (int item in routeParamsIndexes)
		{
			if (machedTemplateRouteParts[item].GetRouteParamName() == parameter.Name)
			{
				value = ParseParameter(routeParts[item], parameter);
				return true;
			}
		}
		return false;
	}

	private bool BindFromQuery(ParameterInfo parameter, ref object value)
	{
		GetRouteParamsIndexes(machedTemplateRouteParts);
		foreach (KeyValuePair<string, string> parameter2 in request.Parameters)
		{
			if (parameter2.Key.EqualsIgnoreCase(parameter.Name))
			{
				value = ParseParameter(parameter2.Value, parameter);
				return true;
			}
		}
		return false;
	}

	private object ParseParameter(string strValue, ParameterInfo parameterInfo)
	{
		object obj = parameterInfo.ParameterType.Parse(strValue);
		if (obj != null)
		{
			return obj;
		}
		throw new ArgumentException($"The value '{strValue}' is not valid", parameterInfo.Name);
	}

	private bool BindFromBody(ShcWebRequest request, ParameterInfo parameter, ref object value)
	{
		if (!string.IsNullOrEmpty(request.RequestContent))
		{
			value = request.RequestContent.FromJson(parameter.ParameterType);
			return true;
		}
		return false;
	}

	public object Invoke()
	{
		CreateController();
		BindActionParameters();
		try
		{
			return action.Invoke(controller, actionParameters);
		}
		catch (TargetInvocationException ex)
		{
			throw ex.InnerException;
		}
	}
}
