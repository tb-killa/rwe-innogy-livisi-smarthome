using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

public interface IStatefulDeviceProvider
{
	IEnumerable<LogicalDevice> GetStatefulDevices(IEnumerable<LogicalDevice> locationDevices);
}
