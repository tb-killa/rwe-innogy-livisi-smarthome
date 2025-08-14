using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

public class SwitchSettingsConditionalSwitch : SwitchSettingsUnconditionalSwitch
{
	public byte DecisionValue { get; set; }

	public int? OffTimer { get; set; }

	public ActionDescription OffAction { get; set; }

	public SwitchSettingsConditionalSwitch(ActivationTime activationTime, byte[] sourceAddress, byte channel, byte keyStrokeCounter, byte decisionValue)
		: base(activationTime, sourceAddress, channel, keyStrokeCounter)
	{
		DecisionValue = decisionValue;
		base.CommandType = CommandType.ConditionalSwitchCommand;
	}
}
