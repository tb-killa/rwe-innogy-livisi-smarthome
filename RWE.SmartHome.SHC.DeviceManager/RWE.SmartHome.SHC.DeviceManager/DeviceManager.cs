using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceInclusion;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;
using RWE.SmartHome.SHC.DeviceManager.BidCosFactoryResetHandlers;
using RWE.SmartHome.SHC.DeviceManager.Configuration;
using RWE.SmartHome.SHC.DeviceManager.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManager.ErrorHandling;
using RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;
using RWE.SmartHome.SHC.DeviceManager.StateHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceFirmwareUpdate;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;
using SHCWrapper.Misc;
using SerialAPI;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManager;

internal sealed class DeviceManager : IDeviceManager, IService
{
	private const int SendRetries = 3;

	private const int MaxCollisionFrames = 15;

	private List<byte[]> shcAddresses;

	private readonly DeviceList deviceList;

	private readonly ISipCosPersistence persistence;

	private byte[] networkKey;

	private readonly IEventManager eventManager;

	private SubscriptionToken encryptedKeyResponseToken;

	private readonly IDeviceMasterKeyRepository deviceMasterKeyRepository;

	private readonly IDeviceKeyRepository deviceKeyRepository;

	private byte[] syncWord;

	private readonly Dictionary<Guid, ConfigureDeviceTask> pendingConfigurationActions = new Dictionary<Guid, ConfigureDeviceTask>();

	private volatile bool softwareUpdateRunning;

	private readonly object syncRoot = new object();

	private readonly ICommunicationWrapper communicationWrapper;

	private readonly Dictionary<Guid, byte[]> enqueuedTimeInfoSequences = new Dictionary<Guid, byte[]>();

	private readonly List<ReceivedSequence> receivedSwitchCommands = new List<ReceivedSequence>();

	private readonly List<SirFRHandler> sirFRHandlers;

	private readonly IScheduler taskScheduler;

	private DeviceUnreachableHandler unreachableHandler;

	private DeviceStateHandler deviceStateHandler;

	private Timer exclusionPendingCheckTimer;

	private readonly IDeviceDefinitionsProvider deviceDefinitionsProvider;

	private bool deviceInclusionActivated;

	private readonly TimeSpan timeOutForNotIncludedDevices = new TimeSpan(0, 1, 0, 0);

	private readonly RWE.SmartHome.SHC.DeviceManager.Configuration.Configuration configuration;

	public byte[] DefaultShcAddress => shcAddresses[0];

	public IList<byte[]> ShcAddresses => shcAddresses;

	public IDeviceList DeviceList => deviceList;

	public IDeviceController this[IDeviceInformation deviceInformation] => new DeviceController(communicationWrapper.SendScheduler, deviceInformation, communicationWrapper.StatusInfoHandler);

	public IDeviceController this[Guid deviceId]
	{
		get
		{
			IDeviceInformation deviceInformation;
			lock (deviceList.SyncRoot)
			{
				deviceInformation = deviceList[deviceId];
			}
			if (deviceInformation == null)
			{
				return null;
			}
			return new DeviceController(communicationWrapper.SendScheduler, deviceInformation, communicationWrapper.StatusInfoHandler);
		}
	}

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

	public event EventHandler<SequenceFinishedEventArgs> SequenceFinished;

	public DeviceManager(IEventManager eventManager, ISipCosPersistence presistence, IScheduler taskScheduler, ICommunicationWrapper communicationWrapper, DeviceList deviceList, IConfigurationManager configurationManager, IDeviceDefinitionsProvider deviceDefinitionsProvider, IDeviceMasterKeyRepository deviceMasterKeyRepository, IDeviceKeyRepository deviceKeyRepository)
	{
		this.deviceList = deviceList;
		this.communicationWrapper = communicationWrapper;
		EventHandler<DeviceInclusionStateChangedEventArgs> value = delegate(object sender, DeviceInclusionStateChangedEventArgs args)
		{
			eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Publish(args);
		};
		deviceList.DeviceInclusionStateChanged += value;
		deviceList.DeviceConfiguredStateChanged += delegate(object sender, DeviceConfiguredEventArgs args)
		{
			eventManager.GetEvent<DeviceConfiguredEvent>().Publish(args);
		};
		deviceList.DeviceUpdateStateChanged += delegate(object sender, DeviceUpdateStateChangedEventArgs args)
		{
			eventManager.GetEvent<DeviceUpdateStateChangedEvent>().Publish(args);
		};
		this.eventManager = eventManager;
		persistence = presistence;
		this.taskScheduler = taskScheduler;
		this.deviceDefinitionsProvider = deviceDefinitionsProvider;
		sirFRHandlers = new List<SirFRHandler>();
		this.deviceMasterKeyRepository = deviceMasterKeyRepository;
		this.deviceKeyRepository = deviceKeyRepository;
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnDatabaseAvailable, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.DatabaseAvailable, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(StartProcessing, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound1, ThreadOption.PublisherThread, null);
		configuration = new RWE.SmartHome.SHC.DeviceManager.Configuration.Configuration(configurationManager);
		communicationWrapper.StartReception();
	}

	private void OnDatabaseAvailable(ShcStartupCompletedEventArgs args)
	{
		InitializeShcAddresses();
	}

	private void InitializeShcAddresses()
	{
		lock (deviceList.SyncRoot)
		{
			foreach (IDeviceInformation item in persistence.LoadAll())
			{
				deviceList.AddDevice(item);
			}
			deviceList.ForceDetectionOfRouters = true;
		}
		try
		{
			AssignNetworkParameters();
			AddressGenerator addressGenerator = new AddressGenerator(communicationWrapper, deviceList, persistence, 4, 3);
			shcAddresses = addressGenerator.DefineAddresses();
		}
		catch (Exception ex)
		{
			Log.Error(Module.DeviceManager, $"Error occurred while initializing SHC Cosip addresses: {ex.Message}. Rebooting...");
			ResetManager.Reset();
			Thread.Sleep(int.MaxValue);
		}
	}

