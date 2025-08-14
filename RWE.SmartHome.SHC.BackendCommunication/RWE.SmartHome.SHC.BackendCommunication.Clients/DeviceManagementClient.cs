using System.Security.Cryptography.X509Certificates;
using RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;
using RWE.SmartHome.SHC.BackendCommunication.DeviceManagementScope;
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

internal class DeviceManagementClient : ClientBase<DeviceManagementServiceClient, IDeviceManagementService>, IDeviceManagementClient
{
	private readonly TLSCipherDetector cipherDetector = new TLSCipherDetector("DeviceManagementClient");

	private ICertificateManager certMgr;

	private DeviceManagementServiceClient serviceClient;

	public DeviceManagementClient(INetworkingMonitor networkingMonitor, Configuration configuration, ICertificateManager certMgr, IRegistrationService registrationService)
		: base(networkingMonitor, configuration.DeviceManagementServiceUrl, registrationService)
	{
		this.certMgr = certMgr;
	}

	private DeviceManagementServiceClient CreateClient(string certificateThumbprint)
	{
		return CreateClient(certificateThumbprint, () => new DeviceManagementServiceClient(new WcfBinding(LogLevel.Debug, cipherDetector.CheckCipherLog), base.Address));
	}

	private void TryCreateClient()
	{
		if (serviceClient == null)
		{
			if (string.IsNullOrEmpty(certMgr.PersonalCertificateThumbprint))
			{
				Log.Error(Module.BackendCommunication, "Cannot create DeviceManagementServiceClient. Personal certificate is not set.");
			}
			else
			{
				serviceClient = CreateClient(certMgr.PersonalCertificateThumbprint);
			}
		}
		else
		{
			serviceClient.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindByThumbprint, certMgr.PersonalCertificateThumbprint);
		}
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UploadFileResponse UploadLogFile(string certificateThumbprint, string shcSerial, byte[] content, int currentPackage, int nextPackage, string correlationId)
	{
		TryCreateClient();
		if (serviceClient != null)
		{
			return serviceClient.UploadLogFile(shcSerial, content, currentPackage, nextPackage, correlationId).Convert();
		}
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UploadFileResponse.BackendFailure;
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UploadFileResponse UploadSystemInfo(string certificateThumbprint, string shcSerial, string content, RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SystemInfoType contentType, string description, int currentPackage, int nextPackage, string correlationId)
	{
		TryCreateClient();
		if (serviceClient != null)
		{
			return serviceClient.UploadSystemInfo(shcSerial, content, contentType.Convert(), description, currentPackage, nextPackage, correlationId).Convert();
		}
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.UploadFileResponse.BackendFailure;
	}
}
