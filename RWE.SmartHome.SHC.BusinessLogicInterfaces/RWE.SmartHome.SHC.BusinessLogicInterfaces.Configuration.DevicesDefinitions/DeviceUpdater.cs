using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;

internal class DeviceUpdater
{
	private IRepository repository;

	private readonly List<BaseDeviceDefinition> baseDeviceDeviceDefinitions = new List<BaseDeviceDefinition>();

	public DeviceUpdater(IRepository repository)
	{
		this.repository = repository;
	}

	private BaseDeviceDefinition FindMatchingDefinition(FirmwareVersion fwVersion)
	{
		return baseDeviceDeviceDefinitions.FirstOrDefault((BaseDeviceDefinition x) => x.MatchFirmware(fwVersion));
	}

	private BaseDevice Clone(BaseDevice oldDevice)
	{
		throw new NotImplementedException();
	}

	public BaseDevice UpdateDevice(BaseDevice oldDevice, FirmwareVersion currentVersion)
	{
		BaseDeviceDefinition baseDeviceDefinition = FindMatchingDefinition(currentVersion);
		if (baseDeviceDefinition == null)
		{
			return oldDevice;
		}
		Clone(oldDevice);
		throw new NotImplementedException();
	}
}
