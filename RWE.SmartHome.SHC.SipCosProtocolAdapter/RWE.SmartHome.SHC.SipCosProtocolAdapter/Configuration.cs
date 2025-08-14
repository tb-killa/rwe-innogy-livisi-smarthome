using System;
using RWE.SmartHome.SHC.Core.Configuration;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter;

public class Configuration : ConfigurationProperties
{
	public DateTime? SequenceCounterReferenceDate => base.ConfigurationSection.GetDate("SequenceCounterReferenceDate");

	public uint? SequenceCounterDailyIncrement => base.ConfigurationSection.GetUInt("SequenceCounterDailyIncrement");

	public int OTAUPackageSendDelay => base.ConfigurationSection.GetInt("OTAUPackageSendDelay") ?? 1000;

	public int OTAUPackageSendDelayEventListeners => base.ConfigurationSection.GetInt("OTAUPackageSendDelayEL").GetValueOrDefault(OTAUPackageSendDelay / 4);

	public Configuration(IConfigurationManager manager)
		: base(manager)
	{
	}
}
