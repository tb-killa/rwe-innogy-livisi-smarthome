using SmartHome.Common.API.Entities.Extensions;

namespace SmartHome.Common.API.Entities.JsonConverters;

internal static class JsonNameResolver
{
	public static string ResolveName(string name)
	{
		if (name.Length > 2 && char.IsLower(name[1]))
		{
			return name.FirstToLower();
		}
		return name;
	}
}
