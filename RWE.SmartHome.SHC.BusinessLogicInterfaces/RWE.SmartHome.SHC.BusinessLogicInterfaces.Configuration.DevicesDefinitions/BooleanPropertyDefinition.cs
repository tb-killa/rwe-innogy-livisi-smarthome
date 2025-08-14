using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;

public class BooleanPropertyDefinition : PropertyDefinition
{
	public bool? DefaultValue { get; private set; }

	public BooleanPropertyDefinition(string name, bool? defaultValue)
		: base(name)
	{
		DefaultValue = defaultValue;
	}

	public override Property NewInstance()
	{
		BooleanProperty booleanProperty = new BooleanProperty();
		booleanProperty.Name = base.Name;
		booleanProperty.Value = DefaultValue;
		return booleanProperty;
	}
}