	private void InitializeInternal()
	{
		communicationWrapper.IcmpHandler.ReceiveData += ReceivedIcmpHandlerData;
		communicationWrapper.SendScheduler.SequenceFinished += SendSchedulerSequenceFinished;
		communicationWrapper.ConfigurationHandler.ReceiveRequestConfigUpdate += ReceivedRequestConfigUpdate;
		communicationWrapper.NetworkHandler.ReceiveDeviceInfo += ReceivedDeviceInfo;
		communicationWrapper.StatusInfoHandler.ReceiveStatus += ReceivedDeviceStatusInfo;
		communicationWrapper.AnswerCommandHandler.ReceiveAnswer += ReceivedAnswer;
		communicationWrapper.TimeInfoHandler.ReceiveTime += ReceivedTimeRequest;
		communicationWrapper.ConditionalSwitchCommandHandler.ReceivedConditionalSwitchCommand += ReceivedSwitchCommand;
		communicationWrapper.UnconditionalSwitchCommandHandler.ReceivedUnconditionalSwitchCommand += ReceivedSwitchCommand;
		communicationWrapper.RouteManagementCommandHandler.ReceiveRouteManagement += ReceivedRouteManagementFrame;
		TimeZoneManager.TimeZoneChanged += OnTimeZoneChanged;
		byte[] version = null;
		CommandExecutor.ExecuteCommand(3, () => communicationWrapper.CommandHandler.GetVersion(out version), "GetVersion");
		Log.Information(Module.DeviceManager, $"Address of SHC {DefaultShcAddress.ToReadable()}");
		Log.Information(Module.DeviceManager, $"Sync Word of SHC {syncWord.ToReadable()}");
		Log.Information(Module.DeviceManager, $"Coprocessor firmware version {version.ToReadable()}");
		LogSequenceCounterIfVerbose();
		deviceStateHandler = new DeviceStateHandler(eventManager, communicationWrapper, DefaultShcAddress, ShcAddresses);
		unreachableHandler = new DeviceUnreachableHandler(deviceList, persistence, communicationWrapper, eventManager, DefaultShcAddress, taskScheduler);
		eventManager.GetEvent<SoftwareUpdateProgressEvent>().Subscribe(SoftwareUpdateProgressChanged, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ShutdownEvent>().Subscribe(ShutdownEventHandler, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<DeviceDiscoveryStatusChangedEvent>().Subscribe(OnDeviceDiscoveryStatusChanged, null, ThreadOption.PublisherThread, null);
		LogCoprocessorPartnersList();
		RegisterAllExistingDevicesInCoprocessor();
		LogCoprocessorPartnersList();
	}

	private void RegisterAllExistingDevicesInCoprocessor()
	{
		deviceList.ToList().ForEach(delegate(IDeviceInformation dev)
		{
			communicationWrapper.CommandHandler.SetPartner(new SIPCosSerialPartner
			{
				ip = dev.Address,
				OperationMode = (byte)dev.BestOperationMode,
				router = new byte[3]
			});
		});
	}

	private void LogCoprocessorPartnersList()
	{
	}

	public ISipcosConfigurator CreateSipcosConfigurator()
	{
		return new SipcosConfigurator(communicationWrapper);
	}

	private void ShutdownEventHandler(ShutdownEventArgs obj)
	{
		lock (syncRoot)
		{
			communicationWrapper.SendScheduler.Stop();
		}
	}

	private void SoftwareUpdateProgressChanged(SoftwareUpdateProgressEventArgs obj)
	{
		lock (syncRoot)
		{
			softwareUpdateRunning = obj.State == SoftwareUpdateState.Started;
			if (softwareUpdateRunning)
			{
				communicationWrapper.SendScheduler.Suspend();
			}
			else
			{
				communicationWrapper.SendScheduler.Resume();
			}
		}
	}

	private void AssignNetworkParameters()
	{
		byte[] array = new byte[2] { 154, 125 };
		SIPCosNetworkParameter sIPCosNetworkParameter = persistence.LoadSIPCosNetworkParameter();
		bool flag = false;
		if (sIPCosNetworkParameter == null)
		{
			sIPCosNetworkParameter = new SIPCosNetworkParameter();
			flag = true;
		}
		if (sIPCosNetworkParameter.NetworkKey == null)
		{
			sIPCosNetworkParameter.NetworkKey = RandomByteGenerator.Instance.GenerateRandomByteSequence(16u);
			flag = true;
		}
		if (sIPCosNetworkParameter.SyncWord == null)
		{
			if (configuration.UseDefaultSyncWord.HasValue && configuration.UseDefaultSyncWord.Value)
			{
				sIPCosNetworkParameter.SyncWord = array;
			}
			else
			{
				sIPCosNetworkParameter.SyncWord = SyncWords.GetRandomSyncWord();
			}
			flag = true;
		}
		if (sIPCosNetworkParameter.ShcAddresses == null)
		{
			sIPCosNetworkParameter.ShcAddresses = new List<byte[]>();
			flag = true;
		}
		if (flag)
		{
			persistence.SaveSIPCosNetworkParameterInTransaction(sIPCosNetworkParameter, suppressEvent: false);
		}
		Log.Information(Module.DeviceManager, $"SHC network key: {sIPCosNetworkParameter.NetworkKey.ToReadable()}");
		Log.Information(Module.DeviceManager, $"SHC network key decimal: {sIPCosNetworkParameter.NetworkKey.ToReadableDecimal()}");
		networkKey = sIPCosNetworkParameter.NetworkKey;
		syncWord = sIPCosNetworkParameter.SyncWord;
		CommandExecutor.ExecuteCommand(3, () => communicationWrapper.CommandHandler.SetSyncWord(syncWord), "SetSyncWord");
		CommandExecutor.ExecuteCommand(3, () => communicationWrapper.CommandHandler.SetNetworkKey(networkKey), "SetNetworkKey");
		communicationWrapper.SipCosHandler.SetBidCosNodes(persistence.LoadBidCosMappings());
	}

	private void StartProcessing(ShcStartupCompletedEventArgs args)
	{
		InitializeInternal();
		communicationWrapper.SendScheduler.Start();
		exclusionPendingCheckTimer = new Timer(RemovePendingExclusionDevices, null, 0, 86400000);
		RemovePendingExclusionDevices(null);
		DownloadDeviceKeysForIncludedDevices();
	}

	private void DownloadDeviceKeyForDevice(byte[] sgtin)
	{
		if (!deviceKeyRepository.DeviceExistsInStorage(sgtin))
		{
			CheckDeviceInclusionEventArgs e = new CheckDeviceInclusionEventArgs();
			e.Sgtin = sgtin;
			CheckDeviceInclusionEventArgs payload = e;
			eventManager.GetEvent<GetDeviceKeyEvent>().Publish(payload);
		}
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
			IDeviceInformation device;
			foreach (IDeviceInformation device2 in deviceList)
			{
				device = device2;
				if (storedDevices.FirstOrDefault((StoredDevice d) => d.Sgtin.SequenceEqual(device.Sgtin)) == null)
				{
					list.Add(device.Sgtin);
				}
			}
			return list;
		}
	}

	private void PublishGetDevicesKeysEvent(List<byte[]> sgtinsOfIncludedDevices)
	{
		GetDevicesKeysEvent getDevicesKeysEvent = eventManager.GetEvent<GetDevicesKeysEvent>();
		getDevicesKeysEvent.Publish(new GetDevicesKeysEventArgs
		{
			Sgtins = sgtinsOfIncludedDevices
		});
	}

	private void SendSchedulerSequenceFinished(object sender, SequenceFinishedEventArgs e)
	{
		ConfigureDeviceTask configureDeviceTask = null;
		lock (pendingConfigurationActions)
		{
			if (pendingConfigurationActions.ContainsKey(e.CorrelationId))
			{
				configureDeviceTask = pendingConfigurationActions[e.CorrelationId];
				pendingConfigurationActions.Remove(e.CorrelationId);
			}
		}
		byte[] array = null;
		lock (enqueuedTimeInfoSequences)
		{
			if (enqueuedTimeInfoSequences.ContainsKey(e.CorrelationId))
			{
				array = enqueuedTimeInfoSequences[e.CorrelationId];
				enqueuedTimeInfoSequences.Remove(e.CorrelationId);
			}
		}
		if (e.State == SequenceState.Success)
		{
			if (configureDeviceTask != null)
			{
				switch (configureDeviceTask.Action)
				{
				case ConfigurationAction.Include:
					OnDeviceIncluded(configureDeviceTask.DeviceId);
					Log.Information(Module.DeviceManager, $"Device {deviceList.LogInfoByGuid(configureDeviceTask.DeviceId)} successfully included.");
					break;
				case ConfigurationAction.Exclude:
				{
					IDeviceInformation deviceInformation;
					lock (deviceList.SyncRoot)
					{
						deviceInformation = deviceList[configureDeviceTask.DeviceId];
					}
					if (deviceInformation != null)
					{
						communicationWrapper.CommandHandler.RemovePartner(deviceInformation.Address);
						SetDeviceInclusionState(deviceInformation, DeviceInclusionState.Excluded);
						persistence.Save(deviceInformation, suppressEvent: false);
						Log.Information(Module.DeviceManager, $"Device {deviceList.LogInfoByGuid(configureDeviceTask.DeviceId)} successfully excluded.");
					}
					communicationWrapper.SendScheduler.RemoveDeviceSpecificQueue(configureDeviceTask.DeviceId);
					break;
				}
				default:
					throw ExceptionFactory.GetException(ErrorCode.InvalidConfigurationAction);
				}
			}
		}
		else if (e.State != SequenceState.Aborted && array != null && deviceList[array] != null)
		{
			EnqueueTimeInfoFrame(array);
		}
		this.SequenceFinished?.Invoke(sender, e);
	}

	private void OnDeviceIncluded(Guid deviceId)
	{
		IDeviceInformation deviceInformation;
		lock (deviceList.SyncRoot)
		{
			deviceInformation = deviceList[deviceId];
			if (deviceInformation == null)
			{
				return;
			}
			deviceInformation.PreparedNetworkAcceptFrame = null;
		}
		if (deviceInformation.ProtocolType == ProtocolType.BidCos)
		{
			communicationWrapper.SendScheduler.UpdateLastOnTimeOfDevice(deviceInformation, AwakeModifier.DeviceInfoReceived);
		}
		SetDeviceInclusionState(deviceInformation, DeviceInclusionState.Included);
		if (NeedsTimeInformation(deviceInformation.ManufacturerCode, deviceInformation.ManufacturerDeviceType))
		{
			Log.Debug(Module.DeviceManager, "Added TimeInfoFrame for " + deviceList.LogInfoByDeviceInfo(deviceInformation));
			EnqueueTimeInfoFrame(deviceInformation.Address);
		}
		if (deviceInformation.ManufacturerDeviceType == 10)
		{
			OnRouterIncluded();
		}
	}

	private static bool NeedsTimeInformation(short manufacturerCode, uint manufacturerDeviceType)
	{
		if (manufacturerCode == 1)
		{
			if (manufacturerDeviceType != 1 && manufacturerDeviceType != 21)
			{
				return manufacturerDeviceType == 15;
			}
			return true;
		}
		return false;
	}

	private void EnqueueTimeInfoFrame(byte[] address)
	{
		SIPcosHeader sIPcosHeader = new SIPcosHeader();
		sIPcosHeader.BiDi = true;
		sIPcosHeader.FrameType = SIPcosFrameType.TIME_INFORMATION;
		sIPcosHeader.Destination = address;
		SIPcosHeader header = sIPcosHeader;
		SIPCOSMessage message = communicationWrapper.TimeInfoHandler.GenerateTimeInfomationCommand(header, SIPcosTimeInforamtionMode.ActualTime, ShcDateTime.Now);
		PacketSequence sequence = new PacketSequence(new Packet(message));
		Guid key = communicationWrapper.SendScheduler.Enqueue(sequence);
		lock (enqueuedTimeInfoSequences)
		{
			enqueuedTimeInfoSequences.Add(key, address);
		}
	}

	public bool RemoveAllPendingConfigurationUpdates()
	{
		if (communicationWrapper.SendScheduler != null)
		{
			return communicationWrapper.SendScheduler.RemoveConfigurationSequences();
		}
		return true;
	}

	private void RaiseEventForNewlyFoundDevices()
	{
		lock (deviceList.SyncRoot)
		{
			DateTime dateTime = DateTime.UtcNow.Subtract(timeOutForNotIncludedDevices);
			foreach (IDeviceInformation device in deviceList)
			{
				if ((device.DeviceInclusionState == DeviceInclusionState.Found || device.DeviceInclusionState == DeviceInclusionState.Excluded) && device.DeviceFound >= dateTime && device.PreparedNetworkAcceptFrame.HasValue)
				{
					RaiseNewDeviceFoundEvent(device, DeviceFoundState.ReadyForInclusion);
				}
			}
		}
	}

	public Guid ExcludeDevice(Guid deviceId)
	{
		Guid result = Guid.Empty;
		lock (deviceList.SyncRoot)
		{
			if (!deviceList.Contains(deviceId))
			{
				throw ExceptionFactory.GetException(ErrorCode.DeviceDoesNotExist, deviceId);
			}
			communicationWrapper.SendScheduler.RemoveDeviceSpecificQueue(deviceId);
			IDeviceInformation deviceInformation = deviceList[deviceId];
			_ = deviceInformation.ManufacturerDeviceType;
			RemovePendingTimeInfoFrames(deviceInformation);
			if (deviceInformation.DeviceInclusionState == DeviceInclusionState.Included || deviceInformation.DeviceInclusionState == DeviceInclusionState.ExclusionPending || deviceInformation.DeviceInclusionState == DeviceInclusionState.InclusionPending)
			{
				deviceInformation.DeviceInclusionState = DeviceInclusionState.ExclusionPending;
				deviceInformation.DeviceExclusionTime = DateTime.Now;
				persistence.Save(deviceInformation, suppressEvent: false);
				Log.Information(Module.DeviceManager, "ExcludeDevice: Added NetworkExclusionFrame for " + deviceList.LogInfoByDeviceInfo(deviceInformation));
				communicationWrapper.SendScheduler.RemoveDeviceSpecificQueue(deviceId);
				PacketSequence packetSequence = CreateNetworkExclusionSequence(deviceInformation);
				pendingConfigurationActions.Add(packetSequence.CorrelationId, new ConfigureDeviceTask(ConfigurationAction.Exclude, deviceId));
				result = communicationWrapper.SendScheduler.Enqueue(packetSequence);
				if (deviceInformation.ManufacturerDeviceType == 10)
				{
					OnRouterExcluded();
				}
			}
			else if (deviceInformation.DeviceInclusionState != DeviceInclusionState.Found)
			{
				deviceInformation.DeviceInclusionState = DeviceInclusionState.Found;
				persistence.DeleteInTransaction(deviceId, suppressEvent: false);
			}
		}
		return result;
	}

	private void RemovePendingTimeInfoFrames(IDeviceInformation deviceInfo)
	{
		Dictionary<Guid, byte[]> dictionary;
		lock (enqueuedTimeInfoSequences)
		{
			dictionary = new Dictionary<Guid, byte[]>(enqueuedTimeInfoSequences);
		}
		Guid? guid = null;
		foreach (KeyValuePair<Guid, byte[]> item in dictionary)
		{
			if (item.Value != null && item.Value.SequenceEqual(deviceInfo.Address))
			{
				guid = item.Key;
				break;
			}
		}
		if (guid.HasValue)
		{
			lock (enqueuedTimeInfoSequences)
			{
				enqueuedTimeInfoSequences.Remove(guid.Value);
				Log.Debug(Module.DeviceManager, $"Removed time sync frame for device {deviceInfo.DeviceId}");
			}
		}
	}

	private PacketSequence CreateNetworkExclusionSequence(IDeviceInformation deviceInformation)
	{
		SIPcosHeader sIPcosHeader = new SIPcosHeader();
		sIPcosHeader.FrameType = SIPcosFrameType.NETWORK_MANAGEMENT_FRAME;
		sIPcosHeader.Destination = deviceInformation.Address;
		sIPcosHeader.Source = DefaultShcAddress;
		sIPcosHeader.SyncWord = syncWord;
		SIPcosHeader header = sIPcosHeader;
		SIPCOSMessage message = communicationWrapper.NetworkHandler.GenerateNetworkExclusionFrame(header);
		return new PacketSequence(message, SequenceType.Exclusion);
	}

	public void IncludeDevice(Guid deviceId)
	{
		lock (deviceList.SyncRoot)
		{
			IDeviceInformation deviceInformation = DeviceList[deviceId];
			if (deviceInformation == null)
			{
				throw ExceptionFactory.GetException(ErrorCode.DeviceDoesNotExist, deviceId);
			}
			if (deviceInformation.DeviceInclusionState == DeviceInclusionState.FactoryReset)
			{
				communicationWrapper.SendScheduler.RemoveDeviceSpecificQueue(deviceId);
			}
			if (deviceInformation.ManufacturerDeviceType == 10 && !deviceList.CanIncludeRouter())
			{
				Log.Error(Module.DeviceManager, "A router is already included in the network; adding a new one is not supported");
				return;
			}
			SetDeviceInclusionState(deviceInformation, DeviceInclusionState.InclusionPending);
			NotifyDeviceAboutToBeIncluded(deviceInformation);
			if (!deviceInformation.PreparedNetworkAcceptFrame.HasValue && deviceInformation.ProtocolType == ProtocolType.BidCos)
			{
				deviceInformation.PreparedNetworkAcceptFrame = new NetworkAcceptFrame
				{
					KeyUsed = NetworkAcceptKey.MAC_SECURITY_DISABLED,
					MIC32 = new byte[4],
					CCM = new byte[16],
					OneTimeKey = new byte[16],
					NetworkSyncWord = syncWord
				};
			}
			if (deviceInformation.PreparedNetworkAcceptFrame.HasValue)
			{
				IncludeDeviceInternal(deviceInformation);
			}
		}
	}

	private void NotifyDeviceAboutToBeIncluded(IDeviceInformation deviceInfo)
	{
		eventManager.GetEvent<DeviceInclusionStartedEvent>().Publish(new DeviceInclusionStartedEventArgs(deviceInfo.Address));
	}

	private void IncludeDeviceInternal(IDeviceInformation deviceInformation)
	{
		NetworkAcceptFrame? preparedNetworkAcceptFrame = deviceInformation.PreparedNetworkAcceptFrame;
		if (preparedNetworkAcceptFrame.HasValue)
		{
			deviceInformation.SequenceNumber = 0;
			SIPcosHeader sIPcosHeader = new SIPcosHeader();
			sIPcosHeader.FrameType = SIPcosFrameType.NETWORK_MANAGEMENT_FRAME;
			sIPcosHeader.Destination = (deviceInformation.IsRoutedInclusion ? deviceInformation.RouterAddress : deviceInformation.Address);
			sIPcosHeader.Source = DefaultShcAddress;
			sIPcosHeader.SequenceNumber = ++deviceInformation.SequenceNumber;
			sIPcosHeader.StayAwake = false;
			SIPcosHeader header = sIPcosHeader;
			Log.Debug(Module.DeviceManager, "IncludeDeviceInternal: Added NetworkAcceptFrame for " + deviceList.LogInfoByDeviceInfo(deviceInformation));
			SIPCOSMessage sIPCOSMessage = (deviceInformation.IsRoutedInclusion ? communicationWrapper.NetworkHandler.GenerateForwardNetworkAcceptFrame(header, preparedNetworkAcceptFrame.Value, deviceInformation.Address) : communicationWrapper.NetworkHandler.GenerateNetworkAcceptFrame(header, preparedNetworkAcceptFrame.Value));
			if (deviceInformation.ProtocolType == ProtocolType.BidCos)
			{
				sIPCOSMessage.Header.BiDi = false;
			}
			PacketSequence packetSequence = new PacketSequence(sIPCOSMessage, SequenceType.Inclusion);
			pendingConfigurationActions.Add(packetSequence.CorrelationId, new ConfigureDeviceTask(ConfigurationAction.Include, deviceInformation.DeviceId));
			communicationWrapper.CommandHandler.SetPartner(new SIPCosSerialPartner
			{
				ip = deviceInformation.Address,
				OperationMode = (byte)deviceInformation.BestOperationMode,
				router = new byte[3]
			});
			LogCoprocessorPartnersList();
			communicationWrapper.SendScheduler.Enqueue(packetSequence);
		}
		else
		{
			Log.Error(Module.DeviceManager, $"Tried to include device {deviceList.LogInfoByDeviceInfo(deviceInformation)} but network accept frame was empty");
		}
	}

	private void GetDoubleEncryptedKey(Guid deviceId, SIPcosDeviceInfoFrame infoFrame)
	{
		IDeviceInformation deviceInformation;
		lock (deviceList.SyncRoot)
		{
			deviceInformation = deviceList[deviceId];
			deviceInformation.PreparedNetworkAcceptFrame = null;
		}
		byte[] NetworkKey = (byte[])networkKey.Clone();
		byte[] oneTimeKey = RandomByteGenerator.Instance.GenerateRandomByteSequence(16u);
		communicationWrapper.NetworkHandler.EncryptNetworkKey(infoFrame, ref NetworkKey, oneTimeKey);
		try
		{
			GetLocalDoubleEncryptedKey(deviceId, deviceInformation, infoFrame, oneTimeKey, NetworkKey);
		}
		catch (Exception ex)
		{
			Log.Information(Module.DeviceManager, $"There was a problem getting the local cosip encryption key: {ex.Message} {ex.StackTrace}");
			PublishGetEncryptedKeyEvent(deviceId, infoFrame, NetworkKey, oneTimeKey);
		}
	}

	private void GetLocalDoubleEncryptedKey(Guid deviceId, IDeviceInformation deviceInformation, SIPcosDeviceInfoFrame infoFrame, byte[] oneTimeKey, byte[] tmpKey)
	{
		List<StoredDevice> allDevicesKeysFromStorage = deviceKeyRepository.GetAllDevicesKeysFromStorage();
		StoredDevice storedDevice = allDevicesKeysFromStorage.FirstOrDefault((StoredDevice storedDevice2) => storedDevice2.Sgtin.SequenceEqual(deviceInformation.Sgtin));
		if (storedDevice == null)
		{
			DownloadDeviceKeyForDevice(deviceInformation.Sgtin);
		}
		allDevicesKeysFromStorage = deviceKeyRepository.GetAllDevicesKeysFromStorage();
		storedDevice = allDevicesKeysFromStorage.FirstOrDefault((StoredDevice storedDevice2) => storedDevice2.Sgtin.SequenceEqual(deviceInformation.Sgtin));
		if (storedDevice == null)
		{
			throw new Exception($"The device with the serial number {SerialForDisplay.FromSgtin(deviceInformation.Sgtin)} was not found in the CSV file");
		}
		byte[] encTwiceNetworkKey = DeviceKeyEncryptor.CCM(infoFrame.SGTIN, infoFrame.SecuritySequenceNumber, tmpKey, storedDevice.Key, 2, 16);
		byte[] keyAuthentication = DeviceKeyEncryptor.CCM(infoFrame.SGTIN, infoFrame.SecuritySequenceNumber, encTwiceNetworkKey, storedDevice.Key, 1, 4);
		eventManager.GetEvent<EncryptedKeyResponseEvent>().Publish(new EncryptedKeyResponseEventArgs
		{
			DeviceId = deviceId,
			Result = EncryptedKeyResponseStatus.Success,
			OneTimeKey = oneTimeKey,
			EncTwiceNetworkKey = encTwiceNetworkKey,
			KeyAuthentication = keyAuthentication
		});
	}

	private void PublishGetEncryptedKeyEvent(Guid deviceId, SIPcosDeviceInfoFrame infoFrame, byte[] tmpKey, byte[] oneTimeKey)
	{
		GetEncryptedKeyEvent getEncryptedKeyEvent = eventManager.GetEvent<GetEncryptedKeyEvent>();
		getEncryptedKeyEvent.Publish(new GetEncryptedKeyEventArgs
		{
			DeviceId = deviceId,
			OneTimeKey = oneTimeKey,
			Sgtin = infoFrame.SGTIN,
			SecNumber = infoFrame.SecuritySequenceNumber,
			EncOnceNetworkKey = tmpKey,
			FirmwareVersion = infoFrame.ManufacturerDeviceAndFirmware.ToString("X2")
		});
	}

	private void ReceivedDoubleEncryptedKey(EncryptedKeyResponseEventArgs args)
	{
		try
		{
			lock (deviceList.SyncRoot)
			{
				IDeviceInformation deviceInformation = DeviceList[args.DeviceId];
				if (deviceInformation == null)
				{
					throw ExceptionFactory.GetException(ErrorCode.DeviceDoesNotExist, args.DeviceId);
				}
				DeviceFoundState deviceFoundState = DeviceFoundState.Unknown;
				switch (args.Result)
				{
				case EncryptedKeyResponseStatus.Success:
					deviceInformation.PreparedNetworkAcceptFrame = new NetworkAcceptFrame
					{
						KeyUsed = NetworkAcceptKey.PROTECTED_SECURITY_SYM_KEY_EXCHANGE,
						MIC32 = args.KeyAuthentication,
						CCM = args.EncTwiceNetworkKey,
						OneTimeKey = args.OneTimeKey,
						NetworkSyncWord = syncWord
					};
					if (deviceInformation.DeviceInclusionState == DeviceInclusionState.InclusionPending)
					{
						IncludeDeviceInternal(deviceInformation);
					}
					deviceFoundState = DeviceFoundState.ReadyForInclusion;
					break;
				case EncryptedKeyResponseStatus.InvalidTenant:
					deviceFoundState = DeviceFoundState.NoDeviceKeyInvalidTenant;
					break;
				case EncryptedKeyResponseStatus.Blacklisted:
					deviceFoundState = DeviceFoundState.NoDeviceKeyDeviceBlacklisted;
					break;
				case EncryptedKeyResponseStatus.DeviceNotFound:
					deviceFoundState = DeviceFoundState.NoDeviceKeyAvailable;
					break;
				case EncryptedKeyResponseStatus.BackendServiceNotReachable:
					deviceFoundState = DeviceFoundState.NoDeviceKeyBackendUnreachable;
					break;
				}
				if (deviceInformation.DeviceInclusionState == DeviceInclusionState.InclusionPending && deviceFoundState == DeviceFoundState.NoDeviceKeyDeviceBlacklisted && DeviceInclusionActivated)
				{
					Log.Error(Module.DeviceManager, string.Format("Failed to get device key from backend. Device {0} is blacklisted {0}", args.DeviceId.ToString()));
					RaiseNewDeviceFoundEvent(deviceInformation, deviceFoundState);
				}
				if (deviceInformation.DeviceInclusionState == DeviceInclusionState.Found || deviceInformation.DeviceInclusionState == DeviceInclusionState.Excluded)
				{
					if (args.Result == EncryptedKeyResponseStatus.Success)
					{
						Log.Information(Module.DeviceManager, $"Received double encrypted key response for {deviceList.LogInfoByDeviceInfo(deviceInformation)} -> SipCosDeviceFoundEvent sent");
					}
					if (deviceFoundState != DeviceFoundState.ReadyForInclusion)
					{
						Log.Error(Module.DeviceManager, $"Failed to get device key from backend. Backend returned {args.Result}");
					}
					if (DeviceInclusionActivated)
					{
						RaiseNewDeviceFoundEvent(deviceInformation, deviceFoundState);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.DeviceManager, ex.Message);
		}
	}

	private void ReceivedDeviceStatusInfo(SIPcosStatusFrame message)
	{
		IDeviceInformation deviceInformation = deviceList[message.Header.Source];
		if (deviceInformation != null && deviceInformation.ManufacturerCode == 1 && deviceInformation.ManufacturerDeviceType == 22)
		{
			SirFRHandler sirRFHandlerForDevice = GetSirRFHandlerForDevice(message.Header.Source);
			sirRFHandlerForDevice.ReceivedInfoStatusFrame(deviceInformation, message.Header.Destination);
			ReceivedMessageFromDevice(message.Header.Source, message.Header.BiDi ? AwakeModifier.Bidi : AwakeModifier.None, included: false);
		}
		else
		{
			ReceivedMessageFromDevice(message.Header.Source, message.Header.BiDi ? AwakeModifier.Bidi : AwakeModifier.None, included: true);
		}
	}

	private void ReceivedIcmpHandlerData(ICMPMessage message)
	{
		if (message.Header.IpSource.SequenceEqual(SipCosAddress.InvalidAddress))
		{
			Log.Error(Module.DeviceManager, $"Received ICMP message with empty IPSource from address {message.Header.MacSource.ToReadable()} ");
		}
		else
		{
			ReceivedMessageFromDevice(message.Header.IpSource, AwakeModifier.None, included: true);
		}
	}

	private void ReceivedRequestConfigUpdate(SIPcosHeader header, byte channel)
	{
		lock (deviceList.SyncRoot)
		{
			IDeviceInformation deviceInformation = deviceList[header.Source];
			if (deviceInformation == null)
			{
				return;
			}
			if (deviceInformation.DeviceInclusionState != DeviceInclusionState.Excluded && (deviceInformation.BestOperationMode != DeviceInfoOperationModes.EventListener || deviceInformation.UpdateState == CosIPDeviceUpdateState.UpToDate) && !communicationWrapper.SendScheduler.ContainsSequences(deviceInformation.DeviceId, (PacketSequence sequence) => sequence.SequenceType == SequenceType.Configuration || sequence.SequenceType == SequenceType.Exclusion || sequence.SequenceType == SequenceType.Inclusion))
			{
				communicationWrapper.SendAppAck(header.IpSource, header.IpDestination, header.SequenceNumber, forceStayAwake: false);
			}
			Log.Debug(Module.DeviceManager, "RequestConfigUpdate from " + deviceList.LogInfoByDeviceInfo(deviceInformation));
		}
		ReceivedMessageFromDevice(header.Source, AwakeModifier.Bidi, included: true);
	}

	private void ReceivedDeviceInfo(SIPcosDeviceInfoFrame infoFrame)
	{
		if (softwareUpdateRunning)
		{
			return;
		}
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		lock (deviceList.SyncRoot)
		{
			stopwatch.Stop();
			if (stopwatch.ElapsedMilliseconds > 1000)
			{
				Log.Information(Module.DeviceManager, "Waited in device info: " + stopwatch.ElapsedMilliseconds);
			}
			Array.Reverse(infoFrame.ManufacturerDeviceType);
			uint num = BitConverter.ToUInt32(infoFrame.ManufacturerDeviceType, 0);
			if (infoFrame.FrameType == SIPcosNetworkManagementFrameType.ForwardedDeviceInfoFrame)
			{
				if (num == 4 && infoFrame.ManufacturerDeviceAndFirmware < 36)
				{
					Log.Warning(Module.DeviceManager, $"Rejecting inclusion of BRC8 with version older than 2.4 ({infoFrame.ManufacturerDeviceAndFirmware}) via the router");
					return;
				}
				if (!deviceList.Contains(infoFrame.Header.MacSource))
				{
					Log.Warning(Module.DeviceManager, $"Rejecting device info frame received from foreign (not included) router with MAC address [{infoFrame.Header.MacSource.ToReadable()}]");
					return;
				}
			}
			IDeviceInformation deviceInformation = deviceList.GetBySGTIN(infoFrame.SGTIN);
			byte[] newDeviceIP = infoFrame.NewDeviceIP;
			if (deviceInformation == null)
			{
				DeviceInformation deviceInformation2 = new DeviceInformation(Guid.NewGuid(), DeviceInclusionState.Found, SipCosAddress.InvalidAddress, infoFrame.Header.SequenceNumber, infoFrame.SGTIN, (byte)infoFrame.OperationMode, infoFrame.ManufacturerDeviceAndFirmware, (short)infoFrame.ManufacturerCode, num, infoFrame.ProtocolType.ToProtocolType(), infoFrame.FrameType == SIPcosNetworkManagementFrameType.ForwardedDeviceInfoFrame);
				deviceInformation2.SupportsEncryption = communicationWrapper.SipCosHandler.IsSupportedEncryption(infoFrame.SGTIN);
				deviceInformation = deviceInformation2;
				deviceList.AddDevice(deviceInformation);
			}
			else
			{
				deviceInformation.SupportsEncryption = communicationWrapper.SipCosHandler.IsSupportedEncryption(infoFrame.SGTIN);
				if (deviceInformation.DeviceInclusionState == DeviceInclusionState.Included)
				{
					Log.Debug(Module.DeviceManager, $"Removing old address [{deviceInformation.Address[0]:X2}{deviceInformation.Address[1]:X2}{deviceInformation.Address[2]:X2}] from partners list for previously included but now reset device");
					communicationWrapper.CommandHandler.RemovePartner(deviceInformation.Address);
					LogCoprocessorPartnersList();
				}
			}
			if (infoFrame.FrameType == SIPcosNetworkManagementFrameType.ForwardedDeviceInfoFrame)
			{
				deviceInformation.IsRoutedInclusion = true;
				deviceInformation.RouterAddress = infoFrame.Header.MacSource;
			}
			else
			{
				deviceInformation.IsRoutedInclusion = false;
				deviceInformation.RouterAddress = SipCosAddress.InvalidAddress;
			}
			deviceInformation.DeviceFound = DateTime.UtcNow;
			deviceInformation.ManufacturerDeviceAndFirmware = infoFrame.ManufacturerDeviceAndFirmware;
			bool flag = !deviceInformation.Address.Compare(newDeviceIP);
			if (flag && deviceList[newDeviceIP] != null)
			{
				SetDeviceInclusionState(deviceInformation, (deviceInformation.DeviceInclusionState == DeviceInclusionState.Found || deviceInformation.DeviceInclusionState == DeviceInclusionState.FoundWithAddressCollision || deviceInformation.DeviceInclusionState == DeviceInclusionState.Excluded) ? DeviceInclusionState.FoundWithAddressCollision : DeviceInclusionState.FactoryResetWithAddressCollision);
				deviceInformation.Address = newDeviceIP;
				Log.Warning(Module.DeviceManager, string.Format("Received DeviceInfo with address collision for {0}: DeviceAddress = {1}, LastCollisionAddress = {2}", deviceList.LogInfoByDeviceInfo(deviceInformation), newDeviceIP.ToReadable(), (deviceInformation.LastCollisionAddress != null) ? deviceInformation.LastCollisionAddress.ToReadable() : "null"));
				bool flag2 = !newDeviceIP.Compare(deviceInformation.LastCollisionAddress);
				if (flag2 || deviceInformation.CollisionFrameCount < 15)
				{
					communicationWrapper.SendScheduler.RemoveDeviceSpecificQueue(deviceInformation.DeviceId);
					Log.Debug(Module.DeviceManager, $"Send CollisionFrame for {deviceList.LogInfoByDeviceInfo(deviceInformation)}");
					SendCollisionFrame(infoFrame, deviceInformation.DeviceId);
					deviceInformation.LastCollisionAddress = newDeviceIP;
					if (flag2)
					{
						deviceInformation.CollisionFrameCount = 0;
					}
					else
					{
						deviceInformation.CollisionFrameCount++;
					}
					communicationWrapper.SendScheduler.UpdateLastOnTimeOfDevice(deviceInformation, AwakeModifier.DeviceInfoReceived);
				}
				else
				{
					Log.Debug(Module.DeviceManager, $"CollisionFrame not sent because DeviceAddress didn't change since last DeviceInfoFrame");
					RaiseNewDeviceFoundEvent(deviceInformation, DeviceFoundState.AddressCollision);
				}
				deviceInformation.Address = SipCosAddress.InvalidAddress;
				return;
			}
			deviceInformation.Address = newDeviceIP;
			communicationWrapper.SendScheduler.UpdateLastOnTimeOfDevice(deviceInformation.Address, AwakeModifier.DeviceInfoReceived);
			if (!flag && deviceInformation.DeviceInclusionState == DeviceInclusionState.Included)
			{
				if (infoFrame.ProtocolType != DeviceInfoProtocolType.BIDcos)
				{
					SendCollisionFrame(infoFrame, deviceInformation.DeviceId);
					Log.Warning(Module.DeviceManager, "Received DeviceInfo from already included {0}, Collision frame sent" + deviceList.LogInfoByDeviceInfo(deviceInformation));
					return;
				}
				if (deviceInformation.ManufacturerCode == 1 && num == 22)
				{
					return;
				}
				Log.Debug(Module.DeviceManager, $"Received DeviceInfo from included BidCos device {deviceList.LogInfoByDeviceInfo(deviceInformation)}, re-inclusion triggered");
				IncludeDevice(deviceInformation.DeviceId);
			}
			switch (deviceInformation.DeviceInclusionState)
			{
			case DeviceInclusionState.FoundWithAddressCollision:
				deviceInformation.DeviceInclusionState = DeviceInclusionState.Found;
				break;
			case DeviceInclusionState.InclusionPending:
				if (flag)
				{
					SetDeviceInclusionState(deviceInformation, DeviceInclusionState.FactoryReset);
				}
				break;
			case DeviceInclusionState.Included:
			case DeviceInclusionState.FactoryResetWithAddressCollision:
				SetDeviceInclusionState(deviceInformation, DeviceInclusionState.FactoryReset);
				break;
			case DeviceInclusionState.ExclusionPending:
			case DeviceInclusionState.Excluded:
				SetDeviceInclusionState(deviceInformation, DeviceInclusionState.Found);
				communicationWrapper.SendScheduler.RemoveDeviceSpecificQueue(deviceInformation.DeviceId);
				break;
			}
			if (flag && deviceInformation.DeviceInclusionState == DeviceInclusionState.FactoryReset && deviceInformation.UpdateState == CosIPDeviceUpdateState.Updating)
			{
				SetDeviceInclusionState(deviceInformation, DeviceInclusionState.InclusionPending);
				communicationWrapper.SendScheduler.RemoveDeviceSpecificQueue(deviceInformation.DeviceId);
			}
			if (infoFrame.ProtocolType == DeviceInfoProtocolType.SIPcos)
			{
				if (!infoFrame.SecurityMIC32.Compare(deviceInformation.Nonce) || flag)
				{
					communicationWrapper.SendScheduler.RemoveSequencesConditionally(deviceInformation.DeviceId, (PacketSequence sequence) => sequence.SequenceType == SequenceType.Inclusion, SequenceState.Aborted);
					Log.Debug(Module.DeviceManager, $"Network accept frame for device {deviceList.LogInfoByDeviceInfo(deviceInformation)} has been removed");
					deviceInformation.Nonce = infoFrame.SecurityMIC32;
					Log.Debug(Module.DeviceManager, $"Received DeviceInfo for {deviceList.LogInfoByDeviceInfo(deviceInformation)} -> Try to get key from the key server");
					GetDoubleEncryptedKey(deviceInformation.DeviceId, infoFrame);
				}
				else
				{
					Log.Debug(Module.DeviceManager, $"Received DeviceInfo from {deviceList.LogInfoByDeviceInfo(deviceInformation)} with same address or identical nonce");
				}
				ReceivedMessageFromDevice(newDeviceIP, AwakeModifier.DeviceInfoReceived, included: false);
			}
			else if (deviceInformation.DeviceInclusionState == DeviceInclusionState.Found || deviceInformation.DeviceInclusionState == DeviceInclusionState.Excluded)
			{
				if (DeviceInclusionActivated)
				{
					RaiseNewDeviceFoundEvent(deviceInformation, DeviceFoundState.ReadyForInclusion);
				}
				deviceInformation.MarkDeviceAsSleeping();
				ReceivedMessageFromDevice(newDeviceIP, AwakeModifier.None, included: false);
			}
			else
			{
				ReceivedMessageFromDevice(newDeviceIP, AwakeModifier.DeviceInfoReceived, included: false);
			}
		}
	}

	private void SetDeviceInclusionState(IDeviceInformation deviceInformation, DeviceInclusionState deviceInclusionState)
	{
		deviceInformation.DeviceInclusionState = deviceInclusionState;
		if (deviceInformation.DeviceInclusionState != DeviceInclusionState.Found)
		{
			persistence.Save(deviceInformation, suppressEvent: false);
		}
		else
		{
			persistence.DeleteInTransaction(deviceInformation.DeviceId, suppressEvent: false);
		}
	}

	private void SendCollisionFrame(SIPcosDeviceInfoFrame infoFrame, Guid deviceId)
	{
		SIPcosHeader sIPcosHeader = new SIPcosHeader();
		sIPcosHeader.Source = DefaultShcAddress;
		sIPcosHeader.Destination = ((infoFrame.FrameType == SIPcosNetworkManagementFrameType.ForwardedDeviceInfoFrame) ? infoFrame.Header.MacSource : infoFrame.NewDeviceIP);
		sIPcosHeader.SequenceNumber = (byte)(infoFrame.Header.SequenceNumber + 1);
		SIPcosHeader header = sIPcosHeader;
		SIPCOSMessage message = ((infoFrame.FrameType == SIPcosNetworkManagementFrameType.ForwardedDeviceInfoFrame) ? communicationWrapper.NetworkHandler.GenerateForwardNetworkInfoFrame(header, NetworkInfoFrameType.AddressCollision, infoFrame.SGTIN, infoFrame.NewDeviceIP) : communicationWrapper.NetworkHandler.GenerateNetworkInfoFrame(header, NetworkInfoFrameType.AddressCollision, infoFrame.SGTIN));
		communicationWrapper.SendScheduler.Enqueue(new PacketSequence(new Packet(message), SequenceType.CollisionNotification), deviceId);
	}

	private void RaiseNewDeviceFoundEvent(IDeviceInformation device, DeviceFoundState deviceFoundState)
	{
		eventManager.GetEvent<PhysicalDeviceFoundEvent>().Publish(new DeviceFoundEventArgs
		{
			FoundDevice = PhysicalDeviceFactory.Create(device, deviceDefinitionsProvider),
			State = deviceFoundState
		});
	}

	private void ReceivedAnswer(SIPcosAnswerFrame answer)
	{
		byte[] source = answer.Header.Source;
		Log.Debug(Module.DeviceManager, "Received AnswerFrame from " + deviceList.LogInfoByAddress(source));
		communicationWrapper.SendScheduler.AcknowledgePacket(source, answer.Header.SequenceNumber, answer.Status, isStatusInfo: false);
		Log.Debug(Module.DeviceManager, "Finished AnswerFrame processing from " + deviceList.LogInfoByAddress(source));
	}

	private void ReceivedTimeRequest(SIPcosTimeInformationFrame timeframe)
	{
		byte[] source = timeframe.Header.Source;
		IDeviceInformation deviceInformation = deviceList[source];
		if (deviceInformation != null)
		{
			ReceivedMessageFromDevice(source, timeframe.Header.BiDi ? AwakeModifier.Bidi : AwakeModifier.None, included: true);
			if (timeframe.Header.BiDi)
			{
				EnqueueTimeInfoFrame(source);
				Log.Debug(Module.DeviceManager, "Received TimeRequest from " + deviceList.LogInfoByDeviceInfo(deviceInformation) + "added TimeInfoFrame");
			}
			else
			{
				Log.Debug(Module.DeviceManager, $"Received TimeInformation from {deviceList.LogInfoByDeviceInfo(deviceInformation)}, Time: {timeframe.Time}");
			}
		}
	}

	private void ReceivedMessageFromDevice(byte[] address, AwakeModifier awakeModifier, bool included)
	{
		lock (deviceList.SyncRoot)
		{
			if (included && deviceList.Contains(address))
			{
				IDeviceInformation deviceInformation = deviceList[address];
				DeviceInclusionState deviceInclusionState = deviceInformation.DeviceInclusionState;
				bool flag = deviceInclusionState == DeviceInclusionState.InclusionPending || deviceInclusionState == DeviceInclusionState.FactoryReset || deviceInclusionState == DeviceInclusionState.FactoryResetWithAddressCollision;
				bool flag2 = deviceInclusionState == DeviceInclusionState.ExclusionPending || deviceInclusionState == DeviceInclusionState.Excluded;
				if (flag)
				{
					Guid deviceId = deviceInformation.DeviceId;
					if (communicationWrapper.SendScheduler.ContainsSequences(deviceId, (PacketSequence packetSequence) => packetSequence.SequenceType == SequenceType.Inclusion))
					{
						communicationWrapper.SendScheduler.RemoveSequencesConditionally(deviceId, (PacketSequence packetSequence) => packetSequence.SequenceType == SequenceType.Inclusion, SequenceState.Success);
					}
					Log.Debug(Module.DeviceManager, $"Device {deviceList.LogInfoByDeviceInfo(deviceInformation)} sent an encrypted frame but was in LogicalState {deviceInclusionState} => changed to LogicalState Included");
					OnDeviceIncluded(deviceInformation.DeviceId);
				}
				if (flag2 && !communicationWrapper.SendScheduler.ContainsSequences(deviceInformation.DeviceId, (PacketSequence packetSequence) => packetSequence.SequenceType == SequenceType.Exclusion))
				{
					Log.Debug(Module.DeviceManager, $"Received config request from excluded device {deviceList.LogInfoByDeviceInfo(deviceInformation)}. Resend exclusion frame.");
					PacketSequence sequence = CreateNetworkExclusionSequence(deviceInformation);
					communicationWrapper.SendScheduler.Enqueue(sequence);
				}
			}
		}
		communicationWrapper.SendScheduler.UpdateLastOnTimeOfDevice(address, awakeModifier);
	}

	private void ReceivedSwitchCommand(SIPcosHeader header, SwitchCommand frame)
	{
		if (header.Destination.Compare(SipCosAddress.AnyRouter) || header.Destination.Compare(SipCosAddress.AllRouters))
		{
			Log.Debug(Module.DeviceManager, $"Switch command ignored, was received for routing");
			return;
		}
		ReceivedSequence receivedSequence;
		lock (deviceList.SyncRoot)
		{
			receivedSequence = receivedSwitchCommands.Find((ReceivedSequence c) => c.SourceAddress.Compare(header.Source) && c.SequenceCounter == header.SequenceNumber && DateTime.UtcNow - c.Timestamp < TimeSpan.FromMilliseconds(2000.0));
			if (receivedSwitchCommands.Count > 10)
			{
				int num = receivedSwitchCommands.RemoveAll((ReceivedSequence c) => DateTime.UtcNow - c.Timestamp > TimeSpan.FromMilliseconds(60000.0));
				Log.Debug(Module.DeviceManager, $"Removed {num} commands older than one minute.");
			}
			IDeviceInformation deviceInformation = deviceList[header.Source];
			if (deviceInformation == null)
			{
				return;
			}
			if (header.BiDi)
			{
				SendAppAckAsync(header, deviceInformation);
			}
			if (receivedSequence != null)
			{
				receivedSequence.Timestamp = DateTime.UtcNow;
			}
			else
			{
				receivedSwitchCommands.Add(new ReceivedSequence
				{
					SourceAddress = header.Source,
					SequenceCounter = header.SequenceNumber,
					Timestamp = DateTime.UtcNow
				});
			}
			ReceivedMessageFromDevice(header.Source, header.BiDi ? AwakeModifier.Bidi : AwakeModifier.None, included: true);
		}
		if (receivedSequence != null)
		{
			Log.Debug(Module.DeviceManager, $"Switch command with same sequence counter already processed, ignoring -> SRC:{receivedSequence.SourceAddress.ToReadable()}, CNT:{receivedSequence.SequenceCounter}, RCV@:{receivedSequence.Timestamp.ToString(CultureInfo.InvariantCulture)}, BiDi: {header.BiDi.ToString(CultureInfo.InvariantCulture)}");
		}
		else
		{
			RaiseSwitchCommandEvent(header, frame);
		}
	}

	private void SendAppAckAsync(SIPcosHeader header, IDeviceInformation deviceInformation)
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			bool forceStayAwake = (deviceInformation.BestOperationMode != DeviceInfoOperationModes.MainsPowered && communicationWrapper.SendScheduler.ContainsSequences(deviceInformation.DeviceId, (PacketSequence sequence) => sequence.SequenceType == SequenceType.Configuration)) || (deviceInformation.BestOperationMode == DeviceInfoOperationModes.EventListener && deviceInformation.UpdateState == CosIPDeviceUpdateState.TransferInProgress);
			if (FrameForDeviceNeedsDelayedAppAck(header, deviceInformation))
			{
				Thread.Sleep(200);
			}
			if (NoReplyRequiredForAppACK(header, deviceInformation))
			{
				communicationWrapper.SendAppAckWithoutExpectedReply(header.IpSource, header.IpDestination, header.SequenceNumber, forceStayAwake);
			}
			else
			{
				communicationWrapper.SendAppAck(header.IpSource, header.IpDestination, header.SequenceNumber, forceStayAwake);
			}
		});
	}

