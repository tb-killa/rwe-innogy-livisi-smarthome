namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StatusReports.Enums;

public enum StatusPartnerInformation
{
	PartnerInfoGetWrongId = 1,
	PartnerInfoSetWrongId = 2,
	PartnerInfoDeleteWrongId = 3,
	PartnerInfoWrongId = 11,
	PartnerInfoFailedToSendToPartner = 12,
	PartnerInfoSetWrongType = 13,
	PartnerInfoGroupInGroupNotAllowed = 14,
	PartnerInfoTooManyPartenersInGroup = 15
}
