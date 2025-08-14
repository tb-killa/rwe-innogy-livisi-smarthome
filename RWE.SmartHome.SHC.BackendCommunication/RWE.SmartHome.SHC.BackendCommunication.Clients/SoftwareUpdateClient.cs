using System.Security.Cryptography.X509Certificates;
using RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;
using RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope;
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

internal class SoftwareUpdateClient : ClientBase<SoftwareUpdateServiceClient, ISoftwareUpdateService>, ISoftwareUpdateClient
{
	private readonly TLSCipherDetector cipherDetector = new TLSCipherDetector("SoftwareUpdateClient");

	private string certificateThumbprint;

	private SoftwareUpdateServiceClient serviceClient;

	public SoftwareUpdateClient(INetworkingMonitor networkingMonitor, Configuration configuration, IRegistrationService registrationService)
		: base(networkingMonitor, configuration.SoftwareUpdateServiceUrl, registrationService)
	{
		certificateThumbprint = string.Empty;
		serviceClient = null;
	}

	private void CreateOrUpdateClient(string certificateThumbprint)
	{
		if (serviceClient == null || this.certificateThumbprint != certificateThumbprint)
		{
			if (this.certificateThumbprint != certificateThumbprint)
			{
				if (!string.IsNullOrEmpty(this.certificateThumbprint))
				{
					Log.Information(Module.BackendCommunication, "Certificate thumbprint changed, reinitializing Software Update service client.");
				}
				if (serviceClient != null)
				{
					serviceClient.ClientCredentials.ClientCertificate.Certificate.Reset();
					serviceClient.Dispose();
					serviceClient = null;
				}
			}
			this.certificateThumbprint = certificateThumbprint;
			serviceClient = CreateClient(this.certificateThumbprint, () => new SoftwareUpdateServiceClient(new WcfBinding(LogLevel.Debug, cipherDetector.CheckCipherLog), base.Address));
		}
		else
		{
			serviceClient.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindByThumbprint, this.certificateThumbprint);
		}
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SwUpdateResultCode CheckForSoftwareUpdate(string certificateThumbprint, string shcSerial, RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcVersionInfo shcVersionInfo, out RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UpdateInfo updateInfo)
	{
		CreateOrUpdateClient(certificateThumbprint);
		RWE.SmartHome.SHC.BackendCommunication.SoftwareUpdateScope.UpdateInfo updateInfo2 = null;
		RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SwUpdateResultCode result = RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SwUpdateResultCode.Failure;
		if (serviceClient != null)
		{
			result = serviceClient.CheckForSoftwareUpdate(shcSerial, shcVersionInfo.Convert(), out updateInfo2).Convert();
		}
		updateInfo = updateInfo2?.Convert();
		return result;
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcUpdateAnnouncementResultCode AnnounceShcUpdate(string certificateThumbprint, RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcVersionInfo shcVersionInfo)
	{
		CreateOrUpdateClient(certificateThumbprint);
		return serviceClient.ShcSoftwareUpdated(shcVersionInfo.Convert()).Convert();
	}
}
