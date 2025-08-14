using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;

internal class DeviceConfigurationAccess : IConfigurationAccess
{
	private enum ConfigAction
	{
		GetStaticConfig,
		GetDynamicConfig,
		SetConfig
	}

	private class PendingConfiguration
	{
		public DeviceInformation Device { get; set; }

		public ConfigAction ConfigAction { get; set; }

		public ConfigDifferenceProvider GetConfigDiff { get; set; }
	}

	private readonly IValueService valueService;

	private readonly IPartnerInformationService partnerInformationService;

	private readonly ITimerService timerService;

	private readonly ICalendarService calendarService;

	private readonly IActionService actionService;

	private readonly IServiceDescriptionService serviceDescriptionService;

	private readonly IValueDescriptionService valueDescriptionService;

	private readonly IMemoryInformationService memoryInformationService;

	private readonly ICalculationService calculationService;

	private readonly IStateMachineService stateMachineService;

	private readonly IConfigurationService configurationService;

	private Queue<PendingConfiguration> configurationQueue = new Queue<PendingConfiguration>();

	private readonly object syncRoot = new object();

	public event EventHandler<CurrentConfigurationReceivedEventArgs> GetDynamicConfigurationCompleted;

	public event EventHandler<DeployConfigurationChangesCompletedEventArgs> DeployConfigurationChangesCompleted;

	public event EventHandler<StaticConfigurationReceivedEventArgs> GetStaticConfigurationCompleted;

	internal DeviceConfigurationAccess(IValueService valueService, IPartnerInformationService partnerInformationService, ITimerService timerService, ICalendarService calendarService, IActionService actionService, IServiceDescriptionService serviceDescriptionService, IValueDescriptionService valueDescriptionService, IMemoryInformationService memoryInformationService, ICalculationService calculationService, IStateMachineService stateMachineService, IConfigurationService configurationService)
	{
		this.valueService = valueService;
		this.partnerInformationService = partnerInformationService;
		this.timerService = timerService;
		this.calendarService = calendarService;
		this.actionService = actionService;
		this.serviceDescriptionService = serviceDescriptionService;
		this.valueDescriptionService = valueDescriptionService;
		this.memoryInformationService = memoryInformationService;
		this.calculationService = calculationService;
		this.stateMachineService = stateMachineService;
		this.configurationService = configurationService;
	}

	public void GetDynamicConfigurationAsync(DeviceInformation device)
	{
		EnqueueConfigAction(new PendingConfiguration
		{
			Device = device,
			ConfigAction = ConfigAction.GetDynamicConfig
		});
	}

	public void DeployConfigurationAsync(DeviceInformation device, ConfigDifferenceProvider configDifferenceProvider)
	{
		EnqueueConfigAction(new PendingConfiguration
		{
			Device = device,
			ConfigAction = ConfigAction.SetConfig,
			GetConfigDiff = configDifferenceProvider
		});
	}

	public void GetStaticConfigurationAsync(DeviceInformation device)
	{
		EnqueueConfigAction(new PendingConfiguration
		{
			Device = device,
			ConfigAction = ConfigAction.GetStaticConfig
		});
	}

