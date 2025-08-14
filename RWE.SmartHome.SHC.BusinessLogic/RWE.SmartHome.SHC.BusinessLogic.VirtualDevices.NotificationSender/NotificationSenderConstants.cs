namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.NotificationSender;

public class NotificationSenderConstants
{
	public const string SmsContingentAppId = "sh://SMSContingent.RWE";

	public const string NotificationSenderDeviceName = "Notification Sender";

	public const string PushNotificationActuatorCapabilityName = "Push Notification Actuator";

	public const string PushNotificationActuatorCapabilityType = "PushNotificationActuator";

	public const string SmsActuatorCapabilityName = "SMS Actuator";

	public const string SmsActuatorCapabilityType = "SmsActuator";

	public const string EmailActuatorCapabilityName = "Email Actuator";

	public const string EmailActuatorCapabilityType = "EmailActuator";

	public const string AppQuotaParameterName = "APP_QUOTA";

	public const int DefaultLimitCount = 100;

	public const SendInterval DefaultLimitInterval = SendInterval.Month;

	public const string PushNotificationTitle = "Title";

	public const string EmailTitle = "Subject";

	public const string NotificationBody = "NotificationBody";

	public const string UserNames = "RecipientAccountNames";

	public const string CustomRecipients = "CustomRecipients";

	public const char RecipientsSeparator = ',';

	public const string SendLimitCount = "SendLimitCount";

	public const string SendLimitInterval = "SendLimitInterval";

	public const string SendNotificationsMonthInterval = "Month";

	public const string SendNotificationsWeekInterval = "Week";

	public const string SendNotificationsDayInterval = "Day";

	public const string AppendTriggerInfo = "AppendTriggerInfo";

	public const string SendMessageLimitExceeded = "SendMessageLimitExceeded";

	public const string LocalAccessActivated = "LocalAccessActivated";

	public const string LocalAccessDeactivated = "LocalAccessDeactivated";

	public const string NotEnoughDiskSpaceAvailableForUpdate = "NotEnoughDiskSpaceAvailableForUpdate";

	public const string MessageLimitPeriod = "SendLimitInterval";

	public const string MessageLimit = "SendLimit";

	public const string MessageCouldNotBeSentIn10Min = "MessageCouldNotBeSentIn10Minutes";

	public const int MessageSentRetryMaxInterval = 10;

	public const int MessageSentRetryInterval = 2;

	public const string InteractionId = "InteractionId";

	public const string InteractionName = "InteractionName";

	public const string RuleId = "RuleId";

	public const int MaxRecipients = 32;

	public const string RemainingQuotaPropertyName = "RemainingQuota";

	public static bool IsSendAction(string actionType)
	{
		return "SendNotification" == actionType;
	}

	public static SendInterval? GetSendInterval(string sendInterval)
	{
		return sendInterval switch
		{
			"Month" => SendInterval.Month, 
			"Week" => SendInterval.Week, 
			"Day" => SendInterval.Day, 
			_ => null, 
		};
	}
}
