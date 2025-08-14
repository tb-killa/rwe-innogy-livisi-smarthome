using System;
using System.Collections.Generic;
using SmartHome.SHC.API.PropertyDefinition;

namespace SmartHome.SHC.API.Configuration;

public class CustomTrigger
{
	public Guid Id { get; set; }

	public string TriggerType { get; set; }

	public IEnumerable<Property> Properties { get; set; }

	public Link Entity { get; set; }
}
