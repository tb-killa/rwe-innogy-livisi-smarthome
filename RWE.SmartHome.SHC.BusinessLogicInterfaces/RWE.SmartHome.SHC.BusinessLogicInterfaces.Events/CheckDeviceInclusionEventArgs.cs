using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class CheckDeviceInclusionEventArgs
{
	public BaseDevice Device { get; set; }

	public byte[] Sgtin { get; set; }
}
