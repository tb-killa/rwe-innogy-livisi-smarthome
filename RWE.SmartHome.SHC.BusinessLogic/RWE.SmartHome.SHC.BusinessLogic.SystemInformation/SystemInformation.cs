using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogic.Persistence;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.Core.Authentication;
using RWE.SmartHome.SHC.Core.Authentication.Entities;
using RWE.SmartHome.SHC.DataAccessInterfaces.Messages;
using RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;

namespace RWE.SmartHome.SHC.BusinessLogic.SystemInformation;

public class SystemInformation
{
	public SHCInformation SHCInformation { get; set; }

	public List<ProtocolSpecificInformation> ProtocolSpecificInformation { get; set; }

	public RWE.SmartHome.Common.ControlNodeSHCContracts.API.Configuration UIConfiguration { get; set; }

	public MessagesAndAlertsContainer MessagesAndAlerts { get; set; }

	public List<User> Users { get; set; }

	public static SystemInformation Create(IRepository configurationRepository, IMessagePersistence messagePersistence, IUserManager userManager, SHCInformation shcInformation, IProtocolSpecificDataPersistence deviceListPersistence, IProtocolMultiplexer protocolMultiplexer)
	{
		SystemInformation systemInformation = new SystemInformation();
		systemInformation.SHCInformation = shcInformation;
		systemInformation.UIConfiguration = new RWE.SmartHome.Common.ControlNodeSHCContracts.API.Configuration
		{
			Locations = configurationRepository.GetLocations(),
			LogicalDevices = configurationRepository.GetLogicalDevices(),
			BaseDevices = configurationRepository.GetBaseDevices(),
			Interactions = configurationRepository.GetInteractions(),
			HomeSetups = GetObfuscatedHomeSetups(configurationRepository)
		};
		systemInformation.MessagesAndAlerts = new MessagesAndAlertsContainer
		{
			MessageInfos = messagePersistence.CreateBackup().ToList()
		};
		systemInformation.Users = HidePasswords(userManager.Users.ToList());
		systemInformation.ProtocolSpecificInformation = protocolMultiplexer.GetProtocolSpecificInformation();
		return systemInformation;
	}

	private static List<User> HidePasswords(IEnumerable<User> users)
	{
		List<User> list = users.ToList();
		list.ForEach(delegate(User user)
		{
			user.Password = "*****";
		});
		return list;
	}

	private static List<HomeSetup> GetObfuscatedHomeSetups(IRepository configurationRepository)
	{
		List<HomeSetup> homeSetups = configurationRepository.GetHomeSetups();
		if (homeSetups != null)
		{
			List<HomeSetup> list = homeSetups.Select((HomeSetup m) => m.Clone()).ToList();
			{
				foreach (HomeSetup item in list)
				{
					ObfusacteHomeSetup(item);
				}
				return list;
			}
		}
		return null;
	}

	private static void ObfusacteHomeSetup(HomeSetup homeSetup)
	{
		if (homeSetup != null && homeSetup.Properties != null)
		{
			if (homeSetup.Properties.Any((Property m) => m.Name == "GeoLocation"))
			{
				homeSetup.Properties.SetString("GeoLocation", "0.00,0.00");
			}
			if (homeSetup.Properties.Any((Property m) => m.Name == "PostCode"))
			{
				homeSetup.Properties.SetString("PostCode", "00000");
			}
		}
	}
}
