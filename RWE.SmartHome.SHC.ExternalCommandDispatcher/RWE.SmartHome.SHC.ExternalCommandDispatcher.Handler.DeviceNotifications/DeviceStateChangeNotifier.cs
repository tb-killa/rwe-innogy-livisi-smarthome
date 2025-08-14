using System;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceState;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler.DeviceNotifications;

internal class DeviceStateChangeNotifier
{
	private SubscriptionToken physicalDeviceStateChangedSubscriptionToken;

	private SubscriptionToken inclusionStateChangedSubscriptionToken;

	private SubscriptionToken configurationStateChangedSubscriptionToken;

	private SubscriptionToken deviceUnreachableEventSubscriptionToken;

	private readonly IEventManager eventManager;

	private readonly INotificationHandler notificationHandler;

	private readonly IProtocolMultiplexer protocolMultiplexer;

	private SubscriptionToken updateStateChangedSubscriptionToken;

	public DeviceStateChangeNotifier(IEventManager eventManager, IProtocolMultiplexer protocolMultiplexer, INotificationHandler notificationHandler)
	{
		this.eventManager = eventManager;
		this.protocolMultiplexer = protocolMultiplexer;
		this.notificationHandler = notificationHandler;
	}

	public void Init()
	{
		if (physicalDeviceStateChangedSubscriptionToken == null)
		{
			physicalDeviceStateChangedSubscriptionToken = eventManager.GetEvent<PhysicalDeviceStateChangedEvent>().Subscribe(PhysicalDeviceStateChanged, (PhysicalDeviceStateChangedEventArgs args) => args != null, ThreadOption.BackgroundThread, null);
		}
		if (deviceUnreachableEventSubscriptionToken == null)
		{
			deviceUnreachableEventSubscriptionToken = eventManager.GetEvent<DeviceUnreachableChangedEvent>().Subscribe(ProcessDeviceUnreachableChanged, (DeviceUnreachableChangedEventArgs args) => args != null, ThreadOption.PublisherThread, null);
		}
		if (inclusionStateChangedSubscriptionToken == null)
		{
			inclusionStateChangedSubscriptionToken = eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Subscribe(ProcessInclusionStateChanged, (DeviceInclusionStateChangedEventArgs args) => args != null, ThreadOption.PublisherThread, null);
		}
		if (configurationStateChangedSubscriptionToken == null)
		{
			configurationStateChangedSubscriptionToken = eventManager.GetEvent<DeviceConfiguredEvent>().Subscribe(ProcessConfigurationStatusUpdated, (DeviceConfiguredEventArgs args) => args != null, ThreadOption.PublisherThread, null);
		}
		if (updateStateChangedSubscriptionToken == null)
		{
			updateStateChangedSubscriptionToken = eventManager.GetEvent<DeviceUpdateStateChangedEvent>().Subscribe(ProcessFirmwareUpdateStateChanged, (DeviceUpdateStateChangedEventArgs args) => args.NewDeviceUpdateState == DeviceUpdateState.ImageTransferred || args.NewDeviceUpdateState == DeviceUpdateState.UpdatePending || args.NewDeviceUpdateState == DeviceUpdateState.UpToDate || (args.OldDeviceUpdateState == DeviceUpdateState.ImageTransferred && args.NewDeviceUpdateState == DeviceUpdateState.UpdatePending), ThreadOption.PublisherThread, null);
		}
	}

	public void Uninit()
	{
		if (inclusionStateChangedSubscriptionToken != null)
		{
			eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Unsubscribe(inclusionStateChangedSubscriptionToken);
			inclusionStateChangedSubscriptionToken = null;
		}
		if (configurationStateChangedSubscriptionToken != null)
		{
			eventManager.GetEvent<DeviceConfiguredEvent>().Unsubscribe(configurationStateChangedSubscriptionToken);
			configurationStateChangedSubscriptionToken = null;
		}
		if (deviceUnreachableEventSubscriptionToken != null)
		{
			eventManager.GetEvent<DeviceUnreachableChangedEvent>().Unsubscribe(deviceUnreachableEventSubscriptionToken);
			deviceUnreachableEventSubscriptionToken = null;
		}
		if (updateStateChangedSubscriptionToken != null)
		{
			eventManager.GetEvent<DeviceUpdateStateChangedEvent>().Unsubscribe(updateStateChangedSubscriptionToken);
			updateStateChangedSubscriptionToken = null;
		}
	}

	private void ProcessDeviceUnreachableChanged(DeviceUnreachableChangedEventArgs eventArgs)
	{
		SendDeviceStateChangedNotification(eventArgs.DeviceId);
	}

	private void PhysicalDeviceStateChanged(PhysicalDeviceStateChangedEventArgs eventArgs)
	{
		eventArgs.PhysicalState.PhysicalDeviceId = eventArgs.DeviceId;
		SendDeviceStateChangedNotification(eventArgs.PhysicalState);
	}

	private void ProcessInclusionStateChanged(DeviceInclusionStateChangedEventArgs eventArgs)
	{
		SendDeviceStateChangedNotification(eventArgs.DeviceId);
	}

	private void ProcessFirmwareUpdateStateChanged(DeviceUpdateStateChangedEventArgs eventArgs)
	{
		if (eventArgs != null)
		{
			SendDeviceStateChangedNotification(eventArgs.DeviceId);
		}
	}

	private void ProcessConfigurationStatusUpdated(DeviceConfiguredEventArgs eventArgs)
	{
		SendDeviceStateChangedNotification(eventArgs.DeviceId);
	}

	private PhysicalDeviceState RemoveLastChangedTimestamp(PhysicalDeviceState deviceState)
	{
		PhysicalDeviceState physicalDeviceState = new PhysicalDeviceState();
		physicalDeviceState.PhysicalDeviceId = deviceState.PhysicalDeviceId;
		PhysicalDeviceState physicalDeviceState2 = physicalDeviceState;
		physicalDeviceState2.DeviceProperties = new PropertyBag
		{
			Properties = deviceState.DeviceProperties.Properties.ToList()
		};
		physicalDeviceState2.DeviceProperties.Properties.ForEach(delegate(Property p)
		{
			p.UpdateTimestamp = null;
		});
		return physicalDeviceState2;
	}

	private void SendDeviceStateChangedNotification(Guid? deviceId)
	{
		if (deviceId.HasValue)
		{
			PhysicalDeviceState physicalDeviceState = protocolMultiplexer.PhysicalState.Get(deviceId.Value);
			if (physicalDeviceState != null)
			{
				SendDeviceStateChangedNotification(physicalDeviceState);
			}
		}
	}

	private void SendDeviceStateChangedNotification(PhysicalDeviceState deviceState)
	{
		if (deviceState != null)
		{
			PhysicalDeviceState deviceState2 = RemoveLastChangedTimestamp(deviceState);
			notificationHandler.SendNotification(new PhysicalDeviceStateChangedNotification(deviceState2)
			{
				Namespace = "core.RWE"
			});
		}
	}
}
