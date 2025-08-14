using System.Security.Cryptography.X509Certificates;
using RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;
using RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.Core.TLSDetector;
using Rebex;
using SmartHome.SHC.SCommAdapter;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients;

internal class DeviceUpdateClient : ClientBase<DeviceUpdateServiceClient, IDeviceUpdateService>, IDeviceUpdateClient
{
	private readonly TLSCipherDetector cipherDetector = new TLSCipherDetector("DeviceUpdateClient");

	private readonly ICertificateManager certManager;

	private DeviceUpdateServiceClient serviceClient;

	public DeviceUpdateClient(INetworkingMonitor networkingMonitor, ICertificateManager certManager, Configuration configuration, IRegistrationService registrationService)
		: base(networkingMonitor, configuration.DeviceUpdateServiceUrl, registrationService)
	{
		this.certManager = certManager;
	}

	private void TryCreateServiceClient()
	{
		if (serviceClient == null)
		{
			if (string.IsNullOrEmpty(certManager.PersonalCertificateThumbprint))
			{
				Log.Error(Module.BackendCommunication, "Cannot create KeyExchangeClient. Personal certificate is not set.");
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

	private DeviceUpdateServiceClient CreateClient(string certificateThumbprint)
	{
		return CreateClient(certificateThumbprint, () => new DeviceUpdateServiceClient(new WcfBinding(LogLevel.Debug, cipherDetector.CheckCipherLog), base.Address));
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceUpdateResultCode CheckForDeviceUpdate(RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceDescriptor deviceDescriptor, out RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceUpdateInfo updateInfo)
	{
		TryCreateServiceClient();
		if (serviceClient != null)
		{
			RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope.DeviceUpdateInfo updateInfo2;
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceUpdateResultCode result = serviceClient.CheckForDeviceUpdate(deviceDescriptor.ToServerDeviceDescriptor(), out updateInfo2).ToShcDeviceUpdateResultCode();
			updateInfo = updateInfo2?.ToShcDeviceUpdateInfo();
			return result;
		}
		updateInfo = null;
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceUpdateResultCode.Failure;
	}
}
