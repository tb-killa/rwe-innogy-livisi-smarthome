using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.HEC;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;

[PossibleActuatorClasses(new ActuatorClass[] { ActuatorClass.DimmableLight })]
[LogicalDeviceStateType(typeof(DimmerActuatorState))]
public class DimmerActuator : LogicalDevice
{
	public const int DEFAULT_MINVALUE = 30;

	public const int DEFAULT_TECHNICALMINVALUE = 30;

	public const int DEFAULT_TECHNICALMAXVALUE = 100;

	private HECElectricDeviceSettings hecSettings;

	[XmlElement(ElementName = "TMxV")]
	public int TechnicalMaxValue { get; set; }

	[XmlElement(ElementName = "TMnV")]
	public int TechnicalMinValue { get; set; }

	[XmlIgnore]
	public override string PrimaryPropertyName
	{
		get
		{
			return "DimLevel";
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

	public DimmerActuator()
	{
		TechnicalMinValue = 30;
		TechnicalMaxValue = 100;
	}

	protected override Entity CreateClone()
	{
		return new DimmerActuator();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		DimmerActuator dimmerActuator = (DimmerActuator)clone;
		dimmerActuator.TechnicalMaxValue = TechnicalMaxValue;
		dimmerActuator.TechnicalMinValue = TechnicalMinValue;
		if (hecSettings != null)
		{
			dimmerActuator.HECSettings = (HECElectricDeviceSettings)hecSettings.Clone();
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

	public new DimmerActuator Clone()
	{
		return (DimmerActuator)base.Clone();
	}

	public new DimmerActuator Clone(Guid tag)
	{
		return (DimmerActuator)base.Clone(tag);
	}

	public override List<Property> GetAllProperties()
	{
		List<Property> allProperties = base.GetAllProperties();
		allProperties.Add(new NumericProperty
		{
			Name = "TechnicalMaxValue",
			Value = TechnicalMaxValue
		});
		allProperties.Add(new NumericProperty
		{
			Name = "TechnicalMinValue",
			Value = TechnicalMinValue
		});
		return allProperties;
	}
}
