using System;
using System.Threading;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Authentication;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.StartupLogic;

internal sealed class OwnershipReassignment : InitializationBase
{
	private readonly IRegistrationService registrationService;

	private readonly IEventManager eventManager;

	public OwnershipReassignment(IUserManager userManagement, IDisplayManager displayManager, IShcInitializationClient initializationClient, Configuration configuration, ICertificateManager certificateManager, IRegistrationService registrationService, IEventManager eventManager)
		: base(userManagement, displayManager, certificateManager, initializationClient, configuration, certificateManager.PersonalCertificateThumbprint, "OwnershipReassignment")
	{
		this.registrationService = registrationService;
		this.eventManager = eventManager;
	}

	public bool PerformOwnershipReassignment()
	{
		try
		{
			SendOwnershipReassignmentRequest();
			if (!PollForOwnershipData(out var record))
			{
				base.DisplayManager.WorkflowProceeded(Workflow.OwnershipReassignment, WorkflowMessage.PressButton, forceDisplay: false);
				return false;
			}
			bool success = false;
			try
			{
				RegisterUsers(record);
				success = true;
			}
			finally
			{
				ConfirmOwnership(success);
			}
			eventManager.GetEvent<OwnershipReassignmentCompletedEvent>().Publish(new OwnershipReassignmentCompletedEventArgs(syncUsersAndRoles: false));
			base.DisplayManager.WorkflowFinished(Workflow.OwnershipReassignment);
			registrationService.ResetIsShcLocalOnlyFlag();
			FactoryResetHandling.UndoFactoryResetRequest();
			registrationService.PublishShcStartupEvents();
		}
		catch (Exception ex)
		{
			Log.Error(Module.StartupLogic, $"There was an exception when performing ownership assigment: {ex.Message} {ex.StackTrace}");
			FilePersistence.LocalAccessEnabled = true;
		}
		return true;
	}

	private void SendOwnershipReassignmentRequest()
	{
		string text = InitializationBase.GeneratePin();
		base.DisplayManager.WorkflowProceeded(Workflow.OwnershipReassignment, WorkflowMessage.Register, forceDisplay: false);
		Log.Information(Module.StartupLogic, base.ProcessName, $"Announce factory reset to backend, serial '{base.Serial}'");
		InitializationErrorCode initializationErrorCode = base.InitializationClient.ShcResetByOwner(base.CertificateManager.PersonalCertificateThumbprint, base.Serial);
		if (initializationErrorCode != InitializationErrorCode.Success)
		{
			throw new RegistrationException("Failed to announce factory reset, error " + initializationErrorCode);
		}
		Log.Information(Module.StartupLogic, base.ProcessName, $"Performing SHC ownership reassignment, serial '{base.Serial}', pin '{text}'");
		Guid sessionToken;
		InitializationErrorCode initializationErrorCode2 = base.InitializationClient.SubmitOwnershipRequest(base.CertificateManager.PersonalCertificateThumbprint, base.Serial, text, out sessionToken);
		if (initializationErrorCode2 != InitializationErrorCode.Success)
		{
			throw new RegistrationException("Failed to reassign ownership, error " + initializationErrorCode2);
		}
		base.DisplayManager.WorkflowDynamicMessage(Workflow.OwnershipReassignment, text);
		base.SessionToken = sessionToken;
	}

	private bool PollForOwnershipData(out ShcSyncRecord record)
	{
		int pollAfterSeconds = 0;
		int num = 0;
		while (true)
		{
			num++;
			if (pollAfterSeconds > 0)
			{
				Thread.Sleep(pollAfterSeconds * 1000);
			}
			Log.Information(Module.StartupLogic, base.ProcessName, "Polling backend");
			InitializationErrorCode initializationErrorCode = InitializationErrorCode.Failure;
			bool furtherPollingRequired;
			try
			{
				initializationErrorCode = base.InitializationClient.RetrieveOwnershipData(base.CertificateManager.PersonalCertificateThumbprint, base.SessionToken, out record, out furtherPollingRequired, out pollAfterSeconds);
			}
			catch (AccountlessShcException ex)
			{
				Log.Information(Module.StartupLogic, $"The backend polling was stopped. {ex.Message}");
				record = null;
				return false;
			}
			switch (initializationErrorCode)
			{
			case InitializationErrorCode.RegistrationProcessExpired:
				Log.Information(Module.StartupLogic, base.ProcessName, $"Ownership reassignement timed out on backend after {num} attempts");
				return false;
			default:
				throw new RegistrationException("Backend polling failed with error " + initializationErrorCode);
			case InitializationErrorCode.Success:
				if (furtherPollingRequired)
				{
					Log.Information(Module.StartupLogic, base.ProcessName, $"Retrying again in {pollAfterSeconds} seconds");
				}
				if (!furtherPollingRequired)
				{
					if (record == null)
					{
						Log.Information(Module.StartupLogic, base.ProcessName, "Ownership information was not received. User didn't perfomed the registration.");
						return false;
					}
					Log.Information(Module.StartupLogic, base.ProcessName, "Received ownership information");
					return true;
				}
				break;
			}
		}
	}
}
