using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.TaskList;

internal class LinkUpdatePendingPatch
{
	public byte ChannelIndex { get; set; }

	public LinkPartner Partner { get; set; }
}
