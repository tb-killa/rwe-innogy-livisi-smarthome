using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;

public abstract class PropertyDefinition
{
	public string Name { get; protected set; }

	public PropertyDefinition(string name)
	{
		Name = name;
	}

	public abstract Property NewInstance();
}
