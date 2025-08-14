using System;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalCommunication;

public interface IEmailSender
{
	void SendSmokeDetectedEmail(string location, string incidentDate, string incidentTime);

	void SendEmailType(Guid deviceId, EmailType emailType);

	void SendCustomEmail(string body);

	SmtpResultCode TestConnection();

	bool CanSendEmail(EmailType emailType);
}
