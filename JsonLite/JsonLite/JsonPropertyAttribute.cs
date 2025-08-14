using System;

namespace JsonLite;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class JsonPropertyAttribute : Attribute
{
	public readonly string Name;

	public JsonPropertyAttribute(string name)
	{
		Name = name;
	}
}
