using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;
using RWE.SmartHome.SHC.DomainModel;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.Exceptions;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

public class ToggleActionHandler : DimmerActionHandler, IDimmerActionHandler
{
	public IEnumerable<string> SupportedActions => new string[1] { "Toggle" };

	public ToggleActionHandler(byte channel)
		: base(channel)
	{
	}

	public SwitchSettings CreateSwitchSettings(ActionDescription action, decimal? currentDimLevel, int technicalMinValue, int technicalMaxValue)
	{
		byte commandTargetValue = GetCommandTargetValue(action, currentDimLevel, technicalMinValue, technicalMaxValue);
		int rampTime = GetRampTime(action, commandTargetValue, currentDimLevel);
		return new SwitchSettingsDirectExecution(RampMode.RampStart, rampTime, base.Channel, commandTargetValue, 0);
	}

	private int GetRampTime(ActionDescription action, byte targetValue, decimal? currentDimLevel)
	{
		if (!currentDimLevel.HasValue)
		{
			throw new NotEnoughDataAvailableException("Current state not available");
		}
		int? num = 0;
		return ((targetValue != 0) ? action.Data.GetIntegerValue("RampOnTime") : action.Data.GetIntegerValue("RampOffTime")) ?? 0;
	}

	private byte GetCommandTargetValue(ActionDescription action, decimal? currentDimLevel, int technicalMin, int technicalMax)
	{
		int num = 0;
		num = GetTargetToggleLevel(action, currentDimLevel);
		return DimmerActionHandler.FromPercent(technicalMin, technicalMax, num);
	}

	private int GetTargetToggleLevel(ActionDescription action, decimal? currentDimLevel)
	{
		if (!currentDimLevel.HasValue)
		{
			throw new NotEnoughDataAvailableException("Current state not available");
		}
		int value = action.Data.GetIntegerValue("DimLevel").Value;
		return ToReverseValue(currentDimLevel.Value, value);
	}

	private static int ToReverseValue(decimal currentDimLevel, int targetDimLevel)
	{
		if (currentDimLevel != 0m)
		{
			return 0;
		}
		return targetDimLevel;
	}
}
