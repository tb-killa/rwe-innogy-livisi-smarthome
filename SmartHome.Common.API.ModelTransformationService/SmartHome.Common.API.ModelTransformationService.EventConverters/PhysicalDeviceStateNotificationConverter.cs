using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceState;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.DeviceConverters;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.EventConverters;

public class PhysicalDeviceStateNotificationConverter : IEventConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(PhysicalDeviceStateNotificationConverter));

	public List<Event> FromNotification(BaseNotification notification)
	{
		logger.DebugEnterMethod("FromNotification");
		List<Event> list = new List<Event>();
		if (!(notification is PhysicalDeviceStateChangedNotification { DeviceState: var deviceState }))
		{
			return list;
		}
		string arg = deviceState.PhysicalDeviceId.ToString("N");
		DeviceConverter deviceConverter = new DeviceConverter();
		Event obj = new Event();
		obj.Type = "StateChanged";
		obj.Timestamp = notification.Timestamp;
		obj.Link = $"/device/{arg}";
		obj.Properties = deviceConverter.FromSmartHomeDeviceState(deviceState);
		obj.SequenceNumber = notification.SequenceNumber;
		obj.Namespace = notification.Namespace;
		Event item = obj;
		list.Add(item);
		logger.DebugExitMethod("FromNotification");
		return list;
	}
}
