using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;

public class UsbDeviceConnectionChangedEventArgs : EventArgs
{
	public ProtocolIdentifier ProtocolIdentifier { get; set; }

	public bool Connected { get; set; }
}
