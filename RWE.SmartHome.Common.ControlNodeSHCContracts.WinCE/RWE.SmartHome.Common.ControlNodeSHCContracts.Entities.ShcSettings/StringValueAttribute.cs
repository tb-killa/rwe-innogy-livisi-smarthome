using System;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcSettings;

public class StringValueAttribute : Attribute
{
	private readonly string _value;

	public string Value => _value;

	public StringValueAttribute(string value)
	{
		_value = value;
	}
}
