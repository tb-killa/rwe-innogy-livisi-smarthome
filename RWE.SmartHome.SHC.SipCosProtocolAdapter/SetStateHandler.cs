using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;
using RWE.SmartHome.SHC.DomainModel;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

public class SetStateHandler : DimmerActionHandler, IDimmerActionHandler
{
	public IEnumerable<string> SupportedActions => new string[1] { "SetState" };

	public SetStateHandler(byte channel)
		: base(channel)
	{
	}

	public SwitchSettings CreateSwitchSettings(ActionDescription action, decimal? currentDimLevel, int technicalMinValue, int technicalMaxValue)
	{
		byte targetRampLevel = GetTargetRampLevel(action, technicalMinValue, technicalMaxValue);
		return new SwitchSettingsDirectExecution(RampMode.RampStart, 0, base.Channel, targetRampLevel, 0);
	}

	private byte GetTargetRampLevel(ActionDescription action, int technicalMinLevel, int technicalMaxLevel)
	{
		int value = action.Data.GetIntegerValue("DimLevel").Value;
		return DimmerActionHandler.FromPercent(technicalMinLevel, technicalMaxLevel, value);
	}
}
