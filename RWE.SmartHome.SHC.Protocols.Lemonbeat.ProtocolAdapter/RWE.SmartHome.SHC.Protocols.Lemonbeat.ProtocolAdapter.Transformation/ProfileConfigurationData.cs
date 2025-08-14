using System;
using System.Collections.Generic;
using System.Net;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Transformation;

internal class ProfileConfigurationData
{
	public IPAddress ProfileMulticastAddress { get; set; }

	public List<PartnerCalculationMember> TriggerOnCalculations { get; set; }

	public List<PartnerCalculationMember> TriggerOffCalculations { get; set; }

	public List<Guid> DirectLinkProfileExecuters { get; set; }

	public ProfileConfigurationData(IPAddress profileAddress)
	{
		TriggerOffCalculations = new List<PartnerCalculationMember>();
		TriggerOnCalculations = new List<PartnerCalculationMember>();
		DirectLinkProfileExecuters = new List<Guid>();
		ProfileMulticastAddress = profileAddress;
	}
}
