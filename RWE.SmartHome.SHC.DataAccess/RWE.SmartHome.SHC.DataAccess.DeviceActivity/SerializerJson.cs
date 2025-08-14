using System.Collections.Generic;
using JsonLite;

namespace RWE.SmartHome.SHC.DataAccess.DeviceActivity;

internal class SerializerJson
{
	private JsonSerializer serializer;

	private JsonDeserializer deserializer;

	public SerializerJson()
	{
		List<JsonConverter> customConverters = new List<JsonConverter>
		{
			new PropertyConverter()
		};
		JsonOptions options = new JsonOptions
		{
			NullPropertyHandling = NullPropertyHandling.Ignore
		};
		serializer = new JsonSerializer(customConverters, options);
		deserializer = new JsonDeserializer(customConverters);
	}

	public string Serialize(object obj)
	{
		return serializer.Serialize(obj);
	}

	public T Deserialize<T>(string json)
	{
		return deserializer.Deserialize<T>(json);
	}
}
