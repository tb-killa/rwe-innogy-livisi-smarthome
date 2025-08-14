using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.Core;

namespace WebServerHost.Services;

public class RebootService : IRebootService
{
	private readonly IEventManager eventManager;

	public RebootService(IEventManager eventManager)
	{
		this.eventManager = eventManager;
	}

	public void Reboot()
	{
		eventManager.GetEvent<ShcRebootScheduledEvent>().Publish(new ShcRebootScheduledEventArgs());
		eventManager.GetEvent<PerformResetEvent>().Publish(new PerformResetEventArgs());
	}
}
