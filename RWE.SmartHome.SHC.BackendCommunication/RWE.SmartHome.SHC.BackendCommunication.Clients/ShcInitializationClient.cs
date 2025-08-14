using System;
using RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;
using RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.Core.TLSDetector;
using Rebex;
using SmartHome.SHC.SCommAdapter;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients;

internal class ShcInitializationClient : ClientBase<ShcInitializationServiceClient, IShcInitializationService>, IShcInitializationClient
{
	private readonly TLSCipherDetector cipherDetector = new TLSCipherDetector("ShcInitializationClient");

	public ShcInitializationClient(INetworkingMonitor networkingMonitor, Configuration configuration, IRegistrationService registrationService)
		: base(networkingMonitor, configuration.ShcInitializationServiceUrl, registrationService)
	{
	}

	private ShcInitializationServiceClient CreateClient(string certificateThumbprint)
	{
		return CreateClient(certificateThumbprint, () => new ShcInitializationServiceClient(new WcfBinding(LogLevel.Debug, cipherDetector.CheckCipherLog), base.Address));
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.InitializationErrorCode RegisterShc(string certificateThumbprint, string shcSerial, string pin, string certificateRequest, out Guid sessionToken)
	{
		using ShcInitializationServiceClient shcInitializationServiceClient = CreateClient(certificateThumbprint);
		string sessionToken2;
		RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.InitializationErrorCode code = shcInitializationServiceClient.SubmitCertificateRequest(shcSerial, pin, certificateRequest, out sessionToken2);
		sessionToken = new Guid(sessionToken2);
		return code.Convert();
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.InitializationErrorCode RetrieveInitializationData(string certificateThumbprint, Guid sessionToken, out string issuedCertificate, out RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcSyncRecord record, out bool furtherPollingRequired, out int pollAfterSeconds)
	{
		using ShcInitializationServiceClient shcInitializationServiceClient = CreateClient(certificateThumbprint);
		RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.ShcSyncRecord shcSyncRecord;
		RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.InitializationErrorCode code = shcInitializationServiceClient.RetrieveInitializationData(sessionToken.ToString(), out issuedCertificate, out shcSyncRecord, out furtherPollingRequired, out pollAfterSeconds);
		record = shcSyncRecord?.Convert();
		return code.Convert();
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.InitializationErrorCode ConfirmShcOwnership(string certificateThumbprint, Guid sessionToken, RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcMetadata shcMetadata, ShcInitializationResult initializationResult)
	{
		using ShcInitializationServiceClient shcInitializationServiceClient = CreateClient(certificateThumbprint);
		return shcInitializationServiceClient.ConfirmShcOwnership(sessionToken.ToString(), shcMetadata.Convert(), initializationResult.Convert()).Convert();
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.InitializationErrorCode ShcResetByOwner(string certificateThumbprint, string shcSerial)
	{
		using ShcInitializationServiceClient shcInitializationServiceClient = CreateClient(certificateThumbprint);
		return shcInitializationServiceClient.ShcResetByOwner(shcSerial).Convert();
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.InitializationErrorCode SubmitOwnershipRequest(string certificateThumbprint, string shcSerial, string pin, out Guid sessionToken)
	{
		using ShcInitializationServiceClient shcInitializationServiceClient = CreateClient(certificateThumbprint);
		string sessionToken2;
		RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.InitializationErrorCode code = shcInitializationServiceClient.SubmitOwnershipRequest(shcSerial, pin, out sessionToken2);
		sessionToken = ((sessionToken2 != null) ? new Guid(sessionToken2) : Guid.Empty);
		return code.Convert();
	}

	public RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.InitializationErrorCode RetrieveOwnershipData(string certificateThumbprint, Guid sessionToken, out RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.ShcSyncRecord shcSyncRecord, out bool furtherPollingRequired, out int pollAfterSeconds)
	{
		using ShcInitializationServiceClient shcInitializationServiceClient = CreateClient(certificateThumbprint);
		if (shcInitializationServiceClient == null)
		{
			shcSyncRecord = null;
			furtherPollingRequired = false;
			pollAfterSeconds = 0;
			throw new AccountlessShcException("Cannot create ShcInitializationClient");
		}
		RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.ShcSyncRecord shcSyncRecord2;
		RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope.InitializationErrorCode code = shcInitializationServiceClient.RetrieveOwnershipData(sessionToken.ToString(), out shcSyncRecord2, out furtherPollingRequired, out pollAfterSeconds);
		shcSyncRecord = shcSyncRecord2?.Convert();
		return code.Convert();
	}
}
