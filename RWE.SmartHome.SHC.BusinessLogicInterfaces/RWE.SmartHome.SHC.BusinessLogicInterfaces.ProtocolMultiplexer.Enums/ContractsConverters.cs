using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;

public static class ContractsConverters
{
	public static RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums.DeviceConfigurationState ToContractsConfigurationState(this DeviceConfigurationState state)
	{
		return state switch
		{
			DeviceConfigurationState.Complete => RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums.DeviceConfigurationState.Complete, 
			DeviceConfigurationState.Pending => RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums.DeviceConfigurationState.Pending, 
			_ => throw new ArgumentOutOfRangeException("Invalid configuration state"), 
		};
	}

	public static RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums.DeviceInclusionState ToContractsInclusionState(this DeviceInclusionState state)
	{
		switch (state)
		{
		case DeviceInclusionState.Found:
		case DeviceInclusionState.FoundWithAddressCollision:
			return RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums.DeviceInclusionState.Found;
		case DeviceInclusionState.InclusionPending:
			return RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums.DeviceInclusionState.InclusionPending;
		case DeviceInclusionState.Included:
			return RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums.DeviceInclusionState.Included;
		case DeviceInclusionState.FactoryReset:
		case DeviceInclusionState.FactoryResetWithAddressCollision:
			return RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums.DeviceInclusionState.FactoryReset;
		case DeviceInclusionState.ExclusionPending:
			return RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums.DeviceInclusionState.ExclusionPending;
		case DeviceInclusionState.Excluded:
			return RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums.DeviceInclusionState.Excluded;
		default:
			return RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums.DeviceInclusionState.Unknown;
		}
	}
}
