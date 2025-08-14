using System.Collections.Generic;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;
using SmartHome.Common.API.Entities.Entities;
using WebServerHost.Web;

namespace WebServerHost.Controllers.ClientAPI;

[Route("email")]
public class EmailController : Controller
{
	private readonly IEmailService emailService;

	public EmailController(IEmailService emailService)
	{
		this.emailService = emailService;
	}

	[HttpGet]
	[Route("settings")]
	public EmailSettingsDto GetEmailSettings()
	{
		EmailSettings emailSettings = emailService.GetEmailSettings();
		EmailSettingsDto emailSettingsDto = new EmailSettingsDto();
		if (emailSettings != null)
		{
			emailSettingsDto.ServerAddress = emailSettings.ServerAddress;
			emailSettingsDto.ServerPortNumber = emailSettings.ServerPortNumber;
			emailSettingsDto.EmailUsername = emailSettings.EmailUsername;
			emailSettingsDto.EmailPassword = emailSettings.EmailPassword;
			emailSettingsDto.Recipients = emailSettings.Recipients;
			emailSettingsDto.NotificationDeviceLowBattery = emailSettings.NotificationDeviceLowBattery;
			emailSettingsDto.NotificationsDeviceUnreachable = emailSettings.NotificationsDeviceUnreachable;
		}
		else
		{
			emailSettingsDto.ServerAddress = string.Empty;
			emailSettingsDto.ServerPortNumber = null;
			emailSettingsDto.EmailUsername = string.Empty;
			emailSettingsDto.EmailPassword = string.Empty;
			emailSettingsDto.Recipients = new List<string>();
			emailSettingsDto.NotificationDeviceLowBattery = true;
			emailSettingsDto.NotificationsDeviceUnreachable = false;
		}
		return emailSettingsDto;
	}

	[Route("settings")]
	[HttpPut]
	public IResult SetEmailSettings([FromBody] EmailSettingsDto emailSettings)
	{
		int serverPortNumber = 0;
		if (emailSettings.ServerPortNumber.HasValue)
		{
			serverPortNumber = emailSettings.ServerPortNumber.Value;
		}
		EmailSettings emailSettings2 = new EmailSettings();
		emailSettings2.ServerAddress = emailSettings.ServerAddress;
		emailSettings2.ServerPortNumber = serverPortNumber;
		emailSettings2.EmailUsername = emailSettings.EmailUsername;
		emailSettings2.EmailPassword = emailSettings.EmailPassword;
		emailSettings2.Recipients = emailSettings.Recipients;
		emailSettings2.NotificationDeviceLowBattery = emailService.EmailSettingsNotificationDefaultValue(emailSettings.NotificationDeviceLowBattery, defaultValue: true);
		emailSettings2.NotificationsDeviceUnreachable = emailService.EmailSettingsNotificationDefaultValue(emailSettings.NotificationsDeviceUnreachable, defaultValue: false);
		EmailSettings emailSettings3 = emailSettings2;
		emailService.SetEmailSettings(emailSettings3);
		return Ok();
	}

	[HttpGet]
	[Route("test")]
	public IResult TestEmailSending()
	{
		return emailService.TestConnection() switch
		{
			SmtpResultCode.Ok => Accepted(string.Format("{{\"result\":\"{0}\"}}", "AVAILABLE")), 
			SmtpResultCode.BadLoginOrPassword => Accepted(string.Format("{{\"result\":\"{0}\"}}", "Bad login or password")), 
			_ => Accepted(string.Format("{{\"result\":\"{0}\"}}", "Undefined error id")), 
		};
	}
}