	private bool FrameForDeviceNeedsDelayedAppAck(SIPcosHeader header, IDeviceInformation deviceInformation)
	{
		SIPcosFrameType frameType = header.FrameType;
		if (frameType == SIPcosFrameType.CONDITIONAL_SWITCH_COMMAND)
		{
			if (deviceInformation.ManufacturerDeviceType != 5)
			{
				return deviceInformation.ManufacturerDeviceType == 6;
			}
			return true;
		}
		return false;
	}

	private bool NoReplyRequiredForAppACK(SIPcosHeader header, IDeviceInformation deviceInformation)
	{
		if (header.FrameType == SIPcosFrameType.CONDITIONAL_SWITCH_COMMAND)
		{
			if (deviceInformation.ManufacturerDeviceType != 5 && deviceInformation.ManufacturerDeviceType != 6 && deviceInformation.ManufacturerDeviceType != 1 && deviceInformation.ManufacturerDeviceType != 21)
			{
				return deviceInformation.ManufacturerDeviceType == 15;
			}
			return true;
		}
		return false;
	}

	private void OnTimeZoneChanged(object sender, EventArgs args)
	{
		List<IDeviceInformation> list;
		lock (deviceList.SyncRoot)
		{
			list = deviceList.Where((IDeviceInformation device) => NeedsTimeInformation(device.ManufacturerCode, device.ManufacturerDeviceType)).ToList();
		}
		foreach (IDeviceInformation item in list)
		{
			IDeviceInformation deviceInformation = item;
			Log.Debug(Module.DeviceManager, "Added TimeInfoFrame for " + deviceList.LogInfoByDeviceInfo(deviceInformation));
			EnqueueTimeInfoFrame(deviceInformation.Address);
		}
	}

