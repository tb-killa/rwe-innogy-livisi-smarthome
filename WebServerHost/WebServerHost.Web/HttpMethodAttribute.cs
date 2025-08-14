using System;

namespace WebServerHost.Web;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public abstract class HttpMethodAttribute : Attribute
{
	public abstract string Method { get; }
}
