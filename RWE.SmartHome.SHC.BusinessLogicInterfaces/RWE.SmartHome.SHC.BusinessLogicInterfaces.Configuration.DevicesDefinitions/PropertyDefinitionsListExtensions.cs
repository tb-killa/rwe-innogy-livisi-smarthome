using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;

public static class PropertyDefinitionsListExtensions
{
	public static List<Property> GenerateMissingProperties(this List<PropertyDefinition> propertyDefinitionsList, List<Property> propertiesList)
	{
		return (from newPropDef in propertyDefinitionsList
			where propertiesList.All((Property prop) => prop.Name != newPropDef.Name)
			select newPropDef.NewInstance()).ToList();
	}
}
