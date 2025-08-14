using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RWE.SmartHome.Common.GlobalContracts;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Authentication;
using RWE.SmartHome.SHC.Core.Authentication.Entities;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;
using RWE.SmartHome.SHC.ErrorHandling;

namespace RWE.SmartHome.SHC.StartupLogic;

internal abstract class InitializationBase
{
	protected const string Container = "shc";

	protected const string Provider = "SHC Trusted Platform Module Cryptographic Service Provider";

	private readonly string certificateThumbprint;

	private readonly IUserManager userManagement;

	protected Configuration Configuration { get; private set; }

	protected IShcInitializationClient InitializationClient { get; private set; }

	protected string Serial { get; private set; }

	protected Guid SessionToken { get; set; }

	protected string ProcessName { get; private set; }

	protected IDisplayManager DisplayManager { get; private set; }

	protected ICertificateManager CertificateManager { get; private set; }

	protected InitializationBase(IUserManager userManagement, IDisplayManager displayManager, ICertificateManager certificateManager, IShcInitializationClient initializationClient, Configuration configuration, string certificateThumbprint, string processName)
	{
		SessionToken = Guid.Empty;
		this.userManagement = userManagement;
		DisplayManager = displayManager;
		InitializationClient = initializationClient;
		CertificateManager = certificateManager;
		ProcessName = processName;
		Configuration = configuration;
		this.certificateThumbprint = certificateThumbprint;
		Serial = SHCSerialNumber.SerialNumber();
	}

	protected void RegisterUsers(ShcSyncRecord record)
	{
		if (record == null)
		{
			return;
		}
		try
		{
			Dictionary<Guid, Role> roles = record.Roles.ToDictionary((ShcRole r) => r.Id, (ShcRole r) => new Role
			{
				Id = r.Id,
				Name = r.Name
			});
			Dictionary<Guid, User> users = record.Users.ToDictionary((ShcUser u) => u.Id, (ShcUser u) => new User
			{
				Id = u.Id,
				Name = u.Name,
				Password = u.PasswordHash,
				CreateDate = u.CreateDate,
				Roles = u.Roles.Select((ShcRef r) => roles[r.RefId]).ToList()
			});
			userManagement.SyncRolesAndUsers(roles, users);
		}
		catch (Exception innerException)
		{
			throw new RegistrationException("Unable to synchronize users and roles!", innerException);
		}
	}

	protected static string GeneratePin()
	{
		byte[] array = RandomByteGenerator.Instance.GenerateRandomByteSequence(4u);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < 4; i++)
		{
			string text = $"{Convert.ToInt16(array[i]):00}";
			stringBuilder.Append(text.Substring(text.Length - 2, 2));
		}
		return stringBuilder.ToString();
	}

	protected void ConfirmOwnership(bool success)
	{
		string deviceIp = NetworkTools.GetDeviceIp();
		string shcFriendlyName = Configuration.ShcFriendlyName;
		ShcInitializationResult initializationResult = ((!success) ? ShcInitializationResult.ShcFailedToStoreTheInitializationData : ShcInitializationResult.Success);
		ShcMetadata shcMetadata = new ShcMetadata();
		shcMetadata.Ip = deviceIp;
		shcMetadata.Name = shcFriendlyName;
		shcMetadata.HardwareVersion = SHCVersion.HardwareVersion;
		shcMetadata.FirmwareVersion = SHCVersion.OsVersion;
		shcMetadata.ApplicationVersion = SHCVersion.ApplicationVersion;
		ShcMetadata shcMetadata2 = shcMetadata;
		InitializationErrorCode initializationErrorCode = InitializationClient.ConfirmShcOwnership(certificateThumbprint, SessionToken, shcMetadata2, initializationResult);
		if (initializationErrorCode == InitializationErrorCode.Success)
		{
			Log.Information(Module.StartupLogic, ProcessName, $"Confirmed SHC ownership for IP {shcMetadata2.Ip}, friendly name {shcMetadata2.Name}, application version {shcMetadata2.ApplicationVersion}, hardware version {shcMetadata2.HardwareVersion} and firmware version {shcMetadata2.FirmwareVersion} (Success = {success})");
			return;
		}
		string message = $"Failed to confirm SHC ownership for IP {shcMetadata2.Ip}, friendly name {shcMetadata2.Name}, application version {shcMetadata2.ApplicationVersion}, hardware version {shcMetadata2.HardwareVersion} and firmware version {shcMetadata2.FirmwareVersion} (Success = {success}): Error: {initializationErrorCode}";
		Log.Error(Module.StartupLogic, ProcessName, message);
		throw new ShcException(message, ProcessName, 5);
	}
}
