using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceInclusion;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.CoreApiConverters;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.FirmwareUpdate;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;
using Rebex.Security.Cryptography;
using SmartHome.SHC.API;
using SmartHome.SHC.API.PropertyDefinition;
using SmartHome.SHC.API.Protocols.Lemonbeat;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;

internal class DeviceInclusionController : IInclusionController
{
	private const int dongleInclusionRetryLimit = 3;

	private const string testDeviceKey = "00030100010a66791dc6988168de7ab77419bb7fb0c001c62710270075142942e19a8d8c51d053b3e3782a1de5dc5af4ebe99468170114a1dfe67cdc9a9af55d655620bbab";

	private const uint SEQUENCE_COUNTER_VALUE_ID = 1u;

	private readonly IDeviceList deviceList;

	private readonly ILemonbeatPersistence persistence;

	private readonly IDeviceDescriptionService deviceDescriptionService;

	private readonly IValueService valueService;

	private readonly ICalendarService calendarService;

	private readonly INetworkManagementService networkManagementService;

	private readonly IEventManager eventManager;

	private readonly IApplicationsHost appHost;

	private readonly IScheduler scheduler;

	private DeviceInformation dongle;

	private List<string> searchedAppIds;

	private bool deviceInclusionActivated;

	private bool useTestKeys;

	private string networkKey;

	private uint DONGLE_SUB_DEVICE = 1u;

	private readonly TimeSpan timeOutForNotIncludedDevices = new TimeSpan(0, 1, 0, 0);

	private bool DeviceInclusionActivated
	{
		get
		{
			return deviceInclusionActivated;
		}
		set
		{
			bool flag = deviceInclusionActivated;
			deviceInclusionActivated = value;
			if (value && !flag)
			{
				RaiseEventForNewlyFoundDevices();
			}
		}
	}

	public event Action DongleInitialized;

	public DeviceInclusionController(IDeviceList deviceList, ILemonbeatPersistence deviceListPersistence, INetworkManagementService networkManagementService, IDeviceDescriptionService deviceDescriptionService, IValueService valueService, ICalendarService calendarService, IEventManager eventManager, IScheduler scheduler, IApplicationsHost appHost)
	{
		this.deviceDescriptionService = deviceDescriptionService;
		this.networkManagementService = networkManagementService;
		this.deviceList = deviceList;
		persistence = deviceListPersistence;
		this.deviceDescriptionService.DeviceDescriptionReceived += OnDeviceDescriptionReceived;
		this.eventManager = eventManager;
		this.scheduler = scheduler;
		this.appHost = appHost;
		this.valueService = valueService;
		this.calendarService = calendarService;
		eventManager.GetEvent<CheckDeviceInclusionResultEvent>().Subscribe(OnBackendRequestKeyResponse, null, ThreadOption.BackgroundThread, null);
		deviceList.DeviceReachabilityChanged += OnDeviceReachabilityChanged;
		valueService.SetValueCompleted += SetValueCompleted;
		eventManager.GetEvent<DeviceDiscoveryStatusChangedEvent>().Subscribe(OnDeviceDiscoveryStatusChanged, null, ThreadOption.PublisherThread, null);
	}

	internal DeviceInclusionController(IDeviceList deviceList, ILemonbeatPersistence deviceListPersistence, INetworkManagementService networkManagementService, IDeviceDescriptionService deviceDescriptionService, IValueService valueService, ICalendarService calendarService, IEventManager eventManager, IScheduler scheduler, IApplicationsHost appHost, bool useTestKeys)
		: this(deviceList, deviceListPersistence, networkManagementService, deviceDescriptionService, valueService, calendarService, eventManager, scheduler, appHost)
	{
		this.useTestKeys = useTestKeys;
	}

	public void DropDiscoveredDevices(BaseDevice[] devices)
	{
		foreach (BaseDevice baseDevice in devices)
		{
			deviceList.Remove(baseDevice.Id);
		}
	}

