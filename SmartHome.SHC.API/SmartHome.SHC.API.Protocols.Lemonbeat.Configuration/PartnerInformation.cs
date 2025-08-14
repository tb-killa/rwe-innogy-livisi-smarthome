using System;

namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;

public class PartnerInformation
{
	public bool IsShc { get; private set; }

	public Guid InteractionId { get; private set; }

	private PartnerInformation()
	{
	}

	public static PartnerInformation NewShcPartnerInformation()
	{
		PartnerInformation partnerInformation = new PartnerInformation();
		partnerInformation.IsShc = true;
		return partnerInformation;
	}

	public static PartnerInformation NewInteractionPartnerInformation(Guid interactionId)
	{
		PartnerInformation partnerInformation = new PartnerInformation();
		partnerInformation.InteractionId = interactionId;
		return partnerInformation;
	}
}
