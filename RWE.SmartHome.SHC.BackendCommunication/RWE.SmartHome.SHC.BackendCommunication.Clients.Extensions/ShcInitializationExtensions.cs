using System;
using System.Linq;
using RWE.SmartHome.Common.GlobalContracts;
using RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using SmartHome.Common.Generic.Contracts.BackendShc;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;

internal static class ShcInitializationExtensions
{
	public static string Convert(this RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcInitializationResult result)
	{
		return result switch
		{
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcInitializationResult.Success => global::SmartHome.Common.Generic.Contracts.BackendShc.ShcInitializationResult.Success, 
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcInitializationResult.ShcFailedToStoreTheInitializationData => global::SmartHome.Common.Generic.Contracts.BackendShc.ShcInitializationResult.ShcFailedToStoreTheInitializationData, 
			_ => throw new ArgumentException("result"), 
		};
	}

	public static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.InitializationErrorCode Convert(this RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.InitializationErrorCode code)
	{
		return code switch
		{
			RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.InitializationErrorCode.Success => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.InitializationErrorCode.Success, 
			RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.InitializationErrorCode.Failure => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.InitializationErrorCode.Failure, 
			RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.InitializationErrorCode.InvalidRegistrationProcessStatus => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.InitializationErrorCode.InvalidRegistrationProcessStatus, 
			RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.InitializationErrorCode.InvalidPin => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.InitializationErrorCode.InvalidPin, 
			RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.InitializationErrorCode.ShcNotSold => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.InitializationErrorCode.ShcNotSold, 
			RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.InitializationErrorCode.InvalidRegistrationToken => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.InitializationErrorCode.InvalidRegistrationToken, 
			RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.InitializationErrorCode.RegistrationProcessExpired => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.InitializationErrorCode.RegistrationProcessExpired, 
			RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.InitializationErrorCode.NotAuthorized => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.InitializationErrorCode.NotAuthorized, 
			_ => throw new ArgumentOutOfRangeException("code"), 
		};
	}

	public static RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.ShcMetadata Convert(this RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcMetadata metadata)
	{
		RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.ShcMetadata shcMetadata = new RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.ShcMetadata();
		shcMetadata.ApplicationVersion = metadata.ApplicationVersion;
		shcMetadata.FirmwareVersion = metadata.FirmwareVersion;
		shcMetadata.HardwareVersion = metadata.HardwareVersion;
		shcMetadata.Ip = metadata.Ip;
		shcMetadata.Name = metadata.Name;
		return shcMetadata;
	}

	public static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcSyncRecord Convert(this RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.ShcSyncRecord record)
	{
		RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcSyncRecord shcSyncRecord = new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcSyncRecord();
		shcSyncRecord.Roles = record.Roles.Select((RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.ShcRole r) => r.Convert()).ToArray();
		shcSyncRecord.Users = record.Users.Select((RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.ShcUser u) => u.Convert()).ToArray();
		return shcSyncRecord;
	}

	private static RWE.SmartHome.Common.GlobalContracts.ShcRole Convert(this RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.ShcRole role)
	{
		RWE.SmartHome.Common.GlobalContracts.ShcRole shcRole = new RWE.SmartHome.Common.GlobalContracts.ShcRole();
		shcRole.Id = new Guid(role.Id);
		shcRole.Name = role.Name;
		return shcRole;
	}

	private static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcUser Convert(this RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.ShcUser user)
	{
		RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcUser shcUser = new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcUser();
		shcUser.CreateDate = user.CreateDate;
		shcUser.Id = new Guid(user.Id);
		shcUser.Name = user.Name;
		shcUser.PasswordHash = user.PasswordHash;
		shcUser.Roles = user.Roles.Select((RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.ShcRef r) => r.Convert()).ToArray();
		return shcUser;
	}

	private static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcRef Convert(this RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.ShcRef sref)
	{
		RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcRef shcRef = new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcRef();
		shcRef.RefId = new Guid(sref.RefId);
		return shcRef;
	}
}
