using System;
using SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Transformation;

internal class PartnerCalculationMember
{
	internal Guid PartnerBaseDeviceId { get; set; }

	internal PartnerCalculation PartnerCalculation { get; set; }

	internal PartnerCalculationMember(Guid partnerBaseDeviceId, PartnerCalculation partnerCalculation)
	{
		PartnerCalculation = partnerCalculation;
		PartnerBaseDeviceId = partnerBaseDeviceId;
	}
}
