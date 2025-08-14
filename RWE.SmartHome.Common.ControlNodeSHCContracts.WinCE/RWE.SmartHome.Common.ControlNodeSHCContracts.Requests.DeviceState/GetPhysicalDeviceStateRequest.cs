using System;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.DeviceState;

public class GetPhysicalDeviceStateRequest : BaseRequest
{
	public Guid PhysicalDeviceId { get; set; }
}
