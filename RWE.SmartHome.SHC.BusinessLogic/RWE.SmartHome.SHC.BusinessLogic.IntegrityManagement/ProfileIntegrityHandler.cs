using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogic.DeviceInclusion;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;

namespace RWE.SmartHome.SHC.BusinessLogic.IntegrityManagement;

internal class ProfileIntegrityHandler : ICoreIntegrityHandler
{
	private IRepository repositoryProxy;

	public ProfileIntegrityHandler(IRepository repositoryProxy)
	{
		this.repositoryProxy = repositoryProxy;
	}

	public void Handle(ConfigEventType eventType, Guid entityId)
	{
		switch (eventType)
		{
		case ConfigEventType.BaseDeviceIncluded:
			CreateDefaultInteraction(entityId);
			break;
		case ConfigEventType.DeviceDeleted:
		{
			foreach (Interaction item in from x in repositoryProxy.GetInteractions()
				select x.Clone())
			{
				if (item.RemoveLogicalDevice(entityId))
				{
					item.RemoveEmptyRules();
					if (item.IsEmpty())
					{
						repositoryProxy.DeleteInteraction(item.Id);
					}
					else
					{
						repositoryProxy.SetInteraction(item);
					}
				}
			}
			break;
		}
		}
	}

	private void CreateDefaultInteraction(Guid entityId)
	{
		BaseDevice baseDevice = repositoryProxy.GetBaseDevice(entityId);
		List<LogicalDevice> logicalDevices = (from ld in repositoryProxy.GetLogicalDevices()
			where ld.BaseDeviceId == baseDevice.Id
			select ld).ToList();
		Interaction interaction = InteractionFactory.CreateDefaultInteractionForDevice(baseDevice.GetBuiltinDeviceDeviceType(), logicalDevices);
		if (interaction != null)
		{
			interaction.Name = baseDevice.Name;
			repositoryProxy.SetInteraction(interaction);
		}
	}

	public List<ConfigEventType> GetHandledEvents()
	{
		List<ConfigEventType> list = new List<ConfigEventType>();
		list.Add(ConfigEventType.DeviceDeleted);
		list.Add(ConfigEventType.BaseDeviceIncluded);
		return list;
	}
}
