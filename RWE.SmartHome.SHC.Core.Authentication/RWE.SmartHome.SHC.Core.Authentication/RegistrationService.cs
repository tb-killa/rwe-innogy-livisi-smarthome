using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Authentication.Entities;
using RWE.SmartHome.SHC.Core.Authentication.Events;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;
using SmartHome.Common.API.Entities.ErrorHandling;

namespace RWE.SmartHome.SHC.Core.Authentication;

public class RegistrationService : IRegistrationService
{
	private const string TaCFlagPath = "\\NandFlash\\TaC";

	private bool isInitializationDone;

	private readonly INetworkingMonitor networkMonitor;

	private readonly IUserManager remoteUserManager;

	private readonly IDisplayManager displayManager;

	private readonly IEventManager eventManager;

	private bool startUpEventsAlreadySent;

	public bool TearmsAndConditionAccepted => File.Exists("\\NandFlash\\TaC");

	public bool RemotelyRegistered
	{
		get
		{
			if (networkMonitor.InternetAccessAllowed)
			{
				return remoteUserManager.Users.Any((User u) => u.Roles.Any((Role r) => r.Name == "Owner"));
			}
			return true;
		}
	}

	public bool IsShcLocalOnly
	{
		get
		{
			return File.Exists(LocalCommunicationConstants.IsShcLocalOnlyFlagPath);
		}
		private set
		{
		}
	}

	public RegistrationService(INetworkingMonitor networkMonitor, IUserManager remoteUserManager, IDisplayManager displayManager, IEventManager eventManager)
	{
		this.networkMonitor = networkMonitor;
		this.remoteUserManager = remoteUserManager;
		this.displayManager = displayManager;
		this.eventManager = eventManager;
	}

	public ShcConfiguration GetShcConfiguration()
	{
		ShcConfiguration shcConfiguration = new ShcConfiguration();
		shcConfiguration.Config = new ShcConfiguration.Configuration
		{
			ShcSerial = SHCSerialNumber.SerialNumber(),
			Timestamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"),
			Active = true,
			HardwareType = "Classic",
			Archives = GetConfigBackups()
		};
		return shcConfiguration;
	}

	private List<string> GetConfigBackups()
	{
		return new List<string>();
	}

	public void Register(ShcInitialization shcInitialization)
	{
		if (!shcInitialization.TearmsOfAgreementAccepted)
		{
			throw new ApiException(ErrorCode.LatestTaCVersionError);
		}
		FilePersistence.LocalAccessEnabled = true;
		if (!RemotelyRegistered && !isInitializationDone)
		{
			isInitializationDone = true;
			SetIsShcLocalOnlyFlagTrue();
			eventManager.GetEvent<ShcIsLocalOnlyEvent>().Publish(new ShcIsLocalOnlyEventArgs());
			PublishShcStartupEvents();
			displayManager.WorkflowFinished(Workflow.OwnershipReassignment);
			FactoryResetHandling.UndoFactoryResetRequest();
		}
		SaveTaCFlag();
	}

	public void SetIsShcLocalOnlyFlagTrue()
	{
		IsShcLocalOnly = true;
		CreateFlagFile(LocalCommunicationConstants.IsShcLocalOnlyFlagPath);
	}

	public void PublishShcStartupEvents()
	{
		if (startUpEventsAlreadySent)
		{
			Log.Information(Module.StartupLogic, $"SHC start up events already sent");
			return;
		}
		eventManager.GetEvent<ShcStartupCompletedEvent>().Publish(new ShcStartupCompletedEventArgs(StartupProgress.DatabaseAvailable));
		eventManager.GetEvent<ShcStartupCompletedEvent>().Publish(new ShcStartupCompletedEventArgs(StartupProgress.CompletedRound1));
		eventManager.GetEvent<ShcStartupCompletedEvent>().Publish(new ShcStartupCompletedEventArgs(StartupProgress.CompletedRound2));
		startUpEventsAlreadySent = true;
		Log.Information(Module.StartupLogic, $"SHC successfully started.");
	}

	private static void SaveTaCFlag()
	{
		CreateFile("\\NandFlash\\TaC");
	}

	private static void CreateFlagFile(string path)
	{
		if (!File.Exists(path))
		{
			CreateFile(path);
		}
	}

	public void ResetTaC()
	{
		DeleteFileIfExists("\\NandFlash\\TaC");
	}

	public void ResetIsShcLocalOnlyFlag()
	{
		DeleteFileIfExists(LocalCommunicationConstants.IsShcLocalOnlyFlagPath);
	}

	private static void CreateFile(string filePath)
	{
		using (File.Create(filePath))
		{
		}
	}

	private void DeleteFileIfExists(string filePath)
	{
		if (File.Exists(filePath))
		{
			File.Delete(filePath);
		}
	}
}
