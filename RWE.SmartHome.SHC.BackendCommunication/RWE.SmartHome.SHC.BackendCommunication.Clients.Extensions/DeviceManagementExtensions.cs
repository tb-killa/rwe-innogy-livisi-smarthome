using System;
using RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;

internal static class DeviceManagementExtensions
{
	public static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UploadFileResponse Convert(this RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope.UploadFileResponse response)
	{
		return response switch
		{
			RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope.UploadFileResponse.Success => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UploadFileResponse.Success, 
			RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope.UploadFileResponse.BackendFailure => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UploadFileResponse.BackendFailure, 
			RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope.UploadFileResponse.PackageSequenceFailure => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UploadFileResponse.PackageSequenceFailure, 
			RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope.UploadFileResponse.InvalidLogFormat => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UploadFileResponse.InvalidLogFormat, 
			_ => throw new ArgumentOutOfRangeException("response"), 
		};
	}

	public static RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope.SystemInfoType Convert(this RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SystemInfoType type)
	{
		if (type == RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SystemInfoType.GeneralInformation)
		{
			return RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope.SystemInfoType.GeneralInformation;
		}
		throw new ArgumentOutOfRangeException("type");
	}
}
