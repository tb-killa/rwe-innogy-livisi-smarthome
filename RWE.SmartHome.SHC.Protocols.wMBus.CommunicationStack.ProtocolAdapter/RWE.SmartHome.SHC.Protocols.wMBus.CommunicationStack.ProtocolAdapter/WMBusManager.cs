using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceInclusion;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceMonitoring;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.CoreApiConverters;
using RWE.SmartHome.SHC.HCI;
using RWE.SmartHome.SHC.HCI.MessageIdentifier;
using RWE.SmartHome.SHC.HCI.Messages;
using RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter.Interfaces;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;
using RWE.SmartHome.SHC.wMBusProtocol;
using SmartHome.SHC.API;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.Control;
using SmartHome.SHC.API.Protocols.wMBus;

namespace RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter;

internal class WMBusManager : IWMBusManager
{
	private class ErrorConditionData
	{
		public MessageType MessageToCreate { get; private set; }

		public byte Mask { get; private set; }

		internal ErrorConditionData(MessageType message, byte mask)
		{
			MessageToCreate = message;
			Mask = mask;
		}
	}

	private class DiscoveryData
	{
		public BaseDevice BaseDevice { get; set; }

		public DeviceFoundState? BackendFoundState { get; set; }

		public WMBusFrame LastInstallationFrame { get; set; }
	}

	private const string Vid = "4292";

	private const string ImstPid = "34797";

	private const int UI_DEVICE_KEY_LENGTH = 8;

	private const int MaximumHeartbeatInterval = 600;

	private readonly Dictionary<string, DiscoveryData> discoveryData = new Dictionary<string, DiscoveryData>();

	private readonly IApplicationsHost applicationsHost;

	private readonly IDeviceList deviceList;

	private readonly IDeviceListPersistence deviceListPersistence;

	private readonly IDeviceMonitor deviceMonitor;

	private readonly IEventManager eventManager;

	private readonly HciClient hciClient;

	private readonly IDeviceKeyRepository deviceKeyRepository;

	private List<string> searchedAppIds;

	private Timer watchDogTimer;

	private Dictionary<Guid, DateTime> lastReadingTimes = new Dictionary<Guid, DateTime>();

	private readonly List<Guid?> keySlotAssignment = new List<Guid?>();

	private readonly string registryKey = string.Format("Drivers\\USB\\ClientDrivers\\CP210xVCP\\Port0\\{0}_{1}", "4292", "34797");

	private readonly TimeSpan timeOutForNotIncludedDevices = new TimeSpan(0, 1, 0, 0);

	private bool deviceInclusionActivated;

	public IDeviceList DeviceList => deviceList;

	private bool DeviceInclusionActivated
	{
		get
		{
			return deviceInclusionActivated;
		}
		set
		{
			if (value && !deviceInclusionActivated)
			{
				RaiseEventForNewlyFoundDevices();
			}
			deviceInclusionActivated = value;
		}
	}

