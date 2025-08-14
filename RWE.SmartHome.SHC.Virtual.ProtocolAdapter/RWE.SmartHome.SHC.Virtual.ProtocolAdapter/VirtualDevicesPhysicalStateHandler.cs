using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.PropertiesSets;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.CoreApiConverters;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;
using SmartHome.SHC.API;
using SmartHome.SHC.API.Control;
using SmartHome.SHC.API.Protocols.CustomProtocol;

namespace RWE.SmartHome.SHC.Virtual.ProtocolAdapter;

internal class VirtualDevicesPhysicalStateHandler : IProtocolSpecificPhysicalStateHandler
{
	private readonly IEventManager eventManager;

	private readonly List<IVirtualCurrentPhysicalStateHandler> deviceHandlers = new List<IVirtualCurrentPhysicalStateHandler>();

	private readonly List<Guid> statefulDeviceIds = new List<Guid>();

	private readonly IRepository repository;

	private readonly IApplicationsHost appHost;

	private readonly Dictionary<string, ICustomProtocolDeviceStateHandler> physicalStateHandlers = new Dictionary<string, ICustomProtocolDeviceStateHandler>();

	private readonly object syncRoot = new object();

	private List<Guid> deviceIds = new List<Guid>();

	public VirtualDevicesPhysicalStateHandler(IEventManager eventManager, IRepository repository, IApplicationsHost appHost)
	{
		this.eventManager = eventManager;
		this.repository = repository;
		this.appHost = appHost;
		appHost.ApplicationStateChanged += OnApplicationDeactivated;
		appHost.ApplicationStateChanged += OnApplicationActivated;
		VirtualDeviceHandlerAvailableEvent virtualDeviceHandlerAvailableEvent = eventManager.GetEvent<VirtualDeviceHandlerAvailableEvent>();
		Action<VirtualDeviceHandlerAvailableEventArgs> action = delegate(VirtualDeviceHandlerAvailableEventArgs args)
		{
			lock (syncRoot)
			{
				IVirtualCurrentPhysicalStateHandler handler = args.Handler;
				if (!deviceHandlers.Contains(handler))
				{
					deviceHandlers.Add(handler);
					handler.StateChanged += deviceHandler_StateChanged;
				}
			}
		};
		virtualDeviceHandlerAvailableEvent.Subscribe(action, null, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ConfigurationProcessedEvent>().Subscribe(OnConfigurationChanged, (ConfigurationProcessedEventArgs args) => args.ConfigurationPhase == ConfigurationProcessedPhase.CompletedInternally, ThreadOption.BackgroundThread, null);
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcStartup, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound1, ThreadOption.BackgroundThread, null);
	}

	private void OnShcStartup(ShcStartupCompletedEventArgs args)
	{
		RefreshDeviceIdCache();
	}

	private void RefreshDeviceIdCache()
	{
		deviceIds = (from x in repository.GetBaseDevices()
			where x.ProtocolId == ProtocolIdentifier.Virtual
			select x.Id).ToList();
	}

	private void OnApplicationActivated(ApplicationLoadStateChangedEventArgs args)
	{
		if (args != null && args.Application != null && IsApplicationActivated(args.ApplicationState))
		{
			SubscribeToPluginPhysicalStateChangedEvents(args.Application);
			RegisterPhysicalHandlers(args.Application);
		}
	}

	private void OnApplicationDeactivated(ApplicationLoadStateChangedEventArgs args)
	{
		if (args != null && args.Application != null && IsApplicationDeactivated(args.ApplicationState))
		{
			UnsubscribeFromPluginPhysicalStateChangedEvents(args.Application);
			UnregisterPhysicalHandlers(args.Application);
		}
	}

	private bool IsApplicationDeactivated(ApplicationStates appState)
	{
		if (appState != ApplicationStates.ApplicationDeactivated && appState != ApplicationStates.ApplicationsUninstalled)
		{
			return appState == ApplicationStates.ApplicationUpdated;
		}
		return true;
	}

	private bool IsApplicationActivated(ApplicationStates appState)
	{
		if (appState != ApplicationStates.ApplicationActivated)
		{
			return appState == ApplicationStates.ApplicationUpdated;
		}
		return true;
	}

	private void SubscribeToPluginPhysicalStateChangedEvents(IAddIn plugin)
	{
		if (plugin != null && plugin is ICustomProtocolDeviceStateHandler customProtocolDeviceStateHandler)
		{
			customProtocolDeviceStateHandler.PhysicalStateChanged += PublishVirtualDeviceStateChangedEvent;
		}
	}

	private void RegisterPhysicalHandlers(IAddIn plugin)
	{
		if (!(plugin is ICustomProtocolDeviceStateHandler))
		{
			return;
		}
		lock (syncRoot)
		{
			if (!physicalStateHandlers.ContainsKey(plugin.ApplicationId))
			{
				physicalStateHandlers.Add(plugin.ApplicationId, plugin as ICustomProtocolDeviceStateHandler);
			}
		}
	}

	private void UnsubscribeFromPluginPhysicalStateChangedEvents(IAddIn plugin)
	{
		if (plugin != null && plugin is ICustomProtocolDeviceStateHandler customProtocolDeviceStateHandler)
		{
			customProtocolDeviceStateHandler.PhysicalStateChanged -= PublishVirtualDeviceStateChangedEvent;
		}
	}

