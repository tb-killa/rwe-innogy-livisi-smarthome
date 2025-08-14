using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogic.ProtocolMultiplexer;

internal static class ProtocolMultiplexerHelpers
{
	public static BaseDevice GetTargetBasedevice(IRepository repository, LinkBinding link)
	{
		if (link.LinkType == EntityType.LogicalDevice)
		{
			LogicalDevice logicalDevice = repository.GetLogicalDevice(link.EntityIdAsGuid());
			if (logicalDevice != null)
			{
				return logicalDevice.BaseDevice;
			}
		}
		else if (link.LinkType == EntityType.BaseDevice)
		{
			return repository.GetBaseDevice(link.EntityIdAsGuid());
		}
		return null;
	}

	public static ProtocolIdentifier GetProtocolIdentifier(LogicalDevice device)
	{
		ProtocolIdentifier result = ProtocolIdentifier.Cosip;
		if (device.BaseDevice != null)
		{
			result = device.BaseDevice.ProtocolId;
		}
		return result;
	}
}
