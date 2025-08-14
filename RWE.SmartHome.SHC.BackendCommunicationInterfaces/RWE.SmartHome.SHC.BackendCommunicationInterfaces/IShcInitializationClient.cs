using System;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces;

public interface IShcInitializationClient
{
	InitializationErrorCode RegisterShc(string certificateThumbprint, string shcSerial, string pin, string certificateRequest, out Guid sessionToken);

	InitializationErrorCode RetrieveInitializationData(string certificateThumbprint, Guid sessionToken, out string issuedCertificate, out ShcSyncRecord record, out bool furtherPollingRequired, out int pollAfterSeconds);

	InitializationErrorCode ConfirmShcOwnership(string certificateThumbprint, Guid sessionToken, ShcMetadata shcMetadata, ShcInitializationResult initializationResult);

	InitializationErrorCode ShcResetByOwner(string certificateThumbprint, string shcSerial);

	InitializationErrorCode SubmitOwnershipRequest(string certificateThumbprint, string shcSerial, string pin, out Guid sessionToken);

	InitializationErrorCode RetrieveOwnershipData(string certificateThumbprint, Guid sessionToken, out ShcSyncRecord shcSyncRecord, out bool furtherPollingRequired, out int pollAfterSeconds);
}
