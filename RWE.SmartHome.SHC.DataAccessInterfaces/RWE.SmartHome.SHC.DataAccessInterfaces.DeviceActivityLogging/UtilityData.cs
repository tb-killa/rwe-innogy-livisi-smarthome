using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;

public class UtilityData
{
	public int Id { get; set; }

	public string EntityId { get; set; }

	public string MeterId { get; set; }

	public DateTime Timestamp { get; set; }

	public UtilityType Utility { get; set; }

	public int Value { get; set; }

	public List<Property> Data { get; set; }
}
