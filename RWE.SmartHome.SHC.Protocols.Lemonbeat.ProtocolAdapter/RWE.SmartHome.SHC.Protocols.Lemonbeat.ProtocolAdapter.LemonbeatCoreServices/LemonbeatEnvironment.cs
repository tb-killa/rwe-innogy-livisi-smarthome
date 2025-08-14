using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;
using SmartHome.SHC.API.Protocols.Lemonbeat;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.LemonbeatCoreServices;

internal class LemonbeatEnvironment : ILemonbeatCoreServices
{
	private IValueHandler valueHandler;

	private IShcValueContainer valueContainer;

	private IPhysicalDeviceProvider physicalDeviceProvider;

	public IShcValueContainer ShcValueContainer => valueContainer;

	public IValueHandler ValueHandler => valueHandler;

	public IPhysicalDeviceProvider PhysicalDeviceProvider => physicalDeviceProvider;

	internal LemonbeatEnvironment(string applicationId, IValueService valueService, IDeviceList deviceList, ShcValueRepository valueRepository)
	{
		valueHandler = new ValueHandler(applicationId, valueService, deviceList);
		valueContainer = new ShcValueContainer(applicationId, valueRepository);
		physicalDeviceProvider = new PhysicalDeviceProvider(deviceList);
	}
}
