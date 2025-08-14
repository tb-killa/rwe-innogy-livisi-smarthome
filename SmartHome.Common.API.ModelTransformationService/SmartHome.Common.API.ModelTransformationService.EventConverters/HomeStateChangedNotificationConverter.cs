using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.EventConverters;

public class HomeStateChangedNotificationConverter : IEventConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(HomeStateChangedNotificationConverter));

	public List<Event> FromNotification(BaseNotification notification)
	{
		logger.DebugEnterMethod("FromNotification");
		try
		{
			if (!(notification is HomeStateChangedNotification { HomeId: var homeId }))
			{
				return new List<Event>();
			}
			string arg = homeId.ToString("N");
			Event obj = new Event();
			obj.Type = "StateChanged";
			obj.Timestamp = notification.Timestamp;
			obj.Link = $"/home/{arg}";
			obj.SequenceNumber = notification.SequenceNumber;
			obj.Namespace = notification.Namespace;
			Event item = obj;
			List<Event> list = new List<Event>();
			list.Add(item);
			return list;
		}
		finally
		{
			logger.DebugExitMethod("FromNotification");
		}
	}
}
