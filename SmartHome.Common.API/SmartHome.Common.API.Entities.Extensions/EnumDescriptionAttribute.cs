using System;

namespace SmartHome.Common.API.Entities.Extensions;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class EnumDescriptionAttribute : Attribute
{
	public readonly string description;

	public EnumDescriptionAttribute(string description)
	{
		this.description = description;
	}
}
