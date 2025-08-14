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

internal class AlarmActuatorHandler : IActuatorHandlerEntityTypes, IActuatorHandler, ILogicalDeviceHandler
{
	private const byte smokeChannel = 1;

	private const int AlarmValue = 200;

	private const int AlarmOffValue = 1;

	private readonly IDeviceManager deviceManager;

	private byte keyPressCounter;

	public IEnumerable<Type> SupportedActuatorTypes
	{
		get
		{
			List<Type> list = new List<Type>();
			list.Add(typeof(AlarmActuator));
			return list;
		}
	}

	public IEnumerable<byte> StatusInfoChannels => new byte[1] { 1 };

	public bool IsStatusRequestAllowed => true;

	public int MinStatusRequestPollingIterval => 86400;

	public AlarmActuatorHandler(IDeviceManager deviceManager)
	{
		this.deviceManager = deviceManager;
	}

	public bool GetIsPeriodicStatusPollingActive(LogicalDevice logicalDevice)
	{
		return true;
	}

	public LogicalDeviceState CreateLogicalDeviceState(LogicalDevice logicalDevice, SortedList<byte, ChannelState> channelStates)
	{
		GenericDeviceState genericDeviceState = null;
		if (logicalDevice is AlarmActuator && channelStates.ContainsKey(1))
		{
			GenericDeviceState genericDeviceState2 = new GenericDeviceState();
			genericDeviceState2.LogicalDeviceId = logicalDevice.Id;
			genericDeviceState = genericDeviceState2;
			genericDeviceState.Properties.Add(new BooleanProperty
			{
				Name = "OnState",
				Value = (channelStates[1].Value == 200),
				UpdateTimestamp = ShcDateTime.UtcNow
			});
		}
		return genericDeviceState;
	}

	public IEnumerable<SwitchSettings> CreateCosIpCommand(ActionContext ac, ActionDescription action)
	{
		if (action.ActionType == "SetState")
		{
			bool? booleanValue = action.Data.GetBooleanValue("OnState");
			return CreateSettings(action, booleanValue, null);
		}
		if (action.ActionType == "SwitchOnWithOffTimer")
		{
			bool? isOn = true;
			int? integerValue = action.Data.GetIntegerValue("SwitchOffDelayTime");
			return CreateSettings(action, isOn, integerValue);
		}
		return null;
	}

	private IEnumerable<SwitchSettings> CreateSettings(ActionDescription originalAction, bool? isOn, int? durationSeconds)
	{
		List<SwitchSettings> list = new List<SwitchSettings>();
		if (isOn.HasValue)
		{
			SwitchSettingsConditionalSwitch switchSettingsConditionalSwitch = new SwitchSettingsConditionalSwitch(ActivationTime.ShortPress, deviceManager.DefaultShcAddress, 1, ++keyPressCounter, (byte)((!isOn.Value) ? 1u : 200u));
			if (isOn.Value && durationSeconds.HasValue && durationSeconds.Value > 0)
			{
				switchSettingsConditionalSwitch.OffTimer = durationSeconds;
				switchSettingsConditionalSwitch.OffAction = new ActionDescription
				{
					ActionType = "SetState",
					Id = Guid.NewGuid(),
					Tags = originalAction.Tags,
					Target = originalAction.Target,
					Version = originalAction.Version,
					Data = new List<Parameter>
					{
						new Parameter
						{
							Name = "OnState",
							Value = new ConstantBooleanBinding
							{
								Value = false
							}
						}
					}
				};
			}
			list.Add(switchSettingsConditionalSwitch);
		}
		return list;
	}

	public bool CanExecuteAction(ActionDescription action)
	{
		if (!(action.ActionType == "SetState"))
		{
			return action.ActionType == "SwitchOnWithOffTimer";
		}
		return true;
	}
}
