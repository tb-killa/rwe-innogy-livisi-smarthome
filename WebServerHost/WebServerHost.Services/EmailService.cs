using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalCommunication;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace WebServerHost.Services;

public class EmailService : IEmailService
{
	private readonly IEmailSender emailSender;

	public EmailService(IEmailSender emailSender)
	{
		this.emailSender = emailSender;
	}

	public EmailSettings GetEmailSettings()
	{
		return FilePersistence.EmailSettings;
	}

	public void SetEmailSettings(EmailSettings emailSettings)
	{
		FilePersistence.EmailSettings = emailSettings;
	}

	public SmtpResultCode TestConnection()
	{
		return emailSender.TestConnection();
	}

	public bool EmailSettingsNotificationDefaultValue(bool? property, bool defaultValue)
	{
		if (property.HasValue)
		{
			return property.Value;
		}
		return defaultValue;
	}
}
