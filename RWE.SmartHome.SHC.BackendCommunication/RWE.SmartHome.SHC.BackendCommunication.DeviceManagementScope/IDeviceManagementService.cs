using System.CodeDom.Compiler;

namespace RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public interface IDeviceManagementService
{
	UploadFileResponse UploadLogFile(string shcSerial, byte[] content, int currentPackage, int nextPackage, string correlationId);

	UploadFileResponse UploadSystemInfo(string shcSerial, string content, SystemInfoType contentType, string description, int currentPackage, int nextPackage, string correlationId);
}
