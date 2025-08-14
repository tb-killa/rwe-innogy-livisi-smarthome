using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.Entities.EventDispatching;

public interface IEventConsumer
{
	void HandleEvent(string source, Event evt);
}
