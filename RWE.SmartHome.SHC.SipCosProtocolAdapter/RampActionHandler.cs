using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;
using RWE.SmartHome.SHC.DomainModel;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.Exceptions;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

public class RampActionHandler : DimmerActionHandler, IDimmerActionHandler
{
	private enum RampDirection
	{
		RampUp = 1,
		RampDown
	}

	private struct RampActionParams
	{
		public RampDirection RampDirection { get; set; }

		public int RampTime { get; set; }
	}

	private const int DEFAULT_RAMP_TIME = 5000;

	public IEnumerable<string> SupportedActions => new string[2] { "StartRamp", "StopRamp" };

	public RampActionHandler(byte channel)
		: base(channel)
	{
	}

	public SwitchSettings CreateSwitchSettings(ActionDescription action, decimal? currentDimLevel, int technicalMinValue, int technicalMaxValue)
	{
		SwitchSettingsDirectExecution switchSettingsDirectExecution = null;
		if (IsRampStopAction(action))
		{
			return new SwitchSettingsDirectExecution(RampMode.RampStop, 0, base.Channel, 0, null);
		}
		byte targetRampLevel = GetTargetRampLevel(action, technicalMinValue, technicalMaxValue);
		int commandRampTime = GetCommandRampTime(action, currentDimLevel);
		return new SwitchSettingsDirectExecution(RampMode.RampStart, commandRampTime, base.Channel, targetRampLevel, 0);
	}

	private int GetCommandRampTime(ActionDescription action, decimal? currentDimLevel)
	{
		if (!currentDimLevel.HasValue)
		{
			throw new NotEnoughDataAvailableException("Current state not available");
		}
		RampActionParams rampParams = GetRampParams(action);
		decimal? num;
		if (rampParams.RampDirection != RampDirection.RampUp)
		{
			num = currentDimLevel;
		}
		else
		{
			decimal? num2 = currentDimLevel;
			num = (decimal?)100m - num2;
		}
		int num3 = (int)(num * (decimal?)rampParams.RampTime / (decimal?)100m).Value;
		return num3 / 100;
	}

	private byte GetTargetRampLevel(ActionDescription action, int technicalMinLevel, int technicalMaxLevel)
	{
		int dimLevel = ((action.Data.GetStringValue("RampDirection") == "RampUp") ? 100 : 0);
		return DimmerActionHandler.FromPercent(technicalMinLevel, technicalMaxLevel, dimLevel);
	}

	private RampActionParams GetRampParams(ActionDescription rampAction)
	{
		string stringValue = rampAction.Data.GetStringValue("RampDirection");
		if (string.IsNullOrEmpty(stringValue))
		{
			throw new ArgumentNullException("RampDirection param missing");
		}
		int? integerValue = rampAction.Data.GetIntegerValue("RampTime");
		RampDirection rampDirection = ((stringValue == "RampUp") ? RampDirection.RampUp : RampDirection.RampDown);
		return new RampActionParams
		{
			RampDirection = rampDirection,
			RampTime = (integerValue.HasValue ? integerValue.Value : 5000)
		};
	}

	private bool IsRampStopAction(ActionDescription action)
	{
		return action.ActionType == "StopRamp";
	}
}