	public WMBusManager(IEventManager eventManager, IDeviceList deviceList, IDeviceListPersistence deviceListPersistence, IApplicationsHost applicationsHost, IDeviceMonitor deviceMonitor, IDeviceKeyRepository deviceKeyRepository)
	{
		this.applicationsHost = applicationsHost;
		this.deviceMonitor = deviceMonitor;
		this.deviceListPersistence = deviceListPersistence;
		this.eventManager = eventManager;
		this.deviceList = deviceList;
		this.deviceKeyRepository = deviceKeyRepository;
		EventHandler<DeviceInclusionStateChangedEventArgs> value = delegate(object sender, DeviceInclusionStateChangedEventArgs args)
		{
			eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Publish(args);
		};
		deviceList.DeviceInclusionStateChanged += value;
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnStartupCompleted, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound1, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<SoftwareUpdateProgressEvent>().Subscribe(OnSoftwareUpdateProgressChanged, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ShutdownEvent>().Subscribe(OnShutdownEvent, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<CheckDeviceInclusionResultEvent>().Subscribe(OnBackendRequestKeyResponse, null, ThreadOption.BackgroundThread, null);
		deviceMonitor.DeviceConnected += DeviceMonitorDeviceConnected;
		hciClient = new HciClient(delegate(string s)
		{
			Log.Information(Module.wMBusProtocolAdapter, s);
		});
		hciClient.ConnectionLost += HCIClientConnectionLost;
		hciClient.HciMessageReceived += DispatchHCIMessage;
		eventManager.GetEvent<DeviceDiscoveryStatusChangedEvent>().Subscribe(OnDeviceDiscoveryStatusChanged, null, ThreadOption.PublisherThread, null);
		SetupWatchdog();
	}

	public void IncludeDevice(Guid deviceId, byte[] decryptionKey)
	{
		IDeviceInformation deviceInformation = deviceList[deviceId];
		if (deviceInformation == null)
		{
			throw ExceptionFactory.GetException(ErrorCode.DeviceDoesNotExist, deviceId);
		}
		byte[] array = new byte[deviceInformation.DeviceIdentifier.Length];
		Array.Copy(deviceInformation.DeviceIdentifier, array, deviceInformation.DeviceIdentifier.Length);
		Array.Reverse(array);
		Log.Information(Module.wMBusProtocolAdapter, $"Successfully included device with id: {array.ToReadable()} / {deviceInformation.DeviceIdentifier.ToReadable()}");
		deviceInformation.DeviceInclusionState = DeviceInclusionState.Included;
		SetAESKey(deviceInformation);
		deviceListPersistence.SaveInTransaction(deviceInformation, suppressEvent: false);
		discoveryData.Remove(deviceInformation.DeviceIdentifier.ToReadable());
	}

	private void PublishGetDevicesKeysEvent(List<byte[]> sgtinsOfIncludedDevices)
	{
		GetDevicesKeysEvent getDevicesKeysEvent = eventManager.GetEvent<GetDevicesKeysEvent>();
		getDevicesKeysEvent.Publish(new GetDevicesKeysEventArgs
		{
			Sgtins = sgtinsOfIncludedDevices
		});
	}

	public void ExcludeDevice(Guid deviceId)
	{
		IDeviceInformation deviceInformation = deviceList[deviceId];
		if (deviceInformation == null)
		{
			throw ExceptionFactory.GetException(ErrorCode.DeviceDoesNotExist, deviceId);
		}
		deviceInformation.DeviceInclusionState = DeviceInclusionState.ExclusionPending;
		int num = keySlotAssignment.IndexOf(deviceId);
		if (num != -1)
		{
			keySlotAssignment[num] = null;
		}
		bool flag = false;
		lock (deviceList.SyncRoot)
		{
			deviceList.Remove(deviceId);
			if (deviceList.Count() == 0)
			{
				flag = true;
			}
		}
		deviceInformation.DeviceInclusionState = DeviceInclusionState.Excluded;
		deviceListPersistence.DeleteInTransaction(deviceId, suppressEvent: false);
		if (flag)
		{
			RaiseUsbDeviceConnectionChangedEvent(connected: true);
		}
	}

	private void HCIClientConnectionLost(string error)
	{
		int num;
		lock (deviceList.SyncRoot)
		{
			num = deviceList.Count();
		}
		if (num > 0)
		{
			RaiseUsbDeviceConnectionChangedEvent(connected: false);
		}
		Log.Error(Module.wMBusProtocolAdapter, error);
	}

	private void DeviceMonitorDeviceConnected(BasicDriverInformation basicDriverInformation)
	{
		if (basicDriverInformation.Key == registryKey && basicDriverInformation.Active)
		{
			StartClient(basicDriverInformation.Name);
			RaiseUsbDeviceConnectionChangedEvent(connected: true);
		}
	}

	private void RaiseEventForNewlyFoundDevices()
	{
		lock (deviceList.SyncRoot)
		{
			BaseDevice baseDevice = null;
			DateTime dateTime = DateTime.UtcNow.Subtract(timeOutForNotIncludedDevices);
			foreach (IDeviceInformation device in deviceList)
			{
				if (device.DeviceInclusionState != DeviceInclusionState.Found && device.DeviceInclusionState != DeviceInclusionState.Excluded)
				{
					continue;
				}
				DeviceFoundState deviceFoundState = DeviceFoundState.ReadyForInclusion;
				string key = device.DeviceIdentifier.ToReadable();
				if (discoveryData[key].BaseDevice == null)
				{
					baseDevice = GetBaseDeviceForInclusion(discoveryData[key].LastInstallationFrame, out var isRequestValid);
					if (!isRequestValid)
					{
						break;
					}
					if (baseDevice == null)
					{
						deviceFoundState = DeviceFoundState.AddInNotFound;
					}
					else
					{
						discoveryData[key].BaseDevice = baseDevice;
					}
				}
				else
				{
					baseDevice = discoveryData[key].BaseDevice;
				}
				if (deviceFoundState != DeviceFoundState.AddInNotFound && device.DeviceFound >= dateTime)
				{
					RaiseNewDeviceFoundEvent(baseDevice, device, deviceFoundState);
				}
			}
		}
	}

	private void OnSoftwareUpdateProgressChanged(SoftwareUpdateProgressEventArgs obj)
	{
		if (obj.State == SoftwareUpdateState.Started)
		{
			hciClient.Pause();
		}
		else if (obj.State == SoftwareUpdateState.Failed)
		{
			hciClient.Resume();
		}
	}

	private void OnShutdownEvent(ShutdownEventArgs obj)
	{
		hciClient.Stop();
	}

	private void OnStartupCompleted(ShcStartupCompletedEventArgs obj)
	{
		lock (lastReadingTimes)
		{
			foreach (IDeviceInformation item in deviceListPersistence.LoadAll())
			{
				deviceList.AddDevice(item);
				lastReadingTimes[item.DeviceId] = DateTime.UtcNow;
				if (item.DeviceInclusionState == DeviceInclusionState.FactoryResetWithAddressCollision || item.DeviceInclusionState == DeviceInclusionState.FactoryReset)
				{
					eventManager.GetEvent<DeviceWasFactoryResetEvent>().Publish(new DeviceWasFactoryResetEventArgs
					{
						DeviceId = item.DeviceId
					});
				}
			}
		}
		DownloadDeviceKeysForIncludedDevices();
		string serialPortName = GetSerialPortName();
		StartClient(serialPortName);
	}

	private void DownloadDeviceKeysForIncludedDevices()
	{
		List<StoredDevice> allDevicesKeysFromStorage = deviceKeyRepository.GetAllDevicesKeysFromStorage();
		List<byte[]> sgtingsToDownload = GetSgtingsToDownload(allDevicesKeysFromStorage);
		if (sgtingsToDownload.Count != 0)
		{
			PublishGetDevicesKeysEvent(sgtingsToDownload);
		}
	}

	private List<byte[]> GetSgtingsToDownload(List<StoredDevice> storedDevices)
	{
		List<byte[]> list = new List<byte[]>();
		lock (deviceList.SyncRoot)
		{
			foreach (IDeviceInformation device in deviceList)
			{
				byte[] vmBusDeviceSgtin = device.SGTIN96.GetSerialData().ToArray();
				if (storedDevices.FirstOrDefault((StoredDevice d) => d.Sgtin.SequenceEqual(vmBusDeviceSgtin)) == null)
				{
					list.Add(vmBusDeviceSgtin);
				}
			}
			return list;
		}
	}

	private void StartClient(string serialPortName)
	{
		if (string.IsNullOrEmpty(serialPortName))
		{
			Log.Information(Module.wMBusProtocolAdapter, "Could not find wM-Bus dongle.");
			return;
		}
		hciClient.Start(serialPortName);
		if (hciClient.Started.WaitOne() && !SetAESKeys())
		{
			Log.Error(Module.wMBusProtocolAdapter, "Failed to set one or more AES keys! Functionality may be limited");
		}
	}

	private string GetSerialPortName()
	{
		BasicDriverInformation basicDriverInformation = deviceMonitor.MonitoredActiveDrivers.FirstOrDefault((BasicDriverInformation driverInfo) => driverInfo.Key == registryKey);
		if (basicDriverInformation.Equals(default(BasicDriverInformation)))
		{
			return string.Empty;
		}
		return basicDriverInformation.Name;
	}

	private bool SetAESKeys()
	{
		bool flag = true;
		foreach (IDeviceInformation device in deviceList)
		{
			if (device.DeviceInclusionState == DeviceInclusionState.Included)
			{
				flag &= SetAESKey(device);
			}
		}
		return flag;
	}

	private bool SetAESKey(IDeviceInformation deviceInformation)
	{
		if (deviceInformation.DecryptionKey == null || deviceInformation.DecryptionKey.Length != 16)
		{
			return true;
		}
		HciMessage hciMessage = new HciMessage();
		hciMessage.Header = new HciHeader();
		hciMessage.Header.EndpointId = EndpointIdentifier.DEVMGMT_ID;
		hciMessage.Header.MessageId = 37;
		SetAES128DecryptionKey setAES128DecryptionKey = new SetAES128DecryptionKey();
		setAES128DecryptionKey.DeviceId = deviceInformation.DeviceIdentifier;
		setAES128DecryptionKey.DeviceType = (byte)deviceInformation.DeviceTypeIdentification;
		setAES128DecryptionKey.Version = deviceInformation.Version;
		setAES128DecryptionKey.ManufacturerID = deviceInformation.ManufacturerCode;
		setAES128DecryptionKey.TableIndex = CalculateSlotNumber(deviceInformation.DeviceId);
		setAES128DecryptionKey.EncryptionKey = deviceInformation.DecryptionKey;
		hciMessage.Payload = setAES128DecryptionKey.GetBytes();
		SimpleResponseMessage simpleResponseMessage = hciClient.Send(hciMessage);
		if (simpleResponseMessage == null || !simpleResponseMessage.Success)
		{
			Log.Error(Module.wMBusProtocolAdapter, "Failed to set AES key: Operation failed (index exceeds table)");
			return false;
		}
		Log.Debug(Module.wMBusProtocolAdapter, "Successfully set AES key for " + deviceInformation.DeviceIdentifier.ToReadable());
		return true;
	}

	private byte CalculateSlotNumber(Guid deviceId)
	{
		int num = keySlotAssignment.IndexOf(deviceId);
		if (num != -1)
		{
			return (byte)num;
		}
		for (int i = 0; i < keySlotAssignment.Count; i++)
		{
			if (!keySlotAssignment[i].HasValue)
			{
				keySlotAssignment[i] = deviceId;
				return (byte)i;
			}
		}
		keySlotAssignment.Add(deviceId);
		return (byte)(keySlotAssignment.Count - 1);
	}

	private void DispatchHCIMessage(HciMessage hciMessage)
	{
		try
		{
			Log.Debug(Module.wMBusProtocolAdapter, "Handling HCI message. Start");
			Log.Debug(Module.wMBusProtocolAdapter, delegate
			{
				StringBuilder stringBuilder = new StringBuilder();
				byte[] payload = hciMessage.Payload;
				foreach (byte b in payload)
				{
					stringBuilder.Append($"{b:X2} ");
				}
				return $"Hex frame payload: {stringBuilder.ToString()}";
			});
			switch (hciMessage.Header.EndpointId)
			{
			case EndpointIdentifier.RADIOLINK_ID:
				HandleRadioLinkMessages(hciMessage);
				break;
			case EndpointIdentifier.DEVMGMT_ID:
				HandleDeviceManagementMessages(hciMessage);
				break;
			}
			Log.Debug(Module.wMBusProtocolAdapter, "Handling HCI message. Done");
		}
		catch (Exception ex)
		{
			Log.Exception(Module.wMBusProtocolAdapter, ex, "Failed to process HCI HciMessage.");
		}
	}

	private void HandleRadioLinkMessages(HciMessage hciMessage)
	{
		if (hciMessage.Header.MessageId == 3)
		{
			byte[] array = new byte[hciMessage.Header.PayloadLength + 1];
			array[0] = hciMessage.Header.PayloadLength;
			Array.Copy(hciMessage.Payload, 0, array, 1, hciMessage.Header.PayloadLength);
			HandleMBusMessage(new WMBusFrame(array));
		}
	}

	private void HandleDeviceManagementMessages(HciMessage hciMessage)
	{
		Log.Debug(Module.wMBusProtocolAdapter, "Received Device Management Frame of type: " + (DeviceManagementMessageIdentifier)hciMessage.Header.MessageId);
	}

	private void HandleMBusMessage(WMBusFrame mBusMessage)
	{
		byte[] array = mBusMessage.Payload.ToArray();
		bool flag = array[3] == 5;
		Log.Debug(Module.wMBusProtocolAdapter, string.Format("Received wMBus Frame of type {0} from {1} meter of manufacturer {2} with identification {3}, {4} encrypted", (ControlCode)mBusMessage.Control, mBusMessage.DeviceTypeIdentification, mBusMessage.Manufacturer, mBusMessage.Identification.ToReadable(), flag ? "" : "NOT "));
		if (flag)
		{
			if (deviceList[mBusMessage.Identification] != null && deviceList[mBusMessage.Identification].DeviceInclusionState == DeviceInclusionState.Included)
			{
				Log.Warning(Module.wMBusProtocolAdapter, "Received encrypted message from included or pending for inclusion device. Key was probably not correctly set");
			}
		}
		else if (mBusMessage.Control == 64)
		{
			Log.Information(Module.wMBusProtocolAdapter, $"Received wMBus Install request aknowledgement from device with identification {mBusMessage.Identification.ToReadable()}; will ignore.");
		}
		else if (array.Length > 5 && (array[4] != 47 || array[5] != 47))
		{
			Log.Warning(Module.wMBusProtocolAdapter, $"Received wMBus Frame from device with identification {mBusMessage.Identification.ToReadable()}, but it couldn't be decrypted with the available key or it has an unrecognized format.");
		}
		else if (mBusMessage.Control == 68)
		{
			UpdateStatus(mBusMessage);
		}
		else if (mBusMessage.Control == 70)
		{
			MBusDeviceFound(mBusMessage);
		}
	}

	private void UpdateStatus(WMBusFrame wmBusFrame)
	{
		IDeviceInformation deviceInformation = deviceList[wmBusFrame.Identification];
		if (deviceInformation == null)
		{
			return;
		}
		deviceInformation.LastTimeActive = ShcDateTime.UtcNow;
		DeviceInclusionState deviceInclusionState = deviceInformation.DeviceInclusionState;
		if (deviceInclusionState != DeviceInclusionState.Included)
		{
			return;
		}
		UpdateLastReadingTime(deviceInformation);
		IWMBusDeviceHandler wMBusDeviceHandler = applicationsHost.GetWMBusDeviceHandler(new WMBusDeviceTypeIdentifier(wmBusFrame.Manufacturer, (global::SmartHome.SHC.API.Protocols.wMBus.DeviceTypeIdentification)wmBusFrame.DeviceTypeIdentification, wmBusFrame.VersionIdentification));
		if (!(wMBusDeviceHandler is IWMBusCurrentStateHandler iWMBusCurrentStateHandler))
		{
			return;
		}
		WMBusDeviceState wMBusDeviceState = wmBusFrame.Convert();
		PhysicalStateTransformationResult physicalStateTransformationResult = null;
		try
		{
			physicalStateTransformationResult = iWMBusCurrentStateHandler.HandlePhysicalState(deviceInformation.DeviceId, wMBusDeviceState);
		}
		catch (Exception ex)
		{
			Log.Error(Module.wMBusProtocolAdapter, "Error coming from add-in when trying to transform device state:\n" + ex.ToString());
		}
		if (physicalStateTransformationResult != null)
		{
			foreach (KeyValuePair<Guid, CapabilityState> capabilityState in physicalStateTransformationResult.CapabilityStates)
			{
				CapabilityState value = capabilityState.Value;
				eventManager.GetEvent<RawLogicalDeviceStateChangedEvent>().Publish(new RawLogicalDeviceStateChangedEventArgs(deviceInformation.DeviceId, value.ToCoreDeviceState(capabilityState.Key)));
			}
		}
		CheckDeviceState(deviceInformation, wMBusDeviceState.DataHeader.Status);
	}

	private void CheckDeviceState(IDeviceInformation deviceInformation, byte status)
	{
		if (!deviceInformation.DeviceStatus.HasValue || (deviceInformation.DeviceStatus.Value & 4) != (status & 4))
		{
			eventManager.GetEvent<DeviceLowBatteryChangedEvent>().Publish(new DeviceLowBatteryChangedEventArgs(deviceInformation.DeviceId, (status & 4) != 0));
			Log.Information(Module.wMBusProtocolAdapter, $"Battery status changed for device {deviceInformation.DeviceIdentifier.ToReadable()}. Low batt = {(status & 4) != 0}");
		}
		deviceInformation.DeviceStatus = status;
	}

	private void MBusDeviceFound(WMBusFrame mBusMessage)
	{
		BaseDevice baseDevice = null;
		Log.Debug(Module.wMBusProtocolAdapter, $"Received installation frame with manufacturer {mBusMessage.Manufacturer}, device type {mBusMessage.DeviceTypeIdentification}, version {mBusMessage.VersionIdentification}");
		if (deviceList.Contains(mBusMessage.Identification) && deviceList[mBusMessage.Identification].DeviceInclusionState == DeviceInclusionState.Included)
		{
			return;
		}
		DeviceFoundState deviceFoundState = DeviceFoundState.ReadyForInclusion;
		string key = mBusMessage.Identification.ToReadable();
		if (!discoveryData.ContainsKey(key))
		{
			discoveryData.Add(key, new DiscoveryData());
		}
		discoveryData[key].LastInstallationFrame = mBusMessage;
		if (discoveryData[key].BaseDevice == null)
		{
			baseDevice = GetBaseDeviceForInclusion(mBusMessage, out var isRequestValid);
			if (baseDevice == null || !isRequestValid)
			{
				return;
			}
			baseDevice.SerialNumber = ToDeviceIdentifier(mBusMessage.Identification);
			discoveryData[key].BaseDevice = baseDevice;
		}
		else
		{
			baseDevice = discoveryData[key].BaseDevice;
		}
		lock (deviceList.SyncRoot)
		{
			IDeviceInformation deviceInformation = deviceList[mBusMessage.Identification];
			if (deviceInformation == null)
			{
				deviceInformation = new DeviceInformation(baseDevice.Id, DeviceInclusionState.Found, ShcDateTime.UtcNow, mBusMessage.Identification, mBusMessage.VersionIdentification, mBusMessage.Manufacturer, mBusMessage.ManufacturerCode, mBusMessage.DeviceTypeIdentification);
				deviceList.AddDevice(deviceInformation);
				lock (lastReadingTimes)
				{
					lastReadingTimes[deviceInformation.DeviceId] = DateTime.UtcNow;
				}
			}
			deviceInformation.LastTimeActive = ShcDateTime.UtcNow;
			if (deviceFoundState != DeviceFoundState.AddInNotFound && (searchedAppIds == null || (searchedAppIds != null && searchedAppIds.Contains(baseDevice.AppId))))
			{
				RaiseNewDeviceFoundEvent(baseDevice, deviceInformation, deviceFoundState);
			}
		}
	}

	private BaseDevice GetBaseDeviceForInclusion(WMBusFrame mBusMessage, out bool isRequestValid)
	{
		BaseDevice baseDevice = null;
		IWMBusDeviceHandler wMBusDeviceHandler = applicationsHost.GetWMBusDeviceHandler(new WMBusDeviceTypeIdentifier(mBusMessage.Manufacturer, (global::SmartHome.SHC.API.Protocols.wMBus.DeviceTypeIdentification)mBusMessage.DeviceTypeIdentification, mBusMessage.VersionIdentification));
		isRequestValid = true;
		if (wMBusDeviceHandler != null)
		{
			Device device = wMBusDeviceHandler.GetDevice(mBusMessage.Convert(), out isRequestValid);
			if (device != null)
			{
				baseDevice = device.ToCoreBaseDevice();
				baseDevice.AppId = wMBusDeviceHandler.ApplicationId;
				baseDevice.ProtocolId = ProtocolIdentifier.wMBus;
			}
			if (!isRequestValid)
			{
				string arg = mBusMessage.Identification.ToReadable();
				Log.Error(Module.wMBusProtocolAdapter, $"Installation request of {arg} was not recognized as valid by {wMBusDeviceHandler.ApplicationId}");
			}
		}
		return baseDevice;
	}

	private void RaiseUsbDeviceConnectionChangedEvent(bool connected)
	{
		eventManager.GetEvent<UsbDeviceConnectionChangedEvent>().Publish(new UsbDeviceConnectionChangedEventArgs
		{
			ProtocolIdentifier = ProtocolIdentifier.wMBus,
			Connected = connected
		});
	}

	private void UpdateBaseDevice(BaseDevice baseDevice, IDeviceInformation device)
	{
		baseDevice.Id = device.DeviceId;
		baseDevice.SerialNumber = ToDeviceIdentifier(device.DeviceIdentifier);
		baseDevice.ProtocolId = ProtocolIdentifier.wMBus;
		baseDevice.TimeOfDiscovery = device.DeviceFound;
	}

	private void RaiseNewDeviceFoundEvent(BaseDevice baseDevice, IDeviceInformation deviceInfo, DeviceFoundState deviceFoundState)
	{
		DeviceFoundState state = DeviceFoundState.AddInNotFound;
		if (deviceFoundState != DeviceFoundState.AddInNotFound)
		{
			string key = deviceInfo.DeviceIdentifier.ToReadable();
			if (!discoveryData[key].BackendFoundState.HasValue)
			{
				byte[] deviceKeyFromCsvStorage = deviceKeyRepository.GetDeviceKeyFromCsvStorage(deviceInfo.SGTIN96);
				if (deviceKeyFromCsvStorage != null)
				{
					OnBackendRequestKeyResponse(new GetDeviceKeyEventArgs
					{
						DeviceKey = deviceKeyFromCsvStorage,
						Result = EncryptedKeyResponseStatus.Success,
						Sgtin = deviceInfo.SGTIN96
					});
				}
				else
				{
					RequestDeviceKeyFromBackend(deviceInfo.SGTIN96);
				}
				return;
			}
			state = discoveryData[key].BackendFoundState.Value;
		}
		UpdateBaseDevice(baseDevice, deviceInfo);
		eventManager.GetEvent<PhysicalDeviceFoundEvent>().Publish(new DeviceFoundEventArgs
		{
			FoundDevice = baseDevice,
			State = state
		});
	}

	private void RequestDeviceKeyFromBackend(SGTIN96 sgtin)
	{
		CheckDeviceInclusionEventArgs e = new CheckDeviceInclusionEventArgs();
		e.Sgtin = sgtin.GetSerialData().ToArray();
		CheckDeviceInclusionEventArgs payload = e;
		Log.Debug(Module.wMBusProtocolAdapter, $"GetDeviceKey: SGTIN={sgtin.ToString()}");
		eventManager.GetEvent<GetDeviceKeyEvent>().Publish(payload);
	}

	private string ToDeviceIdentifier(byte[] devId)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int num = 5; num >= 0; num--)
		{
			if (num >= devId.Length)
			{
				stringBuilder.Append("00");
			}
			else
			{
				stringBuilder.Append($"{devId[num]:X2}");
			}
		}
		return stringBuilder.ToString();
	}

