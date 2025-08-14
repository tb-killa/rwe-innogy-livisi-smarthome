using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace SmartHome.SHC.API.Serialization.Json;

public class JsonDeserializer
{
	private static readonly Type IListType = typeof(IList);

	private readonly JsonReader reader;

	private JsonDeserializer(JsonReader reader)
	{
		this.reader = reader;
	}

	public static T Deserialize<T>(string jsonString)
	{
		using JsonReader jsonReader = new JsonReader(jsonString);
		return (T)new JsonDeserializer(jsonReader).DeserializeValue(typeof(T));
	}

	private object DeserializeValue(Type type)
	{
		reader.SkipWhiteSpaces();
		if ((object)type == typeof(bool))
		{
			return reader.ReadBoolean();
		}
		if ((object)type == typeof(int))
		{
			return reader.ReadInt32();
		}
		if ((object)type == typeof(string))
		{
			return reader.ReadString();
		}
		if ((object)type == typeof(double))
		{
			return reader.ReadDouble();
		}
		if ((object)type == typeof(DateTime))
		{
			return reader.ReadDateTime();
		}
		if (IListType.IsAssignableFrom(type))
		{
			return DeserializeList(type);
		}
		if ((object)type == typeof(char))
		{
			return reader.ReadChar();
		}
		if (type.IsEnum)
		{
			return reader.ReadEnum();
		}
		if ((object)type == typeof(long))
		{
			return reader.ReadInt64();
		}
		if ((object)type == typeof(float))
		{
			return reader.ReadFloat();
		}
		if ((object)type == typeof(short))
		{
			return reader.ReadInt16();
		}
		if ((object)type == typeof(decimal?))
		{
			return reader.ReadDecimal();
		}
		if ((object)type == typeof(Guid))
		{
			return new Guid(reader.ReadString());
		}
		return ParseObject(type);
	}

	private object DeserializeList(Type listType)
	{
		reader.SkipWhiteSpaces();
		if (reader.Peek() != '[')
		{
			reader.AssertAndConsumeNull();
			return null;
		}
		reader.AssertAndConsume('[');
		Type listItemType = ListHelper.GetListItemType(listType);
		bool isReadOnly;
		IList list = ListHelper.CreateContainer(listType, listItemType, out isReadOnly);
		if (reader.Peek() == ']')
		{
			reader.AssertAndConsume(']');
			return list;
		}
		do
		{
			bool flag = true;
			reader.SkipWhiteSpaces();
			list.Add(DeserializeValue(listItemType));
			reader.SkipWhiteSpaces();
		}
		while (!reader.AssertNextIsDelimiterOrSeparator(']'));
		if (listType.IsArray)
		{
			return ListHelper.ToArray((List<object>)list, listItemType);
		}
		if (isReadOnly)
		{
			return listType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[1] { list.GetType() }, null).Invoke(new object[1] { list });
		}
		return list;
	}

	private object ParseObject(Type type)
	{
		reader.AssertAndConsume('{');
		ConstructorInfo defaultConstructor = ReflectionHelper.GetDefaultConstructor(type);
		object obj = defaultConstructor.Invoke(null);
		do
		{
			bool flag = true;
			reader.SkipWhiteSpaces();
			string name = reader.ReadString();
			PropertyInfo propertyInfo = ReflectionHelper.FindProperty(type, name);
			reader.SkipWhiteSpaces();
			reader.AssertAndConsume(':');
			reader.SkipWhiteSpaces();
			if ((object)propertyInfo != null)
			{
				propertyInfo.SetValue(obj, DeserializeValue(propertyInfo.PropertyType), null);
			}
			else
			{
				reader.DiscardFieldValue();
			}
			reader.SkipWhiteSpaces();
		}
		while (!reader.AssertNextIsDelimiterOrSeparator('}'));
		return obj;
	}
}
