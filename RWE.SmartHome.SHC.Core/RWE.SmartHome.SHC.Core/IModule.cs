using Microsoft.Practices.Mobile.ContainerModel;

namespace RWE.SmartHome.SHC.Core;

public interface IModule
{
	void Configure(Container container);
}
