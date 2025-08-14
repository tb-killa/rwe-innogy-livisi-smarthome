using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface IPartnerInformationService
{
	PartnerInformations GetAllPartnersAndGroups(DeviceIdentifier identifier);

	void SetAndDeletePartners(DeviceIdentifier identifier, PartnerInformations partnerInformationsToSet, IEnumerable<uint> partnersToDelete);
}
