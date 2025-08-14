namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class Partner : ConfigurationItem
{
	public DeviceIdentifier Identifier { get; set; }

	public RadioMode? WakeupMode { get; set; }

	public uint? WakeupChannel { get; set; }

	public uint? WakeupInterval { get; set; }

	public uint? WakeupOffset { get; set; }

	public override bool Equals(ConfigurationItem other)
	{
		if (other is Partner partner && base.Id == other.Id && Identifier.Equals(partner.Identifier) && WakeupMode == partner.WakeupMode)
		{
			uint? wakeupChannel = WakeupChannel;
			uint? wakeupChannel2 = partner.WakeupChannel;
			if (wakeupChannel.GetValueOrDefault() == wakeupChannel2.GetValueOrDefault() && wakeupChannel.HasValue == wakeupChannel2.HasValue && WakeupInterval == partner.WakeupInterval)
			{
				return WakeupOffset == partner.WakeupOffset;
			}
		}
		return false;
	}
}
