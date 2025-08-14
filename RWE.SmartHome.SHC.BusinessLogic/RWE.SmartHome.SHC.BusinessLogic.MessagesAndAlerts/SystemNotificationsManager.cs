using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalCommunication;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.MessagesAndAlerts;

public class SystemNotificationsManager
{
	private INotificationServiceClient notificationServiceClient;

	private readonly IEmailSender emailSender;

	private List<SmtpEmail> emailSentForDevices;

	private List<string> supportedMessageTypes = new List<string>
	{
		MessageType.DeviceUnreachable.ToString(),
		MessageType.DeviceLowBattery.ToString()
	};

	public SystemNotificationsManager(INotificationServiceClient notificationServiceClient, IEmailSender emailSender)
	{
		this.notificationServiceClient = notificationServiceClient;
		this.emailSender = emailSender;
		emailSentForDevices = new List<SmtpEmail>();
	}

	public void SendSystemNotification(Message message)
	{
		if (message.Type == MessageType.DeviceUnreachable.ToString() || message.Type == MessageType.DeviceLowBattery.ToString())
		{
			SendSmtpEmail(message.BaseDeviceIds, ToEmailType(message.Type));
		}
		if (!(message.AppId == CoreConstants.CoreAppId) || !supportedMessageTypes.Contains(message.Type))
		{
			return;
		}
		SystemNotification systemNotification = ToSystemNotification(message);
		NotificationResponse notificationResponse = notificationServiceClient.SendSystemNotifications(systemNotification);
		if (notificationResponse.NotificationSendResult == NotificationSendResult.Success)
		{
			Log.Information(Module.BusinessLogic, string.Format("System notification successfully sent. [ProductId]=[{0}], [Type]=[{1}], [Class]=[{2}], [Parameters]=[{3}]", systemNotification.ProductId, systemNotification.Type, systemNotification.Class, string.Join(",", systemNotification.Parameters.Select((KeyValuePair<string, string> param) => $"[{param.Key}]=[{param.Value}]").ToArray())));
		}
		else
		{
			Log.Error(Module.BusinessLogic, string.Format("System notificcation could not be sent. [ProductId]=[{0}], [Type]=[{1}], [Parameters]=[{2}]. Received response: [{3}]", systemNotification.ProductId, systemNotification.Type, string.Join(",", systemNotification.Parameters.Select((KeyValuePair<string, string> param) => $"[{param.Key}]=[{param.Value}]").ToArray()), notificationResponse.NotificationSendResult.ToString()));
		}
	}

	private void SendSmtpEmail(List<Guid> devicesIds, EmailType emailType)
	{
		if (!emailSender.CanSendEmail(emailType))
		{
			return;
		}
		Guid deviceId;
		foreach (Guid devicesId in devicesIds)
		{
			deviceId = devicesId;
			SmtpEmail smtpEmail = emailSentForDevices.FirstOrDefault((SmtpEmail email) => email.DeviceId == deviceId);
			if (smtpEmail != null)
			{
				if (smtpEmail.SendingDate.Day <= ShcDateTime.Now.Day)
				{
					smtpEmail.SendingDate = ShcDateTime.Now;
					emailSender.SendEmailType(deviceId, emailType);
				}
			}
			else
			{
				smtpEmail = new SmtpEmail();
				smtpEmail.SendingDate = ShcDateTime.Now;
				smtpEmail.DeviceId = deviceId;
				emailSender.SendEmailType(deviceId, emailType);
			}
		}
	}

	private EmailType ToEmailType(string messageType)
	{
		if (messageType == MessageType.DeviceUnreachable.ToString())
		{
			return EmailType.DeviceUnreachable;
		}
		if (messageType == MessageType.DeviceLowBattery.ToString())
		{
			return EmailType.DeviceLowBattery;
		}
		return EmailType.SmokeDetected;
	}

	private SystemNotification ToSystemNotification(Message message)
	{
		SystemNotification systemNotification = new SystemNotification();
		systemNotification.ProductId = message.AppId;
		systemNotification.Class = message.Class.ToString();
		systemNotification.Type = message.Type;
		systemNotification.Parameters = ToParams(message);
		return systemNotification;
	}

	private List<KeyValuePair<string, string>> ToParams(Message message)
	{
		List<KeyValuePair<string, string>> templateParams = new List<KeyValuePair<string, string>>();
		message.Properties.ForEach(delegate(StringProperty prop)
		{
			templateParams.Add(new KeyValuePair<string, string>(prop.Name, prop.Value));
		});
		return templateParams;
	}
}
