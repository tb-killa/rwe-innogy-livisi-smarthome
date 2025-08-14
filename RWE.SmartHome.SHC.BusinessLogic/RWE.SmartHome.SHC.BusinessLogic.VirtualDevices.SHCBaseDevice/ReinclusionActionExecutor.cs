using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.PropertiesSets;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations.Enums;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.SHCBaseDevice;

internal class ReinclusionActionExecutor
{
	private struct ValidationResult
	{
		public ExecutionResult executionResult;

		public BaseDevice targetedDevice;
	}

	private const string PhysicalStateDevicePropertyName = "DeviceId";

	private readonly IProtocolMultiplexer protocolMultiplexer;

	private readonly IRepository configRepository;

	private readonly IRepositorySync repositorySync;

	public ReinclusionActionExecutor(IProtocolMultiplexer protocolMultiplexer, IRepository configRepository, IRepositorySync repositorySync)
	{
		this.protocolMultiplexer = protocolMultiplexer;
		this.configRepository = configRepository;
		this.repositorySync = repositorySync;
	}

	public ExecutionResult HandleRequest(List<Parameter> payload)
	{
		bool flag = false;
		ExecutionResult result;
		using (RepositoryLockContext repositoryLockContext = repositorySync.GetLockAsyncRelease("ReinclusionAction", new RepositoryUpdateContextData(CoreConstants.CoreAppId, RestorePointCreationOptions.No, ForcePushDeviceConfiguration.Yes)))
		{
			flag = true;
			ValidationResult validationResult = ValidateAction(payload);
			result = ((validationResult.executionResult.Status == ExecutionStatus.Success) ? PerformReinclusion(validationResult.targetedDevice) : validationResult.executionResult);
			repositoryLockContext.Commit = true;
		}
		if (!flag)
		{
			result = ExecutionResult.Error("Configuration locked.");
			Log.Error(Module.BusinessLogic, "ReinclusionActionExecutor", "Configuration locked.");
		}
		return result;
	}

	private bool IsDeviceInFactoryResetState(BaseDevice device)
	{
		PhysicalDeviceState physicalDeviceState = protocolMultiplexer.PhysicalState.Get(device.Id);
		if (physicalDeviceState == null || physicalDeviceState.DeviceProperties.GetValue<DeviceInclusionState>(PhysicalDeviceBasicProperties.DeviceInclusionState) != DeviceInclusionState.FactoryReset)
		{
			return false;
		}
		return true;
	}

	private ValidationResult ValidateAction(List<Parameter> payload)
	{
		if (payload == null)
		{
			throw new ArgumentNullException("payload");
		}
		BaseDevice baseDeviceToReinclude = GetBaseDeviceToReinclude(payload);
		ExecutionResult executionResult;
		if (baseDeviceToReinclude != null)
		{
			executionResult = ((!IsDeviceInFactoryResetState(baseDeviceToReinclude)) ? ExecutionResult.Error("Device was not in factory reset state and therefore can not be reincluded") : ExecutionResult.Success());
		}
		else
		{
			executionResult = ExecutionResult.Error("Device to be reincluded was not found in the configuration repo. Wrong Id?");
			Log.Error(Module.BusinessLogic, "ReinclusionActionExecutor", "Device to be reincluded was not found in the configuration repo. Wrong Id?");
		}
		return new ValidationResult
		{
			executionResult = executionResult,
			targetedDevice = baseDeviceToReinclude
		};
	}

	private ExecutionResult PerformReinclusion(BaseDevice baseDevice)
	{
		protocolMultiplexer.ResetDeviceInclusionState(baseDevice.Id);
		return new ExecutionResult(ExecutionStatus.Success, new List<Property>());
	}

	private BaseDevice GetBaseDeviceToReinclude(List<Parameter> payload)
	{
		Parameter parameter = payload.FirstOrDefault((Parameter x) => x.Name == "DeviceId");
		if (parameter == null)
		{
			return null;
		}
		if (!(parameter.Value is ConstantStringBinding { Value: not null } constantStringBinding))
		{
			return null;
		}
		return configRepository.GetBaseDevice(new Guid(constantStringBinding.Value));
	}
}
