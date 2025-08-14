using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

public interface ILogicalDeviceHandler
{
	LogicalDeviceState CreateLogicalDeviceState(LogicalDevice logicalDevice, SortedList<byte, ChannelState> channelStates);
}
