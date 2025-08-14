using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Calibration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceInclusion;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceState;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Messages;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.EventConverters;
using SmartHome.Common.API.ModelTransformationService.EventConverters.CapabilitySpecific;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService;

public class EventConverterService : IEventConverterService
{
	private static readonly Dictionary<Type, IEventConverter> eventConverters;

	private static readonly List<Type> skippedEventConverters;

	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(EventConverterService));

	static EventConverterService()
	{
		eventConverters = new Dictionary<Type, IEventConverter>
		{
			{
				typeof(LogicalDeviceStatesChangedNotification),
				new LogicalDeviceStatesNotificationConverter()
			},
			{
				typeof(PhysicalDeviceStateChangedNotification),
				new PhysicalDeviceStateNotificationConverter()
			},
			{
				typeof(InvalidateTokenCacheNotification),
				new InvalidateTokenCacheNotificationConverter()
			},
			{
				typeof(PhysicalDeviceFoundNotification),
				new PhysicalDeviceFoundNotificationConverter()
			},
			{
				typeof(SaveConfigurationNotification),
				new SaveConfigurationNotificationConverter()
			},
			{
				typeof(SaveConfigurationErrorNotification),
				new SaveConfigurationErrorNotificationConverter()
			},
			{
				typeof(CommitConfigurationErrorNotification),
				new CommitConfigurationErrorNotificationConverter()
			},
			{
				typeof(GenericNotification),
				new GenericNotificationConverter()
			},
			{
				typeof(CalibrationNotification),
				new CalibrationNotificationConverter()
			},
			{
				typeof(NewMessageNotification),
				new NewMessageNotificationConverter()
			},
			{
				typeof(MessageStateChangedNotification),
				new MessageStateChangedNotificationConverter()
			},
			{
				typeof(MessageDeletionNotification),
				new MessageDeletionNotificationConverter()
			},
			{
				typeof(DeviceDiscoveryFailureNotification),
				new DeviceDiscoveryFailureNotificationConverter()
			},
			{
				typeof(HomeStateChangedNotification),
				new HomeStateChangedNotificationConverter()
			},
			{
				typeof(MemberStateChangedNotification),
				new MemberStateChangedNotificationConverter()
			},
			{
				typeof(DeviceDiscoveryStatusChangedNotification),
				new DeviceDiscoveryStatusChangedNotificationConverter()
			}
		};
		skippedEventConverters = new List<Type> { typeof(EntityDeploymentNotification) };
	}

	public List<Event> FromNotification(BaseNotification notification)
	{
		if (eventConverters.ContainsKey(notification.GetType()))
		{
			logger.Debug($"Converting SH BaseNotification with Type: {notification.GetType()}");
			return eventConverters[notification.GetType()].FromNotification(notification);
		}
		if (skippedEventConverters.Contains(notification.GetType()))
		{
			logger.Debug($"Skipped Notification Type: {notification.GetType()}");
			return new List<Event>();
		}
		logger.Error($"Notification type not supported: {notification.GetType()}");
		return new List<Event>();
	}
}
