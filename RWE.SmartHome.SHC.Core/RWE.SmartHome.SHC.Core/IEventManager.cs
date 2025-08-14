namespace RWE.SmartHome.SHC.Core;

public interface IEventManager : IService
{
	TEventType GetEvent<TEventType>() where TEventType : EventBase;
}
