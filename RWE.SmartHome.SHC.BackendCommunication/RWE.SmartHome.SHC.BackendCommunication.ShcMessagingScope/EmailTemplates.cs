using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;

[Serializable]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/11/08/Common")]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
public enum EmailTemplates
{
	ForgotUsername,
	ChangeEmailAddress,
	UnblockPin,
	OwnerRemovesUser,
	OwnerDeletesShcToOwner,
	OwnerDeletesShcToUser,
	OwnerDeletesHisAccountToOwner,
	OwnerDeletesHisAccountToUser,
	UserRemovesShc,
	UserDeletesAccount,
	SilverlightLogs,
	ShcResetConfirmationToOwner,
	ShcResetConfirmationToUser,
	SmokeDetected,
	ConfirmationCodeToMaster,
	ConfirmationCode,
	EmailLimitReachedSoon,
	EmailLimitExceeded,
	EmailLeft,
	LastEmail,
	UserEmail,
	ApplicationExpirationWarning,
	ApplicationExpired,
	ChangeEmailAddressWithNewsletter,
	DalCsvExport,
	SmartMeterCsvExportMonthly,
	SmartMeterCsvExportAdHoc,
	SccCsvExportMonthly,
	SccCsvExportAdHoc,
	ApplicationDowngradeWarning,
	ApplicationDowngraded,
	IpvCsvExportMonthly,
	IpvCsvExportAdHoc,
	VartaCsvExportMonthly,
	VartaCsvExportAdHoc,
	FastForwardCsvExportMonthly,
	FastForwardCsvExportAdHoc,
	ApplicationExpirationWarningExtended,
	ApplicationExpiredExtended,
	EmailNotifRfCommFailure,
	EmailNotifRfCommFailureResolved,
	EmailNotifBackendConnectionLost,
	EmailNotifBackendConnectionLostResolved,
	EmailConfirmation,
	FriendCreation,
	ResetPassword,
	Empty,
	EmailConfirmationWithNewsletter,
	EmailNotifDeviceReportingFailure,
	MigrationReport,
	UserDeletesAccountNoShc,
	WaterCsvExportAdHoc,
	WaterCsvExportMonthly,
	EasyOptimizeCsvExportMonthly,
	EasyOptimizeCsvExportAdHoc,
	SMACsvExportMonthly,
	SMACsvExportAdHoc
}
