using System;
using RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;

internal static class SoftwareUpdateExtensions
{
	public static RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.ShcVersionInfo Convert(this RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcVersionInfo versionInfo)
	{
		RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.ShcVersionInfo shcVersionInfo = new RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.ShcVersionInfo();
		shcVersionInfo.ApplicationVersion = versionInfo.ApplicationVersion;
		shcVersionInfo.FirmwareVersion = versionInfo.FirmwareVersion;
		shcVersionInfo.HardwareVersion = versionInfo.HardwareVersion;
		return shcVersionInfo;
	}

	public static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SwUpdateResultCode Convert(this RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.SwUpdateResultCode resultCode)
	{
		return resultCode switch
		{
			RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.SwUpdateResultCode.AlreadyLatestVersion => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SwUpdateResultCode.AlreadyLatestVersion, 
			RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.SwUpdateResultCode.NewerVersionAvailable => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SwUpdateResultCode.NewerVersionAvailable, 
			RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.SwUpdateResultCode.Failure => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SwUpdateResultCode.Failure, 
			RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.SwUpdateResultCode.NotAuthorized => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SwUpdateResultCode.NotAuthorized, 
			_ => throw new ArgumentOutOfRangeException("resultCode"), 
		};
	}

	public static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UpdateInfo Convert(this RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.UpdateInfo updateInfo)
	{
		RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UpdateInfo updateInfo2 = new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UpdateInfo();
		updateInfo2.Category = updateInfo.Category.Convert();
		updateInfo2.DownloadLocation = updateInfo.DownloadLocation;
		updateInfo2.DownloadPassword = updateInfo.DownloadPassword;
		updateInfo2.DownloadUser = updateInfo.DownloadUser;
		updateInfo2.Type = updateInfo.Type.Convert();
		updateInfo2.UpdateDeadline = updateInfo.UpdateDeadline;
		updateInfo2.Version = updateInfo.Version;
		return updateInfo2;
	}

	public static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcUpdateAnnouncementResultCode Convert(this RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.ShcUpdateAnnouncementResultCode resultCode)
	{
		return resultCode switch
		{
			RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.ShcUpdateAnnouncementResultCode.Success => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcUpdateAnnouncementResultCode.Success, 
			RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.ShcUpdateAnnouncementResultCode.TemporaryFailure => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcUpdateAnnouncementResultCode.TemporaryFailure, 
			RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.ShcUpdateAnnouncementResultCode.PermanentFailure => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcUpdateAnnouncementResultCode.PermanentFailure, 
			_ => throw new ArgumentOutOfRangeException("resultCode"), 
		};
	}

	private static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UpdateCategory Convert(this RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.UpdateCategory updateCategory)
	{
		return updateCategory switch
		{
			RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.UpdateCategory.ShcFirmware => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UpdateCategory.ShcFirmware, 
			RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.UpdateCategory.ShcApplication => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UpdateCategory.ShcApplication, 
			_ => throw new ArgumentOutOfRangeException("updateCategory"), 
		};
	}

	private static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UpdateType Convert(this RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.UpdateType updateType)
	{
		return updateType switch
		{
			RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.UpdateType.Forced => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UpdateType.Forced, 
			RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.UpdateType.Mandatory => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UpdateType.Mandatory, 
			RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.UpdateType.Optional => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UpdateType.Optional, 
			_ => throw new ArgumentOutOfRangeException("updateType"), 
		};
	}
}
