using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.ProtocolMultiplexer;

public class LogicalStateRequestor : ILogicalStateRequestor
{
	private readonly IRepository configurationRepository;

	private readonly IApplicationsHost appHost;

	private readonly ILogicalDeviceStateRepository logicalDeviceStateRepository;

	private readonly Dictionary<ProtocolIdentifier, IProtocolSpecificLogicalStateRequestor> protocolSpecificStateRequestors = new Dictionary<ProtocolIdentifier, IProtocolSpecificLogicalStateRequestor>();

	public LogicalStateRequestor(IEventManager eventManager, IRepository configurationRepository, IApplicationsHost appHost, ILogicalDeviceStateRepository logicalDeviceStateRepository)
	{
		this.configurationRepository = configurationRepository;
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(RequestAllStatusInfosOfCoreDevices, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.RuleEngineRunning, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Subscribe(RequestStatusInfoForNewIncludedDevice, (DeviceInclusionStateChangedEventArgs e) => e.DeviceInclusionState == DeviceInclusionState.Included, ThreadOption.PublisherThread, null);
		this.appHost = appHost;
		this.logicalDeviceStateRepository = logicalDeviceStateRepository;
		appHost.ApplicationStateChanged += OnApplicationActivated;
	}

	public void RegisterProtocolSpecificStateRequestor(IProtocolSpecificLogicalStateRequestor protocolSpecificLogicalStateRequestor)
	{
		if (protocolSpecificLogicalStateRequestor != null)
		{
			protocolSpecificStateRequestors.Add(protocolSpecificLogicalStateRequestor.ProtocolId, protocolSpecificLogicalStateRequestor);
		}
	}

	private void RequestStatusInfoForNewIncludedDevice(DeviceInclusionStateChangedEventArgs args)
	{
		BaseDevice baseDevice = configurationRepository.GetBaseDevice(args.DeviceId);
		if (baseDevice != null && protocolSpecificStateRequestors.TryGetValue(baseDevice.ProtocolId, out var value))
		{
			value.RequestState(baseDevice);
		}
	}

	internal void RequestAllStatusInfosOfCoreDevices(ShcStartupCompletedEventArgs args)
	{
		foreach (LogicalDevice item in from d in configurationRepository.GetOriginalLogicalDevices()
			where d.BaseDevice == null || d.BaseDevice.AppId == CoreConstants.CoreAppId
			select d)
		{
			ProtocolIdentifier protocolIdentifier = ProtocolMultiplexerHelpers.GetProtocolIdentifier(item);
			if (protocolSpecificStateRequestors.TryGetValue(protocolIdentifier, out var value))
			{
				value.RequestState(item);
			}
		}
	}

	private void OnApplicationActivated(ApplicationLoadStateChangedEventArgs args)
	{
		if (args == null || args.Application == null || (args.ApplicationState != ApplicationStates.ApplicationActivated && args.ApplicationState != ApplicationStates.ApplicationUpdated))
		{
			return;
		}
		IEnumerable<BaseDevice> enumerable = from device in configurationRepository.GetBaseDevices()
			where device.AppId == args.Application.ApplicationId
			select device;
		foreach (BaseDevice item in enumerable)
		{
			try
			{
				if (protocolSpecificStateRequestors.TryGetValue(item.ProtocolId, out var value))
				{
					value.RequestState(item);
				}
			}
			catch (Exception ex)
			{
				Log.Error(Module.ProtocolMultiplexer, string.Format("Error requesting state for custom application with AppId = {0}; {1}", item.AppId, ex.Message + " -- " + ex.StackTrace));
			}
		}
	}
}
