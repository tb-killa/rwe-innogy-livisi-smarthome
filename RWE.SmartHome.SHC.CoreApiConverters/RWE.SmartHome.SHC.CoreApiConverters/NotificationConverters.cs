using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using SmartHome.SHC.API.Events;

namespace RWE.SmartHome.SHC.CoreApiConverters;

public static class NotificationConverters
{
	public static GenericNotification ToCoreNotification(this Event apiEvent, string appId, string addinVersion)
	{
		GenericNotification genericNotification = new GenericNotification();
		genericNotification.ApplicationId = appId;
		genericNotification.ApplicationVersion = addinVersion;
		genericNotification.EntityId = apiEvent.Link.Id;
		genericNotification.EventType = apiEvent.EventType;
		genericNotification.EntityType = apiEvent.Link.Type.ToCore();
		genericNotification.Namespace = appId.Replace("sh://", string.Empty);
		genericNotification.Data = apiEvent.Data.ToCorePropertyBag(includeTimestamp: false);
		return genericNotification;
	}
}
