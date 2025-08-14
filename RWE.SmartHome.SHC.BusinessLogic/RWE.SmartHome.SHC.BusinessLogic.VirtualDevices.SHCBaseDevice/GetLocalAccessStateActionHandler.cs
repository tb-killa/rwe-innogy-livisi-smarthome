using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.SHCBaseDevice;

internal class GetLocalAccessStateActionHandler
{
	private readonly IRepository configRepository;

	public GetLocalAccessStateActionHandler(IRepository configRepository)
	{
		this.configRepository = configRepository;
	}

	public ExecutionResult HandleRequest(ActionDescription action)
	{
		List<Property> list = new List<Property>();
		try
		{
			BaseDevice baseDevice = configRepository.GetBaseDevice(new Guid(action.Target.EntityId));
			if (baseDevice == null)
			{
				throw new ArgumentNullException($"The device with id: {action.Target.EntityId} does not exist or the provided device id is incorrect");
			}
			list.Add(new StringProperty("localAccessState", GetLocalAccessState()));
			return new ExecutionResult(ExecutionStatus.Success, list);
		}
		catch (Exception ex)
		{
			Log.Exception(Module.ExternalCommandDispatcher, ex, string.Format("{0} request handling failed", "GetLocalAccessState"));
			return new ExecutionResult(ExecutionStatus.Failure, new List<Property>());
		}
	}

	private static string GetLocalAccessState()
	{
		if (FilePersistence.LocalAccessEnabled)
		{
			return "localEnabled";
		}
		return "localDisabled";
	}
}