	private void RaiseSwitchCommandEvent(SIPcosHeader header, SwitchCommand frame)
	{
		byte? decisionValue = ((frame is ConditionalSwitchCommand conditionalSwitchCommand) ? new byte?(conditionalSwitchCommand.DecisionValue) : ((byte?)null));
		eventManager.GetEvent<SipCosSwitchCommandEvent>().Publish(new SipCosSwitchCommandEventArgs(header.Source, frame.KeyChannelNumber, frame.KeyStrokeCounter, frame.ActivationTime, decisionValue));
	}

	private void LogSequenceCounterIfVerbose()
	{
		if (0 >= (int)ModuleInfos.GetLogLevel(Module.DeviceManager))
		{
			CoprocessorAccess coprocessorAccess = new CoprocessorAccess(communicationWrapper);
			Log.Debug(Module.DeviceManager, $"Coprocessor sequence counter {coprocessorAccess.SecuritySequenceCounter}");
		}
	}

	public void Initialize()
	{
		if (encryptedKeyResponseToken == null)
		{
			EncryptedKeyResponseEvent encryptedKeyResponseEvent = eventManager.GetEvent<EncryptedKeyResponseEvent>();
			encryptedKeyResponseToken = encryptedKeyResponseEvent.Subscribe(ReceivedDoubleEncryptedKey, null, ThreadOption.PublisherThread, null);
		}
	}

