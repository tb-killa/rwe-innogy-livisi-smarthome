using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.TaskList;

internal abstract class ConfigurationChange
{
	protected readonly byte Channel;

	protected readonly LinkPartner Partner;

	protected ConfigurationChange(byte channel, LinkPartner partner)
	{
		Channel = channel;
		Partner = partner;
	}

	public abstract void ApplyTo(DeviceConfiguration config);

	public abstract string ToString(string addressStr);
}
