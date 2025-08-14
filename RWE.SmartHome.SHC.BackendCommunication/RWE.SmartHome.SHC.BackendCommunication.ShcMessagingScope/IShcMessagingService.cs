using System;
using System.CodeDom.Compiler;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public interface IShcMessagingService
{
	SendSmokeDetectedNotificationResult SendSmokeDetectionNotification(string shcSerialNo, string roomName, DateTime date, int shcTimeOffset);

	SendSmokeDetectedNotificationResult SendSmokeDetectionNotification14(string shcSerialNo, string roomName, DateTime date, string culture, int shcTimeOffset);

	SendNotificationEmailResult SendNotificationEmail(string shcSerialNo, EmailTemplates emailTemplate, DateTime localDate, int shcTimeOffset, KeyValuePairOfstringstring[] templateParameters);

	MessageAppResultCode SendEmail(string[] receivers, string message, string shcSerial, out int? remainingQuota);

	MessageAppResultCode SendSystemEmail(string receiverEmail, string shcSerial, EmailTemplates template, string templateParameter);

	MessageAppResultCode GetEmailRemainingQuota(string shcSerial, out int? remainingQuota);
}
