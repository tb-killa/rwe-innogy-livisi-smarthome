using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

[XmlType("BD")]
public class BaseDevice : Entity
{
	private string manufacturer;

	private string deviceType;

	private string deviceVersion;

	private List<Guid> logicalDeviceIds;

	private Guid? locationId;

	[XmlAttribute(AttributeName = "SN")]
	public string SerialNumber { get; set; }

	[XmlAttribute(AttributeName = "Name")]
	public string Name { get; set; }

	[XmlAttribute(AttributeName = "PrId")]
	public ProtocolIdentifier ProtocolId { get; set; }

	[XmlArrayItem(ElementName = "Ppt")]
	[XmlArray(ElementName = "Ppts")]
	public List<Property> Properties { get; set; }

	[XmlAttribute]
	public string AppId { get; set; }

	[XmlElement(ElementName = "TOD")]
	public DateTime? TimeOfDiscovery { get; set; }

	[XmlElement(ElementName = "TOA")]
	public DateTime? TimeOfAcceptance { get; set; }

	[XmlAttribute(AttributeName = "DMnf")]
	public string Manufacturer
	{
		get
		{
			if (manufacturer == null)
			{
				manufacturer = string.Empty;
			}
			return manufacturer;
		}
		set
		{
			manufacturer = value;
		}
	}

	[XmlAttribute(AttributeName = "DTyp")]
	public string DeviceType
	{
		get
		{
			if (deviceType == null)
			{
				deviceType = string.Empty;
			}
			return deviceType;
		}
		set
		{
			deviceType = value;
		}
	}

	[XmlArray(ElementName = "LDIds")]
	public List<Guid> LogicalDeviceIds
	{
		get
		{
			if (logicalDeviceIds == null)
			{
				logicalDeviceIds = new List<Guid>();
			}
			return logicalDeviceIds;
		}
		set
		{
			logicalDeviceIds = value;
		}
	}

	[XmlAttribute(AttributeName = "DVer")]
	public string DeviceVersion
	{
		get
		{
			if (deviceVersion == null)
			{
				deviceVersion = string.Empty;
			}
			return deviceVersion;
		}
		set
		{
			deviceVersion = value;
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

	[XmlIgnore]
	public bool VolatilePropertiesSpecified { get; set; }

	public List<Property> VolatileProperties { get; set; }

	public BaseDevice()
	{
		Properties = new List<Property>();
		logicalDeviceIds = new List<Guid>();
		VolatilePropertiesSpecified = true;
	}

	public new BaseDevice Clone()
	{
		return (BaseDevice)base.Clone();
	}

	public new BaseDevice Clone(Guid tag)
	{
		return (BaseDevice)base.Clone(tag);
	}

	protected override Entity CreateClone()
	{
		return new BaseDevice();
	}

	protected override void TransferProperties(Entity clone)
	{
		base.TransferProperties(clone);
		BaseDevice baseDevice = (BaseDevice)clone;
		baseDevice.SerialNumber = SerialNumber;
		baseDevice.Name = Name;
		baseDevice.Properties = Properties.Select((Property prop) => prop.Clone()).ToList();
		baseDevice.ProtocolId = ProtocolId;
		baseDevice.AppId = AppId;
		baseDevice.Manufacturer = Manufacturer;
		baseDevice.DeviceType = DeviceType;
		baseDevice.DeviceVersion = DeviceVersion;
		baseDevice.TimeOfDiscovery = TimeOfDiscovery;
		baseDevice.TimeOfAcceptance = TimeOfAcceptance;
		baseDevice.LocationId = LocationId;
		baseDevice.LogicalDeviceIds.AddRange(LogicalDeviceIds);
	}
}
