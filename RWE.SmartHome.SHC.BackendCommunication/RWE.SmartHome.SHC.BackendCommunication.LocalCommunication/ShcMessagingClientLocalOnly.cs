using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunication.LocalCommunication;

internal class ShcMessagingClientLocalOnly : IShcMessagingClient
{
	public SendSmokeDetectedNotificationResult SendSmokeDetectionNotification(string certificateThumbprint, string shcSerialNo, string roomName, DateTime date, int shcTimeOffset)
	{
		return SendSmokeDetectedNotificationResult.Success;
	}

	public SendNotificationEmailResult SendNotificationEmail(string certificateThumbprint, string shcSerialNo, EmailTemplate emailTemplate, Dictionary<string, string> templateParams, DateTime date, int shcTimeOffset)
	{
		return SendNotificationEmailResult.Success;
	}
}
