using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;

[LogicalDeviceStateType(typeof(RollerShutterActuatorState))]
[PossibleActuatorClasses(new ActuatorClass[]
{
	ActuatorClass.Marquee,
	ActuatorClass.RollerShutter
})]
public class RollerShutterActuator : LogicalDevice
{
	private const int DEFAULT_TIME_FULL_UP = 600;

	private const int DEFAULT_TIME_FULL_DOWN = 600;

	[XmlElement(ElementName = "TmFU")]
	public int TimeFullUp { get; set; }

	[XmlElement(ElementName = "TmFD")]
	public int TimeFullDown { get; set; }

	public bool IsCalibrating { get; set; }

	[XmlIgnore]
	public override string PrimaryPropertyName
	{
		get
		{
			return "ShutterLevel";
		}
		set
		{
		}
	}

	public RollerShutterActuator()
	{
		TimeFullDown = 600;
		TimeFullUp = 600;
	}

	protected override Entity CreateClone()
	{
		return new RollerShutterActuator();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		RollerShutterActuator rollerShutterActuator = (RollerShutterActuator)clone;
		rollerShutterActuator.TimeFullUp = TimeFullUp;
		rollerShutterActuator.TimeFullDown = TimeFullDown;
		rollerShutterActuator.IsCalibrating = IsCalibrating;
	}

	public new RollerShutterActuator Clone()
	{
		return (RollerShutterActuator)base.Clone();
	}

	public new RollerShutterActuator Clone(Guid tag)
	{
		return (RollerShutterActuator)base.Clone(tag);
	}

	public override List<Property> GetAllProperties()
	{
		List<Property> allProperties = base.GetAllProperties();
		allProperties.Add(new NumericProperty
		{
			Name = "TimeFullUp",
			Value = TimeFullUp
		});
		allProperties.Add(new NumericProperty
		{
			Name = "TimeFullDown",
			Value = TimeFullDown
		});
		allProperties.Add(new BooleanProperty
		{
			Name = "IsCalibrating",
			Value = IsCalibrating
		});
		return allProperties;
	}
}
