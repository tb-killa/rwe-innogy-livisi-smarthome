using System.Security.Cryptography.X509Certificates;
using RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;
using RWE.SmartHome.SHC.BackendCommunication.SmsScope;
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

internal class SmsClient : ClientBase<SmsServiceClient, ISmsService>, ISmsClient
{
	private readonly TLSCipherDetector cipherDetector = new TLSCipherDetector("SmsClient");

	private SmsServiceClient client;

	private ICertificateManager certMgr;

	public SmsClient(INetworkingMonitor networkingMonitor, Configuration configuration, ICertificateManager certMgr, IRegistrationService registrationService)
		: base(networkingMonitor, configuration.SmsServiceUrl, registrationService)
	{
		this.certMgr = certMgr;
	}

	private SmsServiceClient CreateClient(string certificateThumbprint)
	{
		return CreateClient(certificateThumbprint, () => new SmsServiceClient(new WcfBinding(LogLevel.Debug, cipherDetector.CheckCipherLog), base.Address));
	}

	private void TryCreateClient()
	{
		if (client == null)
		{
			if (!string.IsNullOrEmpty(certMgr.PersonalCertificateThumbprint))
			{
				client = CreateClient(certMgr.PersonalCertificateThumbprint);
			}
			else
			{
				Log.Error(Module.BackendCommunication, "Cannot create SmsClient. Personal certificate is not set.");
			}
		}
		else
		{
			client.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindByThumbprint, certMgr.PersonalCertificateThumbprint);
		}
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.MessageAppResultCode GetSmsRemainingQuota(string certificateThumbprint, string shcSerial, out int? remainingQuota)
	{
		TryCreateClient();
		if (client != null)
		{
			return client.GetSmsRemainingQuota(shcSerial, out remainingQuota).Convert();
		}
		remainingQuota = null;
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.MessageAppResultCode.Failure;
	}
}
