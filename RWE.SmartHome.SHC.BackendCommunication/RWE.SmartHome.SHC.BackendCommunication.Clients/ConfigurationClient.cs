using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Channels;
using RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;
using RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;
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

internal class ConfigurationClient : ClientBase<ConfigurationServiceClient, IConfigurationService>, IConfigurationClient
{
	private readonly TLSCipherDetector cipherDetector = new TLSCipherDetector("ConfigurationClient");

	private ConfigurationServiceClient client;

	private string certificateThumbprint;

	public ConfigurationClient(INetworkingMonitor networkingMonitor, Configuration configuration, IRegistrationService registrationService)
		: base(networkingMonitor, configuration.ConfigurationServiceUrl, registrationService)
	{
	}

	private void CreateClient(string certificateThumbprint)
	{
		WcfBinding binding = new WcfBinding(LogLevel.Debug, cipherDetector.CheckCipherLog);
		TransportBindingElement element = binding.GetElement<TransportBindingElement>();
		element.MaxReceivedMessageSize = 3145728L;
		client = CreateClient(certificateThumbprint, () => new ConfigurationServiceClient(binding, base.Address));
		this.certificateThumbprint = certificateThumbprint;
		Log.Information(Module.BackendCommunication, "Created configuration service client.");
	}

	private void TryCreateClient(string certificateThumbprint)
	{
		if (client == null)
		{
			CreateClient(certificateThumbprint);
		}
		else
		{
			client.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindByThumbprint, certificateThumbprint);
		}
	}

	public void ReleaseServiceClient()
	{
		if (client != null)
		{
			client.ClientCredentials.ClientCertificate.Certificate.Reset();
			client.Dispose();
			client = null;
		}
		Log.Information(Module.BackendCommunication, "Released configuration service client.");
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode GetShcSyncRecord(string certificateThumbprint, string shcSerial, out RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcSyncRecord syncRecord)
	{
		TryCreateClient(certificateThumbprint);
		if (client != null)
		{
			RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope.ShcSyncRecord syncRecord2;
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode result = client.GetShcSyncRecord(shcSerial, out syncRecord2).Convert();
			syncRecord = syncRecord2?.Convert();
			return result;
		}
		syncRecord = null;
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.Failure;
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode ConfirmShcSyncRecord(string certificateThumbprint, string shcSerial)
	{
		TryCreateClient(certificateThumbprint);
		if (client != null)
		{
			return client.ConfirmShcSyncRecord(shcSerial).Convert();
		}
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.Failure;
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode SetManagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationType configType, string xPath, string bulkXMLData, int currentPacketNumber, int nextPacketNumber, string correlationId, bool createRestorePoint)
	{
		TryCreateClient(certificateThumbprint);
		if (client != null)
		{
			return client.SetManagedSHCConfiguration(shcSerialNo, configDataVersion, configType.Convert(), xPath, bulkXMLData, currentPacketNumber, nextPacketNumber, correlationId, createRestorePoint).Convert();
		}
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.Failure;
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode AddManagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationType configType, string xPath, string bulkXMLData, int currentPacketNumber, int nextPacketNumber, string correlationId, bool createRestorePoint)
	{
		TryCreateClient(certificateThumbprint);
		if (client != null)
		{
			return client.AddManagedSHCConfiguration(shcSerialNo, configDataVersion, configType.Convert(), xPath, bulkXMLData, currentPacketNumber, nextPacketNumber, correlationId, createRestorePoint).Convert();
		}
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.Failure;
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode SetUnmanagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationType configType, byte[] data, int currentPacketNumber, int nextPacketNumber, string correlationId, bool createRestorePoint)
	{
		TryCreateClient(certificateThumbprint);
		if (client != null)
		{
			return client.SetUnmanagedSHCConfiguration(shcSerialNo, configDataVersion, configType.Convert(), data, currentPacketNumber, nextPacketNumber, correlationId, createRestorePoint).Convert();
		}
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.Failure;
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode DeleteManagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationType configType, string xPath, bool createRestorePoint)
	{
		TryCreateClient(certificateThumbprint);
		if (client != null)
		{
			return client.DeleteManagedSHCConfiguration(shcSerialNo, configDataVersion, configType.Convert(), xPath, createRestorePoint).Convert();
		}
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.Failure;
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode DeleteUnmanagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationType configType, bool createRestorePoint)
	{
		TryCreateClient(certificateThumbprint);
		if (client != null)
		{
			return client.DeleteUnmanagedSHCConfiguration(shcSerialNo, configDataVersion, configType.Convert(), createRestorePoint).Convert();
		}
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.Failure;
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode GetManagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationType configType, string xPath, out string result)
	{
		TryCreateClient(certificateThumbprint);
		if (client != null)
		{
			return client.GetManagedSHCConfiguration(shcSerialNo, configDataVersion, configType.ToString(), xPath, out result).Convert();
		}
		result = null;
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.Failure;
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode GetUnmanagedSHCConfiguration(string certificateThumbprint, string shcSerialNo, string configDataVersion, RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationType configType, out byte[] data)
	{
		TryCreateClient(certificateThumbprint);
		if (client != null)
		{
			return client.GetUnmanagedSHCConfiguration(shcSerialNo, configDataVersion, configType.ToString(), out data).Convert();
		}
		data = null;
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.Failure;
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode GetRestorePointShcConfiguration(string certificateThumbprint, string restorePointId, string configDataVersion, out string result)
	{
		TryCreateClient(certificateThumbprint);
		if (client != null)
		{
			return client.GetRestorePointShcConfiguration(restorePointId, configDataVersion, out result).Convert();
		}
		result = null;
		return RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ConfigurationResultCode.Failure;
	}
}
