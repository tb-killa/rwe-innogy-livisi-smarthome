using System;
using System.Linq;
using RWE.SmartHome.Common.GlobalContracts;
using RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;

internal static class ConfigurationExtensions
{
	public static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode Convert(this RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ConfigurationResultCode code)
	{
		return code switch
		{
			RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ConfigurationResultCode.Success => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.Success, 
			RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ConfigurationResultCode.Failure => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.Failure, 
			RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ConfigurationResultCode.NotAuthorized => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.NotAuthorized, 
			RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ConfigurationResultCode.InvalidNodePath => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.InvalidNodePath, 
			RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ConfigurationResultCode.InvalidConfigurationIdentifier => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.InvalidConfigurationIdentifier, 
			RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ConfigurationResultCode.InvalidConfigurationNode => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.InvalidConfigurationNode, 
			RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ConfigurationResultCode.IncorrectSequence => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.IncorrectSequence, 
			_ => throw new ArgumentOutOfRangeException("code"), 
		};
	}

	public static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcSyncRecord Convert(this RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ShcSyncRecord record)
	{
		RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcSyncRecord shcSyncRecord = new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcSyncRecord();
		shcSyncRecord.Roles = record.Roles.Select((RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ShcRole r) => r.Convert()).ToArray();
		shcSyncRecord.Users = record.Users.Select((RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ShcUser u) => u.Convert()).ToArray();
		return shcSyncRecord;
	}

	private static RWE.SmartHome.Common.GlobalContracts.ShcRole Convert(this RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ShcRole role)
	{
		RWE.SmartHome.Common.GlobalContracts.ShcRole shcRole = new RWE.SmartHome.Common.GlobalContracts.ShcRole();
		shcRole.Id = new Guid(role.Id);
		shcRole.Name = role.Name;
		return shcRole;
	}

	private static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcUser Convert(this RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ShcUser user)
	{
		RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcUser shcUser = new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcUser();
		shcUser.CreateDate = user.CreateDate;
		shcUser.Id = new Guid(user.Id);
		shcUser.Name = user.Name;
		shcUser.PasswordHash = user.PasswordHash;
		shcUser.Roles = user.Roles.Select((RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ShcRef r) => r.Convert()).ToArray();
		return shcUser;
	}

	private static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcRef Convert(this RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ShcRef sref)
	{
		RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcRef shcRef = new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcRef();
		shcRef.RefId = new Guid(sref.RefId);
		return shcRef;
	}

	public static RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ConfigurationType Convert(this RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationType type)
	{
		return type switch
		{
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationType.TechnicalConfiguration => RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ConfigurationType.TechnicalConfiguration, 
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationType.UIConfiguration => RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ConfigurationType.UIConfiguration, 
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationType.DeviceList => RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ConfigurationType.DeviceList, 
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationType.MessagesAndAlerts => RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ConfigurationType.MessagesAndAlerts, 
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationType.CustomApplication => RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ConfigurationType.CustomApplication, 
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationType.DeviceActivityLoggingConfiguration => RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ConfigurationType.DeviceActivityLoggingConfiguration, 
			_ => throw new ArgumentOutOfRangeException("type"), 
		};
	}
}
