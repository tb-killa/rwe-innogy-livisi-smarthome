using System;

namespace WebServerHost.Web;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public class FromQueryAttribute : Attribute
{
}
