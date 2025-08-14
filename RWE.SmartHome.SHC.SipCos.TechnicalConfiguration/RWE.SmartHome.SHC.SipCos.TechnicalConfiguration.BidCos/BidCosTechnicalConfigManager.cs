using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DataAccessInterfaces.TechnicalConfiguration;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos;

public class BidCosTechnicalConfigManager
{
	private readonly ITechnicalConfigurationPersistence persistence;

	private readonly IDeviceManager deviceManager;

	public BidCosTechnicalConfigManager(ITechnicalConfigurationPersistence persistence, IDeviceManager deviceManager)
	{
		this.persistence = persistence;
		this.deviceManager = deviceManager;
	}

	public BidCosConfiguration GetPersitedConfig(IEnumerable<Guid> deviceIds)
	{
		List<Guid> ids = deviceIds.ToList();
		List<TechnicalConfigurationEntity> entities = (from m in persistence.LoadAll()
			where m != null && ids.Contains(m.Id)
			select m).ToList();
		return new BidCosConfiguration(entities);
	}

	public void TryIncludeDeviceIfNotIncluded(Guid deviceId, IDeviceInformation deviceInfo)
	{
		if (DeviceInclusionState.Included != deviceInfo.DeviceInclusionState && DeviceInclusionState.FactoryReset != deviceInfo.DeviceInclusionState)
		{
			deviceManager.IncludeDevice(deviceId);
		}
	}

	public void SaveConfiguration(BidCosConfiguration config)
	{
		using DatabaseConnection databaseConnection = persistence.OpenDatabaseTransaction();
		try
		{
			persistence.SaveAll(config.GetTechEntities());
			databaseConnection.CommitTransaction();
		}
		catch (Exception ex)
		{
			Log.Exception(Module.SerialCommunication, ex, "Save BidCos configuration");
			databaseConnection.RollbackTransaction();
		}
	}
}
