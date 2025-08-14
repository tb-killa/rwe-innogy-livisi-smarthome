using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Configuration;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.EventConverters;

public class SaveConfigurationNotificationConverter : IEventConverter
{
	private class data
	{
		public int configVersion;

		public Device[] devices;

		public Capability[] capabilities;

		public SmartHome.Common.API.Entities.Entities.Location[] locations;

		public SmartHome.Common.API.Entities.Entities.Interaction[] interactions;

		public SmartHome.Common.API.Entities.Entities.Home[] homes;

		public SmartHome.Common.API.Entities.Entities.HomeSetup[] homeSetups;

		public SmartHome.Common.API.Entities.Entities.Member[] members;

		public string[] deleted;
	}

	private static ILogger logger = LogManager.Instance.GetLogger(typeof(SaveConfigurationNotificationConverter));

	private static readonly DeviceConverterService deviceConverter = new DeviceConverterService();

	private static readonly CapabilityConverterService capabilityConverter = new CapabilityConverterService(null);

	private static readonly LocationConverterService locationConverter = new LocationConverterService();

	private static readonly InteractionConverterService interactionConverter = new InteractionConverterService();

	private static readonly HomeConverterService homeConverter = new HomeConverterService();

	private static readonly HomeSetupConverterService homeSetupConverter = new HomeSetupConverterService();

	private static readonly MemberConverterService memberConverter = new MemberConverterService();

	public List<Event> FromNotification(BaseNotification notification)
	{
		logger.DebugEnterMethod("FromNotification");
		if (!(notification is SaveConfigurationNotification saveConfigurationNotification))
		{
			return new List<Event>();
		}
		Event obj = new Event();
		obj.Type = "ConfigurationChanged";
		obj.Timestamp = notification.Timestamp;
		obj.Link = string.Format("/desc/device/{0}.{1}/{2}", BuiltinPhysicalDeviceType.SHC.ToString(), "RWE", "1.0");
		obj.Properties = new List<Property>
		{
			new Property
			{
				Name = "ConfigurationVersion",
				Value = saveConfigurationNotification.ConfigurationVersion
			}
		};
		obj.SequenceNumber = notification.SequenceNumber;
		obj.Namespace = notification.Namespace;
		Event obj2 = obj;
		data data = new data();
		data.configVersion = saveConfigurationNotification.ConfigurationVersion;
		data.devices = saveConfigurationNotification.BaseDevices.Select((BaseDevice d) => deviceConverter.FromSmartHomeBaseDevice(d, includeCapabilities: true)).ToArray();
		data.capabilities = saveConfigurationNotification.LogicalDevices.Select((LogicalDevice c) => capabilityConverter.FromSmartHomeLogicalDevice(c)).ToArray();
		data.locations = saveConfigurationNotification.Locations.Select((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Location l) => locationConverter.FromSmartHomeLocation(l)).ToArray();
		data.interactions = saveConfigurationNotification.Interactions.Select((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Interaction i) => interactionConverter.FromSmartHomeInteraction(i)).ToArray();
		data.homes = saveConfigurationNotification.Homes.Select((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Home h) => homeConverter.ToApiEntity(h)).ToArray();
		data.homeSetups = saveConfigurationNotification.HomeSetups.Select((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.HomeSetup hs) => homeSetupConverter.ToApiEntity(hs)).ToArray();
		data.members = saveConfigurationNotification.Members.Select((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Member m) => memberConverter.ToApiEntity(m)).ToArray();
		data.deleted = saveConfigurationNotification.DeletedEntities.ToEntityLinks().ToArray();
		obj2.Data = data;
		logger.DebugExitMethod("FromNotification");
		List<Event> list = new List<Event>(1);
		list.Add(obj2);
		return list;
	}
}
