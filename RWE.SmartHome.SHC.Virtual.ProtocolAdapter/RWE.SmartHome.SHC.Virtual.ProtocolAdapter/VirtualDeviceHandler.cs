using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceInclusion;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.CoreApiConverters;
using SmartHome.SHC.API;
using SmartHome.SHC.API.Configuration.Services;
using SmartHome.SHC.API.Protocols.CustomProtocol;

namespace RWE.SmartHome.SHC.Virtual.ProtocolAdapter;

public class VirtualDeviceHandler
{
	private IEventManager eventManager;

	private IApplicationsHost applicationsHost;

	private bool deviceInclusionActivated;

	public VirtualDeviceHandler(IApplicationsHost applicationsHost, IEventManager eventManager)
	{
		this.eventManager = eventManager;
		this.applicationsHost = applicationsHost;
		SubscribeToEvents();
	}

	private void SubscribeToEvents()
	{
		applicationsHost.ApplicationStateChanged += OnApplicationStateChanged;
		eventManager.GetEvent<DeviceDiscoveryStatusChangedEvent>().Subscribe(OnDeviceDiscoveryStatusChanged, null, ThreadOption.PublisherThread, null);
	}

	private void OnDeviceDiscoveryStatusChanged(DeviceDiscoveryStatusChangedEventArgs args)
	{
		deviceInclusionActivated = true;
		if (args.Phase == DiscoveryPhase.Prepare)
		{
			return;
		}
		if (args.Phase == DiscoveryPhase.Deactivate)
		{
			deviceInclusionActivated = false;
		}
		List<string> list = args.AppIds ?? (from x in applicationsHost.GetCustomDevices<IAddIn>()
			select x.ApplicationId).ToList();
		DiscoveryMode discoveryMode = GetDiscoveryMode(args.Phase, args.AppIds != null);
		List<string> list2 = new List<string>();
		foreach (string item in list)
		{
			if (SetDiscoveryMode(item, discoveryMode) == DiscoveryStatus.Failure)
			{
				list2.Add(item);
			}
		}
		if (list2.Any())
		{
			eventManager.GetEvent<DeviceDiscoveryFailedEvent>().Publish(new DeviceDiscoveryFailedEventArgs
			{
				AppIds = list2
			});
		}
	}

	private void OnApplicationStateChanged(ApplicationLoadStateChangedEventArgs eventArgs)
	{
		if (eventArgs != null && eventArgs.Application != null && eventArgs.Application is ICustomProtocolDeviceHandler virtualDeviceHandler)
		{
			switch (eventArgs.ApplicationState)
			{
			case ApplicationStates.ApplicationActivated:
			case ApplicationStates.ApplicationUpdated:
				OnApplicationActivated(virtualDeviceHandler, eventArgs);
				break;
			case ApplicationStates.ApplicationDeactivated:
			case ApplicationStates.ApplicationsUninstalled:
				OnApplicationDeactivated(virtualDeviceHandler, eventArgs);
				break;
			case ApplicationStates.ApplicationAdded:
				break;
			}
		}
	}

	private void OnApplicationActivated(ICustomProtocolDeviceHandler virtualDeviceHandler, ApplicationLoadStateChangedEventArgs eventArgs)
	{
		virtualDeviceHandler.DeviceFoundEvent += delegate(object sender, global::SmartHome.SHC.API.Configuration.Services.DeviceFoundEventArgs args)
		{
			PublishDeviceFoundEvent(sender, args, eventArgs.Application.ApplicationId);
		};
	}

	private void OnApplicationDeactivated(ICustomProtocolDeviceHandler virtualDeviceHandler, ApplicationLoadStateChangedEventArgs eventArgs)
	{
		virtualDeviceHandler.DeviceFoundEvent -= delegate(object sender, global::SmartHome.SHC.API.Configuration.Services.DeviceFoundEventArgs args)
		{
			PublishDeviceFoundEvent(sender, args, eventArgs.Application.ApplicationId);
		};
	}

	private DiscoveryMode GetDiscoveryMode(DiscoveryPhase phase, bool addinSpecific)
	{
		if (phase == DiscoveryPhase.Activate)
		{
			if (!addinSpecific)
			{
				return DiscoveryMode.Activate;
			}
			return DiscoveryMode.ActivateSpecific;
		}
		return DiscoveryMode.Deactivate;
	}

	private DiscoveryStatus SetDiscoveryMode(string appId, DiscoveryMode discoveryMode)
	{
		ICustomProtocolDeviceHandler customDevice = applicationsHost.GetCustomDevice<ICustomProtocolDeviceHandler>(appId);
		if (customDevice != null)
		{
			try
			{
				customDevice.SetDiscoveryMode((discoveryMode == DiscoveryMode.Activate) ? DiscoveryMode.ActivateSpecific : discoveryMode);
			}
			catch (Exception ex)
			{
				Log.Error(Module.VirtualProtocolAdapter, $"Error occured while starting discovery for App: {appId}, error: {ex.Message}");
				return DiscoveryStatus.Failure;
			}
		}
		return DiscoveryStatus.Success;
	}

	private void PublishDeviceFoundEvent(object sender, global::SmartHome.SHC.API.Configuration.Services.DeviceFoundEventArgs args, string applicationId)
	{
		if (deviceInclusionActivated)
		{
			RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events.DeviceFoundEventArgs e = new RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events.DeviceFoundEventArgs();
			e.State = DeviceFoundState.ReadyForInclusion;
			e.FoundDevice = GetCoreBaseDevice(args, applicationId);
			RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events.DeviceFoundEventArgs payload = e;
			eventManager.GetEvent<PhysicalDeviceFoundEvent>().Publish(payload);
		}
	}

	private BaseDevice GetCoreBaseDevice(global::SmartHome.SHC.API.Configuration.Services.DeviceFoundEventArgs args, string applicationId)
	{
		BaseDevice baseDevice = args.FoundDevice.ToCoreBaseDevice();
		baseDevice.AppId = applicationId;
		baseDevice.ProtocolId = ProtocolIdentifier.Virtual;
		return baseDevice;
	}
}
