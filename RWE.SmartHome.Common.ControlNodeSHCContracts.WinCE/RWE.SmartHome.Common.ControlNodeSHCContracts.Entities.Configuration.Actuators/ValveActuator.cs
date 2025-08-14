using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.HEC;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;

[PossibleActuatorClasses(new ActuatorClass[] { ActuatorClass.Temperature })]
public class ValveActuator : LogicalDevice
{
	private const string PropNameValveIndex = "ValveIndex";

	private const string PropNameValveType = "ValveType";

	private const string PropNameControlMode = "ControlMode";

	private HECTemperatureDeviceSettings hecSettings;

	[XmlIgnore]
	public int ValveIndex
	{
		get
		{
			return (int)(base.Properties.GetDecimalValue("ValveIndex") ?? 0m);
		}
		set
		{
			base.Properties.SetDecimal("ValveIndex", value);
		}
	}

	[XmlIgnore]
	public ValveType? ValveType
	{
		get
		{
			string stringValue = base.Properties.GetStringValue("ValveType");
			return (!string.IsNullOrEmpty(stringValue)) ? ((ValveType?)Enum.Parse(typeof(ValveType), stringValue, ignoreCase: true)) : ((ValveType?)null);
		}
		set
		{
			if (!value.HasValue)
			{
				base.Properties.Delete("ValveType");
			}
			else
			{
				base.Properties.SetString("ValveType", value.ToString());
			}
		}
	}

	[XmlIgnore]
	public ControlMode? ControlMode
	{
		get
		{
			string stringValue = base.Properties.GetStringValue("ControlMode");
			return (!string.IsNullOrEmpty(stringValue)) ? ((ControlMode?)Enum.Parse(typeof(ControlMode), stringValue, ignoreCase: true)) : ((ControlMode?)null);
		}
		set
		{
			if (!value.HasValue)
			{
				base.Properties.Delete("ControlMode");
			}
			else
			{
				base.Properties.SetString("ControlMode", value.ToString());
			}
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

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		ValveActuator valveActuator = (ValveActuator)clone;
		if (hecSettings != null)
		{
			valveActuator.HECSettings = (HECTemperatureDeviceSettings)hecSettings.Clone();
		}
	}

	protected override Entity CreateClone()
	{
		return new ValveActuator();
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

	public new ValveActuator Clone()
	{
		return (ValveActuator)base.Clone();
	}

	public new ValveActuator Clone(Guid tag)
	{
		return (ValveActuator)base.Clone(tag);
	}
}
