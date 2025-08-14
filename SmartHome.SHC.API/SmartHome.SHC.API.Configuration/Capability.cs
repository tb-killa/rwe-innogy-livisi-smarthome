using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SmartHome.SHC.API.PropertyDefinition;

namespace SmartHome.SHC.API.Configuration;

public class Capability
{
	public string Name { get; private set; }

	public Guid Id { get; private set; }

	public string DeviceType { get; private set; }

	public IEnumerable<Property> Properties { get; private set; }

	public Guid DeviceId { get; private set; }

	public string PrimaryPropertyName { get; private set; }

	public Capability(string name, Guid id, IEnumerable<Property> properties, Guid physicalDeviceId, string deviceType, string primaryPropertyName)
	{
		Name = name;
		Id = id;
		DeviceId = physicalDeviceId;
		Properties = new ReadOnlyCollection<Property>(new List<Property>(properties));
		DeviceType = deviceType;
		PrimaryPropertyName = primaryPropertyName;
	}
}
