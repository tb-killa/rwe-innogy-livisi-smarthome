namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public interface ILemonbeatCoreServices
{
	IShcValueContainer ShcValueContainer { get; }

	IValueHandler ValueHandler { get; }

	IPhysicalDeviceProvider PhysicalDeviceProvider { get; }
}
