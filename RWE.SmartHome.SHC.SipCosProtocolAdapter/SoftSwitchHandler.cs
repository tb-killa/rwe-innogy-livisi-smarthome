using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;
using RWE.SmartHome.SHC.DomainModel;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

public class SoftSwitchHandler : DimmerActionHandler, IDimmerActionHandler
{
	public IEnumerable<string> SupportedActions => new string[1] { "SoftSwitchWithOffTimer" };

	public SoftSwitchHandler(byte channel)
		: base(channel)
	{
	}

	public SwitchSettings CreateSwitchSettings(ActionDescription action, decimal? currentDimLevel, int technicalMinValue, int technicalMaxValue)
	{
		byte targetRampLevel = GetTargetRampLevel(action, technicalMinValue, technicalMaxValue);
		int rampTime = GetRampTime(action, targetRampLevel);
		int? commandOffTimer = GetCommandOffTimer(action);
		return new SwitchSettingsDirectExecution(RampMode.RampStart, rampTime, base.Channel, targetRampLevel, commandOffTimer);
	}

	private byte GetTargetRampLevel(ActionDescription action, int technicalMinLevel, int technicalMaxLevel)
	{
		int value = action.Data.GetIntegerValue("DimLevel").Value;
		return DimmerActionHandler.FromPercent(technicalMinLevel, technicalMaxLevel, value);
	}

	private int GetRampTime(ActionDescription action, byte targetValue)
	{
		return ((targetValue == 0) ? action.Data.GetIntegerValue("RampOffTime") : action.Data.GetIntegerValue("RampOnTime")) ?? 0;
	}

	private int? GetCommandOffTimer(ActionDescription action)
	{
		return action.Data.GetIntegerValue("SwitchOffDelayTime");
	}
}
