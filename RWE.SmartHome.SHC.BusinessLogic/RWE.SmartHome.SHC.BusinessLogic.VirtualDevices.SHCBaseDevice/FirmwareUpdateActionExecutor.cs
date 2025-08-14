using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.SHCBaseDevice;

internal class FirmwareUpdateActionExecutor
{
	private readonly IDeviceFirmwareManager deviceFirmwareManager;

	private readonly IRepository configRepository;

	public FirmwareUpdateActionExecutor(IRepository configRepository, IDeviceFirmwareManager deviceFirmwareManager)
	{
		this.configRepository = configRepository;
		this.deviceFirmwareManager = deviceFirmwareManager;
	}

	public ExecutionResult HandleRequest(List<Parameter> payload)
	{
		if (payload == null)
		{
			throw new ArgumentNullException("payload");
		}
		List<Property> list = new List<Property>();
		List<Guid> list2 = new List<Guid>();
		List<Parameter> list3 = payload.FindAll((Parameter param) => param.Name == "Device");
		if (!list3.Any())
		{
			list.Add(new StringProperty("WRONG_DEVICE_PARAM", "MISSING_DEVICE"));
		}
		foreach (Parameter item in list3)
		{
			if (!(item.Value is LinkBinding linkBinding))
			{
				list.Add(new StringProperty("WRONG_DEVICE_FORMAT", "LINK_EXPECTED"));
			}
			else if (GetBaseDevice(linkBinding) == null)
			{
				list.Add(new StringProperty("WRONG_DEVICE_ID", linkBinding.EntityId));
			}
			else
			{
				list2.Add(linkBinding.EntityIdAsGuid());
			}
		}
		if (list.Any())
		{
			string[] value = list.Select((Property strParam) => $"{strParam.Name}={strParam.GetValueAsString()}").ToArray();
			Log.Error(Module.BusinessLogic, string.Format("UpdateDevice action error(s)[{0}]", string.Join(", ", value)));
			return new ExecutionResult(ExecutionStatus.Failure, list);
		}
		return UpdateDeviceFirmware(list2);
	}

	private ExecutionResult UpdateDeviceFirmware(List<Guid> deviceIds)
	{
		try
		{
			deviceFirmwareManager.PerformUpdate(deviceIds);
			return new ExecutionResult(ExecutionStatus.Success, new List<Property>());
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, "SHCActionHandler", "error updating device firmware" + ex.Message);
			return new ExecutionResult(ExecutionStatus.Failure, new List<Property>());
		}
	}

	private BaseDevice GetBaseDevice(LinkBinding linkBinding)
	{
		if (linkBinding == null)
		{
			return null;
		}
		EntityType linkType = linkBinding.LinkType;
		if (linkType == EntityType.BaseDevice)
		{
			return configRepository.GetBaseDevice(linkBinding.EntityIdAsGuid());
		}
		return null;
	}
}
