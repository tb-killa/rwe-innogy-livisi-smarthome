using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.EventConverters;

public class DeviceDiscoveryFailureNotificationConverter : IEventConverter
{
	private static ILogger logger = LogManager.Instance.GetLogger(typeof(DeviceDiscoveryFailureNotificationConverter));

	public List<Event> FromNotification(BaseNotification notification)
	{
		logger.DebugEnterMethod("FromNotification");
		if (!(notification is DeviceDiscoveryFailureNotification deviceDiscoveryFailureNotification))
		{
			return new List<Event>();
		}
		Event obj = new Event();
		obj.Type = "ActivateDiscoveryFailure";
		obj.Timestamp = notification.Timestamp;
		obj.Link = string.Format("/desc/device/{0}.{1}/{2}", BuiltinPhysicalDeviceType.SHC, "RWE", "1.0");
		obj.Properties = GetEventProperties(deviceDiscoveryFailureNotification);
		obj.SequenceNumber = notification.SequenceNumber;
		obj.Namespace = notification.Namespace;
		Event item = obj;
		logger.DebugExitMethod("FromNotification");
		List<Event> list = new List<Event>(1);
		list.Add(item);
		return list;
	}

	private List<Property> GetEventProperties(DeviceDiscoveryFailureNotification deviceDiscoveryFailureNotification)
	{
		List<Property> propertiesList = null;
		if (deviceDiscoveryFailureNotification.AppIds != null && deviceDiscoveryFailureNotification.AppIds.Any())
		{
			propertiesList = new List<Property>();
			deviceDiscoveryFailureNotification.AppIds.ForEach(delegate(string appId)
			{
				propertiesList.Add(new Property
				{
					Name = "Product",
					Value = string.Format("/product/{0}/{1}", appId.Replace("sh://", string.Empty), "2.0")
				});
			});
		}
		return propertiesList;
	}
}