	public void Uninitialize()
	{
		if (encryptedKeyResponseToken != null)
		{
			EncryptedKeyResponseEvent encryptedKeyResponseEvent = eventManager.GetEvent<EncryptedKeyResponseEvent>();
			encryptedKeyResponseEvent.Unsubscribe(encryptedKeyResponseToken);
			encryptedKeyResponseToken = null;
		}
	}

	public void ResetDeviceInclusionState(Guid deviceId)
	{
		lock (deviceList.SyncRoot)
		{
			if (deviceList.Contains(deviceId))
			{
				IDeviceInformation deviceInformation = deviceList[deviceId];
				switch (deviceInformation.DeviceInclusionState)
				{
				case DeviceInclusionState.InclusionPending:
				case DeviceInclusionState.Included:
					persistence.Save(deviceInformation, suppressEvent: false);
					return;
				}
				deviceInformation.DeviceFound = DateTime.UtcNow;
				DeviceInclusionState deviceInclusionState = DeviceInclusionState.Found;
				SetDeviceInclusionState(deviceInformation, deviceInclusionState);
				communicationWrapper.SendScheduler.RemoveDeviceSpecificQueue(deviceId);
			}
		}
	}

	public void DropDiscoveredDevices(BaseDevice[] devices)
	{
		foreach (BaseDevice baseDevice in devices)
		{
			deviceList.Remove(baseDevice.Id);
		}
	}

