using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceInclusion;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.EventConverters;

internal class PhysicalDeviceFoundNotificationConverter : IEventConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(PhysicalDeviceFoundNotificationConverter));

	public List<Event> FromNotification(BaseNotification notification)
	{
		logger.DebugEnterMethod("FromNotification");
		List<Event> list = new List<Event>();
		if (!(notification is PhysicalDeviceFoundNotification physicalDeviceFoundNotification))
		{
			return list;
		}
		if (physicalDeviceFoundNotification.State != DeviceFoundState.ReadyForInclusion)
		{
			logger.Debug("Preparing DiscoveryFailureEvent...");
			list.Add(GetDeviceDiscoveryFailureEvent(physicalDeviceFoundNotification));
		}
		else
		{
			logger.Debug("Preparing DeviceFoundEvent...");
			list.Add(GetDeviceFoundEvent(physicalDeviceFoundNotification));
		}
		logger.DebugExitMethod("FromNotification");
		return list;
	}

	private Event GetDeviceDiscoveryFailureEvent(PhysicalDeviceFoundNotification notification)
	{
		Event obj = new Event();
		obj.Type = "DeviceDiscoveryFailure";
		obj.Timestamp = notification.Timestamp;
		obj.Link = string.Format("/desc/device/{0}.{1}/{2}", BuiltinPhysicalDeviceType.SHC, "RWE", "1.0");
		obj.Properties = new List<Property>
		{
			new Property
			{
				Name = "FailureCode",
				Value = notification.State.ToString()
			},
			new Property
			{
				Name = "Manufacturer",
				Value = notification.FoundDevice.Manufacturer
			},
			new Property
			{
				Name = "DeviceType",
				Value = notification.FoundDevice.DeviceType
			},
			new Property
			{
				Name = "Version",
				Value = notification.FoundDevice.DeviceVersion
			}
		};
		obj.SequenceNumber = notification.SequenceNumber;
		obj.Namespace = notification.Namespace;
		return obj;
	}

	private Event GetDeviceFoundEvent(PhysicalDeviceFoundNotification notification)
	{
		DeviceConverterService deviceConverterService = new DeviceConverterService();
		Event obj = new Event();
		obj.Type = "DeviceFound";
		obj.Timestamp = DateTime.UtcNow;
		obj.Link = string.Format("/desc/device/{0}.{1}/{2}", BuiltinPhysicalDeviceType.SHC.ToString(), "RWE", "1.0");
		obj.Data = deviceConverterService.FromSmartHomeBaseDevice(notification.FoundDevice, includeCapabilities: false);
		obj.SequenceNumber = notification.SequenceNumber;
		obj.Namespace = notification.Namespace;
		return obj;
	}
}
