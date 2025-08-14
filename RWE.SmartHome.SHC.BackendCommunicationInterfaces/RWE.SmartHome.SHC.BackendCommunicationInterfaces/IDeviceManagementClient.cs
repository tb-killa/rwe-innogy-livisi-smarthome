using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces;

public interface IDeviceManagementClient
{
	UploadFileResponse UploadLogFile(string certificateThumbprint, string shcSerial, byte[] content, int currentPackage, int nextPackage, string correlationId);

	UploadFileResponse UploadSystemInfo(string certificateThumbprint, string shcSerial, string content, SystemInfoType contentType, string description, int currentPackage, int nextPackage, string correlationId);
}
