using System;
using System.Collections.Generic;
using System.Threading;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Authentication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;

namespace RWE.SmartHome.SHC.StartupLogic;

internal sealed class InitialRegistration : InitializationBase
{
	public InitialRegistration(IUserManager userManagement, IDisplayManager displayManager, IShcInitializationClient initializationClient, Configuration configuration, ICertificateManager certificateManager)
		: base(userManagement, displayManager, certificateManager, initializationClient, configuration, certificateManager.DefaultCertificateThumbprint, "InitialRegistration")
	{
	}

	public bool PerformIntialRegistration()
	{
		try
		{
			RegisterShc();
			if (!PollForCertificate(out var issuedCertificate, out var record))
			{
				base.DisplayManager.WorkflowProceeded(Workflow.Registration, WorkflowMessage.PressButton, forceDisplay: false);
				return false;
			}
			bool success = false;
			try
			{
				CompleteCertificateRequest(issuedCertificate);
				RegisterUsers(record);
				success = true;
			}
			finally
			{
				ConfirmOwnership(success);
			}
			base.DisplayManager.WorkflowFinished(Workflow.Registration);
		}
		catch
		{
			base.CertificateManager.DeletePersonalCertificate();
			base.DisplayManager.WorkflowFailed(Workflow.Registration, WorkflowError.InitializationServiceUnavailable);
			throw;
		}
		return true;
	}

	private void RegisterShc()
	{
		string text = InitializationBase.GeneratePin();
		base.DisplayManager.WorkflowProceeded(Workflow.Registration, WorkflowMessage.Register, forceDisplay: false);
		string certificateRequest = GenerateCertificateRequest();
		Log.Information(Module.StartupLogic, base.ProcessName, $"Performing initial SHC registration, serial '{base.Serial}', pin '{text}'");
		Guid sessionToken;
		InitializationErrorCode initializationErrorCode = base.InitializationClient.RegisterShc(base.CertificateManager.DefaultCertificateThumbprint, base.Serial, text, certificateRequest, out sessionToken);
		if (initializationErrorCode != InitializationErrorCode.Success)
		{
			throw new RegistrationException("Backend registration failed with error " + initializationErrorCode);
		}
		base.DisplayManager.WorkflowDynamicMessage(Workflow.Registration, text);
		base.SessionToken = sessionToken;
	}

	private string GenerateCertificateRequest()
	{
		string certificateSubjectName = base.Configuration.CertificateSubjectName;
		string certificateTemplateName = base.Configuration.CertificateTemplateName;
		string serial = base.Serial;
		string certificateUpnSuffix = base.Configuration.CertificateUpnSuffix;
		HostNameDefinition hostNameDefinition = new HostNameDefinition(base.Configuration.HostnameFormatString, base.Configuration.NumberOfPossibleHostnames, base.Configuration.NameResolutionWait);
		List<string> list = new List<string>(hostNameDefinition.PossibleHostnames.Length * 2);
		string[] possibleHostnames = hostNameDefinition.PossibleHostnames;
		foreach (string text in possibleHostnames)
		{
			list.Add(text);
			list.Add(text.ToLower() + ".local");
		}
		string text2 = CertificateHandling.CreateCertificateRequest("shc", "SHC Trusted Platform Module Cryptographic Service Provider", certificateSubjectName, certificateTemplateName, serial + '@' + certificateUpnSuffix, list.ToArray(), (CertificateHandling.KeyContainerFlags)0u, CertificateHandling.KeySpecification.AT_KEYEXCHANGE, (CertificateHandling.KeyGenerationFlags)0, 2048);
		if (text2 == null)
		{
			throw new RegistrationException("Failed to create certificate request");
		}
		return text2;
	}

	private static void CompleteCertificateRequest(string certificateResponse)
	{
		if (!CertificateHandling.AddCertificateResponseToStore("shc", "SHC Trusted Platform Module Cryptographic Service Provider", (CertificateHandling.KeyContainerFlags)0u, CertificateHandling.KeySpecification.AT_KEYEXCHANGE, "MY", certificateResponse))
		{
			throw new RegistrationException("Failed to register certificate response");
		}
	}

	private bool PollForCertificate(out string issuedCertificate, out ShcSyncRecord record)
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
			bool furtherPollingRequired;
			InitializationErrorCode initializationErrorCode = base.InitializationClient.RetrieveInitializationData(base.CertificateManager.DefaultCertificateThumbprint, base.SessionToken, out issuedCertificate, out record, out furtherPollingRequired, out pollAfterSeconds);
			switch (initializationErrorCode)
			{
			case InitializationErrorCode.RegistrationProcessExpired:
				Log.Information(Module.StartupLogic, base.ProcessName, $"Initial registration timed out on backend after {num} attempts");
				return false;
			default:
				throw new RegistrationException("Backend polling failed with error " + initializationErrorCode);
			case InitializationErrorCode.Success:
				if (!furtherPollingRequired && string.IsNullOrEmpty(issuedCertificate) && record == null)
				{
					return false;
				}
				Log.Information(Module.StartupLogic, base.ProcessName, $"Retrying again in {pollAfterSeconds} seconds");
				if (!furtherPollingRequired)
				{
					Log.Information(Module.StartupLogic, base.ProcessName, "Received ownership information and certificate");
					return true;
				}
				break;
			}
		}
	}
}
