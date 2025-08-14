using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceInclusion;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.BusinessLogic.DeviceInclusion;

internal class DiscoveryNotificationsDispatcher
{
	private readonly INotificationHandler notificationHandler;

	public DiscoveryNotificationsDispatcher(INotificationHandler notificationHandler)
	{
		this.notificationHandler = notificationHandler;
	}

	public void NotifyDeviceDiscoveryStateChanged(bool active, List<string> appIds)
	{
		DeviceDiscoveryStatusChangedNotification deviceDiscoveryStatusChangedNotification = new DeviceDiscoveryStatusChangedNotification();
		deviceDiscoveryStatusChangedNotification.Active = active;
		deviceDiscoveryStatusChangedNotification.NotificationId = Guid.NewGuid();
		deviceDiscoveryStatusChangedNotification.AppIds = appIds;
		deviceDiscoveryStatusChangedNotification.Namespace = "core.RWE";
		DeviceDiscoveryStatusChangedNotification notification = deviceDiscoveryStatusChangedNotification;
		notificationHandler.SendNotification(notification);
	}

	public void NotifyDeviceDiscovered(BaseDevice foundDevice, DeviceFoundState deviceFoundState)
	{
		if (foundDevice == null)
		{
			Log.Error(Module.BusinessLogic, "Trying to notify discovery of null device");
			return;
		}
		PhysicalDeviceFoundNotification physicalDeviceFoundNotification = new PhysicalDeviceFoundNotification();
		physicalDeviceFoundNotification.FoundDevice = foundDevice;
		physicalDeviceFoundNotification.State = deviceFoundState;
		physicalDeviceFoundNotification.Namespace = "core.RWE";
		PhysicalDeviceFoundNotification notification = physicalDeviceFoundNotification;
		notificationHandler.SendNotification(notification);
	}

	public void NotifyDiscoveredDevices(List<DiscoveredDevice> discoveredDevices)
	{
		foreach (DiscoveredDevice discoveredDevice in discoveredDevices)
		{
			NotifyDeviceDiscovered(discoveredDevice.Device, DeviceFoundState.ReadyForInclusion);
		}
	}
}
