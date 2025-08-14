using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.DeviceState;

public class GetAllPhysicalDeviceStatesResponse : BaseResponse
{
	public List<PhysicalDeviceState> DeviceStates { get; set; }
}
