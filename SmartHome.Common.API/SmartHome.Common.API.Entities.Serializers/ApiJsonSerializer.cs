using System;
using System.Collections.Generic;
using System.IO;
using JsonLite;
using SmartHome.Common.API.Entities.JsonConverters;

namespace SmartHome.Common.API.Entities.Serializers;

public class ApiJsonSerializer
{
	private static List<JsonConverter> CustomConverters = new List<JsonConverter>
	{
		new PropertyJsonConverter(),
		new ParameterListJsonConverter(),
		new ParameterJsonConverter(),
		new ConditionJsonConverter(),
		new CustomTriggerJsonConverter(),
		new RuleJsonConverter(),
		new UtilityEntryJsonConverter()
	};

	private static JsonOptions options = new JsonOptions
	{
		NullPropertyHandling = NullPropertyHandling.Ignore
	};

	private static JsonSerializer serializer = new JsonSerializer(CustomConverters, options);

	private static JsonDeserializer deserializer = new JsonDeserializer(CustomConverters);

	public static string Serialize(object obj)
	{
		return serializer.Serialize(obj);
	}

	public static void Serialize(TextWriter writer, object obj)
	{
		writer.Write(Serialize(obj));
		writer.Flush();
	}

	public static T Deserialize<T>(string json)
	{
		return deserializer.Deserialize<T>(json);
	}

	internal static T Deserialize<T>(JsonParser jsonParser)
	{
		return deserializer.Deserialize<T>(jsonParser);
	}

	public static object Deserialize(Type type, string json)
	{
		return deserializer.Deserialize(type, json);
	}
}
