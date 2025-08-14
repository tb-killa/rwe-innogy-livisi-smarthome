using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceMonitoring;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.Lemonbeat.ProtocolAdapter.Persistence;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.FirmwareUpdate;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.LemonbeatCoreServices;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Persistence;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.ReachableHandling;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Transformation;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;
using SmartHome.SHC.API.Protocols.Lemonbeat;

namespace RWE.SmartHome.SHC.Lemonbeat.ProtocolAdapter;

internal class LemonbeatProtocolAdapter : IProtocolAdapter
{
	private const uint SEQUENCE_COUNTER_VALUE_ID = 1u;

	private readonly LemonbeatPhysicalStateHandler LemonbeatPhysicalStateHandler;

	private readonly LemonbeatDeviceStateController LemonbeatDeviceStateController;

	private readonly IInclusionController deviceInclusionController;

	private readonly IConfigurationAccess deviceConfigurationAccess;

	private readonly LemonbeatTransformation LemonbeatTransformation;

	private readonly IEventManager eventManager;

	private readonly LemonbeatServiceHost LemonbeatServiceHost;

	private readonly CalendarService calendarService;

	private readonly ConfigurationController configurationController;

	private readonly IDeviceList deviceList;

	private readonly ILemonbeatPersistence LemonbeatPersistence;

	private readonly RasConnectionManager connectionManager;

	private readonly ShcValueRepository shcValueRepository;

	private readonly XmlSerializer systemInfoSerializer = new XmlSerializer(typeof(LemonbeatSpecificInformation));

	private readonly IRepository configRepository;

	private readonly ILemonbeatCommunication LemonbeatAggregator;

	private ValueDescriptionService valueDescriptionService;

	private PartnerInformationService partnerInformationService;

	private TimerService timerService;

	private CalculationService calculationService;

	private ActionService actionService;

	private ServiceDescriptionService serviceDescriptionService;

	private StateMachineService stateMachineService;

	private ConfigurationService configSvc;

	private MemoryInformationService memoryInformationService;

	private FirmwareUpdateService firmwareUpdateService;

	private LemonbeatUsbDongle usbDongle;

	private readonly IGatewayRegistrar gatewayRegistrar;

	private readonly Dictionary<IDeviceHandler, LemonbeatEnvironment> RegisteredLemonbeatEnvironment = new Dictionary<IDeviceHandler, LemonbeatEnvironment>();

	public ProtocolIdentifier ProtocolId => ProtocolIdentifier.Lemonbeat;

	public IProtocolSpecificLogicalStateRequestor LogicalState => LemonbeatDeviceStateController;

	public IProtocolSpecificPhysicalStateHandler PhysicalState => LemonbeatPhysicalStateHandler;

	public IProtocolSpecificDeviceController DeviceController => LemonbeatDeviceStateController;

	public IProtocolSpecificTransformation Transformation => LemonbeatTransformation;

	public IProtocolSpecificDataBackup DataBackup => null;

