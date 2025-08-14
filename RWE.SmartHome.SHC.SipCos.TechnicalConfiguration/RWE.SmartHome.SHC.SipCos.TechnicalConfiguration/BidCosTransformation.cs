using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DataAccessInterfaces.TechnicalConfiguration;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.DomainModel.Rules;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos.Configurations;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos.Tasks;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration;

public class BidCosTransformation : BaseProtocolSpecificTransformation
{
	private static readonly object sync = new object();

	private readonly IDeviceList deviceList;

	private readonly IBidCosConfigurator bidCosConfigurator;

	private readonly BidCosTechnicalConfigManager technicalManager;

	private readonly ITechnicalConfigurationPersistence persistence;

	private readonly IDeviceManager deviceManager;

	private readonly IEventManager eventManager;

	private readonly Func<byte[]> getShcAdress;

	private bool isLoadedConfig;

	private BidCosConfiguration currentConfiguration;

	private BidCosConfiguration diffConfiguration;

	private readonly IDictionary<Guid, Guid> pendingPacketIds = new Dictionary<Guid, Guid>();

	public override ProtocolIdentifier ProtocolId => ProtocolIdentifier.Cosip;

	public BidCosTransformation(IRepository configRepository, IDeviceList deviceList, IBidCosConfigurator bidCosConfigurator, ITechnicalConfigurationPersistence persistence, IDeviceManager deviceManager, IEventManager eventManager, Func<byte[]> getShcAdress)
		: base(configRepository)
	{
		currentConfiguration = new BidCosConfiguration();
		this.deviceList = deviceList;
		this.bidCosConfigurator = bidCosConfigurator;
		this.deviceManager = deviceManager;
		technicalManager = new BidCosTechnicalConfigManager(persistence, deviceManager);
		this.getShcAdress = getShcAdress;
		this.persistence = persistence;
		this.eventManager = eventManager;
		eventManager.GetEvent<DeviceInclusionStateChangedEvent>().Subscribe(DeviceFactoryReset, (DeviceInclusionStateChangedEventArgs args) => args.DeviceInclusionState == DeviceInclusionState.FactoryReset, ThreadOption.PublisherThread, null);
		deviceManager.SequenceFinished += SequenceFinished;
	}

	~BidCosTransformation()
	{
		deviceManager.SequenceFinished -= SequenceFinished;
	}

	public override bool PrepareTransformation(IElementaryRuleRepository elementaryRuleRepository)
	{
		base.Errors = new List<ErrorEntry>();
		byte[] shcAdress = getShcAdress();
		List<BaseDevice> bidCosDevices = GetBidCosDevices();
		List<LogicalDevice> logicalDevicesForBaseDevices = GetLogicalDevicesForBaseDevices(bidCosDevices);
		List<BaseBidCosConfigurator> configurators = GetConfigurators(logicalDevicesForBaseDevices, shcAdress);
		try
		{
			LoadConfigurationIfNotLoaded(bidCosDevices.Select((BaseDevice m) => m.Id));
			BidCosConfiguration bidCosConfiguration = new BidCosConfiguration();
			foreach (IGrouping<Guid, BaseBidCosConfigurator> item in from m in configurators
				group m by m.DeviceId)
			{
				if (deviceList.Contains(item.Key) && deviceList[item.Key].DeviceInclusionState != DeviceInclusionState.FactoryReset)
				{
					BidCosDeviceConfiguration bidCosDeviceConfiguration = new BidCosDeviceConfiguration(item.Key);
					item.SelectMany((BaseBidCosConfigurator m) => m.GetTasks()).ToList().ForEach(bidCosDeviceConfiguration.AddTask);
					bidCosConfiguration.AddDeviceConfig(bidCosDeviceConfiguration);
				}
			}
			diffConfiguration = bidCosConfiguration.GetDiff(currentConfiguration);
			ProcessElementaryRules(elementaryRuleRepository);
			currentConfiguration = bidCosConfiguration;
		}
		catch (TransformationException ex)
		{
			base.Errors.Add(ex.Error);
		}
		catch (Exception ex2)
		{
			Log.Exception(Module.SerialCommunication, ex2, "BidCosTransformation failed");
			base.Errors.Add(new ErrorEntry
			{
				ErrorCode = ValidationErrorCode.Unknown
			});
		}
		return base.Errors.Count > 0;
	}

