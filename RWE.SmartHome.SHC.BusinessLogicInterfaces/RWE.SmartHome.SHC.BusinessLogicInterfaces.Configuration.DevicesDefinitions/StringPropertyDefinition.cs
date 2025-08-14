using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;

public class StringPropertyDefinition : PropertyDefinition
{
	public string DefaultValue { get; private set; }

	public StringPropertyDefinition(string name, string defaultValue)
		: base(name)
	{
		DefaultValue = defaultValue;
	}

	public override Property NewInstance()
	{
		return new StringProperty(base.Name, DefaultValue);
	}
}
