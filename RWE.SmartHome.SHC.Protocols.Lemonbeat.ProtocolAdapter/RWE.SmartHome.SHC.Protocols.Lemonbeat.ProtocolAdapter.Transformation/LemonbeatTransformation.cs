using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.CoreApiConverters;
using RWE.SmartHome.SHC.DomainModel.Rules;
using RWE.SmartHome.SHC.Lemonbeat.ProtocolAdapter.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Transformation;

internal class LemonbeatTransformation : BaseProtocolSpecificTransformation
{
	private readonly IDeviceList deviceList;

	private readonly IConfigurationController configurationController;

	private readonly IApplicationsHost appHost;

	private readonly ILemonbeatPersistence LemonbeatPersistence;

	private readonly IShcValueRepository shcValueRepository;

	private readonly ILemonbeatCommunication LemonbeatAggregator;

	private List<IPAddress> shcMulticastGroups;

	private IDictionary<Guid, PhysicalConfiguration> physicalConfigurations;

	private List<Guid> devicesToInclude;

	private List<Guid> rulesRequiringLinksToShc;

	private List<Guid> timeProfilesOnShc = new List<Guid>();

	public override ProtocolIdentifier ProtocolId => ProtocolIdentifier.Lemonbeat;

	public LemonbeatTransformation(IDeviceList deviceList, IConfigurationController configurationController, IApplicationsHost appHost, ILemonbeatPersistence LemonbeatPersistence, IShcValueRepository shcValueRepository, IRepository configRepository, ILemonbeatCommunication aggregator)
		: base(configRepository)
	{
		this.deviceList = deviceList;
		this.configurationController = configurationController;
		this.appHost = appHost;
		this.LemonbeatPersistence = LemonbeatPersistence;
		this.shcValueRepository = shcValueRepository;
		LemonbeatAggregator = aggregator;
	}

	public override bool PrepareTransformation(IElementaryRuleRepository elementaryRuleRepository)
	{
		timeProfilesOnShc.Clear();
		rulesRequiringLinksToShc = new List<Guid>();
		shcValueRepository.BeginUpdate();
		ProcessElementaryRules(elementaryRuleRepository);
		Dictionary<Guid, TransformationResult> dictionary = new Dictionary<Guid, TransformationResult>();
		new List<global::SmartHome.SHC.API.Configuration.Trigger>();
		new List<global::SmartHome.SHC.API.Configuration.ActionDescription>();
		List<BaseDevice> source = (from bd in configRepository.GetBaseDevices()
			where bd.ProtocolId == ProtocolId
			select bd).ToList();
		Dictionary<Guid, ProfileConfigurationData> dictionary2 = BuildProfileConfigurationData(dictionary);
		PhysicalConfigurationBuilder physicalConfigurationBuilder = new PhysicalConfigurationBuilder(shcValueRepository, deviceList, dictionary2);
		physicalConfigurations = new Dictionary<Guid, PhysicalConfiguration>();
		devicesToInclude = new List<Guid>();
		foreach (Guid item in source.Select((BaseDevice bd) => bd.Id))
		{
			DeviceInformation deviceInformation = deviceList[item];
			if (deviceInformation != null && deviceInformation.DeviceInclusionState != LemonbeatDeviceInclusionState.Included)
			{
				devicesToInclude.Add(item);
			}
			if (dictionary.ContainsKey(item))
			{
				physicalConfigurations.Add(item, physicalConfigurationBuilder.BuildConfiguration(item, dictionary[item]));
			}
		}
		ValidateConfiguration();
		shcMulticastGroups = (from pcd in dictionary2.Values
			where pcd.DirectLinkProfileExecuters.Contains(PhysicalConfigurationBuilder.SHC_BASE_DEVICE_ID)
			select pcd.ProfileMulticastAddress).ToList();
		return base.Errors.Count > 0;
	}

	protected override bool AccelerateRule(ElementaryRule rule)
	{
		return false;
	}

	protected override void CreateLinkWithShc(Rule rule, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger trigger)
	{
		if (!rulesRequiringLinksToShc.Contains(rule.Id))
		{
			rulesRequiringLinksToShc.Add(rule.Id);
		}
	}

