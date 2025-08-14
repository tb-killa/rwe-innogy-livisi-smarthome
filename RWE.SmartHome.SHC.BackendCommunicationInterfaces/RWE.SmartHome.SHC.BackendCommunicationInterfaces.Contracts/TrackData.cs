using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

public class TrackData
{
	public string EventType { get; set; }

	public string EntityType { get; set; }

	public DateTime Timestamp { get; set; }

	public string EntityId { get; set; }

	public List<Property> Properties { get; set; }

	public string DeviceId { get; set; }
}
