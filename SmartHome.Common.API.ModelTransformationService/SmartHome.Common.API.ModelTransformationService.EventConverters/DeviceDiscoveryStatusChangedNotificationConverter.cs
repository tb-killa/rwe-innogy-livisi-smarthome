using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.EventConverters;

public class DeviceDiscoveryStatusChangedNotificationConverter : IEventConverter
{
	private static ILogger logger = LogManager.Instance.GetLogger(typeof(DeviceDiscoveryStatusChangedNotificationConverter));

	public List<Event> FromNotification(BaseNotification notification)
	{
		logger.DebugEnterMethod("FromNotification");
		if (!(notification is DeviceDiscoveryStatusChangedNotification deviceDiscoveryStatusChangedNotification))
		{
			return new List<Event>();
		}
		Event obj = new Event();
		obj.Type = "DeviceDiscoveryStatusChanged";
		obj.Timestamp = notification.Timestamp;
		obj.Link = string.Format("/desc/device/{0}.{1}/{2}", BuiltinPhysicalDeviceType.SHC, "RWE", "1.0");
		obj.Properties = GetEventProperties(deviceDiscoveryStatusChangedNotification);
		obj.Namespace = notification.Namespace;
		obj.SequenceNumber = notification.SequenceNumber;
		Event item = obj;
		logger.DebugExitMethod("FromNotification");
		List<Event> list = new List<Event>(1);
		list.Add(item);
		return list;
	}

	private List<Property> GetEventProperties(DeviceDiscoveryStatusChangedNotification deviceDiscoveryStatusChangedNotification)
	{
		List<Property> propertiesList = new List<Property>
		{
			new Property
			{
				Name = "Active",
				Value = deviceDiscoveryStatusChangedNotification.Active
			}
		};
		if (deviceDiscoveryStatusChangedNotification.AppIds != null && deviceDiscoveryStatusChangedNotification.AppIds.Any())
		{
			deviceDiscoveryStatusChangedNotification.AppIds.ForEach(delegate(string appId)
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
