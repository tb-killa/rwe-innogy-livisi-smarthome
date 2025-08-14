using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;
using RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;
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

internal class DataTrackingClient : ClientBase<DataTrackingServiceClient, IDataTrackingService>, IDataTrackingClient
{
	private readonly TLSCipherDetector cipherDetector = new TLSCipherDetector("DataTrackingClient");

	private readonly ICertificateManager certManager;

	private DataTrackingServiceClient serviceClient;

	private readonly IRegistrationService registrationService;

	public DataTrackingClient(INetworkingMonitor networkingMonitor, ICertificateManager certManager, Configuration configuration, IRegistrationService registrationService)
		: base(networkingMonitor, configuration.DataTrackingClientUrl, registrationService)
	{
		this.certManager = certManager;
		TryCreateServiceClient();
		this.registrationService = registrationService;
	}

	private void TryCreateServiceClient()
	{
		if (serviceClient == null)
		{
			if (string.IsNullOrEmpty(certManager.PersonalCertificateThumbprint))
			{
				Log.Error(Module.BackendCommunication, "Cannot create DataTrackingClient. Personal certificate is not set.");
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

	private DataTrackingServiceClient CreateClient(string certificateThumbprint)
	{
		return CreateClient(certificateThumbprint, () => new DataTrackingServiceClient(new WcfBinding(LogLevel.Debug, cipherDetector.CheckCipherLog), base.Address));
	}

	public bool StoreData(List<RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.TrackData> deviceTrackingEntities)
	{
		if (registrationService.IsShcLocalOnly)
		{
			return false;
		}
		TryCreateServiceClient();
		if (serviceClient != null)
		{
			return serviceClient.StoreListData(deviceTrackingEntities.ConvertAll((RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.TrackData e) => e.ToBeTrackData()).ToArray());
		}
		return false;
	}
}
