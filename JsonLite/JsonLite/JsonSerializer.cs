using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsonLite;

public class JsonSerializer
{
	private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ssK";

	private Dictionary<Type, JsonConverter> customConverters;

	private JsonOptions options;

	public JsonSerializer()
		: this(new JsonConverter[0])
	{
	}

	public JsonSerializer(JsonOptions options)
		: this(new JsonConverter[0], options)
	{
	}

	public JsonSerializer(IEnumerable<JsonConverter> customConverters)
		: this(customConverters, new JsonOptions())
	{
	}

	public JsonSerializer(IEnumerable<JsonConverter> customConverters, JsonOptions options)
	{
		this.customConverters = customConverters.ToDictionary((JsonConverter c) => c.Type);
		this.options = options;
	}

	public string Serialize(object obj)
	{
		if (obj == null)
		{
			return "null";
		}
		Type type = obj.GetType();
		if (type.Implements(typeof(IConvertible)))
		{
			if ((object)type.BaseType == typeof(Enum))
			{
				return AsJsonString(obj.ToString());
			}
			if ((object)type == typeof(DateTime))
			{
				return AsJsonString(((DateTime)obj).ToString("yyyy-MM-ddTHH:mm:ssK"));
			}
			return obj.ToString();
		}
		if (customConverters.TryGetValue(type, out var value))
		{
			return value.ToJson(obj).Build();
		}
		if ((object)type == typeof(Guid))
		{
			return AsJsonString(((Guid)obj).ToString("N"));
		}
		JsonBuilder jsonBuilder;
		if (type.IsArray || type.Implements(typeof(IEnumerable<>)))
		{
			jsonBuilder = new JsonArrayBuilder(GetJsonType(obj.GetType().GetTypeOfElements()));
			ConstructArray((IEnumerable)obj, (JsonArrayBuilder)jsonBuilder);
		}
		else
		{
			jsonBuilder = new JsonObjectBuilder();
			ConstructObject(obj, (JsonObjectBuilder)jsonBuilder);
		}
		return jsonBuilder.Build();
	}

	private string AsJsonString(string value)
	{
		return new StringBuilder().Append('"').Append(value).Append('"')
			.ToString();
	}

	private JsonType GetJsonType(Type type)
	{
		if ((object)type == null)
		{
			return JsonType.Null;
		}
		if ((object)type == typeof(bool))
		{
			return JsonType.Boolean;
		}
		if (type.IsNumber())
		{
			return JsonType.Number;
		}
		if ((object)type == typeof(string) || (object)type == typeof(char) || (object)type.BaseType == typeof(Enum) || (object)type == typeof(DateTime))
		{
			return JsonType.String;
		}
		if (type.IsArray || type.Implements(typeof(IEnumerable<>)))
		{
			return JsonType.Array;
		}
		return JsonType.Object;
	}

	private void ConstructArray(IEnumerable enumerable, JsonArrayBuilder arrayBuilder)
	{
		foreach (object item in enumerable)
		{
			Type type = item.GetType();
			if (customConverters.TryGetValue(type, out var value))
			{
				arrayBuilder.Add(value.ToJson(item));
				continue;
			}
			switch (GetJsonType(type))
			{
			case JsonType.String:
			case JsonType.Number:
			case JsonType.Boolean:
			case JsonType.Null:
				arrayBuilder.Add((IConvertible)item);
				break;
			case JsonType.Object:
			{
				if ((object)type == typeof(Guid))
				{
					arrayBuilder.Add(item.ToString());
					break;
				}
				JsonObjectBuilder jsonObjectBuilder = new JsonObjectBuilder();
				ConstructObject(item, jsonObjectBuilder);
				arrayBuilder.Add((JsonBuilder)jsonObjectBuilder);
				break;
			}
			case JsonType.Array:
			{
				JsonArrayBuilder jsonArrayBuilder = new JsonArrayBuilder(GetJsonType(type.GetTypeOfElements()));
				ConstructArray((IEnumerable)item, jsonArrayBuilder);
				arrayBuilder.Add((JsonBuilder)jsonArrayBuilder);
				break;
			}
			}
		}
	}

	private void ConstructObject(object obj, JsonObjectBuilder objectBuilder)
	{
		IEnumerable<MemberInfo> enumerable = from m in obj.GetType().GetFieldsAndProperties()
			where !m.GetCustomAttributes(inherit: false).OfType<JsonIgnoreAttribute>().Any()
			select m;
		foreach (MemberInfo item in enumerable)
		{
			PropertyInfo propertyInfo = item as PropertyInfo;
			FieldInfo fieldInfo = item as FieldInfo;
			string name;
			object value;
			Type type;
			if ((object)propertyInfo != null)
			{
				name = GetName(propertyInfo);
				value = propertyInfo.GetValue(obj, null);
				type = value?.GetType();
			}
			else
			{
				if ((object)fieldInfo == null)
				{
					continue;
				}
				name = GetName(fieldInfo);
				value = fieldInfo.GetValue(obj);
				type = value?.GetType();
			}
			if (type.IsNullableType())
			{
				type = Nullable.GetUnderlyingType(type);
			}
			if ((object)type != null && customConverters.TryGetValue(type, out var value2))
			{
				objectBuilder.Add(name, value2.ToJson(value));
				continue;
			}
			switch (GetJsonType(type))
			{
			case JsonType.String:
			case JsonType.Number:
			case JsonType.Boolean:
				objectBuilder.Add(name, (IConvertible)value);
				break;
			case JsonType.Null:
				if (options.NullPropertyHandling == NullPropertyHandling.Inlude)
				{
					objectBuilder.Add(name, (IConvertible)value);
				}
				break;
			case JsonType.Object:
			{
				if ((object)type == typeof(Guid))
				{
					objectBuilder.Add(name, ((Guid)value).ToString("N"));
					break;
				}
				JsonObjectBuilder jsonObjectBuilder = new JsonObjectBuilder();
				ConstructObject(value, jsonObjectBuilder);
				objectBuilder.Add(name, (JsonBuilder)jsonObjectBuilder);
				break;
			}
			case JsonType.Array:
			{
				JsonArrayBuilder jsonArrayBuilder = new JsonArrayBuilder(GetJsonType(type.GetTypeOfElements()));
				ConstructArray((IEnumerable)value, jsonArrayBuilder);
				objectBuilder.Add(name, (JsonBuilder)jsonArrayBuilder);
				break;
			}
			}
		}
	}

	private string GetName(MemberInfo member)
	{
		IEnumerable<JsonPropertyAttribute> source = member.GetCustomAttributes(inherit: false).OfType<JsonPropertyAttribute>();
		if (source.Any())
		{
			return source.First().Name;
		}
		return ToCamelCase(member.Name);
	}

	private string ToCamelCase(string value)
	{
		if (char.IsUpper(value[0]))
		{
			return new StringBuilder(value).Replace(value[0], char.ToLower(value[0]), 0, 1).ToString();
		}
		return value;
	}
}
