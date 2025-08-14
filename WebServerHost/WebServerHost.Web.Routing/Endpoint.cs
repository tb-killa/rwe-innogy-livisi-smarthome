using System;
using System.Reflection;

namespace WebServerHost.Web.Routing;

public class Endpoint<T>
{
	private MethodInfo Action { get; set; }

	public string Name => Action.Name;

	public static Endpoint<T> From(T action)
	{
		return new Endpoint<T>(action);
	}

	private Endpoint(T action)
	{
		if (!(action is Delegate obj))
		{
			throw new ArgumentException("Cannot convert action to delegate.", "action");
		}
		Action = obj.Method;
	}

	public static implicit operator Endpoint<T>(T action)
	{
		return new Endpoint<T>(action);
	}

	public static implicit operator MethodInfo(Endpoint<T> ep)
	{
		return ep.Action;
	}
}
