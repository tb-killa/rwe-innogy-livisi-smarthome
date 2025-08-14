using System.CodeDom.Compiler;

namespace RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public interface IDeviceUpdateService
{
	DeviceUpdateResultCode CheckForDeviceUpdate(DeviceDescriptor deviceDescriptor, out DeviceUpdateInfo updateInfo);
}
