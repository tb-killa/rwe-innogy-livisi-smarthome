using System;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunication.LocalCommunication;

internal class ShcInitializationClientLocalOnly : IShcInitializationClient
{
	public InitializationErrorCode RegisterShc(string certificateThumbprint, string shcSerial, string pin, string certificateRequest, out Guid sessionToken)
	{
		sessionToken = Guid.Empty;
		return InitializationErrorCode.Success;
	}

	public InitializationErrorCode RetrieveInitializationData(string certificateThumbprint, Guid sessionToken, out string issuedCertificate, out ShcSyncRecord record, out bool furtherPollingRequired, out int pollAfterSeconds)
	{
		issuedCertificate = string.Empty;
		record = new ShcSyncRecord();
		furtherPollingRequired = false;
		pollAfterSeconds = 0;
		return InitializationErrorCode.Success;
	}

	public InitializationErrorCode ConfirmShcOwnership(string certificateThumbprint, Guid sessionToken, ShcMetadata shcMetadata, ShcInitializationResult initializationResult)
	{
		return InitializationErrorCode.Success;
	}

	public InitializationErrorCode ShcResetByOwner(string certificateThumbprint, string shcSerial)
	{
		return InitializationErrorCode.Success;
	}

	public InitializationErrorCode SubmitOwnershipRequest(string certificateThumbprint, string shcSerial, string pin, out Guid sessionToken)
	{
		sessionToken = Guid.Empty;
		return InitializationErrorCode.Success;
	}

	public InitializationErrorCode RetrieveOwnershipData(string certificateThumbprint, Guid sessionToken, out ShcSyncRecord shcSyncRecord, out bool furtherPollingRequired, out int pollAfterSeconds)
	{
		shcSyncRecord = new ShcSyncRecord();
		furtherPollingRequired = false;
		pollAfterSeconds = 0;
		return InitializationErrorCode.Success;
	}
}
