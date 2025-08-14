using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces;

public interface IShcMessagingClient
{
	SendSmokeDetectedNotificationResult SendSmokeDetectionNotification(string certificateThumbprint, string shcSerialNo, string roomName, DateTime date, int shcTimeOffset);

	SendNotificationEmailResult SendNotificationEmail(string certificateThumbprint, string shcSerialNo, EmailTemplate emailTemplate, Dictionary<string, string> templateParams, DateTime date, int shcTimeOffset);
}