	private void OnBackendRequestKeyResponse(GetDeviceKeyEventArgs args)
	{
		IDeviceInformation deviceInformation = deviceList.Where((IDeviceInformation d) => d.SGTIN96.Equals(args.Sgtin)).FirstOrDefault();
		if (deviceInformation != null)
		{
			discoveryData[deviceInformation.DeviceIdentifier.ToReadable()].BackendFoundState = DeviceFoundStateFromKeyState(args.Result);
			if (args.Result == EncryptedKeyResponseStatus.Success)
			{
				deviceInformation.DecryptionKey = args.DeviceKey;
			}
			else
			{
				Log.Warning(Module.wMBusProtocolAdapter, $"Backend device key service returned {args.Result}, device key will be null");
			}
			BaseDevice baseDevice = discoveryData[deviceInformation.DeviceIdentifier.ToReadable()].BaseDevice;
			eventManager.GetEvent<PhysicalDeviceFoundEvent>().Publish(new DeviceFoundEventArgs
			{
				FoundDevice = baseDevice,
				State = DeviceFoundStateFromKeyState(args.Result)
			});
		}
	}

	private DeviceFoundState DeviceFoundStateFromKeyState(EncryptedKeyResponseStatus keyState)
	{
		return keyState switch
		{
			EncryptedKeyResponseStatus.Success => DeviceFoundState.ReadyForInclusion, 
			EncryptedKeyResponseStatus.InvalidTenant => DeviceFoundState.NoDeviceKeyInvalidTenant, 
			EncryptedKeyResponseStatus.Blacklisted => DeviceFoundState.NoDeviceKeyDeviceBlacklisted, 
			EncryptedKeyResponseStatus.DeviceNotFound => DeviceFoundState.NoDeviceKeyAvailable, 
			EncryptedKeyResponseStatus.BackendServiceNotReachable => DeviceFoundState.NoDeviceKeyBackendUnreachable, 
			_ => DeviceFoundState.Unknown, 
		};
	}

