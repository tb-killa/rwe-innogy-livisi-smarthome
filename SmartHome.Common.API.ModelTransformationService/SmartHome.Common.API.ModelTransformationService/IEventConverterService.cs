using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService;

public interface IEventConverterService
{
	List<Event> FromNotification(BaseNotification notification);
}
