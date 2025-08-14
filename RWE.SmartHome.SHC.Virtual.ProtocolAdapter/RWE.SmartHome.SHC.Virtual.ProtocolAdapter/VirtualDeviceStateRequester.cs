using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.CoreApiConverters;
using SmartHome.SHC.API.Control;
using SmartHome.SHC.API.PropertyDefinition;
using SmartHome.SHC.API.Protocols.CustomProtocol;

namespace RWE.SmartHome.SHC.Virtual.ProtocolAdapter;

public class VirtualDeviceStateRequester : IProtocolSpecificLogicalStateRequestor
{
	private readonly IApplicationsHost applicationsHost;

	private readonly IRepository configurationRepository;

	private readonly IEventManager eventManager;

	private readonly List<IVirtualCoreActionHandler> coreStateHandlers;

	public ProtocolIdentifier ProtocolId => ProtocolIdentifier.Virtual;

	public VirtualDeviceStateRequester(IRepository configurationRepository, IApplicationsHost applicationsHost, IEventManager eventManager, List<IVirtualCoreActionHandler> coreStateHandlers)
	{
		this.applicationsHost = applicationsHost;
		this.configurationRepository = configurationRepository;
		this.eventManager = eventManager;
		this.coreStateHandlers = coreStateHandlers;
	}

	public void RequestState(LogicalDevice logicalDevice)
	{
		BaseDevice baseDevice = logicalDevice.BaseDevice;
		if (baseDevice == null)
		{
			return;
		}
		if (baseDevice.AppId == CoreConstants.CoreAppId)
		{
			try
			{
				foreach (IVirtualCoreActionHandler coreStateHandler in coreStateHandlers)
				{
					coreStateHandler.RequestState(logicalDevice.Id);
				}
				return;
			}
			catch (Exception ex)
			{
				Log.Error(Module.VirtualProtocolAdapter, "Error requesting state of core device of type " + baseDevice.DeviceType + ": " + ex.Message);
				return;
			}
		}
		CapabilityState capabilityState = null;
		try
		{
			ICustomProtocolCapabilityStateHandler customDevice = applicationsHost.GetCustomDevice<ICustomProtocolCapabilityStateHandler>(baseDevice.AppId);
			if (customDevice != null)
			{
				capabilityState = customDevice.GetState(logicalDevice.Id);
			}
		}
		catch (Exception arg)
		{
			Log.Error(Module.VirtualProtocolAdapter, $"Could not retrieve state for app {baseDevice.AppId}: {arg}");
		}
		if (capabilityState != null)
		{
			PublishVirtualDeviceStateChangedEvent(this, new CapabilityStateChangedEventArgs(logicalDevice.Id, capabilityState));
		}
	}

	public void RequestState(BaseDevice baseDevice)
	{
		foreach (LogicalDevice item in from ld in configurationRepository.GetOriginalLogicalDevices()
			where ld.BaseDeviceId == baseDevice.Id
			select ld)
		{
			RequestState(item);
		}
	}

	private void PublishVirtualDeviceStateChangedEvent(object sender, CapabilityStateChangedEventArgs args)
	{
		if (args.State.IsTransient)
		{
			eventManager.GetEvent<DeviceEventDetectedEvent>().Publish(new DeviceEventDetectedEventArgs(args.DeviceId, string.Empty, args.State.Properties.ToList().ConvertAll((Property apd) => apd.ToCoreProperty(includeTimestamp: true))));
			return;
		}
		GenericDeviceState genericDeviceState = args.State.ToCoreDeviceState(args.DeviceId);
		if (genericDeviceState == null)
		{
			Log.Warning(Module.VirtualProtocolAdapter, $"Could not convert state for device {args.DeviceId}");
		}
		else
		{
			eventManager.GetEvent<RawLogicalDeviceStateChangedEvent>().Publish(new RawLogicalDeviceStateChangedEventArgs(args.DeviceId, genericDeviceState));
		}
	}
}
