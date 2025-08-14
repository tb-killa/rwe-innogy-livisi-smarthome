using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Calibration;

public class CalibrationNotification : BaseNotification
{
	[XmlAttribute]
	public Guid LogicalDeviceId { get; set; }

	[XmlAttribute]
	public CalibrationStep CalibrationStep { get; set; }

	public int? Value { get; set; }
}
