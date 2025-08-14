using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Authentication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.SwitchDelegate;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.DomainModel.Constants;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.Exceptions;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.RuleEngineCommunication;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces.Events;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter;

internal class SipCosDeviceController : IProtocolSpecificDeviceController
{
	private class SwitchOffTimerAction
	{
		public Guid DeviceId => SwitchSettings.DeviceId;

		public int OffTimer => SwitchSettings.OffTimer.GetValueOrDefault();

		public SwitchSettingsConditionalSwitch SwitchSettings { get; private set; }

		public IActuatorHandler Handler { get; private set; }

		public IDeviceController DeviceManager { get; private set; }

		public void Execute()
		{
			List<SwitchSettings> source = Handler.CreateCosIpCommand(null, SwitchSettings.OffAction).ToList();
			SwitchSettingsConditionalSwitch switchSettingsConditionalSwitch = source.FirstOrDefault() as SwitchSettingsConditionalSwitch;
			if (source.Count() == 1 && switchSettingsConditionalSwitch != null)
			{
				DeviceManager.SendSwitchCommand(switchSettingsConditionalSwitch.ActivationTime, switchSettingsConditionalSwitch.SourceAddress, switchSettingsConditionalSwitch.SourceChannel, switchSettingsConditionalSwitch.KeyStrokeCounter, switchSettingsConditionalSwitch.DecisionValue);
			}
		}

		public SwitchOffTimerAction(SwitchSettingsConditionalSwitch switchSettings, IActuatorHandler handler, IDeviceController deviceManager)
		{
			SwitchSettings = switchSettings;
			Handler = handler;
			DeviceManager = deviceManager;
		}
	}

	private class SwitchOffTimersList
	{
		private readonly Dictionary<Guid, Timer> switchOffTimers = new Dictionary<Guid, Timer>();

		private readonly object switchOffTimersSync = new object();

		public void AddSwitchOff(SwitchOffTimerAction switchOffSettings)
		{
			Guid deviceId = switchOffSettings.DeviceId;
			lock (switchOffTimersSync)
			{
				RemoveTimer(deviceId);
				switchOffTimers.Add(deviceId, new Timer(SwitchOffTimerAction, switchOffSettings, switchOffSettings.OffTimer * 100, -1));
			}
		}

		private void RemoveTimer(Guid id)
		{
			if (switchOffTimers.ContainsKey(id))
			{
				switchOffTimers[id].Dispose();
				switchOffTimers.Remove(id);
			}
		}

		private void SwitchOffTimerAction(object arg)
		{
			if (!(arg is SwitchOffTimerAction switchOffTimerAction))
			{
				Log.Error(Module.DeviceManager, "SwitchOffTimer called with incorrect parameters");
				return;
			}
			switchOffTimerAction.Execute();
			lock (switchOffTimersSync)
			{
				RemoveTimer(switchOffTimerAction.DeviceId);
			}
		}
	}

	private readonly IRepository configurationRepository;

	private readonly IEventManager eventManager;

	private readonly IDeviceManager deviceManager;

	private readonly IUserManager userManager;

	private readonly ITriggerCapableDeviceHandlerCollection triggerCapableDeviceHandlerCollection;

	private readonly LogicalDeviceHandlerCollection logicalDeviceHandlerCollection;

	private readonly Dictionary<LinkEndpoint, byte> lastLongPressPerLinkEndpoint;

	private readonly SwitchOffTimersList switchOffTimers = new SwitchOffTimersList();

	public SipCosDeviceController(IRepository configurationRepository, IEventManager eventManager, IDeviceManager deviceManager, IUserManager userManager, LogicalDeviceHandlerCollection logicalDeviceHandlerCollection, ITriggerCapableDeviceHandlerCollection triggerCapableDeviceHandlerCollection)
	{
		this.logicalDeviceHandlerCollection = logicalDeviceHandlerCollection;
		this.triggerCapableDeviceHandlerCollection = triggerCapableDeviceHandlerCollection;
		this.configurationRepository = configurationRepository;
		this.eventManager = eventManager;
		this.deviceManager = deviceManager;
		this.userManager = userManager;
		lastLongPressPerLinkEndpoint = new Dictionary<LinkEndpoint, byte>();
		eventManager.GetEvent<SipCosSwitchCommandEvent>().Subscribe(SipCosSwitchCommandHandler, null, ThreadOption.BackgroundThread, null);
	}

