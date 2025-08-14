using System.Collections.Generic;
using System.Collections.ObjectModel;
using SmartHome.SHC.API.PropertyDefinition;

namespace SmartHome.SHC.API.Control;

public class CapabilityState
{
	public IEnumerable<Property> Properties { get; private set; }

	public bool IsTransient { get; private set; }

	public CapabilityState(IEnumerable<Property> properties)
	{
		Properties = new ReadOnlyCollection<Property>(new List<Property>(properties));
		IsTransient = false;
	}

	public CapabilityState(IEnumerable<Property> properties, bool isTransient)
	{
		Properties = properties;
		IsTransient = isTransient;
	}
}
