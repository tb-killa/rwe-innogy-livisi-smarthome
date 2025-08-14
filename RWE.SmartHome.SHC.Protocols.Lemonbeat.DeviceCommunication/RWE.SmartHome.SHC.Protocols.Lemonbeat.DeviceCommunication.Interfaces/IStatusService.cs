using System;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface IStatusService
{
	event EventHandler<StatusReportReceivedArgs> StatusReportReceived;
}
