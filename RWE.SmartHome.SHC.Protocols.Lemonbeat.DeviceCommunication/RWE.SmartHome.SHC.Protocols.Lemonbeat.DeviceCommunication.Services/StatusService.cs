using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Status;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StatusReports;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StatusReports.Enums;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class StatusService : DuplexService<network>, IStatusService
{
	private const string DefaultNamespace = "urn:statusxsd";

	public event EventHandler<StatusReportReceivedArgs> StatusReportReceived;

	public StatusService(ILemonbeatCommunication aggregator)
		: base(aggregator, ServiceType.Status, "urn:statusxsd")
	{
	}

	protected override void Handle(int gatewayId, IPAddress remoteAddress, network message)
	{
		DeviceIdentifier identifier = new DeviceIdentifier(remoteAddress, null, gatewayId);
		ProcessAllValues(identifier, message);
	}

	private void OnStatusReportReceived(StatusReportReceivedArgs e)
	{
		this.StatusReportReceived?.Invoke(this, e);
	}

	private void ProcessAllValues(DeviceIdentifier identifier, network message)
	{
		new Dictionary<DeviceIdentifier, ValueCollection>();
		if (message == null || message.device == null)
		{
			return;
		}
		try
		{
			networkDevice[] device = message.device;
			foreach (networkDevice networkDevice in device)
			{
				if (networkDevice.Items == null)
				{
					continue;
				}
				foreach (statusReportType item in networkDevice.Items.OfType<statusReportType>())
				{
					StatusReport report = new StatusReport(item.type_id, item.level, item.code, item.data);
					OnStatusReportReceived(new StatusReportReceivedArgs(identifier, report));
					LogStatusReport(identifier, item.type_id, item.level, item.code, item.data);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Error processing status report: " + ex.Message);
		}
	}

	private string GetErrorCode(StatusType statusType, uint statusCode)
	{
		string empty = string.Empty;
		return statusType switch
		{
			StatusType.DeviceDescription => ((StatusDeviceDescription)statusCode).ToString(), 
			StatusType.ValueDescription => ((StatusValueDescription)statusCode).ToString(), 
			StatusType.Value => ((StatusValue)statusCode).ToString(), 
			StatusType.PartnerInformation => ((StatusPartnerInformation)statusCode).ToString(), 
			StatusType.Action => ((StatusAction)statusCode).ToString(), 
			StatusType.Calculation => ((StatusCalculation)statusCode).ToString(), 
			StatusType.Timer => ((StatusTimer)statusCode).ToString(), 
			StatusType.Calendar => ((StatusCalendar)statusCode).ToString(), 
			StatusType.StateMachine => ((StatusStateMachine)statusCode).ToString(), 
			StatusType.FirmwareUpdate => ((StatusFirmwareUpdate)statusCode).ToString(), 
			StatusType.Configuration => ((StatusConfiguration)statusCode).ToString(), 
			StatusType.System => ((StatusSystem)statusCode).ToString(), 
			_ => statusCode.ToString(), 
		};
	}

	private void LogStatusReport(DeviceIdentifier identifier, uint type_id, uint level, uint code, byte[] data)
	{
		string errorCode = GetErrorCode((StatusType)type_id, code);
		string text = ((data == null) ? string.Empty : string.Join(" ", data.Select((byte m) => m.ToString("X2")).ToArray()));
		string message = $"Status Report from device {identifier} [{(StatusLevel)level}]: type {(StatusType)type_id} - code {errorCode}. Additional data: {text}";
		switch ((StatusLevel)level)
		{
		case StatusLevel.Fatal:
		case StatusLevel.Error:
			Log.Error(Module.LemonbeatProtocolAdapter, message);
			break;
		case StatusLevel.Warning:
			Log.Warning(Module.LemonbeatProtocolAdapter, message);
			break;
		case StatusLevel.Info:
			Log.Information(Module.LemonbeatProtocolAdapter, message);
			break;
		default:
			Log.Debug(Module.LemonbeatProtocolAdapter, message);
			break;
		}
	}
}
