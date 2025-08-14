using System.CodeDom.Compiler;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public interface IShcInitializationService
{
	InitializationErrorCode SubmitCertificateRequest(string shcSerial, string pin, string certificateRequest, out string sessionToken);

	InitializationErrorCode RetrieveInitializationData(string sessionToken, out string issuedCertificate, out ShcSyncRecord shcSyncRecord, out bool furtherPollingRequired, out int pollAfterSeconds);

	InitializationErrorCode ConfirmShcOwnership(string sessionToken, ShcMetadata shcMetadata, string shcInitializationResult);

	InitializationErrorCode ShcResetByOwner(string shcSerial);

	InitializationErrorCode SubmitOwnershipRequest(string shcSerial, string pin, out string sessionToken);

	InitializationErrorCode RetrieveOwnershipData(string sessionToken, out ShcSyncRecord shcSyncRecord, out bool furtherPollingRequired, out int pollAfterSeconds);
}
