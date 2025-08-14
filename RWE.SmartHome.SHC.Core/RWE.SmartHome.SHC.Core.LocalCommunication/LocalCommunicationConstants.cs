namespace RWE.SmartHome.SHC.Core.LocalCommunication;

public static class LocalCommunicationConstants
{
	public const string StopBackendRequestsDate = "3/1/2024";

	public const string SmokeDetectedEmailTempleteTitle = "Smoke Detected";

	public const string SmokeDetectedEmailTempleteSubject = "Mögliche Rauchentwicklung im Raum {0}/Possible smoke detection in room {0}";

	public const string DeviceUnreachableEmailTempleteTitle = "Device Unreachable";

	public const string DeviceUnreachableEmailTempleteSubject = "Gerät nicht erreichbar/Device not reachable";

	public const string DeviceLowBatteryEmailTempleteTitle = "Device low battery";

	public const string DeviceLowBatteryEmailTempleteSubject = "Batteriewechsel erforderlich/Device has low battery";

	public const string CustomEmailTempleteSubject = "Nachricht von Deinem Zuhause/Message from your home";

	public const string TestConnectionEmailTempleteTitle = "TestConnection";

	public const string TestConnectionEmailTempleteSubject = "Verbindung klappt/Connection works";

	public static string IsShcLocalOnlyFlagPath = "\\NandFlash\\IsShcLocalOnlyFlag";

	public static string BackendRequestsStoppedFlagPath = "\\NandFlash\\BackendServicesStoppedFlag";

	public static string EmailTemplatesSmokeDetectedFile = "\\NandFlash\\SHC\\smokeDetected.html";

	public static string EmailTemplatesDeviceUnreachableFile = "\\NandFlash\\SHC\\deviceUnreachable.html";

	public static string EmailTemplatesBatteryLowFile = "\\NandFlash\\SHC\\deviceLowBattery.html";

	public static string EmailTemplatesTestConnectionFile = "\\NandFlash\\SHC\\testConnection.html";
}