	public LemonbeatProtocolAdapter(IEventManager eventManager, IProtocolSpecificDataPersistence protocolSpecificDataPersistence, IApplicationsHost applicationsHost, IDeviceMonitor deviceMonitor, IScheduler scheduler, IRepository configRepository, IDeviceFirmwareManager deviceFirmwareManager)
	{
		deviceList = new DeviceList();
		this.eventManager = eventManager;
		this.configRepository = configRepository;
		deviceList.DeviceInclusionStateChanged += DeviceInclusionStateChanged;
		deviceList.DeviceConfiguredStateChanged += OnDeviceConfiguredStateChanged;
		deviceList.DeviceReachabilityChanged += OnDeviceReachabilityChanged;
		deviceList.DeviceUpdateStateChanged += DeviceUpdateStateChanged;
		LemonbeatPersistence = new LemonbeatPersistence(protocolSpecificDataPersistence, IsKnownDevice);
		LemonbeatAggregator = new LemonbeatAggregatorReachabilityDecorator((ILemonbeatCommunication)(gatewayRegistrar = new LemonbeatAggregator(applicationsHost)), deviceList, scheduler, applicationsHost, eventManager);
		LemonbeatServiceHost = new LemonbeatServiceHost();
		new LemonbeatExiCompressor();
		ValueService valueService = new ValueService(LemonbeatAggregator);
		LemonbeatServiceHost.AddService(valueService);
		DeviceDescriptionService deviceDescriptionService = new DeviceDescriptionService(LemonbeatAggregator);
		LemonbeatServiceHost.AddService(deviceDescriptionService);
		StatusService statusService = new StatusService(LemonbeatAggregator);
		LemonbeatServiceHost.AddService(statusService);
		serviceDescriptionService = new ServiceDescriptionService(LemonbeatAggregator);
		LemonbeatServiceHost.AddService(serviceDescriptionService);
		NetworkManagementService networkManagementService = new NetworkManagementService(LemonbeatAggregator);
		LemonbeatServiceHost.AddService(networkManagementService);
		valueDescriptionService = new ValueDescriptionService(LemonbeatAggregator);
		LemonbeatServiceHost.AddService(valueDescriptionService);
		partnerInformationService = new PartnerInformationService(LemonbeatAggregator);
		LemonbeatServiceHost.AddService(partnerInformationService);
		memoryInformationService = new MemoryInformationService(LemonbeatAggregator);
		LemonbeatServiceHost.AddService(memoryInformationService);
		timerService = new TimerService(LemonbeatAggregator);
		LemonbeatServiceHost.AddService(timerService);
		calendarService = new CalendarService(LemonbeatAggregator);
		LemonbeatServiceHost.AddService(calendarService);
		actionService = new ActionService(LemonbeatAggregator);
		LemonbeatServiceHost.AddService(actionService);
		stateMachineService = new StateMachineService(LemonbeatAggregator);
		LemonbeatServiceHost.AddService(stateMachineService);
		calculationService = new CalculationService(LemonbeatAggregator);
		LemonbeatServiceHost.AddService(calculationService);
		configSvc = new ConfigurationService(LemonbeatAggregator);
		LemonbeatServiceHost.AddService(configSvc);
		firmwareUpdateService = new FirmwareUpdateService(LemonbeatAggregator);
		LemonbeatServiceHost.AddService(firmwareUpdateService);
		shcValueRepository = new ShcValueRepository(LemonbeatPersistence, applicationsHost, eventManager);
		deviceInclusionController = new DeviceInclusionController(deviceList, LemonbeatPersistence, networkManagementService, deviceDescriptionService, valueService, calendarService, eventManager, scheduler, applicationsHost);
		deviceConfigurationAccess = new DeviceConfigurationAccess(valueService, partnerInformationService, timerService, calendarService, actionService, serviceDescriptionService, valueDescriptionService, memoryInformationService, calculationService, stateMachineService, configSvc);
		LemonbeatPhysicalStateHandler = new LemonbeatPhysicalStateHandler(deviceList);
		configurationController = new ConfigurationController(deviceList, deviceInclusionController, deviceConfigurationAccess, LemonbeatPersistence);
		LemonbeatTransformation = new LemonbeatTransformation(deviceList, configurationController, applicationsHost, LemonbeatPersistence, shcValueRepository, configRepository, LemonbeatAggregator);
		LemonbeatDeviceStateController = new LemonbeatDeviceStateController(eventManager, applicationsHost, valueService, deviceList, shcValueRepository, configRepository);
		connectionManager = new RasConnectionManager();
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(StartupCompleted, null, ThreadOption.PublisherThread, null);
		applicationsHost.ApplicationStateChanged += OnApplicationActivated;
		applicationsHost.ApplicationStateChanged += OnApplicationDeactivated;
		valueService.ValueReportReceived += OnValueReportReceived;
		eventManager.GetEvent<DSTChangedEvent>().Subscribe(DSTChanged, null, ThreadOption.PublisherThread, null);
		usbDongle = new LemonbeatUsbDongle(eventManager, new LemonbeatExiCompressor(), deviceMonitor, deviceInclusionController, deviceList, connectionManager);
		gatewayRegistrar.RegisterGateway(usbDongle);
		LemonbeatDeviceFirmwareUpdater updater = new LemonbeatDeviceFirmwareUpdater(eventManager, firmwareUpdateService, deviceList, statusService, LemonbeatPersistence, applicationsHost);
		deviceFirmwareManager.RegisterUpdater(updater);
	}

	private void OnDeviceReachabilityChanged(object sender, DeviceReachabilityChangedEventArgs args)
	{
		LemonbeatPersistence.SaveInTransaction(args.Device, suppressEvent: true);
		eventManager.GetEvent<DeviceUnreachableChangedEvent>().Publish(new DeviceUnreachableChangedEventArgs(args.Device.DeviceId, !args.IsReachable));
	}

