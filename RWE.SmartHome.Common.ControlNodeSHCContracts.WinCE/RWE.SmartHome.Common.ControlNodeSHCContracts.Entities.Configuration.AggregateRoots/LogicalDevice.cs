using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Attributes;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

[XmlInclude(typeof(RollerShutterActuator))]
[XmlInclude(typeof(AlarmActuator))]
[XmlInclude(typeof(ThermostatActuator))]
[XmlInclude(typeof(Router))]
[XmlType("LD")]
[XmlInclude(typeof(MotionDetectionSensor))]
[XmlInclude(typeof(DimmerActuator))]
[XmlInclude(typeof(SwitchActuator))]
[XmlInclude(typeof(ValveActuator))]
[XmlInclude(typeof(RoomSetpoint))]
[XmlInclude(typeof(WindowDoorSensor))]
[XmlInclude(typeof(RoomTemperature))]
[XmlInclude(typeof(LuminanceSensor))]
[XmlInclude(typeof(TemperatureSensor))]
[XmlInclude(typeof(RoomHumidity))]
[XmlInclude(typeof(SmokeDetectorSensor))]
[XmlInclude(typeof(HumiditySensor))]
[XmlInclude(typeof(PushButtonSensor))]
public class LogicalDevice : Entity
{
	private const string EXCEPTION_NO_ACTUATORSETTINGS_DEFINED = "There is no ActuatorSettings type defined for the type: {0}";

	private const string EXCEPTION_NO_DEVICE_STATE_DEFINED = "There is no LogicalDeviceState type defined for the type: {0}";

	private Guid? locationId;

	private string name = "";

	private Guid baseDeviceId;

	[XmlElement(ElementName = "DvTp")]
	public string DeviceType { get; set; }

	[XmlArray(ElementName = "Ppts")]
	[XmlArrayItem(ElementName = "Ppt")]
	public List<Property> Properties { get; set; }

	[XmlAttribute]
	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value ?? string.Empty;
		}
	}

	[XmlIgnore]
	public Guid? LocationId
	{
		get
		{
			return locationId;
		}
		set
		{
			locationId = value;
		}
	}

	[XmlElement(ElementName = "LCID", IsNullable = true)]
	public string LocationIdString
	{
		get
		{
			return locationId.HasValue ? locationId.Value.ToString() : null;
		}
		set
		{
			locationId = (string.IsNullOrEmpty(value) ? ((Guid?)null) : new Guid?(new Guid(value)));
		}
	}

	[XmlIgnore]
	public Location Location
	{
		get
		{
			return Resolver.ToLocation(this, locationId);
		}
		set
		{
			locationId = Resolver.FromLocation(value);
		}
	}

	[XmlElement(ElementName = "BDId")]
	public Guid BaseDeviceId
	{
		get
		{
			return baseDeviceId;
		}
		set
		{
			baseDeviceId = value;
		}
	}

	[XmlIgnore]
	public BaseDevice BaseDevice
	{
		get
		{
			return Resolver.ToBaseDevice(this, baseDeviceId);
		}
		set
		{
			baseDeviceId = Resolver.FromBaseDevice(value);
		}
	}

	[XmlElement(ElementName = "PPpN")]
	public virtual string PrimaryPropertyName { get; set; }

	[XmlElement(ElementName = "dal")]
	public bool? ActivityLogActive { get; set; }

	public LogicalDevice()
	{
		Properties = new List<Property>();
	}

	public LogicalDevice(string name, Location location)
	{
		Name = name;
		Location = location;
	}

	protected override Entity CreateClone()
	{
		return new LogicalDevice();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		LogicalDevice logicalDevice = (LogicalDevice)clone;
		logicalDevice.Name = Name;
		logicalDevice.LocationId = LocationId;
		logicalDevice.baseDeviceId = baseDeviceId;
		logicalDevice.Properties = Properties.Select((Property p) => p.Clone()).ToList();
		logicalDevice.DeviceType = DeviceType;
		logicalDevice.PrimaryPropertyName = PrimaryPropertyName;
		logicalDevice.ActivityLogActive = ActivityLogActive;
	}

	public new LogicalDevice Clone()
	{
		return (LogicalDevice)base.Clone();
	}

	public new LogicalDevice Clone(Guid tag)
	{
		return (LogicalDevice)base.Clone(tag);
	}

	public virtual LogicalDeviceState CreateState()
	{
		LogicalDeviceStateTypeAttribute classAttribute = GetClassAttribute<LogicalDeviceStateTypeAttribute>(GetType());
		if (classAttribute != null)
		{
			LogicalDeviceState logicalDeviceState = (LogicalDeviceState)Activator.CreateInstance(classAttribute.LogicalDeviceStateType);
			logicalDeviceState.LogicalDevice = this;
			return logicalDeviceState;
		}
		throw new InvalidOperationException($"There is no LogicalDeviceState type defined for the type: {GetType().Name}");
	}

	public virtual List<Property> GetAllProperties()
	{
		List<Property> list = Properties.Select((Property x) => x).ToList();
		list.Add(new StringProperty
		{
			Name = "Type",
			Value = DeviceType
		});
		return list;
	}
}
