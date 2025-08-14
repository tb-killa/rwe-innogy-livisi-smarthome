using System.Collections.Generic;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Control;

public class GetAllLogicalDeviceStatesRequest : BaseRequest
{
	public List<string> DeviceIds { get; set; }
}