	private void UnregisterPhysicalHandlers(IAddIn plugin)
	{
		lock (syncRoot)
		{
			if (physicalStateHandlers.ContainsKey(plugin.ApplicationId))
			{
				physicalStateHandlers.Remove(plugin.ApplicationId);
			}
		}
	}

	private void PublishVirtualDeviceStateChangedEvent(object sender, DeviceStateChangedEventArgs args)
	{
		if (deviceIds.Contains(args.DeviceId) && args.DeviceState != null)
		{
			PhysicalDeviceState physicalState = args.DeviceState.ToCorePhysicalDeviceState(args.DeviceId);
			eventManager.GetEvent<PhysicalDeviceStateChangedEvent>().Publish(new PhysicalDeviceStateChangedEventArgs(args.DeviceId, physicalState));
		}
		else
		{
			Log.Warning(Module.VirtualProtocolAdapter, $"Invalid device state emitted for device {args.DeviceId} (nullstate={args.DeviceState == null})");
		}
	}

	private void deviceHandler_StateChanged(object sender, VirtualDeviceAvailableArgs e)
	{
		if (!statefulDeviceIds.Contains(e.DeviceId))
		{
			statefulDeviceIds.Add(e.DeviceId);
		}
	}

	public PhysicalDeviceState Get(Guid physicalDeviceId)
	{
		BaseDevice baseDevice = repository.GetBaseDevice(physicalDeviceId);
		PhysicalDeviceState physicalDeviceState = null;
		if (baseDevice != null && baseDevice.AppId != CoreConstants.CoreAppId)
		{
			ICustomProtocolDeviceStateHandler customDevice = appHost.GetCustomDevice<ICustomProtocolDeviceStateHandler>(baseDevice.AppId);
			if (customDevice != null)
			{
				physicalDeviceState = customDevice.GetPhysicalState(baseDevice.Id).ToCorePhysicalDeviceState(physicalDeviceId);
			}
			else
			{
				Log.Debug(Module.VirtualProtocolAdapter, $"There is no physical device handler registered for AppId={baseDevice.AppId}");
				PropertyBag propertyBag = new PropertyBag();
				propertyBag.SetValue(PhysicalDeviceBasicProperties.DeviceInclusionState, DeviceInclusionState.Included);
				PhysicalDeviceState physicalDeviceState2 = new PhysicalDeviceState();
				physicalDeviceState2.DeviceProperties = propertyBag;
				physicalDeviceState2.PhysicalDeviceId = physicalDeviceId;
				physicalDeviceState = physicalDeviceState2;
			}
		}
		else
		{
			lock (syncRoot)
			{
				foreach (IVirtualCurrentPhysicalStateHandler deviceHandler in deviceHandlers)
				{
					physicalDeviceState = deviceHandler.GetState(physicalDeviceId);
					if (physicalDeviceState != null)
					{
						break;
					}
				}
			}
		}
		return physicalDeviceState;
	}

	public List<PhysicalDeviceState> GetAll()
	{
		List<PhysicalDeviceState> list = new List<PhysicalDeviceState>();
		foreach (Guid statefulDeviceId in statefulDeviceIds)
		{
			PhysicalDeviceState physicalDeviceState = Get(statefulDeviceId);
			if (physicalDeviceState != null)
			{
				list.Add(physicalDeviceState);
			}
		}
		foreach (BaseDevice baseDevice in repository.GetBaseDevices())
		{
			if (physicalStateHandlers.TryGetValue(baseDevice.AppId, out var value) && value != null)
			{
				PhysicalDeviceState physicalDeviceState2 = value.GetPhysicalState(baseDevice.Id).ToCorePhysicalDeviceState(baseDevice.Id);
				if (physicalDeviceState2 != null)
				{
					list.Add(physicalDeviceState2);
				}
			}
		}
		return list;
	}

	public void UpdateDeviceConfigurationState(Guid deviceId, DeviceConfigurationState newConfigurationState)
	{
	}

	private void OnConfigurationChanged(ConfigurationProcessedEventArgs args)
	{
		RefreshDeviceIdCache();
		BaseDevice modifiedBaseDevice;
		foreach (BaseDevice modifiedBaseDevice2 in args.ModifiedBaseDevices)
		{
			modifiedBaseDevice = modifiedBaseDevice2;
			if (args.RepositoryInclusionReport != null && args.RepositoryInclusionReport.Any((EntityMetadata dr) => dr.Id == modifiedBaseDevice.Id && dr.EntityType == EntityType.BaseDevice))
			{
				eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Publish(new DeviceInclusionStateChangedEventArgs(modifiedBaseDevice.Id, DeviceInclusionState.Included, ProtocolIdentifier.Virtual.ToString()));
			}
		}
		foreach (BaseDevice deletedBaseDevice in args.DeletedBaseDevices)
		{
			eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Publish(new DeviceInclusionStateChangedEventArgs(deletedBaseDevice.Id, DeviceInclusionState.ExclusionPending, ProtocolIdentifier.Virtual.ToString()));
		}
	}
}
