using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;

public class StaticConfigurationReceivedEventArgs : EventArgs
{
	public Guid DeviceId { get; set; }

	public List<ValueDescription> ValueDescriptions { get; set; }

	public List<ServiceDescription> ServiceDescriptions { get; set; }

	public List<MemoryInformation> MemoryInformation { get; set; }
}
