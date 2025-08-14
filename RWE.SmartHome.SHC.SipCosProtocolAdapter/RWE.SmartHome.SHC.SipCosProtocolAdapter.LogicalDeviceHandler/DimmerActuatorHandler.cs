using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

internal class DimmerActuatorHandler : IActuatorHandlerEntityTypes, IActuatorHandler, ILogicalDeviceHandler
{
	private const byte channel = 1;

	private readonly IRepository repository;

	private readonly ILogicalDeviceStateRepository logicalDeviceStateRepository;

	private List<IDimmerActionHandler> actionHandlers = new List<IDimmerActionHandler>();

	public IEnumerable<Type> SupportedActuatorTypes
	{
		get
		{
			List<Type> list = new List<Type>();
			list.Add(typeof(DimmerActuator));
			return list;
		}
	}

	public IEnumerable<byte> StatusInfoChannels => new byte[1] { 1 };

	public bool IsStatusRequestAllowed => true;

	public int MinStatusRequestPollingIterval => 0;

	public DimmerActuatorHandler(IRepository repository, ILogicalDeviceStateRepository logicalDeviceStateRepository)
	{
		this.repository = repository;
		this.logicalDeviceStateRepository = logicalDeviceStateRepository;
		actionHandlers.Add(new ToggleActionHandler(1));
		actionHandlers.Add(new SoftSwitchHandler(1));
		actionHandlers.Add(new SetStateHandler(1));
		actionHandlers.Add(new RampActionHandler(1));
	}

	public LogicalDeviceState CreateLogicalDeviceState(LogicalDevice logicalDevice, SortedList<byte, ChannelState> channelStates)
	{
		GenericDeviceState result = null;
		if (logicalDevice is DimmerActuator dimmerActuator && channelStates.ContainsKey(1))
		{
			GenericDeviceState genericDeviceState = new GenericDeviceState();
			genericDeviceState.LogicalDeviceId = logicalDevice.Id;
			genericDeviceState.Properties = new List<Property>
			{
				new NumericProperty
				{
					Name = "DimLevel",
					Value = ToPercent(dimmerActuator.TechnicalMinValue, dimmerActuator.TechnicalMaxValue, channelStates[1].Value),
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

	private static int ToPercent(int minimum, int maximum, int switchLevel)
	{
		if (switchLevel == 0)
		{
			return 0;
		}
		double val = ((double)switchLevel * 0.5 - (double)minimum) / (double)(maximum - minimum) * 99.0 + 1.0;
		double a = Math.Max(1.0, Math.Min(100.0, val));
		return (byte)Math.Round(a);
	}

	public IEnumerable<SwitchSettings> CreateCosIpCommand(ActionContext ac, ActionDescription action)
	{
		decimal? currentDimLevel = GetCurrentDimLevel(action.Target.EntityIdAsGuid());
		SwitchSettings switchSettings = null;
		DimmerActuator dimmerActuator = repository.GetLogicalDevice(action.Target.EntityIdAsGuid()) as DimmerActuator;
		if (dimmerActuator == null)
		{
			throw new ArgumentNullException(dimmerActuator.ToString());
		}
		IDimmerActionHandler dimmerActionHandler = actionHandlers.FirstOrDefault((IDimmerActionHandler ah) => ah.SupportedActions.Contains(action.ActionType));
		if (dimmerActionHandler != null)
		{
			switchSettings = dimmerActionHandler.CreateSwitchSettings(action, currentDimLevel, dimmerActuator.TechnicalMinValue, dimmerActuator.TechnicalMaxValue);
			return new SwitchSettings[1] { switchSettings };
		}
		throw new InvalidOperationException($"Unknown action {action.ActionType}.");
	}

	private decimal? GetCurrentDimLevel(Guid deviceId)
	{
		decimal? result = null;
		LogicalDeviceState logicalDeviceState = logicalDeviceStateRepository.GetLogicalDeviceState(deviceId);
		if (logicalDeviceState != null)
		{
			result = logicalDeviceState.GetProperties().GetDecimalValue("DimLevel").Value;
		}
		return result;
	}

	public bool CanExecuteAction(ActionDescription action)
	{
		return actionHandlers.Any((IDimmerActionHandler ah) => ah.SupportedActions.Contains(action.ActionType));
	}
}
