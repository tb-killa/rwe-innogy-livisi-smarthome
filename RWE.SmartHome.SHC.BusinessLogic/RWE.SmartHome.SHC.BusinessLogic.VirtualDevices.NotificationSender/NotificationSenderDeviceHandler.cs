using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.PropertiesSets;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.NotificationSender;

public class NotificationSenderDeviceHandler : IVirtualCurrentPhysicalStateHandler
{
	private IRepository configRepo;

	public event EventHandler<VirtualDeviceAvailableArgs> StateChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public NotificationSenderDeviceHandler(IRepository configRepo)
	{
		this.configRepo = configRepo;
	}

	public PhysicalDeviceState GetState(Guid deviceId)
	{
		BaseDevice baseDevice = configRepo.GetBaseDevices().FirstOrDefault((BaseDevice d) => d.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.NotificationSender && d.Id == deviceId);
		if (baseDevice == null)
		{
			return null;
		}
		PhysicalDeviceState physicalDeviceState = new PhysicalDeviceState();
		physicalDeviceState.PhysicalDeviceId = baseDevice.Id;
		PhysicalDeviceState physicalDeviceState2 = physicalDeviceState;
		physicalDeviceState2.DeviceProperties.Properties = new List<Property>
		{
			new StringProperty(PhysicalDeviceBasicProperties.DeviceInclusionState, DeviceInclusionState.Included.ToString())
		};
		return physicalDeviceState2;
	}
}
