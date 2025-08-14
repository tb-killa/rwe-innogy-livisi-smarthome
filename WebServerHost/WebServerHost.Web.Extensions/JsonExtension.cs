using System;
using SmartHome.Common.API.Entities.Serializers;

namespace WebServerHost.Web.Extensions;

internal static class JsonExtension
{
	public static string ToJson(this object obj)
	{
		return ApiJsonSerializer.Serialize(obj);
	}

	public static T FromJson<T>(this string json)
	{
		return ApiJsonSerializer.Deserialize<T>(json);
	}

	public static object FromJson(this string json, Type type)
	{
		return ApiJsonSerializer.Deserialize(type, json);
	}
}
