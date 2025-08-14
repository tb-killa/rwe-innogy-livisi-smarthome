using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace WebServerHost;

public interface IEmailService
{
	EmailSettings GetEmailSettings();

	void SetEmailSettings(EmailSettings emailSettings);

	SmtpResultCode TestConnection();

	bool EmailSettingsNotificationDefaultValue(bool? property, bool defaultValue);
}
