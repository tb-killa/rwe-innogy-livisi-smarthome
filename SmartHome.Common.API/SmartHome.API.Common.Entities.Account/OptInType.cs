using System;

namespace SmartHome.API.Common.Entities.Account;

[Serializable]
public enum OptInType
{
	PartnerDataAccess,
	EmailAdvertisement,
	TelephoneAdvertisement
}
