using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

public interface IShcBaseDeviceWatcher
{
	void ProcessUpdate(Property[] oldShcBaseDeviceProperties, Property[] newShcBaseDeviceProperties);
}
