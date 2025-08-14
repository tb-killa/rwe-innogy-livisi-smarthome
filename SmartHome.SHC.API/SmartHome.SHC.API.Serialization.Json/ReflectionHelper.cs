using System;
using System.Collections.Generic;
using System.Reflection;

namespace SmartHome.SHC.API.Serialization.Json;

internal static class ReflectionHelper
{
	public static List<PropertyInfo> GetSerializableProperties(Type type)
	{
		List<PropertyInfo> list = new List<PropertyInfo>(10);
		list.AddRange(type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
		return list;
	}

	public static PropertyInfo FindProperty(Type type, string name)
	{
		PropertyInfo propertyInfo = FindPropertyThroughoutHierarchy(type, name);
		if ((object)propertyInfo == null)
		{
			throw new JsonException(type.FullName + " doesn't have a field named: " + name);
		}
		return propertyInfo;
	}

	public static object GetValue(PropertyInfo property, object @object)
	{
		object value = property.GetValue(@object, null);
		return property.PropertyType.IsEnum ? ((object)(int)value) : value;
	}

	public static ConstructorInfo GetDefaultConstructor(Type type)
	{
		ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
		if ((object)constructor == null)
		{
			throw new JsonException(type.FullName + " must have a parameterless constructor");
		}
		return constructor;
	}

	private static PropertyInfo FindPropertyThroughoutHierarchy(Type type, string name)
	{
		return type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
	}
}
