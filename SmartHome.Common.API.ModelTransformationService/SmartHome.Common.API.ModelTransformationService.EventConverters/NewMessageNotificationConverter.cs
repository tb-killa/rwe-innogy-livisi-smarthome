using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Messages;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.EventConverters;

public class NewMessageNotificationConverter : IEventConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(NewMessageNotificationConverter));

	public List<Event> FromNotification(BaseNotification notification)
	{
		logger.DebugEnterMethod("FromNotification");
		if (!(notification is NewMessageNotification newMessageNotification))
		{
			return new List<Event>();
		}
		MessageConverterService messageConverterService = new MessageConverterService();
		Event obj = new Event();
		obj.Type = "MessageCreated";
		obj.Description = string.Format("/desc/event/{0}", "MessageCreated");
		obj.Timestamp = notification.Timestamp;
		obj.Link = string.Format("/desc/device/{0}.{1}/{2}", BuiltinPhysicalDeviceType.SHC, "RWE", "1.0");
		obj.Data = messageConverterService.FromSmartHomeMessage(newMessageNotification.Message);
		obj.SequenceNumber = notification.SequenceNumber;
		obj.Namespace = notification.Namespace;
		Event obj2 = obj;
		logger.Debug($"Event received: {obj2}");
		List<Event> list = new List<Event>(1);
		list.Add(obj2);
		return list;
	}
}
