using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;
using RWE.SmartHome.SHC.DomainModel;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

internal class SwitchActuatorHandler : IActuatorHandlerEntityTypes, IActuatorHandler, ILogicalDeviceHandler
{
	private const byte channel = 1;

	private readonly ILogicalDeviceStateRepository logicalDeviceStateRepository;

	private static string[] supportedToggleActions = new string[1] { "Toggle" };

	private static string[] supportedSwitchOnWithOffTimerActions = new string[1] { "SwitchOnWithOffTimer" };

	public IEnumerable<Type> SupportedActuatorTypes
	{
		get
		{
			List<Type> list = new List<Type>();
			list.Add(typeof(SwitchActuator));
			return list;
		}
	}

	public IEnumerable<byte> StatusInfoChannels => new byte[1] { 1 };

	public bool IsStatusRequestAllowed => true;

	public int MinStatusRequestPollingIterval => 0;

	public SwitchActuatorHandler(ILogicalDeviceStateRepository logicalDeviceStateRepository)
	{
		this.logicalDeviceStateRepository = logicalDeviceStateRepository;
	}

	public LogicalDeviceState CreateLogicalDeviceState(LogicalDevice logicalDevice, SortedList<byte, ChannelState> channelStates)
	{
		GenericDeviceState result = null;
		if (channelStates.ContainsKey(1))
		{
			GenericDeviceState genericDeviceState = new GenericDeviceState();
			genericDeviceState.LogicalDeviceId = logicalDevice.Id;
			genericDeviceState.Properties = new List<Property>
			{
				new BooleanProperty
				{
					Name = "OnState",
					Value = ToBool(channelStates[1].Value),
					UpdateTimestamp = ShcDateTime.UtcNow
				}
			};
			result = genericDeviceState;
		}
		return result;
	}

	public bool GetIsPeriodicStatusPollingActive(LogicalDevice logicalDevice)
	{
		return true;
	}

	public IEnumerable<SwitchSettings> CreateCosIpCommand(ActionContext ac, ActionDescription action)
	{
		try
		{
			int? commandOffTimer = GetCommandOffTimer(action);
			byte commandTargetValue = GetCommandTargetValue(action);
			return new SwitchSettings[1]
			{
				new SwitchSettingsDirectExecution(RampMode.RampStart, 0, 1, commandTargetValue, commandOffTimer)
			};
		}
		catch (ArgumentNullException)
		{
			return new List<SwitchSettings>();
		}
	}

	private int? GetCommandOffTimer(ActionDescription action)
	{
		if (supportedSwitchOnWithOffTimerActions.Contains(action.ActionType))
		{
			return action.Data.GetIntegerValue("SwitchOffDelayTime");
		}
		return null;
	}

	private byte GetCommandTargetValue(ActionDescription action)
	{
		if (supportedToggleActions.Contains(action.ActionType))
		{
			LogicalDeviceState logicalDeviceState = logicalDeviceStateRepository.GetLogicalDeviceState(action.Target.EntityIdAsGuid());
			if (logicalDeviceState != null)
			{
				return GetToggleCommandTargetValue(logicalDeviceState);
			}
			throw new ArgumentNullException("DimLevel");
		}
		if (supportedSwitchOnWithOffTimerActions.Contains(action.ActionType))
		{
			return ToValue(isOn: true);
		}
		return ToValue(action.Data.GetBooleanValue("OnState").Value);
	}

	private byte GetToggleCommandTargetValue(LogicalDeviceState currentState)
	{
		if (currentState.GetProperties().FirstOrDefault((Property prop) => prop.Name == "OnState") is BooleanProperty { Value: not null, Value: var value })
		{
			return ToReverseValue(value.Value);
		}
		throw new ArgumentException("OnState");
	}

	private static byte ToValue(bool isOn)
	{
		return (byte)(isOn ? 200u : 0u);
	}

	private static byte ToReverseValue(bool isOn)
	{
		return (byte)((!isOn) ? 200u : 0u);
	}

	private static bool ToBool(int value)
	{
		return value == 200;
	}

	public bool CanExecuteAction(ActionDescription action)
	{
		if (!(action.ActionType == "SetState") && !(action.ActionType == "SwitchOnWithOffTimer"))
		{
			return action.ActionType == "Toggle";
		}
		return true;
	}
}
