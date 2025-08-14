using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.PropertiesSets;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler.DeviceNotifications;

internal class CapabilityStateChangeNotifier
{
	private SubscriptionToken logicalDeviceStateChangeSubscriptionToken;

	private readonly IEventManager eventManager;

	private readonly INotificationHandler notificationHandler;

	private readonly IProtocolMultiplexer protocolMultiplexer;

	private readonly IRepository repository;

	public CapabilityStateChangeNotifier(IEventManager eventManager, IProtocolMultiplexer protocolMultiplexer, INotificationHandler notificationHandler, IRepository repository)
	{
		this.eventManager = eventManager;
		this.protocolMultiplexer = protocolMultiplexer;
		this.notificationHandler = notificationHandler;
		this.repository = repository;
	}

	public void Init()
	{
		if (logicalDeviceStateChangeSubscriptionToken == null)
		{
			logicalDeviceStateChangeSubscriptionToken = eventManager.GetEvent<LogicalDeviceStateChangedEvent>().Subscribe(LogicalDeviceStateChanged, null, ThreadOption.BackgroundThread, null);
		}
	}

	public void Uninit()
	{
		if (logicalDeviceStateChangeSubscriptionToken != null)
		{
			eventManager.GetEvent<LogicalDeviceStateChangedEvent>().Unsubscribe(logicalDeviceStateChangeSubscriptionToken);
			logicalDeviceStateChangeSubscriptionToken = null;
		}
	}

	private void LogicalDeviceStateChanged(LogicalDeviceStateChangedEventArgs eventArgs)
	{
		if (eventArgs != null && !SupressStateChangedNotification(eventArgs.LogicalDeviceId))
		{
			NotifyValueChanged(eventArgs);
		}
	}

	private bool SupressStateChangedNotification(Guid logicalDeviceId)
	{
		LogicalDevice logicalDevice = repository.GetLogicalDevice(logicalDeviceId);
		if (logicalDevice != null && logicalDevice.BaseDevice != null)
		{
			PhysicalDeviceState physicalDeviceState = protocolMultiplexer.PhysicalState.Get(logicalDevice.BaseDevice.Id);
			if (physicalDeviceState != null && !IsIncluded(physicalDeviceState) && !HasReachability(physicalDeviceState))
			{
				return true;
			}
		}
		return false;
	}

	private bool HasReachability(PhysicalDeviceState physicalDeviceState)
	{
		return !string.IsNullOrEmpty(physicalDeviceState.DeviceProperties.GetValueAsString(PhysicalDeviceBasicProperties.IsReachable));
	}

	private bool IsIncluded(PhysicalDeviceState physicalDeviceState)
	{
		return physicalDeviceState.DeviceProperties.GetValue<DeviceInclusionState>(PhysicalDeviceBasicProperties.DeviceInclusionState) == DeviceInclusionState.Included;
	}

	private void NotifyValueChanged(LogicalDeviceStateChangedEventArgs eventArgs)
	{
		if (eventArgs != null && eventArgs.NewLogicalDeviceState != null)
		{
			LogicalDeviceStatesChangedNotification logicalDeviceStatesChangedNotification = new LogicalDeviceStatesChangedNotification();
			logicalDeviceStatesChangedNotification.LogicalDeviceStates = new List<LogicalDeviceState> { eventArgs.NewLogicalDeviceState };
			logicalDeviceStatesChangedNotification.Namespace = "core.RWE";
			LogicalDeviceStatesChangedNotification notification = logicalDeviceStatesChangedNotification;
			notificationHandler.SendNotification(notification);
		}
	}
}