	public override void CommitTransformationResults(IEnumerable<Guid> devicesToDelete)
	{
		List<Guid> list = devicesToDelete.Where((Guid d) => deviceList.SyncSelect((DeviceInformation dev) => dev.DeviceId).Contains(d)).ToList();
		list.ForEach(delegate(Guid deviceId)
		{
			if (physicalConfigurations.ContainsKey(deviceId))
			{
				physicalConfigurations.Remove(deviceId);
			}
		});
		configurationController.ConfigureDevices(list, devicesToInclude, physicalConfigurations);
		shcValueRepository.CommitChanges();
		CleanupTransformationResults();
		try
		{
			if (LemonbeatAggregator != null)
			{
				LemonbeatAggregator.SetMulticastSubscriptions(shcMulticastGroups.Select((IPAddress ip) => new DeviceIdentifier(ip, 0u, 1)).ToList());
			}
			else
			{
				Log.Warning(Module.LemonbeatProtocolAdapter, "Null multicast listener. The SHC will not receive multicast profile messages.");
			}
			shcMulticastGroups = null;
		}
		catch (Exception ex)
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, "Failed to join multicast groups: " + ex);
		}
	}

	public override void DiscardTransformationResults()
	{
		base.DiscardTransformationResults();
		shcValueRepository.RollbackChanges();
	}

	private List<DayOfWeek> WeekdaysToDayOfWeekList(WeekDay weekDay)
	{
		List<DayOfWeek> list = new List<DayOfWeek>();
		if (weekDay != 0)
		{
			if ((int)(weekDay & WeekDay.Monday) > 0)
			{
				list.Add(DayOfWeek.Monday);
			}
			if ((int)(weekDay & WeekDay.Tuesday) > 0)
			{
				list.Add(DayOfWeek.Tuesday);
			}
			if ((int)(weekDay & WeekDay.Wednesday) > 0)
			{
				list.Add(DayOfWeek.Wednesday);
			}
			if ((int)(weekDay & WeekDay.Thursday) > 0)
			{
				list.Add(DayOfWeek.Thursday);
			}
			if ((int)(weekDay & WeekDay.Friday) > 0)
			{
				list.Add(DayOfWeek.Friday);
			}
			if ((int)(weekDay & WeekDay.Saturday) > 0)
			{
				list.Add(DayOfWeek.Saturday);
			}
			if ((int)(weekDay & WeekDay.Sunday) > 0)
			{
				list.Add(DayOfWeek.Sunday);
			}
		}
		return list;
	}

	internal TargetLogicalConfiguration GetTargetLogicalConfiguration(BaseDevice baseDevice)
	{
		TargetLogicalConfiguration targetLogicalConfiguration = new TargetLogicalConfiguration();
		IEnumerable<LogicalDevice> source = from ld in configRepository.GetLogicalDevices()
			where baseDevice.Id == ld.BaseDeviceId
			select ld;
		IEnumerable<Guid> logicalDeviceIds = source.Select((LogicalDevice ld) => ld.Id);
		List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.ActionDescription> list = new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.ActionDescription>();
		List<KeyValuePair<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger, InteractionDetails>> list2 = new List<KeyValuePair<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger, InteractionDetails>>();
		Interaction interaction;
		foreach (Interaction interaction2 in configRepository.GetInteractions())
		{
			interaction = interaction2;
			list2.AddRange(from t in interaction.Rules.Where((Rule r) => r.Triggers != null).SelectMany((Rule r) => r.Triggers)
				where logicalDeviceIds.Contains(t.Entity.EntityIdAsGuid())
				select new KeyValuePair<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger, InteractionDetails>(t, new InteractionDetails(interaction.Id, interaction.Name)));
			list.AddRange(from action in interaction.Rules.SelectMany((Rule r) => r.Actions)
				where action.Target.LinkType == EntityType.LogicalDevice && logicalDeviceIds.Contains(action.Target.EntityIdAsGuid())
				select action);
		}
		targetLogicalConfiguration.TimeInteractionSetpoints = new Dictionary<Guid, List<TimeInteractionSetPoint>>();
		targetLogicalConfiguration.Device = baseDevice.ToApiBaseDevice();
		targetLogicalConfiguration.Capabilities = source.Select((LogicalDevice ld) => ld.ToApiCapability());
		targetLogicalConfiguration.ActionDescriptions = list.ConvertAll((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.ActionDescription a) => a.ToApi());
		targetLogicalConfiguration.Triggers = list2.Select((KeyValuePair<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.Trigger, InteractionDetails> t) => t.Key.ToApiInteractionTrigger(t.Value));
		return targetLogicalConfiguration;
	}

	private Dictionary<Guid, ProfileConfigurationData> BuildProfileConfigurationData(IEnumerable<KeyValuePair<Guid, TransformationResult>> tranformationResults)
	{
		ProfileMulticastAddressProvider profileMulticastAddressProvider = new ProfileMulticastAddressProvider(deviceList, LemonbeatPersistence, configRepository);
		Dictionary<Guid, ProfileConfigurationData> dictionary = new Dictionary<Guid, ProfileConfigurationData>();
		foreach (Guid item in rulesRequiringLinksToShc)
		{
			dictionary.Add(item, new ProfileConfigurationData(profileMulticastAddressProvider.GetProfileMulticastAddress(item)));
			dictionary[item].DirectLinkProfileExecuters.Add(PhysicalConfigurationBuilder.SHC_BASE_DEVICE_ID);
		}
		return dictionary;
	}

	private void ValidateConfiguration()
	{
		foreach (KeyValuePair<Guid, PhysicalConfiguration> physicalConfiguration in physicalConfigurations)
		{
			DeviceInformation deviceInformation = deviceList[physicalConfiguration.Key];
			if (deviceInformation == null || deviceInformation.MemoryInformation == null)
			{
				continue;
			}
			foreach (MemoryInformation item in deviceInformation.MemoryInformation)
			{
				if (item.Count < GetConfigItemCount(item.MemoryType, physicalConfiguration.Value))
				{
					AddValidationError(deviceInformation.DeviceId, EntityType.BaseDevice, ValidationErrorCode.LemonbeatDeviceMemoryLimitReached);
				}
			}
		}
	}

	private int GetConfigItemCount(MemoryType memoryType, PhysicalConfiguration config)
	{
		return memoryType switch
		{
			MemoryType.Action => config.Actions.Sum((LemonbeatAction action) => (action.Items != null) ? action.Items.Count : 0), 
			MemoryType.Calculation => config.Calculations.Count, 
			MemoryType.Calendar => config.CalendarEntries.Count, 
			MemoryType.PartnerInformation => config.Partners.Count + config.PartnerGroups.Count, 
			MemoryType.StateMachine => config.StateMachines.Count, 
			MemoryType.StateMachineState => config.StateMachines.Sum((RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines.StateMachine sm) => (sm.States != null) ? sm.States.Sum((RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines.State state) => (state.Transactions != null) ? state.Transactions.Count : 0) : 0), 
			MemoryType.Timer => config.Timers.Count, 
			MemoryType.Value => config.VirtualValueDescriptions.Count, 
			_ => throw new ArgumentException("Unexpected memory type"), 
		};
	}
}
