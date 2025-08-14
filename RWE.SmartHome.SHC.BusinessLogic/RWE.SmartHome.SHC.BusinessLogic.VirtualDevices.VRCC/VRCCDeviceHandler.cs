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
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

public class VRCCDeviceHandler : IVirtualCoreActionHandler
{
	private const string LoggingSource = "VRCCHandler";

	private readonly IRepository configRepo;

	private readonly ILogicalDeviceStateRepository stateRepo;

	private readonly IEventManager eventManager;

	private readonly VRCCDeviceType vrccDeviceType;

	public VRCCDeviceHandler(IRepository configRepository, ILogicalDeviceStateRepository stateRepo, IEventManager eventManager)
	{
		configRepo = configRepository;
		this.stateRepo = stateRepo;
		this.eventManager = eventManager;
		vrccDeviceType = new VRCCDeviceType();
	}

	public void RequestState(Guid deviceId)
	{
	}

	public ExecutionResult ExecuteAction(ActionContext context, ActionDescription action)
	{
		if (action.Target.LinkType != EntityType.LogicalDevice && action.Target.LinkType != EntityType.BaseDevice)
		{
			return ExecutionResult.Error("Invalid target");
		}
		Guid guid = action.Target.EntityIdAsGuid();
		LogicalDevice logicalDevice = configRepo.GetLogicalDevice(guid);
		if (logicalDevice == null || (logicalDevice.BaseDevice != null && logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType() != BuiltinPhysicalDeviceType.VRCC))
		{
			return new ExecutionResult(ExecutionStatus.NotApplicable, new List<Property>());
		}
		List<Property> properties = (from p in action.Data
			select ToCorePropertyValue(p) into p2
			where p2 != null
			select p2).ToList();
		if (!vrccDeviceType.PropertiesAreValidForDeviceType(properties, logicalDevice.DeviceType))
		{
			return ExecutionResult.Error("Invalid Property");
		}
		GenericDeviceState genericDeviceState = new GenericDeviceState();
		genericDeviceState.LogicalDeviceId = guid;
		genericDeviceState.Properties = properties;
		LogicalDeviceState deviceState = genericDeviceState;
		eventManager.GetEvent<RawLogicalDeviceStateChangedEvent>().Publish(new RawLogicalDeviceStateChangedEventArgs(guid, deviceState));
		return ExecutionResult.Success();
	}

	private Property ToCorePropertyValue(Parameter param)
	{
		if (param == null)
		{
			return null;
		}
		if (!(param.Value is ConstantNumericBinding constantNumericBinding))
		{
			return null;
		}
		NumericProperty numericProperty = new NumericProperty();
		numericProperty.Name = param.Name;
		numericProperty.Value = constantNumericBinding.Value;
		NumericProperty numericProperty2 = numericProperty;
		numericProperty2.Value = numericProperty2.FormatPropertyValue();
		return numericProperty2;
	}

	private bool IsTimeTrigger(ActionContext context)
	{
		if (context.Type == ContextType.RuleExecution && context.ReferenceTrigger != null && context.ReferenceTrigger.LinkType == EntityType.LogicalDevice)
		{
			LogicalDevice logicalDevice = configRepo.GetLogicalDevice(context.ReferenceTrigger.EntityIdAsGuid());
			if (logicalDevice != null && logicalDevice.DeviceType == "Calendar")
			{
				return true;
			}
		}
		return false;
	}

	private bool ShouldIgnoreTimeProfileTrigger(ActionContext context, LogicalDevice targetDevice)
	{
		if (!IsTimeTrigger(context))
		{
			return false;
		}
		RoomSetpoint roomSp = targetDevice as RoomSetpoint;
		if (roomSp != null)
		{
			IEnumerable<ThermostatActuator> enumerable = from ta in configRepo.GetLogicalDevices().OfType<ThermostatActuator>()
				where IsInSameRoom(ta, roomSp.LocationId)
				select ta;
			foreach (ThermostatActuator item in enumerable)
			{
				if (stateRepo.GetLogicalDeviceState(item.Id) is GenericDeviceState genericDeviceState)
				{
					string stringValue = genericDeviceState.GetProperties().GetStringValue("OperationMode");
					bool? booleanValue = genericDeviceState.GetProperties().GetBooleanValue("WindowReductionActive");
					if (stringValue == OperationMode.Manu.ToString() || booleanValue == true)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private bool IsInSameRoom(ThermostatActuator ta, Guid? locationId)
	{
		bool result = false;
		if (locationId.HasValue)
		{
			Guid? locationId2 = ta.BaseDevice.LocationId;
			Guid? guid = locationId;
			if (locationId2.HasValue == guid.HasValue)
			{
				if (locationId2.HasValue)
				{
					return locationId2.GetValueOrDefault() == guid.GetValueOrDefault();
				}
				return true;
			}
			return false;
		}
		return result;
	}
}
