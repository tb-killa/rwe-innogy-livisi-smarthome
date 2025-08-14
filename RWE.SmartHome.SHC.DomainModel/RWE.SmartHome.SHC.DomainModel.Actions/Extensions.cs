using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using SmartHome.SHC.API;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.DomainModel.Actions;

public static class Extensions
{
	public static ExecutionStatus ToDomainModel(this global::SmartHome.SHC.API.ExecutionStatus status)
	{
		return status switch
		{
			global::SmartHome.SHC.API.ExecutionStatus.Success => ExecutionStatus.Success, 
			global::SmartHome.SHC.API.ExecutionStatus.Failure => ExecutionStatus.Failure, 
			_ => throw new ArgumentException("Invalid execution status: " + status), 
		};
	}

	public static RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property To_CoreProperty(this global::SmartHome.SHC.API.PropertyDefinition.Property apiProperty)
	{
		global::SmartHome.SHC.API.PropertyDefinition.NumericProperty numericProperty = apiProperty as global::SmartHome.SHC.API.PropertyDefinition.NumericProperty;
		if (numericProperty != null)
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.NumericProperty numericProperty2 = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.NumericProperty();
			numericProperty2.Name = numericProperty.Name;
			numericProperty2.Value = numericProperty.Value;
			return numericProperty2;
		}
		global::SmartHome.SHC.API.PropertyDefinition.StringProperty stringProperty = apiProperty as global::SmartHome.SHC.API.PropertyDefinition.StringProperty;
		if (stringProperty != null)
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty stringProperty2 = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty();
			stringProperty2.Name = stringProperty.Name;
			stringProperty2.Value = stringProperty.Value;
			return stringProperty2;
		}
		global::SmartHome.SHC.API.PropertyDefinition.BooleanProperty booleanProperty = apiProperty as global::SmartHome.SHC.API.PropertyDefinition.BooleanProperty;
		if (booleanProperty != null)
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.BooleanProperty booleanProperty2 = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.BooleanProperty();
			booleanProperty2.Name = booleanProperty.Name;
			booleanProperty2.Value = booleanProperty.Value;
			return booleanProperty2;
		}
		global::SmartHome.SHC.API.PropertyDefinition.DateTimeProperty dateTimeProperty = apiProperty as global::SmartHome.SHC.API.PropertyDefinition.DateTimeProperty;
		if (dateTimeProperty != null)
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.DateTimeProperty dateTimeProperty2 = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.DateTimeProperty();
			dateTimeProperty2.Name = dateTimeProperty.Name;
			dateTimeProperty2.Value = dateTimeProperty.Value;
			return dateTimeProperty2;
		}
		throw new ArgumentException("Input does not have an expected type. Input type is: " + apiProperty.GetType().FullName);
	}
}
