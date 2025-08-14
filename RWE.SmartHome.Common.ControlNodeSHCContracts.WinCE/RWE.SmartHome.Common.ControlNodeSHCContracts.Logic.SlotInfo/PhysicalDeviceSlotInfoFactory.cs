using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Logic.SlotInfo;

public class PhysicalDeviceSlotInfoFactory
{
	public ISlotInfo CreateSlotInfo(BaseDevice baseDevice)
	{
		if (baseDevice.AppId == CoreConstants.CoreAppId && baseDevice.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.FSC8)
		{
			return new EightSlotDevice();
		}
		return new OneSlotDevice();
	}
}
