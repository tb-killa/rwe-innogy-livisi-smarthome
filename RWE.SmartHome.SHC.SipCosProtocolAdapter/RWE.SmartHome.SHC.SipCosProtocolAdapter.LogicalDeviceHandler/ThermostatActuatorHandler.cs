using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DomainModel;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler.ThermostatHandler;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

internal class ThermostatActuatorHandler : IActuatorHandlerEntityTypes, IActuatorHandler, ILogicalDeviceHandler
{
	private const string OP_MODE_PROP_NAME = "OperationMode";

	private const string POINT_TEMP_PROP_NAME = "PointTemperature";

	private readonly TemperatureActuatorStateCache stateCache = new TemperatureActuatorStateCache();

	private readonly PhysicalStateConverter stateConverter;

	private readonly IRepository cfgRepo;

	private readonly ActionConverter actionConverter;

	private readonly IEventManager eventManager;

	public IEnumerable<Type> SupportedActuatorTypes
	{
		get
		{
			List<Type> list = new List<Type>();
			list.Add(typeof(ThermostatActuator));
			return list;
		}
	}

	public IEnumerable<byte> StatusInfoChannels => new byte[3] { 2, 5, 4 };

	public bool IsStatusRequestAllowed => true;

	public int MinStatusRequestPollingIterval => 0;

	private void OnConfigurationChanged(ConfigurationProcessedEventArgs args)
	{
		foreach (EntityMetadata item in args.RepositoryDeletionReport.Where((EntityMetadata entity) => entity.EntityType == EntityType.LogicalDevice))
		{
			stateCache.RemoveDeviceState(item.Id);
		}
	}

	public ThermostatActuatorHandler(IRepository cfgRepo, IEventManager eventManager)
	{
		this.cfgRepo = cfgRepo;
		stateConverter = new PhysicalStateConverter(stateCache);
		actionConverter = new ActionConverter(cfgRepo, stateCache);
		this.eventManager = eventManager;
		eventManager.GetEvent<ConfigurationProcessedEvent>().Subscribe(OnConfigurationChanged, null, ThreadOption.PublisherThread, null);
	}

	public LogicalDeviceState CreateLogicalDeviceState(LogicalDevice logicalDevice, SortedList<byte, ChannelState> channelStates)
	{
		LogicalDeviceState result = null;
		if (logicalDevice is ThermostatActuator thermostat)
		{
			result = stateConverter.ConvertPhysicalState(thermostat, channelStates);
		}
		return result;
	}

	public bool GetIsPeriodicStatusPollingActive(LogicalDevice logicalDevice)
	{
		if (logicalDevice != null && logicalDevice.BaseDevice != null)
		{
			return logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.FSC8;
		}
		return false;
	}

	public bool CanExecuteAction(ActionDescription action)
	{
		string actionType;
		if ((actionType = action.ActionType) != null && actionType == "SetState")
		{
			return true;
		}
		return false;
	}

	public IEnumerable<SwitchSettings> CreateCosIpCommand(ActionContext ac, ActionDescription action)
	{
		IEnumerable<SwitchSettings> result = new List<SwitchSettings>();
		if (action.Target.LinkType == EntityType.LogicalDevice)
		{
			if (cfgRepo.GetLogicalDevice(action.Target.EntityIdAsGuid()) is ThermostatActuator thermostat)
			{
				result = actionConverter.CreateSwitchSettings(ac, thermostat, GetTargetOperationMode(action), GetTargetSetPoint(action));
			}
			else
			{
				Log.Error(Module.SipCosProtocolAdapter, "Passed target device is not a ThermostatActuator.");
			}
		}
		else
		{
			Log.Error(Module.SipCosProtocolAdapter, "ThermostatActuatorHandler cannot handle passed action.");
		}
		return result;
	}

	private decimal? GetTargetSetPoint(ActionDescription action)
	{
		return action.Data.GetNumericValue("PointTemperature");
	}

	private OperationMode? GetTargetOperationMode(ActionDescription action)
	{
		OperationMode? result = null;
		string stringValue = action.Data.GetStringValue("OperationMode");
		if (!string.IsNullOrEmpty(stringValue))
		{
			result = (OperationMode)Enum.Parse(typeof(OperationMode), stringValue, ignoreCase: true);
		}
		return result;
	}
}
