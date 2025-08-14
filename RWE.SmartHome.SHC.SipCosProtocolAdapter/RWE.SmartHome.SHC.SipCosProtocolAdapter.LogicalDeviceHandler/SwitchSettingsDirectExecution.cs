using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

internal class SwitchSettingsDirectExecution : SwitchSettings
{
	public RampMode RampMode { get; set; }

	public int RampTime { get; set; }

	public byte Channel { get; set; }

	public byte Value { get; set; }

	public int OffTimer { get; set; }

	public SwitchSettingsDirectExecution(RampMode rampMode, int rampTime, byte channel, byte value, int? offTimer)
	{
		RampMode = rampMode;
		RampTime = rampTime;
		Channel = channel;
		Value = value;
		OffTimer = offTimer ?? 0;
		base.CommandType = CommandType.DirectExecution;
	}
}
