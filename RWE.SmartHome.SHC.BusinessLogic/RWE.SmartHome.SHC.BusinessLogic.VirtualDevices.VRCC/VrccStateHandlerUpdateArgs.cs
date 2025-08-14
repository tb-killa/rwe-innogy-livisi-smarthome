using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

public class VrccStateHandlerUpdateArgs
{
	public IEnumerable<ActionDescription> StateUpdateActions { get; private set; }

	public VrccStateHandlerUpdateArgs(IEnumerable<ActionDescription> setStateActions)
	{
		StateUpdateActions = setStateActions;
	}
}
