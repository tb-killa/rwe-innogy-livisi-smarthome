using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;

[PossibleActuatorClasses(new ActuatorClass[] { ActuatorClass.Alarm })]
[LogicalDeviceStateType(typeof(AlarmActuatorState))]
public class AlarmActuator : LogicalDevice
{
	private const bool DEFAULT_OFFSTATE = false;

	[XmlIgnore]
	public override string PrimaryPropertyName
	{
		get
		{
			return "OnState";
		}
		set
		{
		}
	}

	public new AlarmActuator Clone()
	{
		return (AlarmActuator)base.Clone();
	}

	public new AlarmActuator Clone(Guid tag)
	{
		return (AlarmActuator)base.Clone(tag);
	}

	protected override Entity CreateClone()
	{
		return new AlarmActuator();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
	}
}