	private void OnRouterIncluded()
	{
		deviceList.ContainsRouter = true;
		communicationWrapper.SendScheduler.ForceEchoRequestForUnreachableDevices();
	}

	private void OnRouterExcluded()
	{
		deviceList.ForceDetectionOfRouters = true;
	}

	private void ReceivedRouteManagementFrame(SIPcosRouteManagementFrame frame)
	{
		Log.Debug(Module.DeviceManager, $"Received routing table entry with address {frame.Address.ToReadable()}");
		ReceivedMessageFromDevice(frame.Address, AwakeModifier.DeviceInfoReceived, included: true);
		eventManager.GetEvent<RoutingMessageReceivedEvent>().Publish(new RoutingMessageReceivedEventArgs(frame));
	}

	private void RemovePendingExclusionDevices(object obj)
	{
		lock (deviceList.SyncRoot)
		{
			List<IDeviceInformation> list = deviceList.Where((IDeviceInformation di) => di.DeviceInclusionState == DeviceInclusionState.ExclusionPending).ToList();
			foreach (IDeviceInformation item in list)
			{
				if (DateTime.Now > item.DeviceExclusionTime + TimeSpan.FromDays(1.0))
				{
					deviceList.Remove(item.DeviceId);
					persistence.DeleteInTransaction(item.DeviceId, suppressEvent: false);
				}
			}
		}
	}

	private void OnDeviceDiscoveryStatusChanged(DeviceDiscoveryStatusChangedEventArgs args)
	{
		if (args.Phase == DiscoveryPhase.Prepare)
		{
			if (args.AppIds == null || (args.AppIds != null && args.AppIds.Contains(CoreConstants.CoreAppId)))
			{
				DeviceInclusionActivated = true;
			}
		}
		else if (args.Phase == DiscoveryPhase.Deactivate)
		{
			DeviceInclusionActivated = false;
		}
	}

	private SirFRHandler GetSirRFHandlerForDevice(byte[] sourceAdress)
	{
		SirFRHandler sirFRHandler = sirFRHandlers.FirstOrDefault((SirFRHandler m) => m.SourceAdress.SequenceEqual(sourceAdress));
		if (sirFRHandler == null)
		{
			sirFRHandler = new SirFRHandler(sourceAdress, delegate(IDeviceInformation device)
			{
				SetDeviceInclusionState(device, DeviceInclusionState.FactoryReset);
			});
			sirFRHandlers.Add(sirFRHandler);
		}
		return sirFRHandler;
	}
}
