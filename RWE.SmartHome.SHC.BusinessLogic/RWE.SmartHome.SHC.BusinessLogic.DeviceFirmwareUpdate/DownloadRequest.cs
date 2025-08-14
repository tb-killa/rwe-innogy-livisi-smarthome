using System;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate;

namespace RWE.SmartHome.SHC.BusinessLogic.DeviceFirmwareUpdate;

internal struct DownloadRequest
{
	public DeviceDescriptor DeviceDescriptor;

	public Uri Url;

	public string MD5Hash;

	public string TargetFile;
}
