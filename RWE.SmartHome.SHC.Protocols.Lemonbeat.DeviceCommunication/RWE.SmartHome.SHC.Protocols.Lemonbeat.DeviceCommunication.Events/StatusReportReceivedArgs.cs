using System;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StatusReports;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;

public class StatusReportReceivedArgs : EventArgs
{
	public DeviceIdentifier Device { get; private set; }

	public StatusReport StatusReport { get; private set; }

	public StatusReportReceivedArgs(DeviceIdentifier device, StatusReport report)
	{
		Device = device;
		StatusReport = report;
	}
}
