using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Configuration;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService.GenericConverters;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.EventConverters;

public class InvalidateTokenCacheNotificationConverter : IEventConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(InvalidateTokenCacheNotificationConverter));

	public List<Event> FromNotification(BaseNotification notification)
	{
		logger.DebugEnterMethod("FromNotification");
		List<Event> list = new List<Event>();
		if (!(notification is InvalidateTokenCacheNotification { AppId: var appId } invalidateTokenCacheNotification))
		{
			return list;
		}
		if (appId == null)
		{
			return list;
		}
		List<Property> list2 = null;
		if (invalidateTokenCacheNotification.Properties != null && invalidateTokenCacheNotification.Properties.Any())
		{
			list2 = new List<Property>();
			list2.AddRange(invalidateTokenCacheNotification.Properties.ConvertAll<Property>(PropertyConverter.ToApiProperty));
		}
		Event obj = new Event();
		obj.Type = "StateChanged";
		obj.Timestamp = notification.Timestamp;
		obj.Link = string.Format("/product/{0}/{1}", appId.Replace("sh://", string.Empty), invalidateTokenCacheNotification.AppVersion.ToMajorDotMinorVersion());
		obj.SequenceNumber = notification.SequenceNumber;
		obj.Namespace = notification.Namespace;
		Event obj2 = obj;
		if (list2 != null)
		{
			obj2.Properties = list2.Where((Property p) => p.Name.ToUpper() != "APP_EXTENSIBLE").ToList();
		}
		list.Add(obj2);
		logger.DebugExitMethod("FromNotification");
		return list;
	}
}
