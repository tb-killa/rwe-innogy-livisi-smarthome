using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;

[LogicalDeviceStateType(typeof(TemperatureSensorState))]
public class TemperatureSensor : LogicalDevice
{
	[XmlElement(ElementName = "FPrA")]
	public bool IsFreezeProtectionActivated { get; set; }

	[XmlElement(ElementName = "FPr")]
	public decimal FreezeProtection { get; set; }

	[XmlIgnore]
	public override string PrimaryPropertyName
	{
		get
		{
			return "Temperature";
		}
		set
		{
		}
	}

	public new TemperatureSensor Clone()
	{
		return (TemperatureSensor)base.Clone();
	}

	public new TemperatureSensor Clone(Guid tag)
	{
		return (TemperatureSensor)base.Clone(tag);
	}

	public override List<Property> GetAllProperties()
	{
		List<Property> allProperties = base.GetAllProperties();
		allProperties.Add(new BooleanProperty
		{
			Name = "IsFreezeProtectionActivated",
			Value = IsFreezeProtectionActivated
		});
		allProperties.Add(new NumericProperty
		{
			Name = "FreezeProtection",
			Value = FreezeProtection
		});
		return allProperties;
	}

	protected override Entity CreateClone()
	{
		return new TemperatureSensor();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		TemperatureSensor temperatureSensor = clone as TemperatureSensor;
		temperatureSensor.IsFreezeProtectionActivated = IsFreezeProtectionActivated;
		temperatureSensor.FreezeProtection = FreezeProtection;
	}
}
