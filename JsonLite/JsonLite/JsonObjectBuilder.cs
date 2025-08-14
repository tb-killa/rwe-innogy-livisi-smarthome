using System;

namespace JsonLite;

public class JsonObjectBuilder : JsonBuilder
{
	public override JsonType JsonTypeBuilder => JsonType.Object;

	public JsonObjectBuilder()
		: base('{', '}')
	{
	}

	public JsonObjectBuilder Add<T>(string name, T value) where T : IConvertible
	{
		if (name == null)
		{
			throw new ArgumentNullException(name);
		}
		if (name == string.Empty)
		{
			throw new ArgumentException("Invalid name", "name");
		}
		AddField(value, name);
		return this;
	}

	public JsonObjectBuilder Add(string name, JsonBuilder jsonBuilder)
	{
		AddField(jsonBuilder, name);
		return this;
	}

	public JsonObjectBuilder AddJson(string name, JsonParser json)
	{
		if (name == null)
		{
			throw new ArgumentNullException(name);
		}
		if (name == string.Empty)
		{
			throw new ArgumentException("Invalid name", "name");
		}
		AddRawJson(json.GetRawValue(), name);
		return this;
	}
}
