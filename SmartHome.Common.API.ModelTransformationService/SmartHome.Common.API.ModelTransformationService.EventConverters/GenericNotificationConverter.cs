using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService.GenericConverters;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.EventConverters;

public class GenericNotificationConverter : IEventConverter
{
	private static readonly ILogger logger = LogManager.Instance.GetLogger(typeof(GenericNotificationConverter));

	public List<Event> FromNotification(BaseNotification notification)
	{
		logger.DebugEnterMethod("FromNotification");
		if (!(notification is GenericNotification genericNotification))
		{
			return new List<Event>();
		}
		string domainLinkPatternByEntityType = GetDomainLinkPatternByEntityType(genericNotification.EntityType);
		Event obj = new Event();
		obj.Type = genericNotification.EventType;
		obj.Timestamp = notification.Timestamp;
		obj.Link = string.Format(domainLinkPatternByEntityType, genericNotification.EntityId.Replace("sh://", string.Empty), genericNotification.ApplicationVersion.ToMajorDotMinorVersion());
		obj.SequenceNumber = notification.SequenceNumber;
		obj.Namespace = genericNotification.Namespace;
		Event obj2 = obj;
		if (genericNotification.Data != null && genericNotification.Data.Properties != null && genericNotification.Data.Properties.Any())
		{
			logger.Debug($"Converting {genericNotification.Data.Properties.Count} data properties...");
			obj2.Properties = new List<SmartHome.Common.API.Entities.Entities.Property>();
			obj2.Properties.AddRange(genericNotification.Data.Properties.ConvertAll<SmartHome.Common.API.Entities.Entities.Property>(PropertyConverter.ToApiProperty));
			logger.Debug("Data properties conversion done!");
		}
		logger.DebugExitMethod("FromNotification");
		List<Event> list = new List<Event>(1);
		list.Add(obj2);
		return list;
	}

	private string GetDomainLinkPatternByEntityType(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType type)
	{
		return type switch
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.BaseDevice => "/device/{0}", 
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Interaction => "/interaction/{0}", 
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Location => "/location/{0}", 
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.LogicalDevice => "/capability/{0}", 
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Product => "/product/{0}/{1}", 
			_ => "/{0}", 
		};
	}
}
