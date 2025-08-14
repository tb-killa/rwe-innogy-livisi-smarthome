using System;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.TaskList;

public class TechnicalConfigurationTask
{
	public Guid DeviceId { get; set; }

	public byte[] DeviceAddress { get; set; }

	public DeviceConfiguration ReferenceConfiguration { get; set; }

	public DeviceConfiguration DefaultConfiguration { get; set; }

	public bool SensorConfigurationChangedByAckDelegate { get; set; }
}