	public List<Property> CreateTriggerEvent(LogicalDevice logicalDevice, int buttonId)
	{
		List<Property> result = null;
		BuiltinPhysicalDeviceType builtinDeviceDeviceType = logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType();
		ITriggerCapableDeviceHandler deviceHandler = triggerCapableDeviceHandlerCollection.GetDeviceHandler(builtinDeviceDeviceType);
		if (deviceHandler != null)
		{
			result = deviceHandler.GetUITriggerProperties(builtinDeviceDeviceType, buttonId);
		}
		return result;
	}

	private void NotifySipCosDeviceEventSwitchCommand(SipCosSwitchCommandEventArgs eventArgs)
	{
		BaseDevice device = GetPhysicalDeviceFromAddress(eventArgs.DeviceAddress);
		if (device == null)
		{
			return;
		}
		ITriggerCapableDeviceHandler deviceHandler = triggerCapableDeviceHandlerCollection.GetDeviceHandler(device.GetBuiltinDeviceDeviceType());
		if (deviceHandler == null)
		{
			return;
		}
		Guid guid = Guid.Empty;
		List<Property> list = null;
		string eventType = string.Empty;
		List<LogicalDevice> list2 = (from s in configurationRepository.GetLogicalDevices()
			where s.BaseDevice != null && s.BaseDevice.Id == device.Id
			select s).ToList();
		if (list2.Count > 0)
		{
			guid = deviceHandler.GetEventSourceId(list2, eventArgs);
			if (guid != Guid.Empty)
			{
				list = deviceHandler.GetTriggerProperties(device.GetBuiltinDeviceDeviceType(), eventArgs);
				eventType = list.GetStringValue(EventConstants.EventTypePropertyName);
			}
		}
		if (guid != Guid.Empty && list != null && list.Count > 0)
		{
			eventManager.GetEvent<DeviceEventDetectedEvent>().Publish(new DeviceEventDetectedEventArgs(guid, eventType, list));
		}
	}

	private BaseDevice GetPhysicalDeviceFromAddress(byte[] address)
	{
		IDeviceInformation deviceInformation = deviceManager.DeviceList[address];
		if (deviceInformation == null)
		{
			return null;
		}
		return configurationRepository.GetBaseDevice(deviceInformation.DeviceId);
	}

	private void SipCosSwitchCommandHandler(SipCosSwitchCommandEventArgs eventArgs)
	{
		if (!IsLongPressAlreadyHandled(eventArgs))
		{
			new LinkEndpoint(eventArgs.DeviceAddress, eventArgs.KeyChannelNumber);
			NotifySipCosDeviceEventSwitchCommand(eventArgs);
		}
	}

	private bool IsLongPressAlreadyHandled(SipCosSwitchCommandEventArgs eventArgs)
	{
		LinkEndpoint key = new LinkEndpoint(eventArgs.DeviceAddress, eventArgs.KeyChannelNumber);
		if (eventArgs.IsLongPress)
		{
			if (lastLongPressPerLinkEndpoint.TryGetValue(key, out var value) && value == eventArgs.KeyStrokeCounter)
			{
				return true;
			}
			lastLongPressPerLinkEndpoint[key] = eventArgs.KeyStrokeCounter;
		}
		else
		{
			lastLongPressPerLinkEndpoint.Remove(key);
		}
		return false;
	}

	private static bool CalculateProfileState(ProfileAction action, byte keyPressCounter)
	{
		return action switch
		{
			ProfileAction.On => true, 
			ProfileAction.Off => false, 
			ProfileAction.Toggle => (keyPressCounter & 1) != 0, 
			_ => throw new ArgumentOutOfRangeException("action", "No action must be performed!"), 
		};
	}

