using System.Security.Cryptography.X509Certificates;
using RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;
using RWE.SmartHome.SHC.BackendCommunication.NotificationScope;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.Core.TLSDetector;
using Rebex;
using SmartHome.SHC.SCommAdapter;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients;

internal class NotificationClient : ClientBase<NotificationServiceClient, INotificationService>, INotificationServiceClient
{
	private readonly TLSCipherDetector cipherDetector = new TLSCipherDetector("NotificationClient");

	private ICertificateManager certManager;

	private NotificationServiceClient serviceClient;

	internal NotificationClient(INetworkingMonitor networkingMonitor, Configuration configuration, ICertificateManager certManager, IRegistrationService registrationService)
		: base(networkingMonitor, configuration.NotificationServiceUrl, registrationService)
	{
		this.certManager = certManager;
		TryCreateServiceClient();
	}

	private void TryCreateServiceClient()
	{
		if (serviceClient == null)
		{
			if (string.IsNullOrEmpty(certManager.PersonalCertificateThumbprint))
			{
				Log.Error(Module.BackendCommunication, "Cannot create NotificationClient. Personal certificate is not set.");
			}
			else
			{
				serviceClient = CreateClient(certManager.PersonalCertificateThumbprint);
			}
		}
		else
		{
			serviceClient.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindByThumbprint, certManager.PersonalCertificateThumbprint);
		}
	}

	private NotificationServiceClient CreateClient(string certificateThumbprint)
	{
		return CreateClient(certificateThumbprint, () => new NotificationServiceClient(new WcfBinding(LogLevel.Debug, cipherDetector.CheckCipherLog), base.Address));
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationResponse SendNotifications(string certificateThumbprint, RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.CustomNotification notification)
	{
		TryCreateServiceClient();
		if (serviceClient != null)
		{
			return serviceClient.SendNotifications(notification.Convert()).Convert();
		}
		return new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationResponse(RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationSendResult.UnexpectedFailure, 0, null);
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationResponse SendSystemNotifications(RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.SystemNotification notification)
	{
		TryCreateServiceClient();
		if (serviceClient != null)
		{
			return serviceClient.SendSystemNotifications(notification.Convert()).Convert();
		}
		return new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationResponse(RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationSendResult.UnexpectedFailure, 0, null);
	}
}
