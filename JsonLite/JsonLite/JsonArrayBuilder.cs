using System;

namespace JsonLite;

public class JsonArrayBuilder : JsonBuilder
{
	public override JsonType JsonTypeBuilder => JsonType.Array;

	public JsonType Type { get; private set; }

	public JsonArrayBuilder(JsonType jsonType)
		: base('[', ']')
	{
		Type = jsonType;
	}

	public JsonArrayBuilder Add<T>(T value) where T : IConvertible
	{
		JsonType jsonType = GetJsonType(value);
		if (jsonType != Type)
		{
			throw new ArrayTypeMismatchException($"Invalid json type {jsonType}, cannot be added to a json array with types of {Type}");
		}
		AddField(value);
		return this;
	}

	public JsonArrayBuilder Add(JsonBuilder jsonBuilder)
	{
		if (jsonBuilder == null)
		{
			switch (Type)
			{
			case JsonType.Object:
			case JsonType.Array:
			case JsonType.String:
			case JsonType.Null:
				break;
			default:
				throw new ArrayTypeMismatchException($"Invalid json type {jsonBuilder.JsonTypeBuilder}, cannot be added to a json array with types of {Type}");
			}
			AddField<string>(null);
		}
		else
		{
			if (jsonBuilder.JsonTypeBuilder != Type)
			{
				throw new ArrayTypeMismatchException($"Invalid json type {jsonBuilder.JsonTypeBuilder}, cannot be added to a json array with types of {Type}");
			}
			AddField(jsonBuilder);
		}
		return this;
	}

	public JsonArrayBuilder AddJson(JsonParser json)
	{
		AddRawJson(json.GetRawValue(), null);
		return this;
	}
}
