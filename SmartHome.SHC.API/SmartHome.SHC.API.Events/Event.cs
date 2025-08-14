using System.Collections.Generic;
using System.Linq;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace SmartHome.SHC.API.Events;

public class Event
{
	public string EventType { get; set; }

	public Link Link { get; set; }

	public Property[] Data { get; private set; }

	public Event(IEnumerable<Property> data)
	{
		Data = data.ToArray();
	}
}
