using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.HEC;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;

[LogicalDeviceStateType(typeof(SwitchActuatorState))]
[PossibleActuatorClasses(BuiltinPhysicalDeviceType.ISS2, new ActuatorClass[]
{
	ActuatorClass.ElectricalDevice,
	ActuatorClass.Light
})]
[PossibleActuatorClasses(BuiltinPhysicalDeviceType.ChargingStation, new ActuatorClass[] { ActuatorClass.ChargingStation })]
[PossibleActuatorClasses(BuiltinPhysicalDeviceType.PSS, new ActuatorClass[]
{
	ActuatorClass.ElectricalDevice,
	ActuatorClass.Light
})]
[PossibleActuatorClasses(BuiltinPhysicalDeviceType.PSSO, new ActuatorClass[]
{
	ActuatorClass.ElectricalDevice,
	ActuatorClass.Light
})]
public class SwitchActuator : LogicalDevice
{
	private const string PropNameSensingBehavior = "SensingBehavior";

	private const bool DEFAULT_OFFVALUE = false;

	private HECElectricDeviceSettings hecSettings;

	[XmlIgnore]
	public SensingBehaviorType? SensingBehavior
	{
		get
		{
			string stringValue = base.Properties.GetStringValue("SensingBehavior");
			return (!string.IsNullOrEmpty(stringValue)) ? ((SensingBehaviorType?)Enum.Parse(typeof(SensingBehaviorType), stringValue, ignoreCase: true)) : ((SensingBehaviorType?)null);
		}
		set
		{
			if (!value.HasValue)
			{
				base.Properties.Delete("SensingBehavior");
			}
			else
			{
				base.Properties.SetString("SensingBehavior", value.ToString());
			}
		}
	}

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

	public HECElectricDeviceSettings HECSettings
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
			hecSettings = new HECElectricDeviceSettings
			{
				HECEnabled = true,
				AllowedOperations = HECOperations.Both
			};
		}
		else
		{
			hecSettings = null;
		}
	}

	public new SwitchActuator Clone()
	{
		return (SwitchActuator)base.Clone();
	}

	public new SwitchActuator Clone(Guid tag)
	{
		return (SwitchActuator)base.Clone(tag);
	}

	protected override Entity CreateClone()
	{
		return new SwitchActuator();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		SwitchActuator switchActuator = (SwitchActuator)clone;
		if (hecSettings != null)
		{
			switchActuator.HECSettings = (HECElectricDeviceSettings)hecSettings.Clone();
		}
	}
}