	internal void UpdateLastReadingTime(IDeviceInformation device)
	{
		SetDeviceReachability(device, isReachable: true);
		lock (lastReadingTimes)
		{
			lastReadingTimes[device.DeviceId] = DateTime.UtcNow;
		}
	}

	private void SetDeviceReachability(IDeviceInformation device, bool isReachable)
	{
		lock (deviceList)
		{
			if (device.Reachable != isReachable)
			{
				device.Reachable = isReachable;
				PublishUnreachableStatus(device.DeviceId, !isReachable);
			}
		}
	}

	private void PublishUnreachableStatus(Guid deviceId, bool status)
	{
		eventManager.GetEvent<DeviceUnreachableChangedEvent>().Publish(new DeviceUnreachableChangedEventArgs(deviceId, status));
	}

	private void SetupWatchdog()
	{
		watchDogTimer = new Timer(OnWatchdogCheck, null, TimeSpan.FromSeconds(600.0), TimeSpan.FromSeconds(600.0));
	}

	private void OnWatchdogCheck(object o)
	{
		try
		{
			DateTime utcNow = DateTime.UtcNow;
			foreach (IDeviceInformation device in deviceList)
			{
				if (device.DeviceInclusionState != DeviceInclusionState.Included)
				{
					continue;
				}
				IWMBusDeviceHandler wMBusDeviceHandler = applicationsHost.GetWMBusDeviceHandler(new WMBusDeviceTypeIdentifier(device.Manufacturer, (global::SmartHome.SHC.API.Protocols.wMBus.DeviceTypeIdentification)device.DeviceTypeIdentification, device.Version));
				lock (lastReadingTimes)
				{
					if (lastReadingTimes.TryGetValue(device.DeviceId, out var value))
					{
						TimeSpan timeSpan = TimeSpan.FromSeconds(wMBusDeviceHandler.GetSendingInterval(device.DeviceId).TotalSeconds * 5.0);
						if (utcNow.Subtract(value) > timeSpan && device.Reachable)
						{
							SetDeviceReachability(device, isReachable: false);
						}
					}
					else
					{
						SetDeviceReachability(device, isReachable: false);
						lastReadingTimes[device.DeviceId] = utcNow;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Warning(Module.wMBusProtocolAdapter, "wMBus watchdog failed: " + ex.ToString());
		}
	}

	private void OnDeviceDiscoveryStatusChanged(DeviceDiscoveryStatusChangedEventArgs args)
	{
		if (args.Phase == DiscoveryPhase.Prepare)
		{
			searchedAppIds = args.AppIds;
			DeviceInclusionActivated = true;
		}
		else if (args.Phase == DiscoveryPhase.Deactivate)
		{
			DeviceInclusionActivated = false;
		}
	}

	internal void DropDiscoveredDevices(BaseDevice[] devices)
	{
		foreach (BaseDevice baseDevice in devices)
		{
			IDeviceInformation deviceInformation = deviceList[baseDevice.Id];
			if (deviceInformation != null)
			{
				discoveryData.Remove(deviceInformation.DeviceIdentifier.ToReadable());
			}
			deviceList.Remove(baseDevice.Id);
		}
	}
}
