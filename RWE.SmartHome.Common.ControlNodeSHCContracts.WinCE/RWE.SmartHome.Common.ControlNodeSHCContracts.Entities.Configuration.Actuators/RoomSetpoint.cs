using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;

[PossibleActuatorClasses(new ActuatorClass[] { ActuatorClass.Temperature })]
[LogicalDeviceStateType(typeof(RoomSetpointState))]
public class RoomSetpoint : LogicalDevice
{
	[XmlElement(ElementName = "MxTp")]
	public decimal MaxTemperature { get; set; }

	[XmlElement(ElementName = "MnTp")]
	public decimal MinTemperature { get; set; }

	[XmlIgnore]
	public override string PrimaryPropertyName
	{
		get
		{
			return "PointTemperature";
		}
		set
		{
		}
	}

	protected override Entity CreateClone()
	{
		return new RoomSetpoint();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		RoomSetpoint roomSetpoint = (RoomSetpoint)clone;
		roomSetpoint.MaxTemperature = MaxTemperature;
		roomSetpoint.MinTemperature = MinTemperature;
	}

	public override List<Property> GetAllProperties()
	{
		List<Property> allProperties = base.GetAllProperties();
		allProperties.Add(new NumericProperty
		{
			Name = "MaxTemperature",
			Value = MaxTemperature
		});
		allProperties.Add(new NumericProperty
		{
			Name = "MinTemperature",
			Value = MinTemperature
		});
		return allProperties;
	}
}