	public void UpdateDeviceTimeZone(DeviceInformation device)
	{
		if (device == null || device.ServiceDescriptions == null)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Timezone update failed. Unable to find device or device service descriptions. Corrupt configuration?");
		}
		else if (device.ServiceDescriptions.Any((ServiceDescription sd) => sd.ServiceType == ServiceType.Calendar))
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Update time zone of device {device} ");
			calendarService.UpdateDeviceTimezone(device.Identifier, device.TimezoneOffset);
			configurationService.CommitDeviceConfiguration(device.Identifier);
		}
	}

	private void EnqueueConfigAction(PendingConfiguration config)
	{
		lock (syncRoot)
		{
			if (config.ConfigAction != ConfigAction.SetConfig && configurationQueue.Any((PendingConfiguration pc) => pc.Device.DeviceId == config.Device.DeviceId && pc.ConfigAction == config.ConfigAction))
			{
				Log.Information(Module.LemonbeatProtocolAdapter, $"DeviceConfigurationAccess: Config action: {config.ConfigAction} was dropped for {config.Device} because already in queue");
				return;
			}
			Log.Debug(Module.LemonbeatProtocolAdapter, $"DeviceConfigurationAccess: Enqueueing config action {config.ConfigAction} for device {config.Device} ");
			configurationQueue.Enqueue(config);
			if (configurationQueue.Count == 1)
			{
				Log.Debug(Module.LemonbeatProtocolAdapter, "DeviceConfigurationAccess: Starting worker thread...");
				Thread thread = new Thread((ThreadStart)delegate
				{
					ConfigurationThread();
				});
				thread.Start();
			}
		}
	}

	private void ConfigurationThread()
	{
		try
		{
			bool flag = true;
			while (flag)
			{
				PendingConfiguration pendingConfig = null;
				lock (syncRoot)
				{
					pendingConfig = configurationQueue.Peek();
				}
				if (pendingConfig != null && pendingConfig.Device.DeviceInclusionState == LemonbeatDeviceInclusionState.Included)
				{
					switch (pendingConfig.ConfigAction)
					{
					case ConfigAction.SetConfig:
						DoWithRetries(delegate
						{
							DeployDeviceConfigurationChanges(pendingConfig.Device, pendingConfig.GetConfigDiff(pendingConfig.Device.DeviceId));
						}, pendingConfig.Device, "deploy device configuration");
						break;
					case ConfigAction.GetDynamicConfig:
						DoWithRetries(delegate
						{
							GetDynamicConfiguration(pendingConfig.Device);
						}, pendingConfig.Device, "get dynamic configuration");
						break;
					case ConfigAction.GetStaticConfig:
						DoWithRetries(delegate
						{
							GetStaticConfiguration(pendingConfig.Device);
						}, pendingConfig.Device, "get dynamic configuration");
						break;
					default:
						Log.Warning(Module.LemonbeatProtocolAdapter, "DeviceConfigurationAccess: Invalid ConfigAction provided to the config job");
						break;
					}
				}
				else if (pendingConfig != null)
				{
					Log.Information(Module.LemonbeatProtocolAdapter, $"DeviceConfigurationAccess: ConfigurationThread aborted config action {pendingConfig.ConfigAction} for {pendingConfig.Device}, Device inclusion state: {pendingConfig.Device.DeviceInclusionState}");
				}
				else
				{
					Log.Warning(Module.LemonbeatProtocolAdapter, "DeviceConfigurationAccess: ConfigurationThread aborted config action because the queue top item is null");
				}
				lock (syncRoot)
				{
					configurationQueue.Dequeue();
					if (configurationQueue.Count == 0)
					{
						flag = false;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Unexpected error on the config deployment thread:\n " + ex.ToString());
		}
	}

	private List<ServiceDescription> RetrieveServiceDescriptions(DeviceInformation device)
	{
		Log.Debug(Module.LemonbeatProtocolAdapter, $"Retrieve service description from device {device}...");
		List<ServiceDescription> serviceDescriptions = serviceDescriptionService.GetServiceDescriptions(device.Identifier);
		Log.Debug(Module.LemonbeatProtocolAdapter, $"Received Service descriptions from device {device}.");
		return serviceDescriptions;
	}

	private List<MemoryInformation> RetrieveMemoryInformation(DeviceInformation device)
	{
		Log.Debug(Module.LemonbeatProtocolAdapter, $"Retrieve memory information from device {device}...");
		List<MemoryInformation> memoryInformation = memoryInformationService.GetMemoryInformation(device.Identifier);
		Log.Debug(Module.LemonbeatProtocolAdapter, $"Received Memory Information from device {device}.");
		return memoryInformation;
	}

	private List<ValueDescription> RetrieveValueDescriptions(DeviceInformation device)
	{
		Log.Debug(Module.LemonbeatProtocolAdapter, $"Retrieve value description from device {device}...");
		List<ValueDescription> result = (from vd in valueDescriptionService.GetValueDescriptions(device.Identifier)
			where !vd.IsVirtual
			select vd).ToList();
		Log.Debug(Module.LemonbeatProtocolAdapter, $"Received value descriptions from device {device}.");
		return result;
	}

	private void GetStaticConfiguration(DeviceInformation device)
	{
		Log.Debug(Module.LemonbeatProtocolAdapter, $"Start retrieving static configuration of {device}");
		List<ServiceDescription> list = RetrieveServiceDescriptions(device);
		List<MemoryInformation> memoryInformation = null;
		List<ValueDescription> valueDescriptions = null;
		if (list.Any((ServiceDescription sd) => sd.ServiceType == ServiceType.MemoryDescription))
		{
			memoryInformation = RetrieveMemoryInformation(device);
		}
		if (list.Any((ServiceDescription sd) => sd.ServiceType == ServiceType.ValueDescription))
		{
			valueDescriptions = RetrieveValueDescriptions(device);
		}
		Log.Information(Module.LemonbeatProtocolAdapter, $"Retrieval of static configuration from device {device} successful.");
		this.GetStaticConfigurationCompleted?.Invoke(this, new StaticConfigurationReceivedEventArgs
		{
			DeviceId = device.DeviceId,
			ServiceDescriptions = list,
			MemoryInformation = memoryInformation,
			ValueDescriptions = valueDescriptions
		});
	}

	private void GetDynamicConfiguration(DeviceInformation device)
	{
		string arg = device.ToString();
		DeviceIdentifier identifier = device.Identifier;
		PhysicalConfiguration physicalConfiguration = new PhysicalConfiguration();
		Log.Debug(Module.LemonbeatProtocolAdapter, $"Start getting dynamic configuration of {device}");
		if (device.ServiceDescriptions.Exists((ServiceDescription sd) => sd.ServiceType == ServiceType.ValueDescription))
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Retrieve virtual value descriptions from device {arg}...");
			IEnumerable<ValueDescription> collection = from vd in valueDescriptionService.GetValueDescriptions(identifier)
				where vd.IsVirtual
				select vd;
			physicalConfiguration.VirtualValueDescriptions.AddRange(collection);
		}
		if (device.ServiceDescriptions.Exists((ServiceDescription sd) => sd.ServiceType == ServiceType.PartnerInformation))
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Retrieve partner informations from device {arg}...");
			PartnerInformations allPartnersAndGroups = partnerInformationService.GetAllPartnersAndGroups(identifier);
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Received partner informations from device {arg}.");
			physicalConfiguration.Partners.AddRange(allPartnersAndGroups.Partners);
			physicalConfiguration.PartnerGroups.AddRange(allPartnersAndGroups.Groups);
		}
		if (device.ServiceDescriptions.Exists((ServiceDescription sd) => sd.ServiceType == ServiceType.Timer))
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Retrieve timers from device {arg}...");
			IEnumerable<LemonbeatTimer> allTimers = timerService.GetAllTimers(identifier);
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Received timers from device {arg}.");
			physicalConfiguration.Timers.AddRange(allTimers);
		}
		if (device.ServiceDescriptions.Exists((ServiceDescription sd) => sd.ServiceType == ServiceType.Calendar))
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Retrieve calendar entries from device {arg}...");
			IEnumerable<CalendarTask> allCalendarEntries = calendarService.GetAllCalendarEntries(identifier);
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Received calendar entries from device {arg}.");
			physicalConfiguration.CalendarEntries.AddRange(allCalendarEntries);
		}
		if (device.ServiceDescriptions.Exists((ServiceDescription sd) => sd.ServiceType == ServiceType.Action))
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Retrieve actions from device {arg}...");
			IEnumerable<LemonbeatAction> allActions = actionService.GetAllActions(identifier);
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Received actions from device {arg}.");
			physicalConfiguration.Actions.AddRange(allActions);
		}
		if (device.ServiceDescriptions.Exists((ServiceDescription sd) => sd.ServiceType == ServiceType.Calculation))
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Retrieve calculations from device {arg}...");
			IEnumerable<Calculation> allCalculations = calculationService.GetAllCalculations(identifier);
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Received calculations from device {arg}.");
			physicalConfiguration.Calculations.AddRange(allCalculations);
		}
		if (device.ServiceDescriptions.Exists((ServiceDescription sd) => sd.ServiceType == ServiceType.Statemachine))
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Retrieve state machines from device {arg}...");
			IEnumerable<StateMachine> allStateMachines = stateMachineService.GetAllStateMachines(identifier);
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Received state machines from device {arg}.");
			physicalConfiguration.StateMachines.AddRange(allStateMachines);
		}
		Log.Information(Module.LemonbeatProtocolAdapter, $"Retrieval of current configuration from device {device} successful.");
		this.GetDynamicConfigurationCompleted?.Invoke(this, new CurrentConfigurationReceivedEventArgs
		{
			DeviceId = device.DeviceId,
			Configuration = physicalConfiguration
		});
	}

	private void DeployDeviceConfigurationChanges(DeviceInformation device, PhysicalConfigurationDifference toDeploy)
	{
		if (toDeploy == null || toDeploy.IsEmpty())
		{
			Log.Information(Module.LemonbeatProtocolAdapter, $"DeployDeviceConfigurationChanges was called for {device}, but the difference to deploy is null or empty");
			return;
		}
		Log.Debug(Module.LemonbeatProtocolAdapter, $"Start deploying configuration for {device}");
		DeviceIdentifier identifier = device.Identifier;
		_ = device.DeviceId;
		PartnerInformations partnerInformations = new PartnerInformations();
		List<uint> list = new List<uint>();
		bool flag = toDeploy.VirtualValueDescriptions != null && !toDeploy.VirtualValueDescriptions.IsEmpty && device.ServiceDescriptions.Exists((ServiceDescription sd) => sd.ServiceType == ServiceType.ValueDescription);
		bool flag2 = false;
		if (device.ServiceDescriptions.Exists((ServiceDescription sd) => sd.ServiceType == ServiceType.PartnerInformation))
		{
			if (toDeploy.Partners != null && !toDeploy.Partners.IsEmpty)
			{
				foreach (Partner item in toDeploy.Partners.ToSet)
				{
					partnerInformations.Partners.Add(item);
				}
				list.AddRange(toDeploy.Partners.ToDelete);
			}
			if (toDeploy.PartnerGroups != null && !toDeploy.PartnerGroups.IsEmpty)
			{
				foreach (Group item2 in toDeploy.PartnerGroups.ToSet)
				{
					partnerInformations.Groups.Add(item2);
				}
				list.AddRange(toDeploy.PartnerGroups.ToDelete);
			}
			flag2 = partnerInformations.Partners.Count > 0 || partnerInformations.Groups.Count > 0 || list.Count > 0;
		}
		if (toDeploy.Links != null)
		{
			_ = toDeploy.Links.IsEmpty;
		}
		bool flag3 = toDeploy.Timers != null && !toDeploy.Timers.IsEmpty && device.ServiceDescriptions.Exists((ServiceDescription sd) => sd.ServiceType == ServiceType.Timer);
		bool flag4 = toDeploy.CalendarEntries != null && !toDeploy.CalendarEntries.IsEmpty && device.ServiceDescriptions.Exists((ServiceDescription sd) => sd.ServiceType == ServiceType.Calendar);
		bool flag5 = toDeploy.Actions != null && !toDeploy.Actions.IsEmpty && device.ServiceDescriptions.Exists((ServiceDescription sd) => sd.ServiceType == ServiceType.Action);
		bool flag6 = toDeploy.Calculations != null && !toDeploy.Calculations.IsEmpty && device.ServiceDescriptions.Exists((ServiceDescription sd) => sd.ServiceType == ServiceType.Calculation);
		bool flag7 = toDeploy.StateMachines != null && !toDeploy.StateMachines.IsEmpty && device.ServiceDescriptions.Exists((ServiceDescription sd) => sd.ServiceType == ServiceType.Statemachine);
		List<ServiceType> list2 = new List<ServiceType>();
		if (flag)
		{
			valueDescriptionService.AddAndDeleteValueDescriptions(identifier, toDeploy.VirtualValueDescriptions.ToSet, toDeploy.VirtualValueDescriptions.ToDelete);
			list2.Add(ServiceType.ValueDescription);
		}
		if (flag2)
		{
			partnerInformationService.SetAndDeletePartners(identifier, partnerInformations, list);
			list2.Add(ServiceType.PartnerInformation);
		}
		if (flag3)
		{
			timerService.SetAndDeleteTimers(identifier, toDeploy.Timers.ToSet, toDeploy.Timers.ToDelete);
			list2.Add(ServiceType.Timer);
		}
		if (flag4)
		{
			calendarService.SetAndDeleteCalendarEntries(identifier, toDeploy.CalendarEntries.ToSet, toDeploy.CalendarEntries.ToDelete);
			list2.Add(ServiceType.Calendar);
		}
		if (flag5)
		{
			actionService.SetAndDeleteActions(identifier, toDeploy.Actions.ToSet, toDeploy.Actions.ToDelete);
			list2.Add(ServiceType.Action);
		}
		if (flag6)
		{
			calculationService.SetAndDeleteCalculations(identifier, toDeploy.Calculations.ToSet, toDeploy.Calculations.ToDelete);
			list2.Add(ServiceType.Calculation);
		}
		if (flag7)
		{
			stateMachineService.SetAndDeleteStateMachines(identifier, toDeploy.StateMachines.ToSet, toDeploy.StateMachines.ToDelete);
			list2.Add(ServiceType.Statemachine);
		}
		configurationService.CommitDeviceConfiguration(device.Identifier);
		FireDeploymentFinishedEvent(device, list2, toDeploy);
		Log.Information(Module.LemonbeatProtocolAdapter, $"Deployment of configuration updates for device {device} successful.");
	}

	private void FireDeploymentFinishedEvent(DeviceInformation deviceInfo, List<ServiceType> deployedServices, PhysicalConfigurationDifference deployedDifference)
	{
		EventHandler<DeployConfigurationChangesCompletedEventArgs> deployConfigurationChangesCompleted = this.DeployConfigurationChangesCompleted;
		if (deployConfigurationChangesCompleted != null)
		{
			DeployConfigurationChangesCompletedEventArgs e = new DeployConfigurationChangesCompletedEventArgs(deviceInfo.DeviceId, deployedServices, deployedDifference);
			deployConfigurationChangesCompleted(this, e);
		}
	}

	private void DoWithRetries(Action action, DeviceInformation device, string description)
	{
		int num = 0;
		do
		{
			try
			{
				num++;
				action();
				return;
			}
			catch (SocketException ex)
			{
				int errorCode = ex.ErrorCode;
				if (errorCode == 10004)
				{
					Log.Warning(Module.LemonbeatProtocolAdapter, $"Failed to {description} of device {device}. Try count: {num}. Error code: {ex.ErrorCode} (Timeout)");
				}
				else
				{
					Log.Warning(Module.LemonbeatProtocolAdapter, $"Failed to {description} of device {device}. Try count: {num}. Error code: {ex.ErrorCode} Exception: {ex.ToString()}");
				}
			}
			catch (Exception ex2)
			{
				Log.Warning(Module.LemonbeatProtocolAdapter, $"Failed to {description} of device {device}. Try count: {num}. Exception: {ex2.ToString()}");
			}
			Thread.Sleep(3000);
		}
		while (num < 3);
		Log.Warning(Module.LemonbeatProtocolAdapter, $"DeviceConfigurationAccess: Trying to {description}  failed after {num} retries. Operation will be aborted");
	}
}
