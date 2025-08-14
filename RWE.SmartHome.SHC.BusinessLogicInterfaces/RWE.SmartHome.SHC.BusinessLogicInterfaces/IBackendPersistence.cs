using System.Threading;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces;

public interface IBackendPersistence
{
	BackendPersistenceResult BackupUIConfiguration(bool createRestorePoint, ManualResetEvent cancellationEvent);

	bool RestoreUIConfiguration();

	bool RestoreUIConfigurationFromRestorePoint(string restorePointId);

	bool DeleteUIConfiguration();

	void ReleaseServiceClient();

	BackendPersistenceResult BackupTechnicalConfiguration(ManualResetEvent cancellationEvent);

	bool RestoreTechnicalConfiguration();

	bool DeleteTechnicalConfiguration();

	BackendPersistenceResult BackupDeviceList(ManualResetEvent cancellationEvent);

	bool RestoreDeviceList();

	bool DeleteDeviceList();

	BackendPersistenceResult BackupMessagesAndAlerts(ManualResetEvent cancellationEvent);

	bool RestoreMessagesAndAlerts();

	bool DeleteMessagesAndAlerts();

	BackendPersistenceResult BackupCustomApplicationsSettings(ManualResetEvent cancellationEvent);

	bool RestoreCustomApplicationsSettings();
}
