using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StatusReports.Enums;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.FirmwareUpdate;

public class LemonbeatDeviceFirmwareUpdater : IProtocolSpecificDeviceUpdater
{
	private class QueuedDeviceUpdate
	{
		public Guid DeviceId { get; private set; }

		public DeviceFirmwareDescriptor Firmware { get; private set; }

		public QueuedDeviceUpdate(Guid deviceId, DeviceFirmwareDescriptor firmware)
		{
			DeviceId = deviceId;
			Firmware = firmware;
		}
	}

	private const string LoggingSource = "LemonbeatFirmwareUpdate";

	private FirmwareUpdateService firmwareUpdateService;

	private List<QueuedDeviceUpdate> firmwareTransferQueue;

	private SubscriptionToken deviceInclusionStateToken;

	private IStatusService statusService;

	private IEventManager eventManager;

	private IApplicationsHost appHost;

	private IDeviceList deviceList;

	private ILemonbeatPersistence lemonbeatPersistence;

	private readonly Dictionary<Guid, byte> deviceUpdateRetries;

	private Thread transferQueueThread;

	private QueuedDeviceUpdate currentUpdate;

	public event EventHandler<UpdateFailedEventArgs> UpdateFailed;

	public LemonbeatDeviceFirmwareUpdater(IEventManager eventManager, FirmwareUpdateService service, IDeviceList deviceList, IStatusService statusService, ILemonbeatPersistence persistence, IApplicationsHost appHost)
	{
		firmwareUpdateService = service;
		this.deviceList = deviceList;
		this.statusService = statusService;
		lemonbeatPersistence = persistence;
		this.eventManager = eventManager;
		this.appHost = appHost;
		deviceUpdateRetries = new Dictionary<Guid, byte>();
		currentUpdate = null;
		SubscribeToEvents();
		firmwareTransferQueue = new List<QueuedDeviceUpdate>();
	}

