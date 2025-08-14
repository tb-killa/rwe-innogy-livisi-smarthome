using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
internal sealed class PossibleActuatorClassesAttribute : Attribute
{
	private KeyValuePair<BuiltinPhysicalDeviceType?, List<ActuatorClass>> possibleActuatorClassesInfo;

	public PossibleActuatorClassesAttribute(params ActuatorClass[] possibleActuatorClasses)
	{
		possibleActuatorClassesInfo = new KeyValuePair<BuiltinPhysicalDeviceType?, List<ActuatorClass>>(null, possibleActuatorClasses.ToList());
	}

	public PossibleActuatorClassesAttribute(BuiltinPhysicalDeviceType builtinPhysicalDeviceType, params ActuatorClass[] possibleActuatorClasses)
	{
		possibleActuatorClassesInfo = new KeyValuePair<BuiltinPhysicalDeviceType?, List<ActuatorClass>>(builtinPhysicalDeviceType, possibleActuatorClasses.ToList());
	}

	public List<ActuatorClass> GetPossibleActuatorClasses(BuiltinPhysicalDeviceType builtinPhysicalDeviceType)
	{
		if (possibleActuatorClassesInfo.Key == builtinPhysicalDeviceType || !possibleActuatorClassesInfo.Key.HasValue)
		{
			return possibleActuatorClassesInfo.Value;
		}
		return new List<ActuatorClass>();
	}
}
