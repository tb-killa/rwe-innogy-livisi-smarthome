using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogic.IntegrityManagement.ClimateControl;
using RWE.SmartHome.SHC.BusinessLogic.VirtualDevices;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;

namespace RWE.SmartHome.SHC.BusinessLogic.IntegrityManagement;

public class ClimateControlIntegrityHandler : ICoreIntegrityHandler
{
	private List<ICCIntegrityHandler> ccIntegrityHandlers = new List<ICCIntegrityHandler>();

	private readonly IRepository repositoryProxy;

	public ClimateControlIntegrityHandler(IRepository repositoryProxy)
	{
		this.repositoryProxy = repositoryProxy;
		ccIntegrityHandlers.Add(new CosIPCCIntegrityHandler(repositoryProxy));
		ccIntegrityHandlers.Add(new VRCCIntegrityHandler(repositoryProxy));
	}

	public bool IsCCPendingUpdate(ConfigEventType eventType, Guid entityId)
	{
		bool result = false;
		switch (eventType)
		{
		case ConfigEventType.DeviceIncluded:
		case ConfigEventType.DeviceUpdated:
		{
			LogicalDevice logicalDevice = repositoryProxy.GetLogicalDevice(entityId);
			result = logicalDevice.IsVrccCompatibleDevice();
			break;
		}
		case ConfigEventType.BaseDeviceDeleted:
			result = repositoryProxy.GetOriginalLogicalDevices().Any((LogicalDevice ld) => ld.BaseDeviceId == entityId && ld.IsVrccCompatibleDevice());
			break;
		case ConfigEventType.BaseDeviceUpdated:
			result = repositoryProxy.GetLogicalDevices().Any((LogicalDevice dev) => dev.BaseDeviceId == entityId && dev.IsVrccCompatibleDevice());
			break;
		}
		return result;
	}

	public void Handle(ConfigEventType eventType, Guid entityId)
	{
		if (IsCCPendingUpdate(eventType, entityId))
		{
			ccIntegrityHandlers.ForEach(delegate(ICCIntegrityHandler handler)
			{
				handler.Handle(eventType, entityId);
			});
		}
	}

	public List<ConfigEventType> GetHandledEvents()
	{
		List<ConfigEventType> list = new List<ConfigEventType>();
		list.Add(ConfigEventType.DeviceUpdated);
		list.Add(ConfigEventType.DeviceIncluded);
		list.Add(ConfigEventType.DeviceDeleted);
		list.Add(ConfigEventType.BaseDeviceUpdated);
		list.Add(ConfigEventType.BaseDeviceDeleted);
		return list;
	}
}