	private void DeviceDoUpdate(DeviceInformation device)
	{
		try
		{
			if (device.DeviceUpdateState == LemonbeatDeviceUpdateState.UpdatePending)
			{
				FirmwareUpdateStatus firmwareUpdateStatus = firmwareUpdateService.DoUpdate(device.Identifier);
				Log.Information(Module.LemonbeatProtocolAdapter, string.Concat("Firmware update for ", device, " ended with status [", firmwareUpdateStatus.StatusCode, "]"));
				if (firmwareUpdateStatus.StatusCode == UpdateStatusCode.OK)
				{
					SetUpdateState(device.DeviceId, LemonbeatDeviceUpdateState.Updating);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, string.Concat("Unexpected error when sending update command to device ", device, ":", ex.Message));
		}
	}

	private void TransferDeviceUpdate(Guid deviceId, string fileName)
	{
		DeviceInformation deviceInformation = deviceList[deviceId];
		if (deviceInformation == null)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", "Unable to start firmware transfer for non existing device!");
			return;
		}
		for (int i = 0; i <= 3; i++)
		{
			try
			{
				FirmwareInformation firmwareInformation = firmwareUpdateService.GetFirmwareInformation(deviceInformation.Identifier);
				if (firmwareInformation == null)
				{
					continue;
				}
				Log.Debug(Module.LemonbeatProtocolAdapter, "Firmware info received for " + deviceInformation.ToString());
				using FileStream updateStream = new FileStream(fileName, FileMode.Open);
				DeviceFirmwareUpdate update = new DeviceFirmwareUpdate(updateStream, (int)firmwareInformation.ChunkSize);
				Log.Debug(Module.LemonbeatProtocolAdapter, "Start transfer");
				if (TransferUpdateData(deviceInformation, firmwareInformation, update))
				{
					SetUpdateState(deviceInformation.DeviceId, LemonbeatDeviceUpdateState.ImageTransferred);
				}
				break;
			}
			catch (TimeoutException)
			{
				if (i < 3)
				{
					Log.Debug(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", "Timeout received while getting firmware info from device. Retrying " + (3 - i) + " time(s).");
					Thread.Sleep(5000);
					continue;
				}
				Log.Debug(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", "Firmware information retrieval failed too many times. Giving up.");
				break;
			}
			catch (Exception ex2)
			{
				Log.Error(Module.LemonbeatProtocolAdapter, string.Concat("Unexpected error while updating device ", deviceInformation, ":", ex2.Message));
				break;
			}
		}
	}

	private bool TransferUpdateData(DeviceInformation device, FirmwareInformation firmwareInfo, DeviceFirmwareUpdate update)
	{
		if (firmwareInfo.ReceivedSize == update.Size)
		{
			return true;
		}
		int num = 0;
		int num2 = 0;
		uint num3 = 0u;
		while (num3 < update.NumberOfChunks)
		{
			if (device.DeviceUpdateState != LemonbeatDeviceUpdateState.TransferInProgress || currentUpdate == null)
			{
				return false;
			}
			if (num > 2)
			{
				Log.Debug(Module.LemonbeatProtocolAdapter, "Too many errors detected during fw update. Stopping update...");
				SignalDeviceUpdateFailure(device.DeviceId, FailedUpdateStep.Transfer);
				return false;
			}
			byte[] getChunkData = update.GetGetChunkData(num3);
			try
			{
				FirmwareUpdateStatus firmwareUpdateStatus = firmwareUpdateService.TransferUpdate(device.Identifier, getChunkData, num3 * firmwareInfo.ChunkSize);
				switch (firmwareUpdateStatus.StatusCode)
				{
				case UpdateStatusCode.OK:
					num3++;
					num = 0;
					break;
				case UpdateStatusCode.WrongOffset:
					if (firmwareUpdateStatus.ExpectedOffset == update.Size)
					{
						Log.Debug(Module.LemonbeatProtocolAdapter, "Device update already transferred.");
						return true;
					}
					num3 = update.GetChunkIndexByOffset((int)firmwareUpdateStatus.ExpectedOffset.Value);
					Log.Debug(Module.LemonbeatProtocolAdapter, "Skip to chunk " + num3);
					num++;
					break;
				case UpdateStatusCode.NotInitialized:
					firmwareUpdateStatus = firmwareUpdateService.FirmwareUpdateInit(device.Identifier, update.FirmwareID, update.Checksum, update.Size);
					num3 = 0u;
					num++;
					Log.Debug(Module.LemonbeatProtocolAdapter, "Initializing FW update. ");
					break;
				default:
					Log.Error(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", "Error occured while transferring device update: " + firmwareUpdateStatus.StatusCode);
					num++;
					break;
				}
				num2 = 0;
			}
			catch (TimeoutException)
			{
				if (num2 < 3)
				{
					Log.Debug(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", "Timeout received while sending update packet. Retrying " + (3 - num2) + " times.");
					num2++;
					Thread.Sleep(5000);
					continue;
				}
				Log.Debug(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", "Update packet sending timed out too many times. Giving up.");
				return false;
			}
		}
		return true;
	}

	private void AbortDeviceUpdate(Guid deviceId)
	{
		try
		{
			deviceUpdateRetries.Remove(deviceId);
			lock (firmwareTransferQueue)
			{
				if (currentUpdate != null && currentUpdate.DeviceId == deviceId)
				{
					currentUpdate = null;
				}
				QueuedDeviceUpdate queuedDeviceUpdate = firmwareTransferQueue.FirstOrDefault((QueuedDeviceUpdate du) => du.DeviceId == deviceId);
				if (queuedDeviceUpdate != null)
				{
					firmwareTransferQueue.Remove(queuedDeviceUpdate);
				}
			}
			DeviceInformation deviceInformation = deviceList[deviceId];
			if (deviceInformation != null && deviceInformation.DeviceInclusionState == LemonbeatDeviceInclusionState.Included && !deviceInformation.IsReachable)
			{
				SetUpdateState(deviceId, LemonbeatDeviceUpdateState.UpToDate);
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", "Error while aborting device update: " + ex.Message);
		}
	}

	private void HandleDeviceInclusionStateChanged(DeviceInclusionStateChangedEventArgs args)
	{
		DeviceInformation deviceInformation = deviceList[args.DeviceId];
		if (deviceInformation != null)
		{
			SetUpdateState(args.DeviceId, LemonbeatDeviceUpdateState.UpToDate);
		}
	}

	private void LemonbeatDeviceStatusReceived(object sender, StatusReportReceivedArgs args)
	{
		StatusType type = args.StatusReport.Type;
		if (type != StatusType.FirmwareUpdate)
		{
			return;
		}
		StatusFirmwareUpdate code = (StatusFirmwareUpdate)args.StatusReport.Code;
		if (code == StatusFirmwareUpdate.FirmwareUpdateFailedToUpgrade)
		{
			DeviceInformation deviceInformation = deviceList[args.Device];
			if (deviceInformation == null)
			{
				Log.Error(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", "Received firmware update failure status report for unknown device [" + args.Device.ToString() + "]");
				return;
			}
			SetUpdateState(deviceInformation.DeviceId, LemonbeatDeviceUpdateState.UpToDate);
			SignalDeviceUpdateFailure(deviceInformation.DeviceId, FailedUpdateStep.Update);
		}
	}

	private void SubscribeToEvents()
	{
		if (deviceInclusionStateToken == null)
		{
			DeviceInclusionStateChangedEvent deviceInclusionStateChangedEvent = eventManager.GetEvent<DeviceInclusionStateChangedEvent>();
			deviceInclusionStateToken = deviceInclusionStateChangedEvent.Subscribe(HandleDeviceInclusionStateChanged, (DeviceInclusionStateChangedEventArgs args) => args.DeviceInclusionState == DeviceInclusionState.Included || args.DeviceInclusionState == DeviceInclusionState.FactoryReset || args.DeviceInclusionState == DeviceInclusionState.Excluded, ThreadOption.BackgroundThread, null);
		}
		statusService.StatusReportReceived += LemonbeatDeviceStatusReceived;
	}

	private void SetUpdateState(Guid deviceId, LemonbeatDeviceUpdateState state)
	{
		try
		{
			DeviceInformation deviceInformation = deviceList[deviceId];
			if (deviceInformation == null)
			{
				Log.Error(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", string.Concat("Trying to set update state for non existing device [", deviceId, "]"));
			}
			else if (deviceInformation.DeviceUpdateState != state)
			{
				deviceInformation.DeviceUpdateState = state;
				lemonbeatPersistence.SaveInTransaction(deviceInformation, suppressEvent: false);
				Log.Information(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", $"Changing update state of {deviceInformation.ToString()} to {state}");
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", "Unexpected error while setting device update state: " + ex.Message);
		}
	}

	private void EnqueueDevice(DeviceInformation device, DeviceFirmwareDescriptor firmwareDescriptor)
	{
		try
		{
			lock (firmwareTransferQueue)
			{
				if (firmwareTransferQueue.Any((QueuedDeviceUpdate upd) => upd.DeviceId == device.DeviceId))
				{
					Log.Debug(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", "Device already queued for update");
					return;
				}
				QueuedDeviceUpdate item = new QueuedDeviceUpdate(device.DeviceId, firmwareDescriptor);
				SetUpdateState(device.DeviceId, LemonbeatDeviceUpdateState.UpdateAvailable);
				if (device.DeviceDescription.RadioMode == RadioMode.WakeOnEvent)
				{
					firmwareTransferQueue.Insert(0, item);
					if (currentUpdate != null)
					{
						firmwareTransferQueue.Add(currentUpdate);
						SetUpdateState(currentUpdate.DeviceId, LemonbeatDeviceUpdateState.UpdateAvailable);
					}
				}
				else
				{
					firmwareTransferQueue.Add(item);
				}
			}
			ProcessTransferQueue();
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", "Unexpected error while enqueuing device for firmware transfer: " + ex.Message);
		}
	}

	private void RemoveQueuedDevice(Guid deviceId)
	{
		lock (firmwareTransferQueue)
		{
			QueuedDeviceUpdate queuedDeviceUpdate = firmwareTransferQueue.FirstOrDefault((QueuedDeviceUpdate upd) => upd.DeviceId == deviceId);
			if (queuedDeviceUpdate == null)
			{
				Log.Debug(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", "Device not queued for udpate.");
			}
			else
			{
				firmwareTransferQueue.Remove(queuedDeviceUpdate);
			}
		}
	}

	private QueuedDeviceUpdate DequeueDeviceTransfer()
	{
		lock (firmwareTransferQueue)
		{
			if (firmwareTransferQueue.Count > 0)
			{
				QueuedDeviceUpdate result = firmwareTransferQueue[0];
				firmwareTransferQueue.RemoveAt(0);
				return result;
			}
		}
		return null;
	}

	private void ProcessTransferQueue()
	{
		if (transferQueueThread == null)
		{
			transferQueueThread = new Thread(DoFirmwareTransfer);
			transferQueueThread.Start();
		}
	}

	private void DoFirmwareTransfer()
	{
		QueuedDeviceUpdate queuedDeviceUpdate = DequeueDeviceTransfer();
		do
		{
			SetUpdateState(queuedDeviceUpdate.DeviceId, LemonbeatDeviceUpdateState.TransferInProgress);
			currentUpdate = queuedDeviceUpdate;
			TransferDeviceUpdate(queuedDeviceUpdate.DeviceId, queuedDeviceUpdate.Firmware.ImageFile);
			queuedDeviceUpdate = DequeueDeviceTransfer();
		}
		while (queuedDeviceUpdate != null);
		transferQueueThread = null;
		currentUpdate = null;
	}

	private void SignalDeviceUpdateFailure(Guid deviceId, FailedUpdateStep updateStep)
	{
		try
		{
			this.UpdateFailed?.Invoke(this, new UpdateFailedEventArgs
			{
				DeviceId = deviceId,
				UpdateStep = updateStep
			});
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", "Unexpected error while signalizing device update failure: " + ex.Message);
		}
	}

	public List<DeviceUpdateInfo> GetDeviceInfo()
	{
		List<DeviceUpdateInfo> list = new List<DeviceUpdateInfo>();
		try
		{
			foreach (DeviceInformation item in deviceList.SyncWhere((DeviceInformation d) => d.DeviceInclusionState == LemonbeatDeviceInclusionState.Included))
			{
				list.Add(item.GetDeviceUpdateInfo(appHost));
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", "Unexpected error encountered while retrieving device list: " + ex.Message);
		}
		return list;
	}

	public DeviceUpdateInfo GetDeviceInfo(Guid deviceId)
	{
		DeviceInformation deviceInformation = deviceList[deviceId];
		if (deviceInformation == null || deviceInformation.DeviceInclusionState != LemonbeatDeviceInclusionState.Included)
		{
			return null;
		}
		return deviceInformation.GetDeviceUpdateInfo(appHost);
	}

	public void EnqueueFirmwareTransfer(Guid deviceId, DeviceFirmwareDescriptor firmware)
	{
		DeviceInformation deviceInformation = deviceList[deviceId];
		if (deviceInformation == null)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "LemonbeatFirmwareUpdate", "Trying to enqueue firmware transfer for non existing device [" + deviceId.ToString() + "]");
		}
		else
		{
			EnqueueDevice(deviceInformation, firmware);
		}
	}

	public void AbortUpdate(Guid deviceId)
	{
		AbortDeviceUpdate(deviceId);
	}

	public void PerformUpdate(List<Guid> deviceIdList)
	{
		foreach (Guid deviceId in deviceIdList)
		{
			DeviceInformation deviceInformation = deviceList[deviceId];
			if (deviceInformation == null)
			{
				break;
			}
			SetUpdateState(deviceInformation.DeviceId, LemonbeatDeviceUpdateState.UpdatePending);
			DeviceDoUpdate(deviceInformation);
		}
	}
}
