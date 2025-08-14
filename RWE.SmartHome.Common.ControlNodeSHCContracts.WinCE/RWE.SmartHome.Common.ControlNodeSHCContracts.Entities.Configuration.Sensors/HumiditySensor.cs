using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.SensorStates;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;

[LogicalDeviceStateType(typeof(HumiditySensorState))]
public class HumiditySensor : LogicalDevice
{
	[XmlElement(ElementName = "MPrA")]
	public bool IsMoldProtectionActivated { get; set; }

	[XmlElement(ElementName = "HMPr")]
	public decimal HumidityMoldProtection { get; set; }

	[XmlIgnore]
	public override string PrimaryPropertyName
	{
		get
		{
			return "Humidity";
		}
		set
		{
		}
	}

	public new HumiditySensor Clone()
	{
		return (HumiditySensor)base.Clone();
	}

	public new HumiditySensor Clone(Guid tag)
	{
		return (HumiditySensor)base.Clone(tag);
	}

	protected override Entity CreateClone()
	{
		return new HumiditySensor();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		HumiditySensor humiditySensor = clone as HumiditySensor;
		humiditySensor.IsMoldProtectionActivated = IsMoldProtectionActivated;
		humiditySensor.HumidityMoldProtection = HumidityMoldProtection;
	}

	public override List<Property> GetAllProperties()
	{
		List<Property> allProperties = base.GetAllProperties();
		allProperties.Add(new BooleanProperty
		{
			Name = "IsMoldProtectionActivated",
			Value = IsMoldProtectionActivated
		});
		allProperties.Add(new NumericProperty
		{
			Name = "HumidityMoldProtection",
			Value = HumidityMoldProtection
		});
		return allProperties;
	}
}
