using System;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Calibration;

public class ResetCalibrationRequest : BaseRequest
{
	public Guid LogicalDeviceId { get; set; }
}
