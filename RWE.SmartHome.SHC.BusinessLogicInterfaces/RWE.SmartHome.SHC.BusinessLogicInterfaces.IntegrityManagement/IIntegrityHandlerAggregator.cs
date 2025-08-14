namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;

public interface IIntegrityHandlerAggregator
{
	void SubscribeHandler(ICoreIntegrityHandler integrityHandler);

	void ClearSubscribers();
}
