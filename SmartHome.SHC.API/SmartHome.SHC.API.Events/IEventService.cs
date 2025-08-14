namespace SmartHome.SHC.API.Events;

public interface IEventService
{
	void SendEvent(Event notification);
}
