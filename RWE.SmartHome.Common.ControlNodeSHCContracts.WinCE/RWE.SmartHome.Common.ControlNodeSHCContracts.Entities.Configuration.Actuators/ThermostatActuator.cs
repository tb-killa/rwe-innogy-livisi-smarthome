using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.HEC;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;

[LogicalDeviceStateType(typeof(ThermostatActuatorState))]
[PossibleActuatorClasses(new ActuatorClass[] { ActuatorClass.Temperature })]
public class ThermostatActuator : LogicalDevice
{
	private HECTemperatureDeviceSettings hecSettings;

	[XmlElement(ElementName = "MxTp")]
	public decimal MaxTemperature { get; set; }

	[XmlElement(ElementName = "MnTp")]
	public decimal MinTemperature { get; set; }

	[XmlElement(ElementName = "ChLk")]
	public bool ChildLock { get; set; }

	[XmlElement(ElementName = "WOpTp")]
	public decimal WindowOpenTemperature { get; set; }

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

	public HECTemperatureDeviceSettings HECSettings
	{
		get
		{
			return hecSettings;
		}
		set
		{
			if (value != null)
			{
				if (!value.HECEnabled)
				{
					hecSettings = null;
				}
				else
				{
					hecSettings = value;
				}
			}
			else
			{
				hecSettings = null;
			}
		}
	}

	public void EnableForHEC(bool enable)
	{
		if (enable)
		{
			hecSettings = new HECTemperatureDeviceSettings
			{
				HECEnabled = true
			};
		}
		else
		{
			hecSettings = null;
		}
	}

	protected override Entity CreateClone()
	{
		return new ThermostatActuator();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		ThermostatActuator thermostatActuator = (ThermostatActuator)clone;
		thermostatActuator.MaxTemperature = MaxTemperature;
		thermostatActuator.MinTemperature = MinTemperature;
		thermostatActuator.ChildLock = ChildLock;
		thermostatActuator.WindowOpenTemperature = WindowOpenTemperature;
		if (hecSettings != null)
		{
			thermostatActuator.HECSettings = (HECTemperatureDeviceSettings)hecSettings.Clone();
		}
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
		allProperties.Add(new BooleanProperty
		{
			Name = "ChildLock",
			Value = ChildLock
		});
		allProperties.Add(new NumericProperty
		{
			Name = "WindowOpenTemperature",
			Value = WindowOpenTemperature
		});
		return allProperties;
	}
}
