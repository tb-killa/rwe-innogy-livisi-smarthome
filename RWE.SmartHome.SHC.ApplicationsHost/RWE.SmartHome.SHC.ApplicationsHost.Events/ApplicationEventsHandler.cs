using RWE.SmartHome.SHC.CoreApiConverters;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;
using SmartHome.SHC.API.Events;

namespace RWE.SmartHome.SHC.ApplicationsHost.Events;

public class ApplicationEventsHandler : IEventService
{
	private IExternalCommandDispatcher externalCommandDispatcher;

	private string applicationId;

	private string applicationVersion;

	public ApplicationEventsHandler(IExternalCommandDispatcher externalCommandDispatcher, string applicationId, string applicationVersion)
	{
		this.externalCommandDispatcher = externalCommandDispatcher;
		this.applicationId = applicationId;
		this.applicationVersion = applicationVersion;
	}

	void IEventService.SendEvent(Event ev)
	{
		externalCommandDispatcher.SendNotification(ev.ToCoreNotification(applicationId, applicationVersion));
	}
}
