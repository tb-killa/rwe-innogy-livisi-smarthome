using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsonLite;

public class JsonDeserializer
{
	private readonly Dictionary<Type, JsonConverter> customConverters;

	public JsonDeserializer()
		: this(new JsonConverter[0])
	{
	}

	public JsonDeserializer(IEnumerable<JsonConverter> customConverters)
	{
		this.customConverters = customConverters.ToDictionary((JsonConverter c) => c.Type);
	}

	public T Deserialize<T>(string json)
	{
		JsonParser parser = new JsonParser(json);
		return (T)Deserialize(typeof(T), parser);
	}

	public object Deserialize(Type type, string json)
	{
		return Deserialize(type, new JsonParser(json));
	}

	public T Deserialize<T>(JsonParser jsonParser)
	{
		return (T)Deserialize(typeof(T), jsonParser);
	}

	private object Deserialize(Type objType, JsonParser parser)
	{
		if (!parser.HasValue)
		{
			return null;
		}
		if (objType.IsNullableType())
		{
			objType = Nullable.GetUnderlyingType(objType);
		}
		if (customConverters.TryGetValue(objType, out var value))
		{
			return value.ToObject(parser);
		}
		if (parser.IsPrimitive && (objType.Implements(typeof(IConvertible)) || (object)objType == typeof(Guid)))
		{
			return DeserializePrimitive(objType, parser);
		}
		if (parser.IsArray && objType.Implements(typeof(IEnumerable)))
		{
			return DeserializeArray(objType, parser);
		}
		if (parser.IsObject)
		{
			return DeserializeObject(objType, parser);
		}
		throw new InvalidCastException($"Cannot create instance of type {objType} from provided JSON string");
	}

	private object DeserializePrimitive(Type type, JsonParser parser)
	{
		if ((object)type == typeof(bool))
		{
			return parser.GetValue<bool>();
		}
		if ((object)type == typeof(byte))
		{
			return parser.GetValue<byte>();
		}
		if ((object)type == typeof(sbyte))
		{
			return parser.GetValue<sbyte>();
		}
		if ((object)type == typeof(short))
		{
			return parser.GetValue<short>();
		}
		if ((object)type == typeof(ushort))
		{
			return parser.GetValue<ushort>();
		}
		if ((object)type == typeof(int))
		{
			return parser.GetValue<int>();
		}
		if ((object)type == typeof(uint))
		{
			return parser.GetValue<uint>();
		}
		if ((object)type == typeof(long))
		{
			return parser.GetValue<long>();
		}
		if ((object)type == typeof(ulong))
		{
			return parser.GetValue<ulong>();
		}
		if ((object)type == typeof(float))
		{
			return parser.GetValue<float>();
		}
		if ((object)type == typeof(double))
		{
			return parser.GetValue<double>();
		}
		if ((object)type == typeof(decimal))
		{
			return parser.GetValue<decimal>();
		}
		if ((object)type == typeof(char))
		{
			return parser.GetValue<char>();
		}
		if ((object)type == typeof(string))
		{
			return parser.GetValue<string>();
		}
		if ((object)type.BaseType == typeof(Enum))
		{
			return Enum.Parse(type, parser.GetValue<string>(), ignoreCase: true);
		}
		if ((object)type == typeof(DateTime))
		{
			return DateTime.Parse(parser.GetValue<string>(), null, DateTimeStyles.RoundtripKind);
		}
		if ((object)type == typeof(Guid))
		{
			return new Guid(parser.GetValue<string>());
		}
		throw new InvalidCastException($"Cannot convert json value {parser.GetRawValue()} to type of {type}");
	}

	private object DeserializeArray(Type objType, JsonParser parser)
	{
		object obj = ((!objType.IsArray) ? objType.GetConstructor(new Type[0]).Invoke(new object[0]) : Array.CreateInstance(objType.GetTypeOfElements(), parser.FieldsCount));
		PopulateArray(obj, parser);
		return obj;
	}

	private void PopulateArray(object arrayObj, JsonParser parser)
	{
		Type typeOfElements = arrayObj.GetType().GetTypeOfElements();
		if (arrayObj.GetType().IsArray)
		{
			Array array = arrayObj as Array;
			for (int i = 0; i < parser.FieldsCount; i++)
			{
				object value = Deserialize(typeOfElements, parser.GetField(i));
				array.SetValue(value, i);
			}
			return;
		}
		if (arrayObj.GetType().Implements(typeof(IDictionary)))
		{
			IDictionary dictionary = arrayObj as IDictionary;
			Type objType = arrayObj.GetType().GetGenericArguments()[0];
			Type objType2 = arrayObj.GetType().GetGenericArguments()[1];
			{
				foreach (JsonParser allField in parser.GetAllFields())
				{
					object key = Deserialize(objType, allField["key"]);
					object value2 = Deserialize(objType2, allField["value"]);
					dictionary.Add(key, value2);
				}
				return;
			}
		}
		if (!arrayObj.GetType().Implements(typeof(IEnumerable)))
		{
			return;
		}
		IList list = arrayObj as IList;
		foreach (JsonParser allField2 in parser.GetAllFields())
		{
			object value3 = Deserialize(typeOfElements, allField2);
			list.Add(value3);
		}
	}

	private object DeserializeObject(Type objType, JsonParser parser)
	{
		object obj = Activator.CreateInstance(objType);
		IEnumerable<MemberInfo> enumerable = from m in objType.GetFieldsAndProperties()
			where !m.GetCustomAttributes(inherit: false).OfType<JsonIgnoreAttribute>().Any()
			select m;
		foreach (MemberInfo item in enumerable)
		{
			string name = GetName(item);
			PropertyInfo propertyInfo = item as PropertyInfo;
			FieldInfo fieldInfo = item as FieldInfo;
			JsonParser field = parser.GetField(name);
			if (field != null)
			{
				propertyInfo?.SetValue(obj, Deserialize(propertyInfo.PropertyType, field), null);
				fieldInfo?.SetValue(obj, Deserialize(fieldInfo.FieldType, field));
			}
		}
		return obj;
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
