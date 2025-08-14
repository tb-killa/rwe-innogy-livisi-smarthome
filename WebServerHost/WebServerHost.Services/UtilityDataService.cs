using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;
using SmartHome.Common.API.Entities.Data;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace WebServerHost.Services;

internal class UtilityDataService : IUtilityDataService
{
	private const string EnergyTypeProperty = "energyType";

	private const string PowerTypeProperty = "powerType";

	private ILogger logger = LogManager.Instance.GetLogger<UtilityDataService>();

	private IUtilityDataPersistence peristence;

	private IRepository repository;

	public UtilityDataService(IUtilityDataPersistence peristence, IRepository repository)
	{
		this.peristence = peristence;
		this.repository = repository;
	}

	public void HandleUtilityReadNotification(GenericNotification utilityRead)
	{
		logger.Debug("Handling UtilityRead event");
		UtilityData utilityData = new UtilityData();
		utilityData.EntityId = utilityRead.EntityId;
		utilityData.Data = new List<Property>();
		UtilityData utilityData2 = utilityData;
		string valueAsString = utilityRead.Data.GetValueAsString("meterId");
		DateTime? value = utilityRead.Data.GetValue<DateTime>("timestamp");
		decimal? value2 = utilityRead.Data.GetValue<decimal>("utilityType");
		decimal? value3 = utilityRead.Data.GetValue<decimal>("value");
		if (valueAsString.IsNotEmptyOrNull() && value.HasValue && value2.HasValue && value3.HasValue)
		{
			utilityData2.MeterId = valueAsString;
			utilityData2.Timestamp = value.Value;
			utilityData2.Utility = (UtilityType)(byte)value2.Value;
			utilityData2.Value = (int)value3.Value;
			switch (utilityData2.Utility)
			{
			default:
				return;
			case UtilityType.Energy:
				utilityData2.Data.AddRange(new List<Property>
				{
					new Property
					{
						Name = "energyType",
						Value = utilityRead.Data.GetValueAsString("energyType")
					},
					new Property
					{
						Name = "tt",
						Value = (int)utilityRead.Data.GetValue<decimal>("tariff").Value
					}
				});
				break;
			case UtilityType.Power:
				utilityData2.Data.Add(new Property
				{
					Name = "powerType",
					Value = utilityRead.Data.GetValueAsString("powerType")
				});
				break;
			case UtilityType.Storage:
				utilityData2.Data.Add(new Property
				{
					Name = "p",
					Value = (int)utilityRead.Data.GetValue<decimal>("percentage").Value
				});
				break;
			case UtilityType.Water:
				break;
			}
			peristence.Add(utilityData2);
		}
		else
		{
			logger.Debug("Incomplete info for UtilityRead event. Discarding event");
		}
	}

	public Utility GetEnergyData(string meterId, string energyType, DateTime start, DateTime end, int page, int pageSize)
	{
		List<UtilityData> source = peristence.Get(UtilityType.Energy, meterId, start, end, (page - 1) * pageSize, pageSize, energyType);
		Utility utility = new Utility();
		utility.Type = energyType;
		utility.MeterId = meterId;
		utility.Data = source.Select((UtilityData e) => new UtilityEntry
		{
			value = e.Value,
			timestamp = e.Timestamp,
			tariff = GetTariff(e)
		}).ToList();
		return utility;
	}

	private static int? GetTariff(UtilityData e)
	{
		Property property = e.Data.FirstOrDefault((Property d) => d.Name == "tt");
		if (property == null)
		{
			return null;
		}
		return (int)(decimal)property.Value;
	}

	public Utility GetPowerData(string meterId, string powerType, DateTime start, DateTime end, int page, int pageSize)
	{
		List<UtilityData> source = peristence.Get(UtilityType.Power, meterId, start, end, (page - 1) * pageSize, pageSize, powerType);
		Utility utility = new Utility();
		utility.Type = powerType;
		utility.MeterId = meterId;
		utility.Data = source.Select((UtilityData e) => new UtilityEntry
		{
			value = e.Value,
			timestamp = e.Timestamp
		}).ToList();
		return utility;
	}

	public Utility GetStorageData(string meterId, DateTime start, DateTime end, int page, int pageSize)
	{
		List<UtilityData> source = peristence.Get(UtilityType.Storage, meterId, start, end, (page - 1) * pageSize, pageSize, null);
		Utility utility = new Utility();
		utility.Type = GetStorageType(meterId);
		utility.MeterId = meterId;
		utility.Data = source.Select((UtilityData e) => new UtilityEntry
		{
			value = e.Value,
			timestamp = e.Timestamp,
			percentage = GetPercentage(e)
		}).ToList();
		return utility;
	}

	private string GetStorageType(string meterId)
	{
		string result = null;
		BaseDevice baseDevice = repository.GetBaseDevices().FirstOrDefault((BaseDevice d) => d.SerialNumber.EqualsIgnoreCase(meterId));
		if (baseDevice != null)
		{
			result = baseDevice.DeviceType;
		}
		return result;
	}

	private int? GetPercentage(UtilityData e)
	{
		Property property = e.Data.FirstOrDefault((Property d) => d.Name == "p");
		if (property == null)
		{
			return null;
		}
		return (int)(decimal)property.Value;
	}

	public Survey GetEnergySurvey(string meterId, string energyType)
	{
		return peristence.GetSurvey(UtilityType.Energy, meterId, energyType);
	}

	public Survey GetPowerSurvey(string meterId, string powerType)
	{
		return peristence.GetSurvey(UtilityType.Power, meterId, powerType);
	}

	public Survey GetStorageSurvey(string meterId)
	{
		return peristence.GetSurvey(UtilityType.Storage, meterId, null);
	}

	public void Delete()
	{
		peristence.Delete();
	}

	public void Delete(UtilityType utilityType, string meterId)
	{
		peristence.Delete(utilityType, meterId);
	}
}
