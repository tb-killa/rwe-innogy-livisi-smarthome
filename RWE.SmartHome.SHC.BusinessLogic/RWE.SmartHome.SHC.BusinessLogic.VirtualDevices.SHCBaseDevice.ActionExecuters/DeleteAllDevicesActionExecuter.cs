using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations.Enums;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.SHCBaseDevice.ActionExecuters;

internal class DeleteAllDevicesActionExecuter
{
	private readonly IRepository configRepository;

	private readonly IRepositorySync repositorySync;

	public DeleteAllDevicesActionExecuter(IRepository configRepository, IRepositorySync repositorySync)
	{
		this.configRepository = configRepository;
		this.repositorySync = repositorySync;
	}

	public ExecutionResult HandleRequest()
	{
		bool flag = false;
		RepositoryUpdateContextData updateContextData = new RepositoryUpdateContextData(CoreConstants.CoreAppId, RestorePointCreationOptions.No, ForcePushDeviceConfiguration.Yes, SkipConfigurationValidation.Yes);
		ExecutionResult result;
		using (RepositoryLockContext repositoryLockContext = repositorySync.GetLockAsyncRelease("DeleteAllDevicesAction", updateContextData))
		{
			try
			{
				flag = true;
				List<Guid> deviceIds = (from x in configRepository.GetBaseDevices()
					where x.DeviceType != BuiltinPhysicalDeviceType.NotificationSender.ToString() && x.DeviceType != BuiltinPhysicalDeviceType.PresenceDevice.ToString() && x.DeviceType != BuiltinPhysicalDeviceType.SHC.ToString()
					select x.Id).ToList();
				DeleteDevices(deviceIds);
				repositoryLockContext.Commit = true;
				result = new ExecutionResult(ExecutionStatus.Success, new List<Property>());
			}
			catch (Exception ex)
			{
				Log.Exception(Module.ExternalCommandDispatcher, ex, "DeleteAllDevicesAction request handling failed");
				result = new ExecutionResult(ExecutionStatus.Failure, new List<Property>());
			}
		}
		if (!flag)
		{
			result = ExecutionResult.Error("Configuration locked.");
			Log.Error(Module.BusinessLogic, "DeleteAllDevicesActionExecuter", "Configuration locked.");
		}
		return result;
	}

	private void DeleteDevices(List<Guid> deviceIds)
	{
		foreach (Guid deviceId in deviceIds)
		{
			configRepository.DeleteBaseDevice(deviceId);
		}
	}
}
