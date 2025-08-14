using System;

namespace JsonLite;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class JsonIgnoreAttribute : Attribute
{
}
