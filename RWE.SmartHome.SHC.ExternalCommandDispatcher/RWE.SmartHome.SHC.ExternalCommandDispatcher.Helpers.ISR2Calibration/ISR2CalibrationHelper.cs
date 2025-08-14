using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Helpers.ISR2Calibration;

public class ISR2CalibrationHelper
{
	public static bool IsRequestForRollerShutterCapability(SetEntitiesRequest request)
	{
		if (request.HomeSetups.Count == 0 && request.Interactions.Count == 0 && request.Locations.Count == 0 && request.BaseDevices.Count == 0 && request.LogicalDevices.Count == 1)
		{
			LogicalDevice logicalDevice = request.LogicalDevices[0];
			if (logicalDevice != null && logicalDevice.DeviceType == "RollerShutterActuator")
			{
				return logicalDevice is RollerShutterActuator;
			}
			return false;
		}
		return false;
	}
}
