namespace SmartHome.Common.API.Entities.Constants;

public class EventConstants
{
	public const string RollerShutterCalibratedEventName = "RollerShutterCalibrated";

	public const string CalibrationStepPropertyName = "CalibrationStep";

	public const string CalibrationValuePropertyName = "Value";

	public const string MessageCreatedEventName = "MessageCreated";

	public const string DeviceFoundEventName = "DeviceFound";

	public const string StateChangedEventName = "StateChanged";

	public const string ConfigurationSavedEventName = "ConfigSave";

	public const string ConfigurationChangedEventName = "ConfigurationChanged";

	public const string ConfigurationCommitErrorEventName = "CommitConfigError";

	public const string DeviceDiscoveryStatusChangedEventName = "DeviceDiscoveryStatusChanged";

	public const string DeviceDiscoveryStatusChangedPropertyName = "Active";

	public const string ActivateDiscoveryFailureEventName = "ActivateDiscoveryFailure";

	public const string ActivateDiscoveryFailurePropertyName = "Product";

	public const string DeviceDiscoveryFailureEventName = "DeviceDiscoveryFailure";

	public const string DeviceDiscoveryFailureCodePropertyName = "FailureCode";

	public const string DeviceDiscoveryFailureDeviceTypePropertyName = "DeviceType";

	public const string DeviceDiscoveryFailureDeviceManufacturerPropertyName = "Manufacturer";

	public const string DeviceDiscoveryFailureDeviceVersionPropertyName = "Version";

	public const string ConfigurationCommitErrorMessagePropertyName = "Message";

	public const string ConfigurationSaveErrorEventName = "SaveConfigError";

	public const string LicensingErrorEventName = "LicensingError";

	public const string ConfigurationSaveErrorReasonPropertyName = "ErrorReason";

	public const string LicensingStateQuotaReachedPropertyName = "LicensingStateQuota";

	public const string ConfigurationSaveErrorEntryParameterName = "ErrorEntry";

	public const string BrcButtonPressedEventName = "ButtonPressed";

	public const string WmdMotionDetectedEventName = "MotionDetected";

	public const string ConfigurationVersionPropertyName = "ConfigurationVersion";

	public const string UpdatedEntitiesParameterName = "Updated";

	public const string DeletedEntitiesParameterName = "Deleted";

	public const string EntityDeploymentEventName = "EntityDeploy";

	public const string GenericEventName = "Generic";

	public const string GenericEventApplicationIdPropertyName = "Product";

	public const string DisconnectEventTypeName = "Disconnect";

	public const string DisconnectionReasonPropertyName = "Reason";

	public const string DalFlushNotification = "DeviceActivityLoggingFlush";

	public const string DalFlushedNotificationSuccessPropertyName = "Success";

	public const string MessageStateChangedEventName = "MessageUpdated";

	public const string MessageStateChangedPropertyName = "State";

	public const string MessageDeletedEventName = "MessageDeleted";

	public const string EventDeliveryErrorName = "EventDeliveryError";

	public const string EventDeliveryPropertyName = "EventType";

	public const string ControllerConnectivityChanged = "ControllerConnectivityChanged";

	public const string IsConnected = "IsConnected";

	public const string SerialNumberPropertyName = "SerialNumber";

	public const string UserDataChangedEventTypeName = "UserDataChanged";

	public const string UserDataDeletedEventTypeName = "UserDataDeleted";

	public const string EventMetadataPath = "/event/";

	public const string EventTriggerType = "Event";

	public const string ApplicationsVersion = "2.0";

	public const string SystemEventSource = "System";
}
