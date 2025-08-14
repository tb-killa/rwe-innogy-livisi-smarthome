using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceInclusion;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;

namespace RWE.SmartHome.SHC.BusinessLogic.IntegrityManagement;

internal class LogicalDeviceIntegrityHandler : ICoreIntegrityHandler
{
	private IRepository repositoryProxy;

	private ILogicalDeviceInclusionFactory inclusionFactory;

	public LogicalDeviceIntegrityHandler(IRepository repositoryProxy, ILogicalDeviceInclusionFactory inclusionFactory)
	{
		this.repositoryProxy = repositoryProxy;
		this.inclusionFactory = inclusionFactory;
	}

	public void Handle(ConfigEventType eventType, Guid entityId)
	{
		if (eventType == ConfigEventType.DeviceUpdated || eventType == ConfigEventType.DeviceIncluded)
		{
			LogicalDevice logicalDevice = repositoryProxy.GetLogicalDevice(entityId).Clone();
			Guid? locationId = logicalDevice.LocationId;
			if (locationId.HasValue && repositoryProxy.GetLocation(locationId.Value) == null)
			{
				logicalDevice.LocationId = null;
				logicalDevice.Location = null;
				repositoryProxy.SetLogicalDevice(logicalDevice);
			}
		}
		switch (eventType)
		{
		case ConfigEventType.BaseDeviceDeleted:
		{
			List<LogicalDevice> list = (from ld in repositoryProxy.GetLogicalDevices()
				where ld.BaseDeviceId == entityId
				select ld).ToList();
			{
				foreach (LogicalDevice item in list)
				{
					repositoryProxy.DeleteLogicalDevice(item.Id);
				}
				break;
			}
		}
		case ConfigEventType.BaseDeviceIncluded:
		{
			BaseDevice baseDevice = repositoryProxy.GetBaseDevice(entityId);
			IEnumerable<LogicalDevice> enumerable = inclusionFactory.CreateLogicalDevices(baseDevice);
			foreach (LogicalDevice item2 in enumerable)
			{
				item2.BaseDeviceId = baseDevice.Id;
				if (baseDevice.GetBuiltinDeviceDeviceType() != BuiltinPhysicalDeviceType.SHC && baseDevice.GetBuiltinDeviceDeviceType() != BuiltinPhysicalDeviceType.NotificationSender)
				{
					item2.ActivityLogActive = true;
				}
				repositoryProxy.SetLogicalDevice(item2);
				if (!baseDevice.LogicalDeviceIds.Contains(item2.Id))
				{
					baseDevice.LogicalDeviceIds.Add(item2.Id);
				}
			}
			(repositoryProxy as ConfigurationRepositoryProxy).UpdateBaseDevice(baseDevice);
			break;
		}
		}
	}

	public List<ConfigEventType> GetHandledEvents()
	{
		List<ConfigEventType> list = new List<ConfigEventType>();
		list.Add(ConfigEventType.BaseDeviceDeleted);
		list.Add(ConfigEventType.BaseDeviceIncluded);
		list.Add(ConfigEventType.DeviceIncluded);
		list.Add(ConfigEventType.DeviceUpdated);
		return list;
	}
}