	public void IncludeAsync(DeviceInformation device)
	{
		if (device.DeviceInclusionState == LemonbeatDeviceInclusionState.PublicKeyReceived || device.DeviceInclusionState == LemonbeatDeviceInclusionState.InclusionPending)
		{
			device.DeviceInclusionState = LemonbeatDeviceInclusionState.InclusionPending;
			persistence.SaveInTransaction(device, suppressEvent: false);
			Thread thread = new Thread((ThreadStart)delegate
			{
				IncludeInternal(device);
			});
			thread.Start();
		}
		else
		{
			Log.Information(Module.LemonbeatProtocolAdapter, $"Device {device} was requested to be included but it has an incompatible state: {device.DeviceInclusionState}");
		}
	}

	public void ExcludeAsync(DeviceInformation device)
	{
		device.DeviceInclusionState = LemonbeatDeviceInclusionState.ExclusionPending;
		persistence.DeleteInTransaction(device.DeviceId, suppressEvent: false);
		ThreadPool.QueueUserWorkItem(delegate
		{
			SendExclusionMessage(device);
		});
	}

	public bool InitializeDongle(IPAddress address)
	{
		try
		{
			Log.Information(Module.LemonbeatProtocolAdapter, $"Trying to initialize dongle with address: {address.ToString()}");
			DeviceDescription deviceDescription = deviceDescriptionService.GetDeviceDescription(new DeviceIdentifier(address, DONGLE_SUB_DEVICE, LemonbeatUsbDongle.LemonbeatUSBDongleGatewayID));
			if (deviceDescription == null)
			{
				Log.Information(Module.LemonbeatProtocolAdapter, "Unable to initialize dongle");
				return false;
			}
			dongle = UpdateDongleInfo(address, deviceDescription);
			if (!deviceDescription.Included)
			{
				dongle.DeviceInclusionState = LemonbeatDeviceInclusionState.Found;
				RequestEncryptionKeyFromBackend(dongle);
			}
			else
			{
				Log.Information(Module.LemonbeatProtocolAdapter, "Dongle was already initialized, possible before reboot or software update");
				dongle.DeviceInclusionState = LemonbeatDeviceInclusionState.Included;
				FireDongleReadyEvent();
			}
			Log.Information(Module.LemonbeatProtocolAdapter, "Using Lemonbeat network key: " + GetNetworkKey());
			return true;
		}
		catch (Exception ex)
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, "Failed to initialize dongle: " + ex);
		}
		return false;
	}

	public void ResetDeviceInclusionState(Guid deviceId)
	{
		DeviceInformation deviceInformation = deviceList[deviceId];
		if (deviceInformation != null)
		{
			deviceInformation.DeviceInclusionState = LemonbeatDeviceInclusionState.PublicKeyReceived;
			persistence.DeleteInTransaction(deviceId, suppressEvent: false);
		}
	}

	private void RaiseDeviceKeyRetrievalFailedEvent(DeviceInformation device, EncryptedKeyResponseStatus backendResponse)
	{
		eventManager.GetEvent<PhysicalDeviceFoundEvent>().Publish(new DeviceFoundEventArgs
		{
			FoundDevice = new BaseDevice
			{
				Id = device.DeviceId,
				TimeOfDiscovery = device.DeviceFound,
				ProtocolId = ProtocolIdentifier.Lemonbeat,
				SerialNumber = RweSerialNumberStrategy.CreateSerialNumberFromSgtin(device.DeviceDescription.SGTIN)
			},
			State = DeviceFoundStateFromKeyState(backendResponse)
		});
	}

	private void IncludeDongle(IPAddress address)
	{
		lock (dongle)
		{
			dongle.DeviceInclusionState = LemonbeatDeviceInclusionState.InclusionPending;
			IncludeInternal(dongle);
			if (deviceList.SyncWhere((DeviceInformation d) => d.DeviceInclusionState == LemonbeatDeviceInclusionState.Included).Count() == 0)
			{
				ScanChannels();
				SetUsedChannels(dongle);
			}
			RestoreSequenceCounter();
		}
	}

	private void ScanChannels()
	{
	}

	private void SetUsedChannels(DeviceInformation device)
	{
	}

	private DeviceInformation UpdateDongleInfo(IPAddress address, DeviceDescription dongleDescription)
	{
		DeviceInformation deviceInformation = persistence.LoadDongle();
		if (deviceInformation == null)
		{
			deviceInformation = new DeviceInformation(Guid.NewGuid(), new DeviceIdentifier(address, DONGLE_SUB_DEVICE, LemonbeatUsbDongle.LemonbeatUSBDongleGatewayID), dongleDescription, ShcDateTime.UtcNow);
			persistence.SaveDongle(deviceInformation, suppressEvent: false);
		}
		else if (!deviceInformation.DeviceDescription.SGTIN.Equals(dongleDescription.SGTIN))
		{
			deviceInformation.DeviceDescription = dongleDescription;
			deviceInformation.Identifier = new DeviceIdentifier(address, DONGLE_SUB_DEVICE, LemonbeatUsbDongle.LemonbeatUSBDongleGatewayID);
			deviceInformation.DeviceKey = null;
			persistence.SaveDongle(deviceInformation, suppressEvent: false);
		}
		return deviceInformation;
	}

	private void OnDeviceReachabilityChanged(object sender, DeviceReachabilityChangedEventArgs e)
	{
		if (!e.IsReachable)
		{
			return;
		}
		if (e.Device.DeviceInclusionState == LemonbeatDeviceInclusionState.InclusionPending)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				IncludeInternal(e.Device);
			});
		}
		if (e.Device.DeviceInclusionState == LemonbeatDeviceInclusionState.ExclusionPending)
		{
			SendExclusionMessage(e.Device);
		}
	}

	private DeviceInformation GetNewDeviceInformation(DeviceIdentifier identifier, DeviceDescription deviceDescription)
	{
		DeviceInformation deviceInformation = new DeviceInformation(Guid.NewGuid(), identifier, deviceDescription, ShcDateTime.UtcNow);
		deviceInformation.DeviceInclusionState = LemonbeatDeviceInclusionState.Found;
		DeviceInformation deviceInformation2 = deviceInformation;
		deviceList.AddDevice(deviceInformation2);
		return deviceInformation2;
	}

	private void OnDeviceDescriptionReceived(object sender, DeviceDescriptionReceivedArgs e)
	{
		DeviceInformation device;
		if (!e.DeviceDescription.Included)
		{
			if (e.DeviceDescription.IsDongle)
			{
				return;
			}
			device = deviceList[e.DeviceDescription.SGTIN];
			if (device == null)
			{
				device = GetNewDeviceInformation(e.Identifier, e.DeviceDescription);
			}
			device.DeviceDescription = e.DeviceDescription;
			device.DeviceFound = ShcDateTime.UtcNow;
			switch (device.DeviceInclusionState)
			{
			case LemonbeatDeviceInclusionState.Found:
				RequestEncryptionKeyFromBackend(device);
				break;
			case LemonbeatDeviceInclusionState.ExclusionPending:
				Log.Information(Module.LemonbeatProtocolAdapter, $"Exclusion of device {device} successful.");
				device.DeviceInclusionState = LemonbeatDeviceInclusionState.Found;
				deviceList.Remove(device.DeviceId);
				device = GetNewDeviceInformation(e.Identifier, e.DeviceDescription);
				RequestEncryptionKeyFromBackend(device);
				break;
			case LemonbeatDeviceInclusionState.InclusionInProgress:
				Log.Debug(Module.LemonbeatProtocolAdapter, $"Device description received from device: {device}, but the inclusion is already in progress.");
				break;
			case LemonbeatDeviceInclusionState.InclusionPending:
				ThreadPool.QueueUserWorkItem(delegate
				{
					IncludeInternal(device);
				});
				break;
			case LemonbeatDeviceInclusionState.Included:
				if (device.DeviceUpdateState == LemonbeatDeviceUpdateState.Updating)
				{
					HandleDeviceFirmwareUpdated(device);
				}
				else
				{
					UpdateInclusionState(device, LemonbeatDeviceInclusionState.FactoryReset);
				}
				break;
			default:
				Log.Warning(Module.LemonbeatProtocolAdapter, "Received description from not included device but the device state is incompatibe. Device:" + device);
				break;
			case LemonbeatDeviceInclusionState.PublicKeyReceived:
			case LemonbeatDeviceInclusionState.FactoryReset:
			case LemonbeatDeviceInclusionState.PublicKeyRetrievalPending:
			case LemonbeatDeviceInclusionState.PublicKeyMissing:
				break;
			}
		}
		else if (e.DeviceDescription.IsDongle)
		{
			dongle.DeviceInclusionState = LemonbeatDeviceInclusionState.Included;
			Log.Information(Module.LemonbeatProtocolAdapter, "Dongle included successfully.");
		}
		else
		{
			device = deviceList[e.DeviceDescription.SGTIN];
			if (device == null)
			{
				Log.Error(Module.LemonbeatProtocolAdapter, "Device reports itself as included, but not present in device list! Please DFR the device.");
			}
			else if (device.DeviceInclusionState == LemonbeatDeviceInclusionState.InclusionInProgress || device.DeviceInclusionState == LemonbeatDeviceInclusionState.InclusionPending)
			{
				Log.Information(Module.LemonbeatProtocolAdapter, $"Received device description from device {device}, device is now included (encryption key exchange done).");
				device.DeviceInclusionState = LemonbeatDeviceInclusionState.Included;
				persistence.SaveInTransaction(device, suppressEvent: false);
			}
		}
	}

	private void HandleDeviceFirmwareUpdated(DeviceInformation device)
	{
		if (device != null)
		{
			UpdateInclusionState(device, LemonbeatDeviceInclusionState.InclusionPending);
		}
	}

	private void UpdateInclusionState(DeviceInformation device, LemonbeatDeviceInclusionState inclusionState)
	{
		device.DeviceInclusionState = inclusionState;
		device.ServiceDescriptions = null;
		device.MemoryInformation = null;
		device.ValueDescriptions = null;
		switch (inclusionState)
		{
		case LemonbeatDeviceInclusionState.FactoryReset:
			persistence.SaveInTransaction(device, suppressEvent: false);
			break;
		case LemonbeatDeviceInclusionState.InclusionPending:
			persistence.DeleteInTransaction(device.DeviceId, suppressEvent: false);
			break;
		}
	}

	private void RequestEncryptionKeyFromBackend(DeviceInformation device)
	{
		SGTIN96 sGTIN = device.DeviceDescription.SGTIN;
		CheckDeviceInclusionEventArgs e = new CheckDeviceInclusionEventArgs();
		e.Sgtin = sGTIN.GetSerialData().ToArray();
		CheckDeviceInclusionEventArgs payload = e;
		Log.Debug(Module.LemonbeatProtocolAdapter, $"GetDeviceKey: SGTIN={sGTIN.ToString()}");
		device.DeviceInclusionState = LemonbeatDeviceInclusionState.PublicKeyRetrievalPending;
		if (device.Identifier.GatewayId == LemonbeatUsbDongle.LemonbeatUSBDongleGatewayID)
		{
			if (useTestKeys)
			{
				OnBackendRequestKeyResponse(new GetDeviceKeyEventArgs
				{
					DeviceKey = "00030100010a66791dc6988168de7ab77419bb7fb0c001c62710270075142942e19a8d8c51d053b3e3782a1de5dc5af4ebe99468170114a1dfe67cdc9a9af55d655620bbab".ToByteArray(),
					Result = EncryptedKeyResponseStatus.Success,
					Sgtin = sGTIN
				});
			}
			else
			{
				eventManager.GetEvent<GetDeviceKeyEvent>().Publish(payload);
			}
			return;
		}
		byte[] deviceKey = new byte[8] { 1, 35, 69, 103, 137, 171, 205, 239 };
		OnBackendRequestKeyResponse(new GetDeviceKeyEventArgs
		{
			DeviceKey = deviceKey,
			Result = EncryptedKeyResponseStatus.Success,
			Sgtin = sGTIN
		});
	}

	private void OnBackendRequestKeyResponse(GetDeviceKeyEventArgs args)
	{
		SGTIN96 sgtin = args.Sgtin;
		DeviceInformation deviceInformation = deviceList[sgtin];
		if (dongle != null && dongle.DeviceDescription.SGTIN.Equals(sgtin))
		{
			switch (args.Result)
			{
			case EncryptedKeyResponseStatus.BackendServiceNotReachable:
				if (dongle.DeviceKey != null)
				{
					Log.Information(Module.LemonbeatProtocolAdapter, "Backend service not available. Trying to include dongle with the last known key");
					IncludeDongle(dongle.Identifier.IPAddress);
				}
				break;
			case EncryptedKeyResponseStatus.Success:
			{
				string text = args.DeviceKey.ToReadable();
				if (dongle.DeviceKey != text)
				{
					dongle.DeviceKey = text;
					persistence.SaveDongle(dongle, suppressEvent: false);
				}
				IncludeDongle(dongle.Identifier.IPAddress);
				break;
			}
			default:
				Log.Error(Module.LemonbeatProtocolAdapter, "Could not retrieve key for Lemonbeat dongle. Backend returned: " + args.Result);
				break;
			}
		}
		else if (deviceInformation != null && (dongle != null || deviceInformation.Identifier.GatewayId != LemonbeatUsbDongle.LemonbeatUSBDongleGatewayID))
		{
			if (args.Result == EncryptedKeyResponseStatus.Success)
			{
				deviceInformation.DeviceKey = args.DeviceKey.ToReadable();
				deviceInformation.DeviceInclusionState = LemonbeatDeviceInclusionState.PublicKeyReceived;
				RaiseEventForNewlyFoundDevice(deviceInformation);
			}
			else
			{
				deviceInformation.DeviceInclusionState = LemonbeatDeviceInclusionState.PublicKeyMissing;
				RaiseDeviceKeyRetrievalFailedEvent(deviceInformation, args.Result);
			}
		}
	}

	private void RestoreSequenceCounter()
	{
		try
		{
			uint num = persistence.LoadSequenceCounter();
			num++;
			byte[] array = BitConverter.GetBytes(num);
			if (BitConverter.IsLittleEndian)
			{
				array = array.Reverse().ToArray();
			}
			valueService.SetValueAsync(dongle.Identifier, 1u, array, RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Transport.Udp, new KeyValuePair<string, uint>("sequenceCounter", num));
			Log.Information(Module.LemonbeatProtocolAdapter, $"Lemonbeat sequence counter had been updated! New value: {num}");
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, $"There was an error in setting the sequence counter: {ex.Message}");
		}
	}

	private void SetValueCompleted(object sender, AsyncCompletedEventArgs args)
	{
		try
		{
			if (args.Error == null && !args.Cancelled)
			{
				if (args.UserState != null && args.UserState is KeyValuePair<string, uint> && ((KeyValuePair<string, uint>)args.UserState).Key == "sequenceCounter")
				{
					persistence.SaveSequenceCounter(((KeyValuePair<string, uint>)args.UserState).Value);
					Log.Information(Module.LemonbeatProtocolAdapter, $"Lemonbeat sequence number persisted after successful update. New value: {((KeyValuePair<string, uint>)args.UserState).Value}.");
					FireDongleReadyEvent();
				}
			}
			else
			{
				Log.Error(Module.LemonbeatProtocolAdapter, string.Format("There was an error setting Lemonbeat sequence counter. Operation cancelled or other error. Details {0}", (args.Error != null) ? args.Error.Message : ""));
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, $"There was an error setting Lemonbeat sequence counter. Details {ex.ToString()}");
		}
	}

	private void FireDongleReadyEvent()
	{
		this.DongleInitialized?.Invoke();
	}

	private void IncludeInternal(DeviceInformation device)
	{
		if (device.DeviceInclusionState == LemonbeatDeviceInclusionState.InclusionPending)
		{
			device.DeviceInclusionState = LemonbeatDeviceInclusionState.InclusionInProgress;
			int num = 0;
			int num2 = 30000;
			do
			{
				try
				{
					num++;
					Log.Information(Module.LemonbeatProtocolAdapter, "Sending inclusion data to the device, " + num + " attempt");
					SendControllerAndNetworkKey(device);
					int num3 = 0;
					do
					{
						num3 += 5000;
						Thread.Sleep(5000);
					}
					while (num3 < num2 && device.DeviceInclusionState == LemonbeatDeviceInclusionState.InclusionInProgress);
				}
				catch (SocketException ex)
				{
					string text = ((ex.ErrorCode == 10004) ? "(Timeout)" : ("Message: " + ex.Message));
					Log.Warning(Module.LemonbeatProtocolAdapter, $"Inclusion of device {device} failed {num} times, error code: {ex.ErrorCode} {text}");
				}
				catch (Exception ex2)
				{
					Log.Warning(Module.LemonbeatProtocolAdapter, $"Inclusion of device {device} failed {num} times, exception: {((object)ex2).GetType()}, message: {ex2.Message}");
				}
				if (device.DeviceInclusionState != LemonbeatDeviceInclusionState.InclusionInProgress)
				{
					continue;
				}
				Log.Information(Module.LemonbeatProtocolAdapter, "Device description not received, asking for the inclusion state...");
				bool flag = false;
				try
				{
					flag = RequestDeviceInclusionState(device);
				}
				catch (Exception)
				{
					Log.Warning(Module.LemonbeatProtocolAdapter, $"Inclusion state of device {device} could not be retrieved");
				}
				if (flag)
				{
					device.DeviceInclusionState = LemonbeatDeviceInclusionState.Included;
					if (!device.DeviceDescription.IsDongle)
					{
						persistence.SaveInTransaction(device, suppressEvent: false);
					}
					Log.Information(Module.LemonbeatProtocolAdapter, $"No description received from device {device}, but it finally answered to be included");
				}
			}
			while (device.DeviceInclusionState == LemonbeatDeviceInclusionState.InclusionInProgress && num < 3);
			if (device.DeviceInclusionState == LemonbeatDeviceInclusionState.InclusionInProgress)
			{
				Log.Warning(Module.LemonbeatProtocolAdapter, $"Inclusion of device {device} failed");
				device.DeviceInclusionState = LemonbeatDeviceInclusionState.InclusionPending;
			}
		}
		else
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, $"IncludeInternal was called for: {device}, but the user has not marked the device for inclusion yet.Current device state: {device.DeviceInclusionState}");
		}
	}

	private void SendControllerAndNetworkKey(DeviceInformation device)
	{
		if (device.Identifier.GatewayId == LemonbeatUsbDongle.LemonbeatUSBDongleGatewayID)
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Send controller and network keys to device {device}...");
			string text = RandomByteGenerator.Instance.GenerateRandomByteSequence(16u).ToReadable();
			byte[] second = EncryptNetworkKey(text);
			byte[] array = text.ToByteArray().Concat(second).ToArray();
			byte[] data = array.Concat(GetCRC32(array)).ToArray();
			string inclusionData = RSAEncrypt(data, device.DeviceKey).ToReadable();
			networkManagementService.SetInclusionData(device.Identifier, inclusionData);
			device.ControllerKey = text;
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Controller and network keys sent to device {device}");
		}
		else
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Send fake controller and network keys to device {device}...");
			string text2 = device.DeviceDescription.SGTIN.GetSerialData().ToArray().ToReadable();
			networkManagementService.SetInclusionData(device.Identifier, text2);
			deviceDescriptionService.IncludeDevice(device.Identifier);
			device.ControllerKey = text2;
		}
	}

	private bool RequestDeviceInclusionState(DeviceInformation device)
	{
		Log.Debug(Module.LemonbeatProtocolAdapter, $"Retrieve device description from device {device}...");
		DeviceDescription deviceDescription = deviceDescriptionService.GetDeviceDescription(device.Identifier);
		if (deviceDescription == null)
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, "Device description not received");
			return false;
		}
		Log.Debug(Module.LemonbeatProtocolAdapter, string.Format("Device description from device {0} received, device is ", device, deviceDescription.Included));
		return deviceDescription.Included;
	}

	private void SendExclusionMessage(DeviceInformation device)
	{
		try
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Send exclusion to device {device}...");
			deviceDescriptionService.ExcludeDevice(device.Identifier);
		}
		catch (Exception ex)
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, $"Failed to send packet to device {device}. Exception: {((object)ex).GetType()} Message: {ex.Message}");
		}
	}

	private byte[] RSAEncrypt(byte[] data, string deviceKey)
	{
		byte[] result = null;
		using (RSAManaged rSAManaged = new RSAManaged(512))
		{
			try
			{
				RSAParameters rsaParameters = GetRsaParameters(deviceKey);
				rSAManaged.ImportParameters(rsaParameters);
				result = rSAManaged.Encrypt(data);
			}
			catch (Exception ex)
			{
				Log.Error(Module.LemonbeatProtocolAdapter, $"Error occurred while encrypting data: {ex.Message}");
			}
		}
		return result;
	}

	private RSAParameters GetRsaParameters(string deviceKey)
	{
		RSAParameters result = default(RSAParameters);
		ushort num = ushort.Parse(deviceKey.Substring(0, 4));
		string me = deviceKey.Substring(4, num * 2);
		string me2 = deviceKey.Substring(4 + num * 2);
		result.Exponent = me.ToByteArray();
		result.Modulus = me2.ToByteArray();
		return result;
	}

	private byte[] GetCRC32(byte[] data)
	{
		byte[] bytes = BitConverter.GetBytes(LemonbeatCrc.CalcCrc(data, data.Count()));
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse(bytes);
		}
		return bytes;
	}

	private byte[] EncryptNetworkKey(string controllerKey)
	{
		string me = GetNetworkKey();
		byte[] array = new byte[16];
		SymmetricKeyAlgorithm symmetricKeyAlgorithm = new SymmetricKeyAlgorithm(SymmetricKeyAlgorithmId.AES);
		symmetricKeyAlgorithm.SetKey(controllerKey.ToByteArray());
		symmetricKeyAlgorithm.BlockSize = 128;
		symmetricKeyAlgorithm.Mode = CipherMode.ECB;
		symmetricKeyAlgorithm.Padding = PaddingMode.None;
		ICryptoTransform cryptoTransform = symmetricKeyAlgorithm.CreateEncryptor();
		byte[] array2 = me.ToByteArray();
		array = cryptoTransform.TransformFinalBlock(array2, 0, array2.Length);
		cryptoTransform.Dispose();
		return array;
	}

	private string GetNetworkKey()
	{
		if (string.IsNullOrEmpty(networkKey))
		{
			networkKey = persistence.LoadNetworkKey();
			if (string.IsNullOrEmpty(networkKey))
			{
				networkKey = RandomByteGenerator.Instance.GenerateRandomByteSequence(16u).ToReadable();
				persistence.SaveNetworkKey(networkKey, suppressEvent: false);
			}
		}
		return networkKey;
	}

	private void RaiseEventForNewlyFoundDevice(DeviceInformation device)
	{
		if (!DeviceInclusionActivated)
		{
			return;
		}
		BaseDevice baseDevice = new BaseDevice();
		baseDevice.Id = device.DeviceId;
		baseDevice.TimeOfDiscovery = device.DeviceFound;
		baseDevice.ProtocolId = ProtocolIdentifier.Lemonbeat;
		baseDevice.SerialNumber = RweSerialNumberStrategy.CreateSerialNumberFromSgtin(device.DeviceDescription.SGTIN);
		BaseDevice baseDevice2 = baseDevice;
		IDeviceHandler lemonbeatDeviceHandler = appHost.GetLemonbeatDeviceHandler(device.DeviceTypeIdentifier);
		DeviceFoundState state;
		if (lemonbeatDeviceHandler == null)
		{
			state = DeviceFoundState.AddInNotFound;
		}
		else
		{
			state = DeviceFoundState.ReadyForInclusion;
			LemonbeatPhysicalDeviceDescription lemonbeatPhysicalDeviceDescription = lemonbeatDeviceHandler.TranslateDeviceDescription(LemonbeatApiConverters.ToApiDeviceDescription(device.DeviceDescription));
			if (lemonbeatPhysicalDeviceDescription == null || lemonbeatPhysicalDeviceDescription.Status != PhysicalDescriptionStatus.Valid)
			{
				return;
			}
			baseDevice2.Properties = lemonbeatPhysicalDeviceDescription.DeviceProperties.Select((Property p) => p.ToCoreProperty(includeTimestamp: true)).ToList();
			baseDevice2.AppId = (lemonbeatDeviceHandler as IAddIn).ApplicationId;
			baseDevice2.ProtocolId = ProtocolIdentifier.Lemonbeat;
			baseDevice2.Manufacturer = lemonbeatPhysicalDeviceDescription.Manufacturer;
			baseDevice2.DeviceVersion = lemonbeatPhysicalDeviceDescription.Version;
			baseDevice2.DeviceType = lemonbeatPhysicalDeviceDescription.PhysicalDeviceType;
			baseDevice2.Name = lemonbeatPhysicalDeviceDescription.DeviceName;
		}
		if (searchedAppIds == null || (searchedAppIds != null && searchedAppIds.Contains(baseDevice2.AppId)))
		{
			DeviceFoundEventArgs e = new DeviceFoundEventArgs();
			e.State = state;
			e.FoundDevice = baseDevice2;
			DeviceFoundEventArgs payload = e;
			eventManager.GetEvent<PhysicalDeviceFoundEvent>().Publish(payload);
		}
	}

	private void RaiseEventForNewlyFoundDevices()
	{
		DateTime oldestValidDiscoveryTime = ShcDateTime.UtcNow.Subtract(timeOutForNotIncludedDevices);
		foreach (DeviceInformation item in deviceList.SyncWhere((DeviceInformation device) => device.DeviceFound >= oldestValidDiscoveryTime && device.DeviceInclusionState == LemonbeatDeviceInclusionState.PublicKeyReceived))
		{
			RaiseEventForNewlyFoundDevice(item);
		}
	}

	private DeviceFoundState DeviceFoundStateFromKeyState(EncryptedKeyResponseStatus keyState)
	{
		return keyState switch
		{
			EncryptedKeyResponseStatus.InvalidTenant => DeviceFoundState.NoDeviceKeyInvalidTenant, 
			EncryptedKeyResponseStatus.Blacklisted => DeviceFoundState.NoDeviceKeyDeviceBlacklisted, 
			EncryptedKeyResponseStatus.DeviceNotFound => DeviceFoundState.NoDeviceKeyAvailable, 
			EncryptedKeyResponseStatus.BackendServiceNotReachable => DeviceFoundState.NoDeviceKeyBackendUnreachable, 
			_ => DeviceFoundState.Unknown, 
		};
	}

	internal void OnDeviceDiscoveryStatusChanged(DeviceDiscoveryStatusChangedEventArgs args)
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
}
