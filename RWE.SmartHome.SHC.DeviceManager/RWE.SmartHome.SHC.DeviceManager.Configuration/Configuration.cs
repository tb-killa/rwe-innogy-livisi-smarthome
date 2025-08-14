using RWE.SmartHome.SHC.Core.Configuration;

namespace RWE.SmartHome.SHC.DeviceManager.Configuration;

public class Configuration : ConfigurationProperties
{
	public long? StatisticsObservationPeriod => base.ConfigurationSection.GetLong("StatisticsObservationPeriod");

	public bool? UseDefaultSyncWord => base.ConfigurationSection.GetBool("UseDefaultSyncWord");

	public int? RouterWaitForAppAckTime => base.ConfigurationSection.GetInt("SendScheduler.RouterWaitForAppAckTime");

	public int? MaxPendingAcks => base.ConfigurationSection.GetInt("SendScheduler.MaxPendingAcks");

	public Configuration(IConfigurationManager manager)
		: base(manager)
	{
	}
}
