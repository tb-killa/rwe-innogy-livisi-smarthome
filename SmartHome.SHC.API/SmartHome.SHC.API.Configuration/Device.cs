using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SmartHome.SHC.API.PropertyDefinition;

namespace SmartHome.SHC.API.Configuration;

public class Device
{
	public Guid Id { get; private set; }

	public IEnumerable<Property> Properties { get; private set; }

	public string SerialNumber { get; private set; }

	public DateTime TimeOfDiscovery { get; private set; }

	public DateTime TimeOfInclusion { get; private set; }

	public string Manufacturer { get; set; }

	public string DeviceType { get; set; }

	public string DeviceVersion { get; set; }

	public string Name { get; set; }

	public List<Property> VolatileProperties { get; set; }

	public Device(Guid id, IEnumerable<Property> properties, string serialNumber, DateTime discoveryTime, DateTime inclusionTime)
		: this(id, properties, serialNumber, discoveryTime, inclusionTime, string.Empty, string.Empty, string.Empty, string.Empty)
	{
	}

	public Device(Guid id, IEnumerable<Property> properties, string serialNumber, DateTime discoveryTime, DateTime inclusionTime, string manufacturer, string deviceType, string deviceVersion, string name)
	{
		Id = id;
		Properties = new ReadOnlyCollection<Property>(properties.ToList());
		SerialNumber = serialNumber;
		TimeOfDiscovery = discoveryTime;
		TimeOfInclusion = inclusionTime;
		Manufacturer = manufacturer;
		DeviceType = deviceType;
		DeviceVersion = deviceVersion;
		Name = name;
	}
}