	private void OnApplicationActivated(ApplicationLoadStateChangedEventArgs obj)
	{
		if (obj.ApplicationState != ApplicationStates.ApplicationActivated)
		{
			return;
		}
		IDeviceHandler lemonbeatHandler = obj.Application as IDeviceHandler;
		if (lemonbeatHandler == null)
		{
			return;
		}
		IValueService service = LemonbeatServiceHost.GetService<ValueService>();
		if (service == null)
		{
			return;
		}
		lock (RegisteredLemonbeatEnvironment)
		{
			IDeviceHandler deviceHandler = RegisteredLemonbeatEnvironment.Keys.FirstOrDefault((IDeviceHandler key) => key.HandledDeviceTypes.SequenceEqual(lemonbeatHandler.HandledDeviceTypes));
			if (deviceHandler != null)
			{
				RegisteredLemonbeatEnvironment.Remove(deviceHandler);
			}
		}
		LemonbeatEnvironment lemonbeatEnvironment = new LemonbeatEnvironment(obj.Application.ApplicationId, service, deviceList, shcValueRepository);
		try
		{
			lemonbeatHandler.SetLemonbeatCoreServices(lemonbeatEnvironment);
			lock (RegisteredLemonbeatEnvironment)
			{
				RegisteredLemonbeatEnvironment[lemonbeatHandler] = lemonbeatEnvironment;
			}
		}
		catch (Exception ex)
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, $"Error setting Lemonbeat core services for app with id {obj.Application.ApplicationId}, Details: \n{ex.ToString()}");
		}
	}

	private void OnApplicationDeactivated(ApplicationLoadStateChangedEventArgs obj)
	{
		if ((obj.ApplicationState != ApplicationStates.ApplicationDeactivated && obj.ApplicationState != ApplicationStates.ApplicationsUninstalled) || !(obj.Application is IDeviceHandler key))
		{
			return;
		}
		try
		{
			lock (RegisteredLemonbeatEnvironment)
			{
				RegisteredLemonbeatEnvironment.Remove(key);
			}
		}
		catch (Exception ex)
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, $"LemonbeatHandler already deregistered for application with ID: {obj.Application.ApplicationId} Details: \n{ex.ToString()}");
		}
	}

	private void OnValueReportReceived(object sender, ValueReportReceivedArgs args)
	{
		try
		{
			if (usbDongle == null || args.DeviceIdentifier.GatewayId != usbDongle.GatewayId || args.DeviceIdentifier.IPAddress == null || !args.DeviceIdentifier.IPAddress.Equals(connectionManager.PeerAddress))
			{
				return;
			}
			CoreHexBinaryValue coreHexBinaryValue = args.HexBinaryValues.FirstOrDefault((CoreHexBinaryValue nv) => nv.Id == 1);
			if (coreHexBinaryValue != null)
			{
				byte[] array = coreHexBinaryValue.Value;
				if (BitConverter.IsLittleEndian)
				{
					array = array.Reverse().ToArray();
				}
				uint num = BitConverter.ToUInt32(array, 0);
				LemonbeatPersistence.SaveSequenceCounter(num);
				Log.Information(Module.LemonbeatProtocolAdapter, $"Lemonbeat sequence counter saved. New value is {num}");
			}
		}
		catch (Exception ex)
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, $"Error processing value report for dongle sequence counter, {ex.Message}");
		}
	}

	private void DeviceInclusionStateChanged(object sender, LemonbeatDeviceInclusionStateChangedEventArgs e)
	{
		DeviceInclusionStateChangedEventArgs payload = new DeviceInclusionStateChangedEventArgs(e.DeviceId, e.DeviceInclusionState.ToProtocolMultiplexerState(), ProtocolIdentifier.Lemonbeat.ToString());
		eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Publish(payload);
	}

	private void OnDeviceConfiguredStateChanged(object sender, DeviceConfiguredEventArgs deviceConfiguredEventArgs)
	{
		eventManager.GetEvent<DeviceConfiguredEvent>().Publish(deviceConfiguredEventArgs);
	}

	private void DeviceUpdateStateChanged(object sender, DeviceUpdateStateChangedEventArgs args)
	{
		eventManager.GetEvent<DeviceUpdateStateChangedEvent>().Publish(args);
	}

	private void StartupCompleted(ShcStartupCompletedEventArgs eventArgs)
	{
		try
		{
			switch (eventArgs.Progress)
			{
			case StartupProgress.DatabaseAvailable:
			{
				IEnumerable<DeviceInformation> deviceInformations = LemonbeatPersistence.LoadAllDevices();
				IEnumerable<DeviceInformation> enumerable = CleanUpDeviceInformations(deviceInformations);
				{
					foreach (DeviceInformation item in enumerable)
					{
						deviceList.AddDevice(item);
						if (item.DeviceInclusionState == LemonbeatDeviceInclusionState.FactoryReset)
						{
							eventManager.GetEvent<DeviceWasFactoryResetEvent>().Publish(new DeviceWasFactoryResetEventArgs
							{
								DeviceId = item.DeviceId
							});
						}
					}
					break;
				}
			}
			case StartupProgress.CompletedRound2:
				UpdateDevicesTimezoneOffset();
				break;
			case StartupProgress.CompletedRound1:
				break;
			}
		}
		catch (Exception ex)
		{
			Log.Information(Module.LemonbeatProtocolAdapter, $"Failed to start Lemonbeat services {ex.Message}");
			Log.Debug(Module.LemonbeatProtocolAdapter, ex.ToString());
		}
	}

	private IEnumerable<DeviceInformation> CleanUpDeviceInformations(IEnumerable<DeviceInformation> deviceInformations)
	{
		List<DeviceInformation> list = new List<DeviceInformation>();
		List<Guid> list2 = (from bd in configRepository.GetBaseDevices()
			select bd.Id).ToList();
		foreach (DeviceInformation deviceInformation in deviceInformations)
		{
			if (list2.Contains(deviceInformation.DeviceId))
			{
				list.Add(deviceInformation);
				continue;
			}
			LemonbeatPersistence.DeleteInTransaction(deviceInformation.DeviceId, suppressEvent: false);
			Log.Warning(Module.LemonbeatProtocolAdapter, $"Lemonbeat device with serial number {deviceInformation.DeviceDescription.SGTIN} removed from device list.No correspondent in logical configuration");
		}
		return list;
	}

	public IEnumerable<Guid> GetHandledDevices()
	{
		return deviceList.SyncSelect((DeviceInformation d) => d.DeviceId);
	}

	public void UpdateProtocolSpecificData()
	{
	}

	public string GetDeviceDescription(Guid deviceId)
	{
		try
		{
			return deviceList[deviceId].ToString();
		}
		catch
		{
			return string.Empty;
		}
	}

	public void ResetDeviceInclusionState(Guid deviceId)
	{
		configurationController.ResetDevicePartners(deviceId, configRepository);
		deviceInclusionController.ResetDeviceInclusionState(deviceId);
	}

	public void DropDiscoveredDevices(BaseDevice[] devices)
	{
		deviceInclusionController.DropDiscoveredDevices(devices);
	}

	public ProtocolSpecificInformation GetProtocolSpecificInformation()
	{
		ProtocolSpecificInformation protocolSpecificInformation = new ProtocolSpecificInformation(ProtocolId);
		protocolSpecificInformation.XmlInformation = CreateProtocolSpecificInformation();
		return protocolSpecificInformation;
	}

	private string CreateProtocolSpecificInformation()
	{
		LemonbeatSpecificInformation lemonbeatSpecificInformation = new LemonbeatSpecificInformation();
		lemonbeatSpecificInformation.Devices = LemonbeatPersistence.GetAllDevicesInformation();
		lemonbeatSpecificInformation.DongleParameters = LemonbeatPersistence.GetDongleInformation();
		LemonbeatSpecificInformation lemonbeatSpecificInformation2 = lemonbeatSpecificInformation;
		if (lemonbeatSpecificInformation2.DongleParameters != null)
		{
			lemonbeatSpecificInformation2.DongleParameters.DeviceKey = "";
		}
		lemonbeatSpecificInformation2.Devices.ForEach(delegate(DeviceInformationEntity e)
		{
			e.MemoryInformation = null;
			e.ServiceDescriptions = null;
			e.DeviceKey = "";
			e.ValueDescriptions = null;
		});
		string empty = string.Empty;
		XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[1] { XmlQualifiedName.Empty });
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.OmitXmlDeclaration = true;
		xmlWriterSettings.Indent = true;
		using StringWriter stringWriter = new StringWriter();
		using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);
		systemInfoSerializer.Serialize(xmlWriter, lemonbeatSpecificInformation2, namespaces);
		empty = stringWriter.ToString();
		xmlWriter.Close();
		return empty;
	}

	private void UpdateDevicesTimezoneOffset()
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			int currentTZOffset = Convert.ToInt32(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours * 60.0 * 60.0);
			deviceList.SyncWhere((DeviceInformation di) => di.DeviceInclusionState == LemonbeatDeviceInclusionState.Included && di.IsReachable && di.TimezoneOffset != currentTZOffset).ForEach(delegate(DeviceInformation dev)
			{
				try
				{
					dev.TimezoneOffset = currentTZOffset;
					deviceConfigurationAccess.UpdateDeviceTimeZone(dev);
					LemonbeatPersistence.SaveInTransaction(dev, suppressEvent: false);
					Log.Information(Module.LemonbeatProtocolAdapter, "Updated timezone for device {0}" + dev.ToString());
				}
				catch (Exception ex)
				{
					Log.Warning(Module.LemonbeatProtocolAdapter, "Could not update timezone on device " + dev.ToString() + "\n" + ex);
				}
			});
		});
	}

	private bool IsKnownDevice(Guid deviceId)
	{
		BaseDevice baseDevice = configRepository.GetBaseDevice(deviceId);
		return baseDevice != null;
	}

	private void DSTChanged(DSTChangedEventArgs eventArgs)
	{
		UpdateDevicesTimezoneOffset();
	}
}
