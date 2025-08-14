using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

public class SwitchSettingsTestSound : SwitchSettings
{
	public byte[] SourceAddress { get; set; }

	public byte Channel { get; set; }

	public byte SoundId { get; set; }

	public byte CurrentSoundId { get; set; }

	public int Delay { get; set; }

	public SwitchSettingsTestSound()
	{
		base.CommandType = CommandType.VirtualTestSoundCommand;
	}
}
