using System;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.HomeSetupEntity;

public class HomeSetupHandler
{
	private readonly IRepository repository;

	public HomeSetupHandler(IRepository repository)
	{
		this.repository = repository;
	}

	public bool AddHomeSetupIfNotExists()
	{
		if (!repository.GetHomeSetups().Any())
		{
			IncludeHomeSetup();
			return true;
		}
		return false;
	}

	private void IncludeHomeSetup()
	{
		HomeSetup homeSetup = new HomeSetup();
		homeSetup.AppId = CoreConstants.CoreAppId;
		homeSetup.HomeId = Guid.Empty;
		HomeSetup homeSetup2 = homeSetup;
		BaseDevice baseDevice = repository.GetBaseDevices().FirstOrDefault((BaseDevice bd) => bd.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.SHC);
		if (baseDevice != null)
		{
			MovePropertyOrCreateDefault(baseDevice, homeSetup2, "GeoLocation", (string s) => new StringProperty(s, null), "GeoLocation");
			MovePropertyOrCreateDefault(baseDevice, homeSetup2, "PostCode", (string s) => new StringProperty(s, null));
			MovePropertyOrCreateDefault(baseDevice, homeSetup2, "Country", (string s) => new StringProperty(s, null));
			MovePropertyOrCreateDefault(baseDevice, homeSetup2, "HouseholdType", (string s) => new StringProperty(s, null));
			MovePropertyOrCreateDefault(baseDevice, homeSetup2, "NumberOfPersons", (string s) => new NumericProperty
			{
				Name = s,
				Value = null
			});
			MovePropertyOrCreateDefault(baseDevice, homeSetup2, "NumberOfFloors", (string s) => new NumericProperty
			{
				Name = s,
				Value = null
			});
			MovePropertyOrCreateDefault(baseDevice, homeSetup2, "LivingArea", (string s) => new NumericProperty
			{
				Name = s,
				Value = null
			});
			(repository as ConfigurationRepositoryProxy).UpdateBaseDevice(baseDevice);
			Log.Debug(Module.BusinessLogic, "Successfully moved properties from SHCBaseDevice to HomeSetup");
		}
		else
		{
			Log.Error(Module.BusinessLogic, "SHCBaseDevice not found");
		}
		repository.SetHomeSetup(homeSetup2);
	}

	private void MovePropertyOrCreateDefault<T>(BaseDevice from, HomeSetup to, string propertyName, Func<string, T> newProperty) where T : Property, new()
	{
		MovePropertyOrCreateDefault(from, to, propertyName, newProperty, propertyName);
	}

	private void MovePropertyOrCreateDefault<T>(BaseDevice from, HomeSetup to, string propertyName, Func<string, T> newProperty, string originalName) where T : Property
	{
		T item = from.Properties.OfType<T>().FirstOrDefault((T m) => m.Name.Equals(originalName)) ?? newProperty(originalName);
		from.Properties.Remove(item);
		item.Name = propertyName;
		to.Properties.Add(item);
	}
}
