using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

public class SwitchSettingsUnconditionalSwitch : SwitchSettings
{
	public ActivationTime ActivationTime { get; set; }

	public byte[] SourceAddress { get; set; }

	public byte SourceChannel { get; set; }

	public byte KeyStrokeCounter { get; set; }

	public SwitchSettingsUnconditionalSwitch(ActivationTime activationTime, byte[] sourceAddress, byte channel, byte keyStrokeCounter)
	{
		ActivationTime = activationTime;
		SourceAddress = sourceAddress;
		SourceChannel = channel;
		KeyStrokeCounter = keyStrokeCounter;
		base.CommandType = CommandType.UnconditionalSwitchCommand;
	}
}
