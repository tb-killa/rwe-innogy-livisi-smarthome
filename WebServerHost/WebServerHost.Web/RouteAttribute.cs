using System;

namespace WebServerHost.Web;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class RouteAttribute : Attribute
{
	public readonly string template;

	public RouteAttribute(string template)
	{
		this.template = template;
	}
}
