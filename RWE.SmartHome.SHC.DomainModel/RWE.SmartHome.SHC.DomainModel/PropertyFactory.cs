using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.DomainModel;

public class PropertyFactory
{
	public static IDProperty FromContracts(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property property)
	{
		if (property == null)
		{
			return null;
		}
		if (property is RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty stringProperty)
		{
			return new StringDProperty(stringProperty.Name, stringProperty.Value, stringProperty.UpdateTimestamp);
		}
		if (property is RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.NumericProperty numericProperty)
		{
			return new NumericDProperty(numericProperty.Name, numericProperty.Value, numericProperty.UpdateTimestamp);
		}
		if (property is RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.BooleanProperty booleanProperty)
		{
			return new BooleanDProperty(booleanProperty.Name, booleanProperty.Value, booleanProperty.UpdateTimestamp);
		}
		if (property is RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.DateTimeProperty dateTimeProperty)
		{
			return new DateTimeDProperty(dateTimeProperty.Name, dateTimeProperty.Value, dateTimeProperty.UpdateTimestamp);
		}
		return null;
	}

	public static IDProperty FromDeviceSDK(global::SmartHome.SHC.API.PropertyDefinition.Property property)
	{
		if (property == null)
		{
			return null;
		}
		global::SmartHome.SHC.API.PropertyDefinition.StringProperty stringProperty = property as global::SmartHome.SHC.API.PropertyDefinition.StringProperty;
		if (stringProperty != null)
		{
			return new StringDProperty(stringProperty.Name, stringProperty.Value);
		}
		global::SmartHome.SHC.API.PropertyDefinition.NumericProperty numericProperty = property as global::SmartHome.SHC.API.PropertyDefinition.NumericProperty;
		if (numericProperty != null)
		{
			return new NumericDProperty(numericProperty.Name, numericProperty.Value);
		}
		global::SmartHome.SHC.API.PropertyDefinition.BooleanProperty booleanProperty = property as global::SmartHome.SHC.API.PropertyDefinition.BooleanProperty;
		if (booleanProperty != null)
		{
			return new BooleanDProperty(booleanProperty.Name, booleanProperty.Value);
		}
		global::SmartHome.SHC.API.PropertyDefinition.DateTimeProperty dateTimeProperty = property as global::SmartHome.SHC.API.PropertyDefinition.DateTimeProperty;
		if (dateTimeProperty != null)
		{
			return new DateTimeDProperty(dateTimeProperty.Name, dateTimeProperty.Value);
		}
		return null;
	}
}
