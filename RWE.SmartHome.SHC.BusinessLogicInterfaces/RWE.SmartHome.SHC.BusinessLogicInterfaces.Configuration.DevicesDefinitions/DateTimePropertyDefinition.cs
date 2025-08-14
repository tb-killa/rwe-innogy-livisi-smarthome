using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;

public class DateTimePropertyDefinition : PropertyDefinition
{
	public DateTime? DefaultValue { get; private set; }

	public DateTimePropertyDefinition(string name, DateTime? defaultValue)
		: base(name)
	{
		DefaultValue = defaultValue;
	}

	public override Property NewInstance()
	{
		DateTimeProperty dateTimeProperty = new DateTimeProperty();
		dateTimeProperty.Name = base.Name;
		dateTimeProperty.Value = DefaultValue;
		return dateTimeProperty;
	}
}
