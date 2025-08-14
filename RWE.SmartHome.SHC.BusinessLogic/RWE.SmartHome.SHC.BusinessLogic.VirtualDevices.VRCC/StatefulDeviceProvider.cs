using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.VRCC;

public class StatefulDeviceProvider
{
	private readonly IRepository configRepo;

	public StatefulDeviceProvider(IRepository cfgRepository)
	{
		configRepo = cfgRepository;
	}

	public IEnumerable<LogicalDevice> GetRelatedHeatingDevices(LogicalDevice logicalDevice, string propertyName)
	{
		LogicalDevice[] array = (from l in configRepo.GetLogicalDevices()
			where l.Properties != null && l.Properties.Any((Property p) => p.Name == propertyName) && l.BaseDevice.LocationId == logicalDevice.BaseDevice.LocationId && l.Id != logicalDevice.Id
			select l).ToArray();
		List<LogicalDevice> list = new List<LogicalDevice>();
		LogicalDevice[] array2 = array.Where((LogicalDevice device) => device.BaseDevice.ProtocolId == ProtocolIdentifier.Cosip).ToArray();
		list.AddRange(GetCosipStatefulDevices(array2));
		LogicalDevice[] collection = array.Except(array2).ToArray();
		if (propertyName != "VRCCSetPoint")
		{
			if (array2.All((LogicalDevice ld) => ld.BaseDevice.GetBuiltinDeviceDeviceType() != BuiltinPhysicalDeviceType.WRT))
			{
				list.AddRange(collection);
			}
		}
		else
		{
			list.AddRange(collection);
		}
		return list;
	}

	private IEnumerable<LogicalDevice> GetCosipStatefulDevices(IEnumerable<LogicalDevice> locationDevices)
	{
		LogicalDevice[] array = locationDevices.ToArray();
		IOrderedEnumerable<LogicalDevice> source = from ld in array
			where ld.BaseDevice.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.WRT
			orderby ld.BaseDevice.TimeOfAcceptance
			select ld;
		if (source.Any())
		{
			if (!(source.FirstOrDefault() is ThermostatActuator))
			{
				return new LogicalDevice[1] { source.FirstOrDefault() };
			}
			return source.ToArray();
		}
		return array;
	}
}
