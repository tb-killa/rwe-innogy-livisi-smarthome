using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;

public class NumericPropertyDefinition : PropertyDefinition
{
	public decimal? DefaultValue { get; private set; }

	public NumericPropertyDefinition(string name, decimal? defaultValue)
		: base(name)
	{
		DefaultValue = defaultValue;
	}

	public override Property NewInstance()
	{
		NumericProperty numericProperty = new NumericProperty();
		numericProperty.Name = base.Name;
		numericProperty.Value = DefaultValue;
		return numericProperty;
	}
}
