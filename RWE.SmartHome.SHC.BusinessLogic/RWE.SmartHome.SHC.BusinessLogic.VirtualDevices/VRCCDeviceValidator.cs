using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices;

public class VRCCDeviceValidator : IConfigurationValidator
{
	public IEnumerable<ErrorEntry> GetConfigurationErrors(IRepository configuration, RepositoryUpdateContextData updateContextData)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		list.AddRange(CheckVRCCInLocation(configuration));
		return list;
	}

	private IEnumerable<ErrorEntry> CheckVRCCInLocation(IRepository configuration)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		Location location;
		foreach (Location location2 in configuration.GetLocations())
		{
			location = location2;
			if (configuration.GetLogicalDevices().Any((LogicalDevice ld) => ld.BaseDevice.LocationId == location.Id && ld.IsVrccCompatibleDevice()) && !configuration.GetBaseDevices().Any((BaseDevice bd) => bd.LocationId == location.Id && bd.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.VRCC))
			{
				list.Add(new ErrorEntry
				{
					ErrorCode = ValidationErrorCode.VRCCDeletion,
					AffectedEntity = new EntityMetadata
					{
						EntityType = EntityType.Location,
						Id = location.Id
					}
				});
			}
		}
		return list;
	}
}
