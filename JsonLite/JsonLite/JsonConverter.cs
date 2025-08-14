using System;

namespace JsonLite;

public abstract class JsonConverter
{
	private readonly Type type;

	public Type Type => type;

	public JsonConverter(Type type)
	{
		this.type = type;
	}

	public abstract JsonBuilder ToJson(object obj);

	public abstract object ToObject(JsonParser json);
}
