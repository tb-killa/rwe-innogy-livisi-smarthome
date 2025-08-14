using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SmartHome.SHC.API.PropertyDefinition;

namespace SmartHome.SHC.API.Configuration;

public class ActionDescription
{
	public Guid Id { get; private set; }

	public Link Target { get; private set; }

	public string ActionType { get; private set; }

	public IEnumerable<Property> Properties { get; private set; }

	public ActionDescription(Guid id, Link target, string actionType, IEnumerable<Property> properties)
	{
		Id = id;
		Target = target;
		ActionType = actionType;
		Properties = new ReadOnlyCollection<Property>(new List<Property>(properties));
	}
}
