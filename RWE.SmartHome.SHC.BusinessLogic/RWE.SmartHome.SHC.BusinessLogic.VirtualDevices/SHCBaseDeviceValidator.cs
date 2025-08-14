using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices;

public class SHCBaseDeviceValidator : IConfigurationValidator
{
	private const double latitudeMinValue = -90.0;

	private const double latitudeMaxValue = 90.0;

	private const double longitudeMinValue = -180.0;

	private const double longitudeMaxValue = 180.0;

	private readonly IShcBaseDeviceHandler shcDeviceHandler;

	public SHCBaseDeviceValidator(IConfigurationProcessor configProcessor, IShcBaseDeviceHandler shcBaseDeviceHandler)
	{
		configProcessor?.RegisterConfigurationValidator(this);
		shcDeviceHandler = shcBaseDeviceHandler;
	}

	public IEnumerable<ErrorEntry> GetConfigurationErrors(IRepository configuration, RepositoryUpdateContextData updateContextData)
	{
		List<ErrorEntry> list = new List<ErrorEntry>();
		if (!shcDeviceHandler.DeviceId.HasValue)
		{
			return list;
		}
		List<BaseDevice> list2 = configuration.GetBaseDevices().Where(ShcBaseDeviceIRepositoryExtensions.ShcBaseDevicesPredicate).ToList();
		if (list2.Count == 0)
		{
			BuildErrorEntry(Guid.Empty);
		}
		else if (list2.Count > 1)
		{
			foreach (BaseDevice item in list2)
			{
				if (item.Id != shcDeviceHandler.DeviceId.Value)
				{
					list.Add(BuildErrorEntry(item.Id));
				}
			}
		}
		return list;
	}

	private ErrorEntry BuildErrorEntry(Guid baseDeviceId)
	{
		ErrorEntry errorEntry = new ErrorEntry();
		errorEntry.AffectedEntity = new EntityMetadata
		{
			EntityType = EntityType.BaseDevice,
			Id = baseDeviceId
		};
		errorEntry.ErrorCode = ValidationErrorCode.InvalidSHCBaseDevice;
		return errorEntry;
	}
}
