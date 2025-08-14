using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;

namespace RWE.SmartHome.SHC.CoreApiConverters;

public static class TemporaryConverters
{
	public static ActionDescription FromActuatorState(LogicalDeviceState actuatorState)
	{
		ActionDescription actionDescription = new ActionDescription();
		actionDescription.ActionType = "SetState";
		actionDescription.Id = Guid.NewGuid();
		actionDescription.Tags = new List<Tag>();
		actionDescription.Target = new LinkBinding(EntityType.LogicalDevice, actuatorState.LogicalDeviceId);
		actionDescription.Version = 1;
		actionDescription.Data = (from prop in actuatorState.GetProperties()
			where prop.GetValueAsComparable() != null
			select ToParameter(prop)).ToList();
		return actionDescription;
	}

	public static Parameter ToParameter(Property prop)
	{
		if (prop is NumericProperty numericProperty)
		{
			Parameter parameter = new Parameter();
			parameter.Name = numericProperty.Name;
			parameter.Value = new ConstantNumericBinding
			{
				Value = numericProperty.Value.GetValueOrDefault()
			};
			return parameter;
		}
		if (prop is StringProperty stringProperty)
		{
			Parameter parameter2 = new Parameter();
			parameter2.Name = stringProperty.Name;
			parameter2.Value = new ConstantStringBinding
			{
				Value = stringProperty.Value
			};
			return parameter2;
		}
		if (prop is BooleanProperty booleanProperty)
		{
			Parameter parameter3 = new Parameter();
			parameter3.Name = booleanProperty.Name;
			parameter3.Value = new ConstantBooleanBinding
			{
				Value = (booleanProperty.Value == true)
			};
			return parameter3;
		}
		if (prop is DateTimeProperty dateTimeProperty)
		{
			Parameter parameter4 = new Parameter();
			parameter4.Name = dateTimeProperty.Name;
			parameter4.Value = new ConstantDateTimeBinding
			{
				Value = dateTimeProperty.Value.GetValueOrDefault()
			};
			return parameter4;
		}
		throw new ArgumentException("Input API property does not have an expected type. Input type is: " + prop.GetType().FullName);
	}
}
