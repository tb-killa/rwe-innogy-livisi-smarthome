using System;
using System.Collections.Generic;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.FirmwareUpdate;

public class TriggerFirmwareUpdateRequest : BaseRequest
{
	public List<Guid> DevicesToBeUpdated { get; set; }
}
