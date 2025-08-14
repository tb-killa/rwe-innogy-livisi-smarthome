using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Messages;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.EventConverters;

public class MessageStateChangedNotificationConverter : IEventConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(MessageStateChangedNotificationConverter));

	public List<Event> FromNotification(BaseNotification notification)
	{
		logger.DebugEnterMethod("FromNotification");
		if (!(notification is MessageStateChangedNotification messageStateChangedNotification))
		{
			return new List<Event>();
		}
		Event obj = new Event();
		obj.Type = "MessageUpdated";
		obj.Timestamp = notification.Timestamp;
		obj.Description = string.Format("/desc/event/{0}", "MessageUpdated");
		obj.Link = string.Format("/message/{0}", messageStateChangedNotification.MessageId.ToString("N"));
		obj.Properties = new List<Property>
		{
			new Property
			{
				Name = "State",
				Value = messageStateChangedNotification.State.ToString()
			}
		};
		obj.Data = new SmartHome.Common.API.Entities.Entities.Message
		{
			Id = messageStateChangedNotification.MessageId.ToString("N"),
			Read = (messageStateChangedNotification.State == MessageState.Read)
		};
		obj.SequenceNumber = notification.SequenceNumber;
		obj.Namespace = notification.Namespace;
		Event obj2 = obj;
		logger.Debug($"Event received: {obj2}");
		List<Event> list = new List<Event>(1);
		list.Add(obj2);
		return list;
	}
}
