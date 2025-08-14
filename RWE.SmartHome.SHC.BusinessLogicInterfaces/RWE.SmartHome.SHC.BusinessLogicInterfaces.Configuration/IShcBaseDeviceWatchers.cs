using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

public interface IShcBaseDeviceWatchers
{
	void RegisterWatcher(IShcBaseDeviceWatcher watcher);

	void UnregisterWatcher(IShcBaseDeviceWatcher watcher);

	void ProcessUpdate(Property[] originalBaseDeviceProperties, Property[] baseDeviceProperties);
}
