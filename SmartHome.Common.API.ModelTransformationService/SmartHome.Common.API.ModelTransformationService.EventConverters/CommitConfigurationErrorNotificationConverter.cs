using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Configuration;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.EventConverters;

public class CommitConfigurationErrorNotificationConverter : IEventConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(CommitConfigurationErrorNotificationConverter));

	public List<Event> FromNotification(BaseNotification notification)
	{
		logger.DebugEnterMethod("FromNotification");
		if (!(notification is CommitConfigurationErrorNotification commitConfigurationErrorNotification))
		{
			return new List<Event>();
		}
		Event obj = new Event();
		obj.Type = "CommitConfigError";
		obj.Timestamp = notification.Timestamp;
		obj.Link = string.Format("/desc/device/{0}.{1}/{2}", BuiltinPhysicalDeviceType.SHC, "RWE", "1.0");
		obj.Properties = new List<Property>
		{
			new Property
			{
				Name = "ConfigurationVersion",
				Value = commitConfigurationErrorNotification.ConfigurationVersion
			},
			new Property
			{
				Name = "Message",
				Value = commitConfigurationErrorNotification.Message
			}
		};
		obj.SequenceNumber = notification.SequenceNumber;
		obj.Namespace = notification.Namespace;
		Event item = obj;
		logger.DebugExitMethod("FromNotification");
		List<Event> list = new List<Event>(1);
		list.Add(item);
		return list;
	}
}
