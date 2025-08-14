using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;

internal static class ShcMessagingExtensions
{
	public static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SendSmokeDetectedNotificationResult Convert(this RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope.SendSmokeDetectedNotificationResult response)
	{
		return response switch
		{
			RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope.SendSmokeDetectedNotificationResult.Success => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SendSmokeDetectedNotificationResult.Success, 
			RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope.SendSmokeDetectedNotificationResult.Failure => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SendSmokeDetectedNotificationResult.Failure, 
			RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope.SendSmokeDetectedNotificationResult.NotAuthorized => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SendSmokeDetectedNotificationResult.NotAuthorized, 
			_ => throw new ArgumentOutOfRangeException("response"), 
		};
	}

	public static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SendNotificationEmailResult ToShcResult(this RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope.SendNotificationEmailResult response)
	{
		return response switch
		{
			RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope.SendNotificationEmailResult.Success => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SendNotificationEmailResult.Success, 
			RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope.SendNotificationEmailResult.Failure => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SendNotificationEmailResult.Failure, 
			RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope.SendNotificationEmailResult.NotAuthorized => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.SendNotificationEmailResult.NotAuthorized, 
			_ => throw new ArgumentOutOfRangeException("response"), 
		};
	}

	public static EmailTemplates ToBETemplate(this EmailTemplate emailTemplate)
	{
		return emailTemplate switch
		{
			EmailTemplate.ForgotUsername => EmailTemplates.ForgotUsername, 
			EmailTemplate.ChangeEmailAddress => EmailTemplates.ChangeEmailAddress, 
			EmailTemplate.UnblockPin => EmailTemplates.UnblockPin, 
			EmailTemplate.OwnerRemovesUser => EmailTemplates.OwnerRemovesUser, 
			EmailTemplate.OwnerDeletesShcToOwner => EmailTemplates.OwnerDeletesShcToOwner, 
			EmailTemplate.OwnerDeletesShcToUser => EmailTemplates.OwnerDeletesShcToUser, 
			EmailTemplate.OwnerDeletesHisAccountToOwner => EmailTemplates.OwnerDeletesHisAccountToOwner, 
			EmailTemplate.OwnerDeletesHisAccountToUser => EmailTemplates.OwnerDeletesHisAccountToUser, 
			EmailTemplate.UserRemovesShc => EmailTemplates.UserRemovesShc, 
			EmailTemplate.UserDeletesAccount => EmailTemplates.UserDeletesAccount, 
			EmailTemplate.SilverlightLogs => EmailTemplates.SilverlightLogs, 
			EmailTemplate.ShcResetConfirmationToOwner => EmailTemplates.ShcResetConfirmationToOwner, 
			EmailTemplate.ShcResetConfirmationToUser => EmailTemplates.ShcResetConfirmationToUser, 
			EmailTemplate.SmokeDetected => EmailTemplates.SmokeDetected, 
			EmailTemplate.ConfirmationCodeToMaster => EmailTemplates.ConfirmationCodeToMaster, 
			EmailTemplate.ConfirmationCode => EmailTemplates.ConfirmationCode, 
			EmailTemplate.EmailLimitReachedSoon => EmailTemplates.EmailLimitReachedSoon, 
			EmailTemplate.EmailLimitExceeded => EmailTemplates.EmailLimitExceeded, 
			EmailTemplate.EmailLeft => EmailTemplates.EmailLeft, 
			EmailTemplate.LastEmail => EmailTemplates.LastEmail, 
			EmailTemplate.UserEmail => EmailTemplates.UserEmail, 
			EmailTemplate.ApplicationExpirationWarning => EmailTemplates.ApplicationExpirationWarning, 
			EmailTemplate.ApplicationExpired => EmailTemplates.ApplicationExpired, 
			EmailTemplate.ChangeEmailAddressWithNewsletter => EmailTemplates.ChangeEmailAddressWithNewsletter, 
			EmailTemplate.DalCsvExport => EmailTemplates.DalCsvExport, 
			EmailTemplate.SmartMeterCsvExportMonthly => EmailTemplates.SmartMeterCsvExportMonthly, 
			EmailTemplate.SmartMeterCsvExportAdHoc => EmailTemplates.SmartMeterCsvExportAdHoc, 
			EmailTemplate.SccCsvExportMonthly => EmailTemplates.SccCsvExportMonthly, 
			EmailTemplate.SccCsvExportAdHoc => EmailTemplates.SccCsvExportAdHoc, 
			EmailTemplate.ApplicationDowngradeWarning => EmailTemplates.ApplicationDowngradeWarning, 
			EmailTemplate.ApplicationDowngraded => EmailTemplates.ApplicationDowngraded, 
			EmailTemplate.IpvCsvExportMonthly => EmailTemplates.IpvCsvExportMonthly, 
			EmailTemplate.IpvCsvExportAdHoc => EmailTemplates.IpvCsvExportAdHoc, 
			EmailTemplate.VartaCsvExportMonthly => EmailTemplates.VartaCsvExportMonthly, 
			EmailTemplate.VartaCsvExportAdHoc => EmailTemplates.VartaCsvExportAdHoc, 
			EmailTemplate.FastForwardCsvExportMonthly => EmailTemplates.FastForwardCsvExportMonthly, 
			EmailTemplate.FastForwardCsvExportAdHoc => EmailTemplates.FastForwardCsvExportAdHoc, 
			EmailTemplate.ApplicationExpirationWarningExtended => EmailTemplates.ApplicationExpirationWarningExtended, 
			EmailTemplate.ApplicationExpiredExtended => EmailTemplates.ApplicationExpiredExtended, 
			EmailTemplate.EmailNotifRfCommFailure => EmailTemplates.EmailNotifRfCommFailure, 
			EmailTemplate.EmailNotifRfCommFailureResolved => EmailTemplates.EmailNotifRfCommFailureResolved, 
			_ => throw new ArgumentOutOfRangeException("emailTemplate"), 
		};
	}

	public static KeyValuePairOfstringstring[] ToBETemplateParameters(Dictionary<string, string> templateParams)
	{
		return templateParams?.Select((KeyValuePair<string, string> templateParam) => new KeyValuePairOfstringstring
		{
			key = templateParam.Key,
			value = templateParam.Value
		}).ToArray();
	}
}
