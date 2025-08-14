using System.Security.Cryptography.X509Certificates;
using RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.Core.TLSDetector;
using Rebex;
using SmartHome.SHC.BackendCommunication.KeyExchangeScope;
using SmartHome.SHC.SCommAdapter;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients;

internal class KeyExchangeClient : ClientBase<KeyExchangeServiceClient, IKeyExchangeService>, IKeyExchangeClient
{
	private readonly TLSCipherDetector cipherDetector = new TLSCipherDetector("KeyExchangeClient");

	private KeyExchangeServiceClient serviceClient;

	private ICertificateManager certMgr;

	public KeyExchangeClient(INetworkingMonitor networkingMonitor, Configuration configuration, ICertificateManager certMgr, IRegistrationService registrationService)
		: base(networkingMonitor, configuration.KeyExchangeServiceUrl, registrationService)
	{
		this.certMgr = certMgr;
	}

	private KeyExchangeServiceClient CreateClient(string certificateThumbprint)
	{
		return CreateClient(certificateThumbprint, () => new KeyExchangeServiceClient(new WcfBinding(LogLevel.Debug, cipherDetector.CheckCipherLog), base.Address));
	}

	private void TryCreateClient()
	{
		if (serviceClient == null)
		{
			if (string.IsNullOrEmpty(certMgr.PersonalCertificateThumbprint))
			{
				Log.Error(Module.BackendCommunication, "Cannot create KeyExchangeClient. Personal certificate is not set.");
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

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.KeyExchangeResult EncryptNetworkKey(string certificateThumbprint, byte[] sgtin, byte[] secNumber, byte[] encOnceNetworkKey, string deviceFirmwareVersion, out byte[] encTwiceNetworkKey, out byte[] keyAuthentication)
	{
		TryCreateClient();
		if (serviceClient != null)
		{
			return serviceClient.EncryptNetworkKey(sgtin, secNumber, encOnceNetworkKey, deviceFirmwareVersion, out encTwiceNetworkKey, out keyAuthentication).Convert();
		}
		encTwiceNetworkKey = null;
		keyAuthentication = null;
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.KeyExchangeResult.UnexpectedException;
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.KeyExchangeResult GetDeviceKey(string certificateThumbprint, byte[] sgtin, out byte[] deviceKey)
	{
		TryCreateClient();
		if (serviceClient != null)
		{
			return serviceClient.GetDeviceKey(sgtin, out deviceKey).Convert();
		}
		deviceKey = null;
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.KeyExchangeResult.UnexpectedException;
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.KeyExchangeResult GetMasterKey(string certificateThumbprint, out string masterKey)
	{
		TryCreateClient();
		if (serviceClient != null)
		{
			global::SmartHome.SHC.BackendCommunication.KeyExchangeScope.KeyExchangeResult masterKey2 = serviceClient.GetMasterKey(out masterKey);
			return masterKey2.Convert();
		}
		masterKey = null;
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.KeyExchangeResult.UnexpectedException;
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.KeyExchangeResult GetDevicesKeys(string certificateThumbprint, byte[][] sgtins, out RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary[] deviceKeys)
	{
		TryCreateClient();
		if (serviceClient != null)
		{
			global::SmartHome.SHC.BackendCommunication.KeyExchangeScope.ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary[] devicesKeys2;
			global::SmartHome.SHC.BackendCommunication.KeyExchangeScope.KeyExchangeResult devicesKeys = serviceClient.GetDevicesKeys(sgtins, out devicesKeys2);
			deviceKeys = devicesKeys2?.Convert();
			return devicesKeys.Convert();
		}
		deviceKeys = null;
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.KeyExchangeResult.UnexpectedException;
	}
}
