using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;
using RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.Core.TLSDetector;
using Rebex;
using SmartHome.SHC.SCommAdapter;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients;

internal class ShcMessagingClient : ClientBase<ShcMessagingServiceClient, IShcMessagingService>, IShcMessagingClient
{
	private readonly TLSCipherDetector cipherDetector = new TLSCipherDetector("ShcMessagingClient");

	private ShcMessagingServiceClient serviceClient;

	private ICertificateManager certMgr;

	public ShcMessagingClient(INetworkingMonitor networkingMonitor, Configuration configuration, ICertificateManager certMgr, IRegistrationService registrationService)
		: base(networkingMonitor, configuration.ShcMessagingServiceUrl, registrationService)
	{
		this.certMgr = certMgr;
	}

	private ShcMessagingServiceClient CreateClient(string certificateThumbprint)
	{
		return CreateClient(certificateThumbprint, () => new ShcMessagingServiceClient(new WcfBinding(LogLevel.Debug, cipherDetector.CheckCipherLog), base.Address));
	}

	private void TryCreateClient()
	{
		if (serviceClient == null)
		{
			if (!string.IsNullOrEmpty(certMgr.PersonalCertificateThumbprint))
			{
				serviceClient = CreateClient(certMgr.PersonalCertificateThumbprint);
			}
			else
			{
				Log.Error(Module.BackendCommunication, "Cannot create ShcMessagingClient. Personal certificate is not set.");
			}
		}
		else
		{
			serviceClient.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindByThumbprint, certMgr.PersonalCertificateThumbprint);
		}
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SendSmokeDetectedNotificationResult SendSmokeDetectionNotification(string certificateThumbprint, string shcSerialNo, string roomName, DateTime date, int shcTimeOffset)
	{
		TryCreateClient();
		if (serviceClient != null)
		{
			return serviceClient.SendSmokeDetectionNotification(shcSerialNo, roomName, date, shcTimeOffset).Convert();
		}
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SendSmokeDetectedNotificationResult.Failure;
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SendNotificationEmailResult SendNotificationEmail(string certificateThumbprint, string shcSerialNo, EmailTemplate emailTemplate, Dictionary<string, string> templateParams, DateTime date, int shcTimeOffset)
	{
		TryCreateClient();
		if (serviceClient != null)
		{
			return serviceClient.SendNotificationEmail(shcSerialNo, emailTemplate.ToBETemplate(), date, shcTimeOffset, ShcMessagingExtensions.ToBETemplateParameters(templateParams)).ToShcResult();
		}
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SendNotificationEmailResult.Failure;
	}
}