	public override void CommitTransformationResults(IEnumerable<Guid> devicesToDelete)
	{
		CommitConfig();
		technicalManager.SaveConfiguration(currentConfiguration);
	}

	protected override bool AccelerateRule(ElementaryRule rule)
	{
		return false;
	}

	private List<BaseDevice> GetBidCosDevices()
	{
		return (from m in configRepository.GetBaseDevices()
			where m.ProtocolId == ProtocolIdentifier.Cosip
			where deviceList.Contains(m.Id) && deviceList[m.Id].ProtocolType == ProtocolType.BidCos
			select m).ToList();
	}

	private List<LogicalDevice> GetLogicalDevicesForBaseDevices(IEnumerable<BaseDevice> devices)
	{
		return (from m in devices.SelectMany((BaseDevice m) => m.LogicalDeviceIds)
			select configRepository.GetLogicalDevice(m)).ToList();
	}

	private List<BaseBidCosConfigurator> GetConfigurators(IEnumerable<LogicalDevice> lds, byte[] shcAdress)
	{
		return (from m in lds
			select BidCosConfigurationFactory.GetConfiguration(m, shcAdress, deviceList[m.BaseDeviceId].Address) into m
			where m != null
			select m).ToList();
	}

	private void LoadConfigurationIfNotLoaded(IEnumerable<Guid> deviceIds)
	{
		if (isLoadedConfig)
		{
			return;
		}
		currentConfiguration = technicalManager.GetPersitedConfig(deviceIds);
		foreach (Guid deviceId in deviceIds)
		{
			if (deviceList.Contains(deviceId) && deviceList[deviceId].DeviceInclusionState == DeviceInclusionState.FactoryReset)
			{
				currentConfiguration.RemoveDevice(deviceId);
			}
		}
		isLoadedConfig = true;
	}

	private void CommitConfig()
	{
		lock (sync)
		{
			pendingPacketIds.Clear();
			foreach (BidCosDeviceConfiguration deviceConfig in diffConfiguration.GetDeviceConfigs())
			{
				Guid deviceId = deviceConfig.DeviceId;
				IDeviceInformation deviceInformation = deviceList[deviceId];
				if (deviceInformation != null)
				{
					technicalManager.TryIncludeDeviceIfNotIncluded(deviceId, deviceInformation);
				}
				if (deviceConfig.IsEmpty)
				{
					continue;
				}
				foreach (BaseBidCosTask task in deviceConfig.GetTasks())
				{
					Guid key = BidCosTaskFactory.ExecuteTask(task, bidCosConfigurator);
					pendingPacketIds.Add(key, deviceConfig.DeviceId);
				}
				MarkDeviceConfigPending(deviceId);
			}
		}
		bidCosConfigurator.Flush();
	}

	private void DeviceFactoryReset(DeviceInclusionStateChangedEventArgs args)
	{
		Guid deviceId = args.DeviceId;
		if (currentConfiguration != null)
		{
			currentConfiguration.RemoveDevice(deviceId);
		}
	}

	private void SequenceFinished(object sender, SequenceFinishedEventArgs e)
	{
		Guid correlationId = e.CorrelationId;
		SequenceState state = e.State;
		Guid deviceId = Guid.Empty;
		lock (sync)
		{
			if (pendingPacketIds.TryGetValue(correlationId, out deviceId))
			{
				if (state == SequenceState.Success)
				{
					pendingPacketIds.Remove(correlationId);
				}
				if (pendingPacketIds.All((KeyValuePair<Guid, Guid> m) => m.Value != deviceId))
				{
					eventManager.GetEvent<DeviceConfigurationFinishedEvent>().Publish(new DeviceConfigurationFinishedEventArgs(deviceId, e.State == SequenceState.Success));
				}
			}
		}
	}

	private void MarkDeviceConfigPending(Guid deviceId)
	{
		IDeviceInformation deviceInformation = deviceManager.DeviceList[deviceId];
		if (deviceInformation != null)
		{
			deviceInformation.DeviceConfigurationState = DeviceConfigurationState.Pending;
		}
	}
}
