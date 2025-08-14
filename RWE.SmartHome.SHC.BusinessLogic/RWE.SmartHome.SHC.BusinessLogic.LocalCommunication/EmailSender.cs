using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalCommunication;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using Rebex.Mail;
using Rebex.Net;
using Rebex.Security.Certificates;

namespace RWE.SmartHome.SHC.BusinessLogic.LocalCommunication;

public class EmailSender : IEmailSender
{
	private const int SmtpDefaultServerPortNumber = 465;

	private List<EmailTemplate> emailTemplates;

	private readonly IRepository configurationRepository;

	public EmailSender(IRepository configurationRepository)
	{
		this.configurationRepository = configurationRepository;
		GetEmailTempletes();
	}

	public void SendSmokeDetectedEmail(string location, string incidentDate, string incidentTime)
	{
		EmailTemplate emailTemplate = emailTemplates.FirstOrDefault((EmailTemplate email) => string.Equals(email.Title, "Smoke Detected"));
		SendEmail(string.Format(emailTemplate.Subject, location), string.Format(emailTemplate.Body, location, incidentDate, incidentTime, NetworkTools.GetDeviceIp(), NetworkTools.GetHostName()));
	}

	public void SendEmailType(Guid deviceId, EmailType emailType)
	{
		EmailTemplate emailTemplate = null;
		switch (emailType)
		{
		case EmailType.DeviceUnreachable:
			emailTemplate = emailTemplates.FirstOrDefault((EmailTemplate email) => string.Equals(email.Title, "Device Unreachable"));
			break;
		case EmailType.DeviceLowBattery:
			emailTemplate = emailTemplates.FirstOrDefault((EmailTemplate email) => string.Equals(email.Title, "Device low battery"));
			break;
		}
		if (emailTemplate == null)
		{
			Log.Error(Module.BusinessLogic, $"These is no email template for the email type: {emailType.ToString()}");
			return;
		}
		BaseDevice baseDevice = configurationRepository.GetBaseDevice(deviceId);
		SendEmail(emailTemplate.Subject, string.Format(emailTemplate.Body, baseDevice.Name, baseDevice.Location.Name, NetworkTools.GetDeviceIp(), NetworkTools.GetHostName()));
	}

	public void SendCustomEmail(string body)
	{
		SendEmail("Nachricht von Deinem Zuhause/Message from your home", body);
	}

	public SmtpResultCode TestConnection()
	{
		EmailTemplate emailTemplate = emailTemplates.FirstOrDefault((EmailTemplate email) => string.Equals(email.Title, "TestConnection"));
		return SendEmail(emailTemplate.Subject, emailTemplate.Body);
	}

	public bool CanSendEmail(EmailType emailType)
	{
		bool result = true;
		switch (emailType)
		{
		case EmailType.DeviceLowBattery:
			if (!FilePersistence.EmailSettings.NotificationDeviceLowBattery)
			{
				Log.Debug(Module.BusinessLogic, "NotificationDeviceLowBattery settings is set to false, do not send the email");
				result = false;
			}
			break;
		case EmailType.DeviceUnreachable:
			if (!FilePersistence.EmailSettings.NotificationsDeviceUnreachable)
			{
				Log.Debug(Module.BusinessLogic, "NotificationsDeviceUnreachable settings is set to false, do not send the email");
				result = false;
			}
			break;
		}
		return result;
	}

	private SmtpResultCode SendEmail(string title, string body)
	{
		try
		{
			EmailSettings emailSettings = FilePersistence.EmailSettings;
			if (emailSettings == null || emailSettings.ServerAddress == null || emailSettings.EmailUsername == null || emailSettings.EmailPassword == null || emailSettings.Recipients == null)
			{
				Log.Error(Module.BusinessLogic, "The SMTP email settings were not set, cannot send an email");
				return SmtpResultCode.UndefinedError;
			}
			using Smtp smtp = new Smtp();
			smtp.ValidatingCertificate += ValidatingCertificate;
			try
			{
				try
				{
					int serverPortNumber = emailSettings.ServerPortNumber;
					if (serverPortNumber == 465)
					{
						smtp.Connect(emailSettings.ServerAddress, serverPortNumber, SslMode.Implicit);
					}
					else
					{
						smtp.Connect(emailSettings.ServerAddress, serverPortNumber, SslMode.Explicit);
					}
				}
				catch (Exception ex)
				{
					Log.Error(Module.BusinessLogic, $"Cannot get the server port number from the port settings or there was an error connecting to the SMTP server {ex.Message} {ex.StackTrace}");
					smtp.Connect(emailSettings.ServerAddress, SslMode.Implicit);
				}
			}
			catch (Exception ex2)
			{
				Log.Error(Module.BusinessLogic, $"The server address of the port number are not valid {ex2.Message} {ex2.StackTrace}");
				return SmtpResultCode.IncorrectServerAddress;
			}
			try
			{
				smtp.Login(emailSettings.EmailUsername, emailSettings.EmailPassword);
			}
			catch (Exception ex3)
			{
				Log.Error(Module.BusinessLogic, $"There was an exception when using the login method for SMTP {ex3.Message} {ex3.StackTrace}");
				return SmtpResultCode.BadLoginOrPassword;
			}
			foreach (string recipient in emailSettings.Recipients)
			{
				MailMessage mailMessage = new MailMessage();
				mailMessage.From = emailSettings.EmailUsername;
				mailMessage.To = recipient;
				mailMessage.Subject = title;
				mailMessage.BodyHtml = body;
				smtp.Send(mailMessage);
			}
			smtp.Disconnect();
			return SmtpResultCode.Ok;
		}
		catch (Exception ex4)
		{
			Log.Error(Module.BusinessLogic, $"There was an error sending the SMTP email. {ex4.Message} {ex4.StackTrace}");
			return SmtpResultCode.UndefinedError;
		}
	}

	private static void ValidatingCertificate(object sender, SslCertificateValidationEventArgs e)
	{
		ValidationResult validationResult = e.CertificateChain.Validate(e.ServerName, ValidationOptions.None);
		if (validationResult.Valid)
		{
			e.Accept();
		}
		_ = e.CertificateChain[0];
		e.Accept();
	}

	private void GetEmailTempletes()
	{
		emailTemplates = new List<EmailTemplate>();
		try
		{
			LoadEmailTemplatesFromPersistence(LocalCommunicationConstants.EmailTemplatesSmokeDetectedFile, "Smoke Detected", "Mögliche Rauchentwicklung im Raum {0}/Possible smoke detection in room {0}");
			LoadEmailTemplatesFromPersistence(LocalCommunicationConstants.EmailTemplatesDeviceUnreachableFile, "Device Unreachable", "Gerät nicht erreichbar/Device not reachable");
			LoadEmailTemplatesFromPersistence(LocalCommunicationConstants.EmailTemplatesBatteryLowFile, "Device low battery", "Batteriewechsel erforderlich/Device has low battery");
			LoadEmailTemplatesFromPersistence(LocalCommunicationConstants.EmailTemplatesTestConnectionFile, "TestConnection", "Verbindung klappt/Connection works");
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, $"There was a problem loading the email templates file. {ex.Message} {ex.StackTrace}");
		}
	}

	private void LoadEmailTemplatesFromPersistence(string file, string emailName, string subject)
	{
		try
		{
			string text;
			using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using StreamReader streamReader = new StreamReader(stream);
				text = streamReader.ReadToEnd();
			}
			if (!string.IsNullOrEmpty(text))
			{
				emailTemplates.Add(new EmailTemplate(emailName, subject, text));
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, $"{ex.Message} {ex.StackTrace}");
		}
	}
}
