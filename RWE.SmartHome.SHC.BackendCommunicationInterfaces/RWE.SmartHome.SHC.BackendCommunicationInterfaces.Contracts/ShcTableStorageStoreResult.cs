namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

public enum ShcTableStorageStoreResult
{
	Success,
	PartialSuccess,
	Failed,
	InvalidPayload,
	InvalidStorageType,
	UploadQuotaExceeded
}
