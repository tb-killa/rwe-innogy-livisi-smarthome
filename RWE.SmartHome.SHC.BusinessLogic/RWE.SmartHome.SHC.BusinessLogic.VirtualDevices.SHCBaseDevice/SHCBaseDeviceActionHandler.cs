using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.SHCBaseDevice.ActionExecuters;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceInclusion;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.SHCBaseDevice;

public class SHCBaseDeviceActionHandler : IVirtualCoreActionHandler
{
	private readonly IRepository configRepository;

	private readonly FirmwareUpdateActionExecutor firmwareUpdaterExecutor;

	private readonly ReinclusionActionExecutor reinclusionActionExecutor;

	private readonly TriggerRuleActionExecutor triggerRuleActionExecutor;

	private readonly SoftwareUpdateActionExecutor swuActionExecutor;

	private readonly DiscoveryActionHandler discoveryActionHandler;

	private readonly DeleteAllDevicesActionExecuter deleteAllDevicesActionExecuter;

	private readonly GetLocalAccessStateActionHandler getLocalAccessStateActionHandler;

	public SHCBaseDeviceActionHandler(IProtocolMultiplexer protocolMultiplexer, IEventManager eventManager, IRepository configRepository, IRepositorySync repositorySync, IDeviceFirmwareManager deviceFirmwareManager, ISoftwareUpdateProcessor swuProcessor, IScheduler scheduler, IShcBaseDeviceHandler shcBaseDeviceHandler, IDiscoveryController discoveryController)
	{
		this.configRepository = configRepository;
		firmwareUpdaterExecutor = new FirmwareUpdateActionExecutor(configRepository, deviceFirmwareManager);
		reinclusionActionExecutor = new ReinclusionActionExecutor(protocolMultiplexer, configRepository, repositorySync);
		triggerRuleActionExecutor = new TriggerRuleActionExecutor(eventManager);
		swuActionExecutor = new SoftwareUpdateActionExecutor(swuProcessor);
		discoveryActionHandler = new DiscoveryActionHandler(shcBaseDeviceHandler, scheduler, discoveryController);
		deleteAllDevicesActionExecuter = new DeleteAllDevicesActionExecuter(configRepository, repositorySync);
		getLocalAccessStateActionHandler = new GetLocalAccessStateActionHandler(configRepository);
	}

	public void RequestState(Guid deviceId)
	{
	}

	public ExecutionResult ExecuteAction(ActionContext context, ActionDescription action)
	{
		ExecutionResult executionResult = null;
		if (IsTargetShcBaseDevice(action.Target))
		{
			switch (action.ActionType)
			{
			case "SetState":
				executionResult = discoveryActionHandler.HandleRequest(action.Data);
				break;
			case "UpdateDevice":
				executionResult = firmwareUpdaterExecutor.HandleRequest(action.Data);
				break;
			case "ReincludeDevice":
				executionResult = reinclusionActionExecutor.HandleRequest(action.Data);
				break;
			case "TriggerRule":
				executionResult = triggerRuleActionExecutor.HandleRequest(action.Data);
				break;
			case "CheckForSoftwareUpdate":
			case "TriggerSoftwareUpdate":
				executionResult = swuActionExecutor.HandleRequest(action);
				break;
			case "DeleteAllDevices":
				executionResult = deleteAllDevicesActionExecuter.HandleRequest();
				break;
			case "GetLocalAccessState":
				executionResult = getLocalAccessStateActionHandler.HandleRequest(action);
				break;
			}
		}
		return executionResult ?? new ExecutionResult(ExecutionStatus.NotApplicable, new List<Property>());
	}

	private bool IsTargetShcBaseDevice(LinkBinding target)
	{
		BaseDevice baseDevice = GetBaseDevice(target);
		if (baseDevice != null)
		{
			return ShcBaseDeviceIRepositoryExtensions.ShcBaseDevicesPredicate(baseDevice);
		}
		return false;
	}

	private BaseDevice GetBaseDevice(LinkBinding linkBinding)
	{
		if (linkBinding == null)
		{
			return null;
		}
		switch (linkBinding.LinkType)
		{
		case EntityType.LogicalDevice:
		{
			LogicalDevice logicalDevice = configRepository.GetLogicalDevice(linkBinding.EntityIdAsGuid());
			return logicalDevice.BaseDevice;
		}
		case EntityType.BaseDevice:
			return configRepository.GetBaseDevice(linkBinding.EntityIdAsGuid());
		default:
			return null;
		}
	}
}