	public ExecutionResult ExecuteAction(ActionContext context, ActionDescription action)
	{
		if (action.Target.LinkType != EntityType.LogicalDevice)
		{
			Log.Debug(Module.SipCosProtocolAdapter, "SipCosProtocolAdapter does not support " + action.Target.LinkType.ToString() + " action type");
			return ExecutionResult.Error("Unsupported link type");
		}
		LogicalDevice logicalDevice = configurationRepository.GetLogicalDevice(action.Target.EntityIdAsGuid());
		if (logicalDevice == null || !logicalDeviceHandlerCollection.TryGetHandler(logicalDevice, out var logicalDeviceHandler))
		{
			return ExecutionResult.Error("Unknown device error");
		}
		Guid baseDeviceId = logicalDevice.BaseDeviceId;
		if (!(logicalDeviceHandler is IActuatorHandler actuatorHandler))
		{
			return ExecutionResult.Error("Unknown device error.");
		}
		if (!actuatorHandler.CanExecuteAction(action))
		{
			return ExecutionResult.Error("The device cannot execute the specified action.");
		}
		try
		{
			IEnumerable<SwitchSettings> enumerable = actuatorHandler.CreateCosIpCommand(context, action);
			foreach (SwitchSettings item in enumerable)
			{
				if (item.DeviceId == Guid.Empty)
				{
					item.DeviceId = baseDeviceId;
				}
				IDeviceController deviceController = deviceManager[item.DeviceId];
				if (deviceController != null)
				{
					switch (item.CommandType)
					{
					case CommandType.DirectExecution:
					{
						SwitchSettingsDirectExecution switchSettingsDirectExecution = (SwitchSettingsDirectExecution)item;
						deviceController.ChangeDeviceState(switchSettingsDirectExecution.RampMode, switchSettingsDirectExecution.RampTime, switchSettingsDirectExecution.Value, switchSettingsDirectExecution.Channel, switchSettingsDirectExecution.OffTimer);
						break;
					}
					case CommandType.ConditionalSwitchCommand:
					{
						SwitchSettingsConditionalSwitch switchSettingsConditionalSwitch = (SwitchSettingsConditionalSwitch)item;
						deviceController.SendSwitchCommand(switchSettingsConditionalSwitch.ActivationTime, switchSettingsConditionalSwitch.SourceAddress, switchSettingsConditionalSwitch.SourceChannel, switchSettingsConditionalSwitch.KeyStrokeCounter, switchSettingsConditionalSwitch.DecisionValue);
						if (switchSettingsConditionalSwitch.OffTimer.HasValue && switchSettingsConditionalSwitch.OffTimer.Value > 0)
						{
							switchOffTimers.AddSwitchOff(new SwitchOffTimerAction(switchSettingsConditionalSwitch, actuatorHandler, deviceController));
						}
						if (actuatorHandler is AlarmActuatorHandler && (context.Type == ContextType.ClientRequest || context.Type == ContextType.RuleExecution))
						{
							eventManager.GetEvent<SmokeDetectorStateChangeTriggeredEvent>().Publish(new SmokeDetectorStateChangeTriggeredEventArgs(logicalDevice.BaseDevice.Id));
						}
						break;
					}
					case CommandType.UnconditionalSwitchCommand:
					{
						SwitchSettingsUnconditionalSwitch switchSettingsUnconditionalSwitch = (SwitchSettingsUnconditionalSwitch)item;
						deviceController.SendSwitchCommand(switchSettingsUnconditionalSwitch.ActivationTime, switchSettingsUnconditionalSwitch.SourceAddress, switchSettingsUnconditionalSwitch.SourceChannel, switchSettingsUnconditionalSwitch.KeyStrokeCounter, null);
						break;
					}
					case CommandType.VirtualTestSoundCommand:
					{
						SwitchSettingsTestSound switchSettingsTestSound = (SwitchSettingsTestSound)item;
						deviceController.SendTestSound(switchSettingsTestSound.SourceAddress, switchSettingsTestSound.Channel, switchSettingsTestSound.SoundId, switchSettingsTestSound.CurrentSoundId, switchSettingsTestSound.Delay);
						break;
					}
					case CommandType.CustomAction:
						(item as CustomSwitchSettings).Execute();
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
				else
				{
					Log.Error(Module.SipCosProtocolAdapter, $"Device {deviceManager.DeviceList.LogInfoByGuid(baseDeviceId)} is unknown");
				}
			}
		}
		catch (NotEnoughDataAvailableException)
		{
			if (IsDeviceUnreachable(baseDeviceId))
			{
				return new ExecutionResult(ExecutionStatus.Success, new List<Property>());
			}
		}
		catch (ExecuteActionException ex2)
		{
			return new ExecutionResult(ExecutionStatus.Failure, ex2.Properties);
		}
		return new ExecutionResult(ExecutionStatus.Success, new List<Property>());
	}

	private bool IsDeviceUnreachable(Guid deviceId)
	{
		IDeviceList deviceList = deviceManager.DeviceList;
		if (deviceList != null && deviceList.Contains(deviceId))
		{
			return deviceList[deviceId].DeviceUnreachable;
		}
		return false;
	}
}
