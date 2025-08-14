using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.GenericConverters;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.EventConverters;

public class LogicalDeviceStatesNotificationConverter : IEventConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(LogicalDeviceStatesNotificationConverter));

	public List<Event> FromNotification(BaseNotification notification)
	{
		logger.DebugEnterMethod("FromNotification");
		List<Event> list = new List<Event>();
		if (!(notification is LogicalDeviceStatesChangedNotification logicalDeviceStatesChangedNotification))
		{
			return list;
		}
		foreach (LogicalDeviceState logicalDeviceState in logicalDeviceStatesChangedNotification.LogicalDeviceStates)
		{
			string arg = logicalDeviceState.LogicalDeviceId.ToString("N");
			Event obj = new Event();
			obj.Type = "StateChanged";
			obj.Timestamp = notification.Timestamp;
			obj.Link = $"/capability/{arg}";
			obj.SequenceNumber = notification.SequenceNumber;
			obj.Namespace = notification.Namespace;
			Event obj2 = obj;
			List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> properties = logicalDeviceState.GetProperties();
			if (properties != null && properties.Any())
			{
				obj2.Properties = new List<SmartHome.Common.API.Entities.Entities.Property>();
				obj2.Properties.AddRange(properties.ConvertAll<SmartHome.Common.API.Entities.Entities.Property>(PropertyConverter.ToApiProperty));
			}
			logger.Debug($"Event received: {obj2}");
			list.Add(obj2);
		}
		logger.DebugExitMethod("FromNotification");
		return list;
	}
}
