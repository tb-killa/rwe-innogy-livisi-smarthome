using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;
using RWE.SmartHome.SHC.DomainModel;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

internal class RollerShutterActuatorHandler : IActuatorHandlerEntityTypes, IActuatorHandler, ILogicalDeviceHandler
{
	private const byte channel = 1;

	public IEnumerable<Type> SupportedActuatorTypes
	{
		get
		{
			List<Type> list = new List<Type>();
			list.Add(typeof(RollerShutterActuator));
			return list;
		}
	}

	public IEnumerable<byte> StatusInfoChannels => new byte[1] { 1 };

	public bool IsStatusRequestAllowed => true;

	public int MinStatusRequestPollingIterval => 0;

	public LogicalDeviceState CreateLogicalDeviceState(LogicalDevice logicalDevice, SortedList<byte, ChannelState> channelStates)
	{
		GenericDeviceState genericDeviceState = null;
		if (channelStates.ContainsKey(1))
		{
			GenericDeviceState genericDeviceState2 = new GenericDeviceState();
			genericDeviceState2.LogicalDeviceId = logicalDevice.Id;
			genericDeviceState = genericDeviceState2;
			genericDeviceState.Properties.Add(new NumericProperty
			{
				Name = "ShutterLevel",
				Value = ToPercent(channelStates[1]),
				UpdateTimestamp = ShcDateTime.UtcNow
			});
		}
		return genericDeviceState;
	}

	public bool GetIsPeriodicStatusPollingActive(LogicalDevice logicalDevice)
	{
		if (logicalDevice == null)
		{
			throw new ArgumentNullException("logicalDevice");
		}
		if (!(logicalDevice is RollerShutterActuator rollerShutterActuator))
		{
			throw new ArgumentException("Passed logical device must be of type RollerShutterActuator", "logicalDevice");
		}
		return !rollerShutterActuator.IsCalibrating;
	}

	private static int? ToPercent(ChannelState channelState)
	{
		if (channelState.Condition == DeviceCondition.UnknownLevel)
		{
			return null;
		}
		return channelState.Value + 1 >> 1;
	}

	internal static byte FromPercent(int shutterContractLevel)
	{
		if (shutterContractLevel < 0)
		{
			return 0;
		}
		if (shutterContractLevel > 100)
		{
			return 200;
		}
		return (byte)(shutterContractLevel * 2);
	}

	public IEnumerable<SwitchSettings> CreateCosIpCommand(ActionContext ac, ActionDescription action)
	{
		switch (action.ActionType)
		{
		case "SetState":
			return CreateCommandForSetStateAction(action);
		case "StartRamp":
		case "StopRamp":
			return CreateCommandForRampAction(action);
		case "SetStateWithBehavior":
			return CreateCommandForSetStateWithBehavior(action);
		default:
			return new List<SwitchSettings>();
		}
	}

	private SwitchSettings[] CreateCommandForSetStateAction(ActionDescription action)
	{
		return new SwitchSettings[1]
		{
			new SwitchSettingsDirectExecution(RampMode.RampStart, 0, 1, FromPercent(GetShutterLevel(action)), 0)
		};
	}

	private SwitchSettings[] CreateCommandForRampAction(ActionDescription action)
	{
		return new SwitchSettings[1]
		{
			new SwitchSettingsDirectExecution(IsRampStopAction(action) ? RampMode.RampStop : RampMode.RampStart, 0, 1, FromPercent(GetShutterLevel(action)), 0)
		};
	}

	private SwitchSettings[] CreateCommandForSetStateWithBehavior(ActionDescription action)
	{
		int? integerValue = action.Data.GetIntegerValue("StepDriveTime");
		string stringValue = action.Data.GetStringValue("ShutterControlBehavior");
		ValidateActionParams(integerValue, stringValue);
		SwitchSettingsDirectExecution switchSettingsDirectExecution = null;
		switch (stringValue)
		{
		case "Normal":
			switchSettingsDirectExecution = new SwitchSettingsDirectExecution(RampMode.RampStart, 0, 1, FromPercent(GetShutterLevel(action)), 0);
			break;
		case "Inverted":
			switchSettingsDirectExecution = new SwitchSettingsDirectExecution(RampMode.RampStart, integerValue.Value, 1, FromPercent(GetShutterLevel(action)), 0);
			break;
		}
		return new SwitchSettings[1] { switchSettingsDirectExecution };
	}

	private static void ValidateActionParams(int? shortDriveTimeParam, string controlBehaviorParam)
	{
		if (string.IsNullOrEmpty(controlBehaviorParam))
		{
			throw new ArgumentNullException("ControlBehavior param missing from action.");
		}
		if (controlBehaviorParam != "Normal" && controlBehaviorParam != "Inverted")
		{
			throw new ArgumentOutOfRangeException("Unknown Control Behavior pattern specified");
		}
		if (controlBehaviorParam == "Inverted" && !shortDriveTimeParam.HasValue)
		{
			throw new ArgumentNullException("StepDriveTime param must be specified if control behavior is Inverted.");
		}
	}

	private int GetShutterLevel(ActionDescription action)
	{
		if (IsRampAction(action))
		{
			if (IsRampStopAction(action) || !IsRampUp(action))
			{
				return 0;
			}
			return 100;
		}
		int? integerValue = action.Data.GetIntegerValue("ShutterLevel");
		if (integerValue.HasValue)
		{
			return integerValue.Value;
		}
		throw new ArgumentNullException("ShutterLevel was not specified");
	}

	private bool IsRampUp(ActionDescription action)
	{
		string stringValue = action.Data.GetStringValue("RampDirection");
		if (!string.IsNullOrEmpty(stringValue))
		{
			return stringValue == "RampUp";
		}
		throw new ArgumentNullException("RampDirection was not specified");
	}

	public bool CanExecuteAction(ActionDescription action)
	{
		if (action.ActionType == "SetState" || action.ActionType == "SetStateWithBehavior" || IsRampAction(action))
		{
			return true;
		}
		return false;
	}

	private bool IsRampAction(ActionDescription action)
	{
		if (!(action.ActionType == "StartRamp"))
		{
			return action.ActionType == "StopRamp";
		}
		return true;
	}

	private bool IsRampStopAction(ActionDescription action)
	{
		return action.ActionType == "StopRamp";
	}
}
