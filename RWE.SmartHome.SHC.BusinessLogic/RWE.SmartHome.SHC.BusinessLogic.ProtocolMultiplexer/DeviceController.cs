using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Control;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.BusinessLogic.ProtocolMultiplexer;

public class DeviceController : IDeviceController
{
	private readonly Dictionary<ProtocolIdentifier, IProtocolSpecificDeviceController> protocolSpecificDeviceControllers = new Dictionary<ProtocolIdentifier, IProtocolSpecificDeviceController>();

	private readonly Dictionary<Guid, bool> profileStates;

	private readonly IRepository configurationRepository;

	private readonly IApplicationsHost applicationsHost;

	private readonly IEventManager eventManager;

	public DeviceController(IEventManager eventManager, IRepository configurationRepository, IApplicationsHost applicationsHost)
	{
		this.eventManager = eventManager;
		this.configurationRepository = configurationRepository;
		this.applicationsHost = applicationsHost;
		profileStates = new Dictionary<Guid, bool>();
	}

	public void RegisterProtocolSpecificDeviceController(ProtocolIdentifier protocolIdentifier, IProtocolSpecificDeviceController deviceController)
	{
		if (deviceController != null)
		{
			protocolSpecificDeviceControllers.Add(protocolIdentifier, deviceController);
		}
	}

	public ExecutionResult ExecuteAction(ActionContext context, ActionDescription action)
	{
		BaseDevice targetBasedevice = ProtocolMultiplexerHelpers.GetTargetBasedevice(configurationRepository, action.Target);
		if (targetBasedevice == null)
		{
			return ExecutionResult.Error($"Invalid ID received: {action.Target.EntityId}");
		}
		if (context.Type == ContextType.ClimateControlSync)
		{
			Log.Debug(Module.BusinessLogic, $"Actuator {action.Target.EntityId} will be switched in ClimateControlSync context");
		}
		else
		{
			Log.Information(Module.BusinessLogic, $"Actuator {action.Target.EntityId} will be switched in context {context.ToString()}");
		}
		ExecutionResult result = null;
		if (protocolSpecificDeviceControllers.TryGetValue(targetBasedevice.ProtocolId, out var value))
		{
			result = value.ExecuteAction(context, action);
			LogDeviceSwitchAction(context, targetBasedevice, result);
		}
		return result;
	}

	public ControlResult TriggerSensorRequest(ActionContext context, Guid sensorId, int buttonId)
	{
		LogicalDevice logicalDevice = configurationRepository.GetLogicalDevice(sensorId);
		if (logicalDevice == null)
		{
			throw new ArgumentException("Invalid sensor ID: " + sensorId);
		}
		ProtocolIdentifier protocolIdentifier = ProtocolMultiplexerHelpers.GetProtocolIdentifier(logicalDevice);
		ControlResult result = ControlResult.NotActive;
		if (protocolSpecificDeviceControllers.TryGetValue(protocolIdentifier, out var value))
		{
			List<Property> list = value.CreateTriggerEvent(logicalDevice, buttonId);
			if (list != null && list.Count > 0)
			{
				eventManager.GetEvent<DeviceEventDetectedEvent>().Publish(new DeviceEventDetectedEventArgs(sensorId, string.Empty, list));
				result = ControlResult.Ok;
			}
		}
		return result;
	}

	private void LogDeviceSwitchAction(ActionContext context, BaseDevice device, ExecutionResult result)
	{
		if (context != null && device != null)
		{
			Log.Debug(Module.BusinessLogic, $"Action triggered on device {device.Name}. {context.ToString()}. {result.ToString()}");
		}
	}
}
