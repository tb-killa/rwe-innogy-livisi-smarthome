using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.DeviceState;

public class GetPhysicalDeviceStateResponse : BaseResponse
{
	public PhysicalDeviceState DeviceState { get; set; }
}
