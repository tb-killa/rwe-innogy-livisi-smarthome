using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunication.LocalCommunication;

internal class DeviceManagementClientLocalOnly : IDeviceManagementClient
{
	public UploadFileResponse UploadLogFile(string certificateThumbprint, string shcSerial, byte[] content, int currentPackage, int nextPackage, string correlationId)
	{
		return UploadFileResponse.Success;
	}

	public UploadFileResponse UploadSystemInfo(string certificateThumbprint, string shcSerial, string content, SystemInfoType contentType, string description, int currentPackage, int nextPackage, string correlationId)
	{
		return UploadFileResponse.Success;
	}
}
