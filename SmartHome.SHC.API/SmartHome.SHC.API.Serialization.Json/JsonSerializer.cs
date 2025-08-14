using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace SmartHome.SHC.API.Serialization.Json;

public class JsonSerializer
{
	private readonly JsonWriter writer;

	private List<object> currentGraph;

	private JsonSerializer(JsonWriter writer)
	{
		this.writer = writer;
		currentGraph = new List<object>(0);
	}

	public static string Serialize(object instance)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (JsonWriter jsonWriter = new JsonWriter(stringBuilder))
		{
			new JsonSerializer(jsonWriter).SerializeValue(instance);
		}
		return stringBuilder.ToString();
	}

	private void SerializeValue(object value)
	{
		if (value == null)
		{
			writer.WriteNull();
		}
		else if (value is string)
		{
			writer.WriteString((string)value);
		}
		else if (value is int || value is long || value is short || value is float || value is byte || value is sbyte || value is uint || value is ulong || value is ushort || value is double)
		{
			writer.WriteRaw(value.ToString());
		}
		else if (value is decimal)
		{
			writer.WriteRaw(((decimal)value).ToString(CultureInfo.InvariantCulture));
		}
		else if (value is char)
		{
			writer.WriteChar((char)value);
		}
		else if (value is bool)
		{
			writer.WriteBool((bool)value);
		}
		else if (value is DateTime)
		{
			writer.WriteDate((DateTime)value);
		}
		else if (value is IDictionary)
		{
			SerializeDictionary((IDictionary)value);
		}
		else if (value is IEnumerable)
		{
			SerializeEnumerable((IEnumerable)value);
		}
		else if (value is Guid)
		{
			writer.WriteString(value.ToString());
		}
		else
		{
			SerializeObject(value);
		}
	}

	private void SerializeObject(object @object)
	{
		if (@object == null)
		{
			return;
		}
		List<PropertyInfo> serializableProperties = ReflectionHelper.GetSerializableProperties(@object.GetType());
		if (serializableProperties.Count != 0)
		{
			if (currentGraph.Contains(@object))
			{
				throw new JsonException("Recursive reference found. Serialization cannot complete. Consider marking the offending field with the NonSerializedAttribute");
			}
			List<object> list = currentGraph;
			List<object> list2 = new List<object>(currentGraph);
			currentGraph = list2;
			currentGraph.Add(@object);
			writer.BeginObject();
			SerializeKeyValue(serializableProperties[0].Name, ReflectionHelper.GetValue(serializableProperties[0], @object), isFirst: true);
			for (int i = 1; i < serializableProperties.Count; i++)
			{
				SerializeKeyValue(serializableProperties[i].Name, ReflectionHelper.GetValue(serializableProperties[i], @object), isFirst: false);
			}
			writer.EndObject();
			currentGraph = list;
		}
	}

	private void SerializeEnumerable(IEnumerable value)
	{
		IEnumerator enumerator = value.GetEnumerator();
		writer.BeginArray();
		if (enumerator.MoveNext())
		{
			SerializeValue(enumerator.Current);
		}
		while (enumerator.MoveNext())
		{
			writer.SeparateElements();
			SerializeValue(enumerator.Current);
		}
		writer.EndArray();
	}

	private void SerializeDictionary(IDictionary value)
	{
		IDictionaryEnumerator enumerator = value.GetEnumerator();
		writer.BeginObject();
		if (enumerator.MoveNext())
		{
			SerializeKeyValue(enumerator.Key.ToString(), enumerator.Value, isFirst: true);
		}
		while (enumerator.MoveNext())
		{
			SerializeKeyValue(enumerator.Key.ToString(), enumerator.Value, isFirst: false);
		}
		writer.EndObject();
	}

	private void SerializeKeyValue(string key, object value, bool isFirst)
	{
		if (!isFirst)
		{
			writer.SeparateElements();
		}
		writer.WriteKey(key);
		SerializeValue(value);
	}
}
