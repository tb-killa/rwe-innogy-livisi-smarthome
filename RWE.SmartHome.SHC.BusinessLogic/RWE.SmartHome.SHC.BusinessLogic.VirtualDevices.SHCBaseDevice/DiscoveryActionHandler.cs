using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceInclusion;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.DomainModel;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.SHCBaseDevice;

public class DiscoveryActionHandler
{
	private readonly IShcBaseDeviceHandler baseDeviceHandler;

	private readonly IScheduler scheduler;

	private readonly IDiscoveryController discoveryController;

	private Guid? discoveryTimeoutTaskId;

	private readonly TimeSpan discoveryTimeout = TimeSpan.FromMinutes(5.0);

	private readonly object syncRoot = new object();

	public DiscoveryActionHandler(IShcBaseDeviceHandler baseDeviceHandler, IScheduler scheduler, IDiscoveryController discoveryController)
	{
		this.baseDeviceHandler = baseDeviceHandler;
		this.scheduler = scheduler;
		this.discoveryController = discoveryController;
	}

	public ExecutionResult HandleRequest(List<Parameter> data)
	{
		bool? booleanValue = data.GetBooleanValue("DiscoveryActive");
		if (!booleanValue.HasValue)
		{
			Log.Error(Module.BusinessLogic, "SHCActionHandler", "Invalid data for DiscoveryActive property");
			return new ExecutionResult(ExecutionStatus.Failure, new List<Property>());
		}
		lock (syncRoot)
		{
			ProcessDiscoveryRequest(booleanValue.Value);
		}
		return new ExecutionResult(ExecutionStatus.Success, new List<Property>());
	}

	private void ProcessDiscoveryRequest(bool discoveryActive)
	{
		baseDeviceHandler.UpdateDiscoveryActiveProperty(discoveryActive);
		if (discoveryActive)
		{
			StartDiscovery();
			AddTimeoutTask();
		}
		else
		{
			StopDiscovery();
			RemoveTimeoutTask();
		}
	}

	private void StopDiscovery()
	{
		baseDeviceHandler.UpdateDiscoveryActiveProperty(value: false);
		discoveryController.StopDiscovery();
	}

	private void StartDiscovery()
	{
		baseDeviceHandler.UpdateDiscoveryActiveProperty(value: true);
		discoveryController.StartDiscovery(null);
	}

	private void RemoveTimeoutTask()
	{
		if (discoveryTimeoutTaskId.HasValue)
		{
			scheduler.RemoveSchedulerTask(discoveryTimeoutTaskId.Value);
			discoveryTimeoutTaskId = null;
		}
	}

	private void AddTimeoutTask()
	{
		if (discoveryTimeoutTaskId.HasValue)
		{
			scheduler.RemoveSchedulerTask(discoveryTimeoutTaskId.Value);
		}
		discoveryTimeoutTaskId = Guid.NewGuid();
		scheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(discoveryTimeoutTaskId.Value, StopDiscovery, discoveryTimeout, runOnce: true));
	}
}
