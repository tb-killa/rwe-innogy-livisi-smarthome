using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Messages;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.EventConverters;

public class MessageDeletionNotificationConverter : IEventConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(MessageDeletionNotificationConverter));

	public List<Event> FromNotification(BaseNotification notification)
	{
		logger.DebugEnterMethod("FromNotification");
		if (!(notification is MessageDeletionNotification messageDeletionNotification))
		{
			return new List<Event>();
		}
		Event obj = new Event();
		obj.Type = "MessageDeleted";
		obj.Description = string.Format("/desc/event/{0}", "MessageDeleted");
		obj.Timestamp = notification.Timestamp;
		obj.Link = string.Format("/message/{0}", messageDeletionNotification.MessageId.ToString("N"));
		obj.Data = new
		{
			Id = messageDeletionNotification.MessageId.ToString("N")
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
