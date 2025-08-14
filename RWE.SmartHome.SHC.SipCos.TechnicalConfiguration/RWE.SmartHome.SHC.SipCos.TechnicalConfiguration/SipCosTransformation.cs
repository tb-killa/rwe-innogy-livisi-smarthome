using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DataAccessInterfaces.TechnicalConfiguration;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Calibrators;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.SwitchDelegate;
using RWE.SmartHome.SHC.DomainModel.Rules;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.TaskList;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration;

public class SipCosTransformation : BaseProtocolSpecificTransformation, ISipcosChannelUsage
{
	private readonly Func<Guid, LogicalDeviceState> getLogicalDeviceState;

	private readonly Func<IList<byte[]>> shcAddressesFunc;

	private IList<byte[]> shcAddresses;

	private readonly ISipCosProtocolAdapter sipCosProtocolAdapter;

	private readonly ISwitchDelegate switchDelegate;

	private readonly IRollerShutterCalibrator rollerShutterCalibrator;

	private readonly ITechnicalConfigurationManager technicalConfigurationManager;

	private readonly ProtocolIdentifier protocolId;

	private readonly IDeviceList deviceList;

	private readonly BidCosTransformation bidCosTransformation;

	private List<LogicalDevice> logicalDevices;

	private IDictionary<Guid, SensorConfiguration> sensorConfigurations;

	private IDictionary<Guid, ActuatorConfiguration> actuatorConfigurations;

	private Dictionary<Guid, TechnicalConfigurationTask> technicalConfigurationTasks;

	private List<Guid> unprocessedSetpoints = new List<Guid>();

	private List<LogicalDeviceState> ccAutoModeChanges = new List<LogicalDeviceState>();

	private List<ElementaryRule> processedElementaryRules = new List<ElementaryRule>();

	private readonly CustomTriggerProcessor customTriggerProcessor;

	private int AckDelegateThreshhold { get; set; }

	public override ProtocolIdentifier ProtocolId => ProtocolIdentifier.Cosip;

	internal IList<TechnicalConfigurationTask> CompleteConfiguration { get; set; }

	private IEnumerable<KeyValuePair<LinkEndpoint, IEnumerable<byte[]>>> AckDelegates { get; set; }

	private IEnumerable<Guid> DevicesWithPendingCalibration { get; set; }

	public SipCosTransformation(IDeviceList deviceList, Func<Guid, LogicalDeviceState> getLogicalDeviceState, Func<IList<byte[]>> shcAddresses, ISipCosProtocolAdapter sipCosProtocolAdapter, ISwitchDelegate switchDelegate, ITechnicalConfigurationManager technicalConfigurationManager, IRollerShutterCalibrator rollerShutterCalibrator, ProtocolIdentifier protocolId, IRepository configRepository, IBidCosConfigurator bidCosConfigurator, ITechnicalConfigurationPersistence technicalConfigurationPersistence, IDeviceManager deviceManager, IEventManager eventManager)
		: base(configRepository)
	{
		AckDelegateThreshhold = 4;
		this.deviceList = deviceList;
		this.getLogicalDeviceState = getLogicalDeviceState;
		shcAddressesFunc = shcAddresses;
		this.sipCosProtocolAdapter = sipCosProtocolAdapter;
		this.switchDelegate = switchDelegate;
		this.rollerShutterCalibrator = rollerShutterCalibrator;
		this.technicalConfigurationManager = technicalConfigurationManager;
		this.protocolId = protocolId;
		customTriggerProcessor = new CustomTriggerProcessor(configRepository);
		if (bidCosConfigurator != null)
		{
			bidCosTransformation = new BidCosTransformation(configRepository, deviceList, bidCosConfigurator, technicalConfigurationPersistence, deviceManager, eventManager, () => this.shcAddresses[1]);
		}
	}

	public override bool PrepareTransformation(IElementaryRuleRepository elementaryRuleRepository)
	{
		shcAddresses = shcAddressesFunc();
		logicalDevices = configRepository.GetLogicalDevices();
		base.Errors = new List<ErrorEntry>();
		base.ImmediateStateChanges = new List<LogicalDeviceState>();
		sensorConfigurations = new Dictionary<Guid, SensorConfiguration>();
		actuatorConfigurations = new Dictionary<Guid, ActuatorConfiguration>();
		technicalConfigurationTasks = new Dictionary<Guid, TechnicalConfigurationTask>();
		foreach (BaseDevice item in from d in configRepository.GetBaseDevices()
			where d.ProtocolId == ProtocolIdentifier.Cosip
			select d)
		{
			byte[] array = null;
			IDeviceInformation deviceInformation = null;
			if (deviceList.Contains(item.Id))
			{
				deviceInformation = deviceList[item.Id];
				array = deviceInformation.Address;
			}
			if (item == null || item.GetBuiltinDeviceDeviceType() != BuiltinPhysicalDeviceType.SIR)
			{
				if (array != null)
				{
					technicalConfigurationTasks.Add(item.Id, new TechnicalConfigurationTask
					{
						DeviceId = item.Id,
						DeviceAddress = array,
						ReferenceConfiguration = new DeviceConfiguration(),
						DefaultConfiguration = new DeviceConfiguration()
					});
				}
				else
				{
					Log.Error(Module.TechnicalConfiguration, $"Device with SN {item.SerialNumber}, Name {item.Name} address is null. Corrupted configuration?");
					base.Errors.Add(new ErrorEntry
					{
						AffectedEntity = new EntityMetadata
						{
							EntityType = EntityType.BaseDevice,
							Id = item.Id
						},
						ErrorCode = ValidationErrorCode.UnknownPhysicalDevice
					});
				}
			}
		}
		try
		{
			CreateDefaultTechnicalConfiguration();
			InitializeTargetTechnicalConfiguration();
		}
		catch (TransformationException ex)
		{
			base.Errors.Add(ex.Error);
		}
		if (base.Errors.Count > 0)
		{
			return true;
		}
		if (bidCosTransformation != null)
		{
			bidCosTransformation.PrepareTransformation(elementaryRuleRepository);
			bidCosTransformation.Errors.ToList().ForEach(base.Errors.Add);
		}
		processedElementaryRules.Clear();
		ProcessElementaryRules(elementaryRuleRepository);
		try
		{
			foreach (WindowDoorSensorConfiguration item2 in sensorConfigurations.Values.OfType<WindowDoorSensorConfiguration>())
			{
				item2.CreateLinks(new Trigger
				{
					Entity = new LinkBinding(EntityType.LogicalDevice, Guid.Empty)
				}, null, actuatorConfigurations[Guid.Empty], null);
			}
			foreach (MotionDetectorConfiguration item3 in sensorConfigurations.Values.OfType<MotionDetectorConfiguration>())
			{
				item3.CreateLinks(new Trigger
				{
					Entity = new LinkBinding(EntityType.LogicalDevice, Guid.Empty)
				}, null, actuatorConfigurations[Guid.Empty], null);
			}
			foreach (PushButtonSensorConfiguration item4 in sensorConfigurations.Values.OfType<PushButtonSensorConfiguration>())
			{
				Trigger trigger = CreatePushButtonSensorTrigger(item4);
				item4.CreateLinks(trigger, null, actuatorConfigurations[Guid.Empty], null);
			}
			DevicesWithPendingCalibration = GetDevicesWithPendingCalibration();
			CreateCalibrationLinks();
			SaveAllDeviceConfigs(sensorConfigurations, isReferenceConfig: true);
			SaveAllDeviceConfigs(actuatorConfigurations, isReferenceConfig: true);
			CompleteConfiguration = technicalConfigurationTasks.Values.ToList();
			AckDelegates = ReplaceByAckDelegates();
			base.ImmediateStateChanges = GetStateChangesList();
		}
		catch (TransformationException ex2)
		{
			base.Errors.Add(ex2.Error);
		}
		return base.Errors.Count > 0;
	}

	private void CreateCalibrationLinks()
	{
		Guid rsId;
		foreach (Guid item in DevicesWithPendingCalibration)
		{
			rsId = item;
			PushButtonSensorConfiguration pushButtonSensorConfiguration = sensorConfigurations.Values.OfType<PushButtonSensorConfiguration>().FirstOrDefault((PushButtonSensorConfiguration sc) => sc.PhysicalDeviceId == rsId && sc.PushButtons.Count() == 2);
			if (pushButtonSensorConfiguration != null && ButtonsNotLinkedToActuator(pushButtonSensorConfiguration))
			{
				pushButtonSensorConfiguration.CreateInternalLinks(actuatorConfigurations.Values);
			}
		}
	}

	private static bool ButtonsNotLinkedToActuator(PushButtonSensorConfiguration button)
	{
		Func<LinkPartner, bool> IsActuatorLink = (LinkPartner lp) => lp.DeviceId == button.PhysicalDeviceId && lp.Channel == 1;
		if (!button.PushButtons[0].Links.Keys.All((LinkPartner lp) => !IsActuatorLink(lp)))
		{
			return button.PushButtons[1].Links.Keys.All((LinkPartner lp) => !IsActuatorLink(lp));
		}
		return true;
	}

	private List<LogicalDeviceState> GetStateChangesList()
	{
		List<LogicalDeviceState> list = new List<LogicalDeviceState>();
		list.AddRange(GetStateChangesForMovedRSTs());
		list.AddRange(GetAutoModeVRCCChanges());
		return list;
	}

	private IEnumerable<LogicalDeviceState> GetAutoModeVRCCChanges()
	{
		return ccAutoModeChanges;
	}

	private IEnumerable<LogicalDeviceState> GetStateChangesForMovedRSTs()
	{
		List<LogicalDeviceState> list = new List<LogicalDeviceState>();
		BuiltinPhysicalDeviceType[] rstWrtTypes = new BuiltinPhysicalDeviceType[3]
		{
			BuiltinPhysicalDeviceType.WRT,
			BuiltinPhysicalDeviceType.RST,
			BuiltinPhysicalDeviceType.RST2
		};
		List<BaseDevice> list2 = (from m in configRepository.GetModifiedBaseDevices()
			where rstWrtTypes.Contains(m.GetBuiltinDeviceDeviceType())
			select m).ToList();
		List<BaseDevice> source = (from m in configRepository.GetBaseDevices()
			where rstWrtTypes.Contains(m.GetBuiltinDeviceDeviceType())
			select m).ToList();
		foreach (BaseDevice modifiedBaseDevice in list2)
		{
			BaseDevice originalBaseDevice = configRepository.GetOriginalBaseDevice(modifiedBaseDevice.Id);
			if (originalBaseDevice != null && (!modifiedBaseDevice.LocationId.HasValue || originalBaseDevice.LocationId.Equals(modifiedBaseDevice.LocationId)))
			{
				continue;
			}
			Guid? locationId = modifiedBaseDevice.LocationId;
			List<BaseDevice> source2 = source.Where((BaseDevice m) => !m.Id.Equals(modifiedBaseDevice.Id) && m.LocationId.Equals(locationId)).ToList();
			if (source2.Any())
			{
				BaseDevice baseDevice = (source2.Any((BaseDevice m) => m.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.WRT) ? source2.OrderBy((BaseDevice m) => m.TimeOfAcceptance).FirstOrDefault((BaseDevice m) => m.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.WRT) : source2.FirstOrDefault());
				ThermostatActuator firstLogicalDeviceFromBaseDevice = GetFirstLogicalDeviceFromBaseDevice<ThermostatActuator>(baseDevice);
				ThermostatActuator firstLogicalDeviceFromBaseDevice2 = GetFirstLogicalDeviceFromBaseDevice<ThermostatActuator>(modifiedBaseDevice);
				if (getLogicalDeviceState(firstLogicalDeviceFromBaseDevice.Id) is GenericDeviceState genericDeviceState && genericDeviceState.Clone() is GenericDeviceState genericDeviceState2)
				{
					genericDeviceState2.LogicalDeviceId = firstLogicalDeviceFromBaseDevice2.Id;
					list.Add(genericDeviceState2);
				}
			}
		}
		return list;
	}

	private T GetFirstLogicalDeviceFromBaseDevice<T>(BaseDevice baseDevice) where T : LogicalDevice
	{
		return baseDevice.LogicalDeviceIds.Select(configRepository.GetLogicalDevice).OfType<T>().FirstOrDefault();
	}

	private Trigger CreatePushButtonSensorTrigger(PushButtonSensorConfiguration configuration)
	{
		List<Condition> triggerConditions = configuration.PushButtons.Select((PushButtonChannel button) => new Condition
		{
			Operator = ConditionOperator.Equal,
			LeftHandOperand = new FunctionBinding
			{
				Function = FunctionIdentifier.GetEventProperty,
				Parameters = new List<Parameter>
				{
					new Parameter
					{
						Name = "EventPropertyName",
						Value = new ConstantStringBinding
						{
							Value = "index"
						}
					}
				}
			},
			RightHandOperand = new ConstantNumericBinding
			{
				Value = configuration.GetButtonNumberFromChannelIndex(button.ChannelIndex)
			}
		}).ToList();
		Trigger trigger = new Trigger();
		trigger.Entity = new LinkBinding(EntityType.LogicalDevice, Guid.Empty);
		trigger.TriggerConditions = triggerConditions;
		return trigger;
	}

	public override void CommitTransformationResults(IEnumerable<Guid> devicesToDelete)
	{
		technicalConfigurationManager.SetConfiguration(devicesToDelete, CompleteConfiguration);
		if (bidCosTransformation != null)
		{
			bidCosTransformation.CommitTransformationResults(devicesToDelete);
		}
		switchDelegate.SetLookupTable(AckDelegates);
		rollerShutterCalibrator.SetCalibrationTargets(DevicesWithPendingCalibration);
		CleanupTransformationResults();
	}

	public override void DiscardTransformationResults()
	{
		CleanupTransformationResults();
		if (bidCosTransformation != null)
		{
			bidCosTransformation.DiscardTransformationResults();
		}
	}

	protected override bool AccelerateRule(ElementaryRule rule)
	{
		bool flag = false;
		if (rule.Action != null)
		{
			if (rule.Trigger.Trigger != null)
			{
				flag = CreateLinks(rule.SourceRule, rule.Trigger.Trigger, rule.Action);
				if (flag)
				{
					processedElementaryRules.Add(rule);
				}
			}
			else if (rule.Trigger.CustomTrigger != null)
			{
				flag = CreateLinks(rule);
			}
		}
		return flag;
	}

	protected override void CleanupTransformationResults()
	{
		logicalDevices = null;
		base.Errors = null;
		sensorConfigurations = null;
		actuatorConfigurations = null;
		technicalConfigurationTasks = null;
		CompleteConfiguration = null;
		DevicesWithPendingCalibration = null;
		AckDelegates = null;
		base.ImmediateStateChanges = null;
		unprocessedSetpoints.Clear();
		ccAutoModeChanges.Clear();
		processedElementaryRules.Clear();
	}

	private IEnumerable<Guid> GetDevicesWithPendingCalibration()
	{
		return (from rollerShutterActuator in logicalDevices.OfType<RollerShutterActuator>()
			where rollerShutterActuator.IsCalibrating && rollerShutterActuator.BaseDevice != null
			select rollerShutterActuator.BaseDevice.Id).ToList();
	}

	private IEnumerable<KeyValuePair<LinkEndpoint, IEnumerable<byte[]>>> ReplaceByAckDelegates()
	{
		List<KeyValuePair<LinkEndpoint, IEnumerable<byte[]>>> list = new List<KeyValuePair<LinkEndpoint, IEnumerable<byte[]>>>();
		foreach (KeyValuePair<Guid, SensorConfiguration> sensorConfiguration in sensorConfigurations)
		{
			Guid physicalDeviceId = sensorConfiguration.Value.PhysicalDeviceId;
			if (physicalDeviceId == Guid.Empty)
			{
				continue;
			}
			TechnicalConfigurationTask configuration = technicalConfigurationTasks[physicalDeviceId];
			IDictionary<byte, ConfigurationChannel> channels = configuration.ReferenceConfiguration.Channels;
			foreach (KeyValuePair<byte, ConfigurationChannel> item in channels)
			{
				if (item.Value.ChannelType == ChannelType.ActuatorHasFlag || item.Value.ChannelType == ChannelType.ActuatorWithoutFlag)
				{
					continue;
				}
				Dictionary<LinkPartner, ConfigurationLink> links = item.Value.Links;
				LinkPartner[] array = (from pair in links
					select pair.Key into linkPartner
					where linkPartner != LinkPartner.Empty && !linkPartner.Address.SequenceEqual(configuration.DeviceAddress)
					select linkPartner).ToArray();
				if (array.Length <= AckDelegateThreshhold)
				{
					continue;
				}
				LinkEndpoint key = new LinkEndpoint(configuration.DeviceAddress, item.Key);
				byte[][] value = array.Select((LinkPartner partner) => partner.Address).ToArray();
				list.Add(new KeyValuePair<LinkEndpoint, IEnumerable<byte[]>>(key, value));
				ConfigurationLink configurationLink = links[array[0]];
				LinkPartner[] array2 = array;
				foreach (LinkPartner key2 in array2)
				{
					links.Remove(key2);
				}
				for (byte b = 1; b < 3; b++)
				{
					LinkPartner key3 = new LinkPartner(Guid.Empty, shcAddresses[b], 63);
					if (!links.ContainsKey(key3))
					{
						links.Add(key3, configurationLink.Clone());
					}
				}
				configuration.SensorConfigurationChangedByAckDelegate = true;
			}
		}
		return list;
	}

	private void CreateDefaultTechnicalConfiguration()
	{
		CreateConfigurationBuilders();
		foreach (SensorConfiguration value in sensorConfigurations.Values)
		{
			value.CreateInternalLinks(actuatorConfigurations.Values);
		}
		foreach (ThermostatActuatorConfiguration item in actuatorConfigurations.Values.OfType<ThermostatActuatorConfiguration>())
		{
			item.CreateInternalLinks(actuatorConfigurations.Values);
		}
		SaveAllDeviceConfigs(sensorConfigurations, isReferenceConfig: false);
		SaveAllDeviceConfigs(actuatorConfigurations, isReferenceConfig: false);
	}

	private void InitializeTargetTechnicalConfiguration()
	{
		CreateConfigurationBuilders();
		foreach (SensorConfiguration value in sensorConfigurations.Values)
		{
			value.InitializeConfiguration(shcAddresses, sensorConfigurations, actuatorConfigurations);
		}
		foreach (ActuatorConfiguration value2 in actuatorConfigurations.Values)
		{
			value2.InitializeConfiguration(shcAddresses, sensorConfigurations, actuatorConfigurations);
		}
	}

	private void CreateConfigurationBuilders()
	{
		actuatorConfigurations.Clear();
		sensorConfigurations.Clear();
		foreach (LogicalDevice logicalDevice in logicalDevices)
		{
			byte[] deviceAddress = null;
			IDeviceInformation deviceInformation = deviceList[logicalDevice.BaseDeviceId];
			if (deviceInformation != null)
			{
				deviceAddress = deviceInformation.Address;
			}
			TechnicalConfigurationCreator configurationCreator = ConfigurationFactory.GetConfigurationCreator(logicalDevice, deviceAddress);
			if (configurationCreator != null)
			{
				configurationCreator.ConfigurationRepository = configRepository;
				if (configurationCreator is SensorConfiguration)
				{
					sensorConfigurations[logicalDevice.Id] = configurationCreator as SensorConfiguration;
				}
				else
				{
					actuatorConfigurations[logicalDevice.Id] = configurationCreator as ActuatorConfiguration;
				}
			}
		}
		actuatorConfigurations[Guid.Empty] = new ShcActuatorConfiguration(null);
	}

	private void SaveAllDeviceConfigs<T>(IEnumerable<KeyValuePair<Guid, T>> configurations, bool isReferenceConfig) where T : TechnicalConfigurationCreator
	{
		foreach (KeyValuePair<Guid, T> configuration2 in configurations)
		{
			try
			{
				T value = configuration2.Value;
				Guid physicalDeviceId = value.PhysicalDeviceId;
				if (!(physicalDeviceId == Guid.Empty))
				{
					TechnicalConfigurationTask value2 = null;
					if (technicalConfigurationTasks.TryGetValue(physicalDeviceId, out value2))
					{
						DeviceConfiguration configuration = (isReferenceConfig ? value2.ReferenceConfiguration : value2.DefaultConfiguration);
						value.SaveConfiguration(configuration);
					}
				}
			}
			catch (TransformationException ex)
			{
				ex.Error.AffectedEntity = new EntityMetadata
				{
					EntityType = EntityType.LogicalDevice,
					Id = configuration2.Key
				};
				base.Errors.Add(ex.Error);
			}
		}
	}

	private bool CreateLinks(ElementaryRule rule)
	{
		ActionDescription action = rule.Action;
		CustomTrigger customTrigger = rule.Trigger.CustomTrigger;
		if (action.Target.LinkType == EntityType.LogicalDevice)
		{
			Guid guid = action.Target.EntityIdAsGuid();
			configRepository.GetLogicalDevice(guid);
			if (actuatorConfigurations.TryGetValue(guid, out var value))
			{
				bool flag = customTriggerProcessor.ProcessCustomTriggers(customTrigger, action, value);
				if (flag && IsUpdatedHeatingTimeProfile(rule.SourceInteractionId, customTrigger.Type, value))
				{
					CreateModeStateChange(value as RoomSetpointConfiguration);
				}
				return flag;
			}
		}
		return false;
	}

	private bool CreateLinks(Rule rule, Trigger trigger, ActionDescription action)
	{
		if (trigger.Entity.LinkType != EntityType.LogicalDevice || action.Target.LinkType != EntityType.LogicalDevice)
		{
			return false;
		}
		bool flag = false;
		LogicalDevice logicalDevice = null;
		LogicalDevice logicalDevice2 = null;
		try
		{
			logicalDevice = configRepository.GetLogicalDevice(trigger.Entity.EntityIdAsGuid());
			logicalDevice2 = configRepository.GetLogicalDevice(action.Target.EntityIdAsGuid());
			if (sensorConfigurations.TryGetValue(logicalDevice.Id, out var value) && actuatorConfigurations.TryGetValue(logicalDevice2.Id, out var value2) && (rule.Conditions == null || rule.Conditions.Count == 0) && CanAccelerate(logicalDevice, logicalDevice2))
			{
				flag = value.CreateLinks(trigger, action, value2, rule);
				if (!flag)
				{
					Log.Warning(Module.SipCosProtocolAdapter, $"Could not create direct link between {logicalDevice.Id} and {logicalDevice2.Id}.");
				}
			}
		}
		catch (TransformationException ex)
		{
			HandleTranformationError(rule, logicalDevice, logicalDevice2, ex);
		}
		return flag;
	}

	private void HandleTranformationError(Rule rule, LogicalDevice sensor, LogicalDevice actuator, TransformationException ex)
	{
		if (ex.Error.AffectedEntity.EntityType == EntityType.Interaction && sensor != null && actuator != null)
		{
			ElementaryRule elementaryRule = processedElementaryRules.Where((ElementaryRule er) => ContainsDeviceCombination(er, sensor.Id, actuator.Id)).FirstOrDefault();
			if (elementaryRule != null)
			{
				ex.Error.AffectedEntity.Id = elementaryRule.SourceInteractionId;
			}
			else
			{
				ex.Error.AffectedEntity.Id = rule.InteractionId;
			}
		}
		else
		{
			ex.Error.ErrorParameters.Add(new ErrorParameter
			{
				Key = ErrorParameterKey.ProfileId,
				Value = rule.InteractionId.ToString()
			});
		}
		base.Errors.Add(ex.Error);
	}

	private bool ContainsDeviceCombination(ElementaryRule elemRule, Guid triggerId, Guid targetId)
	{
		if (elemRule.IsHandled && elemRule.Trigger.Trigger.Entity.EntityId == triggerId.ToString("N"))
		{
			return elemRule.Action.Target.EntityId == targetId.ToString("N");
		}
		return false;
	}

	private bool CanAccelerate(LogicalDevice sensor, LogicalDevice actuator)
	{
		if (!(sensor is LuminanceSensor) && !(sensor is SmokeDetectorSensor) && !(actuator is AlarmActuator) && (!(sensor is MotionDetectionSensor) || !(actuator is RoomSetpoint)))
		{
			if (sensor is WindowDoorSensor)
			{
				if (!(actuator is RollerShutterActuator) && !(actuator is DimmerActuator))
				{
					return !(actuator is RoomSetpoint);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	protected override void CreateLinkWithShc(Rule rule, Trigger trigger)
	{
		Guid guid = rule?.Id ?? Guid.Empty;
		if (configRepository.GetLogicalDevice(trigger.Entity.EntityIdAsGuid()) == null)
		{
			AddValidationError(trigger.Entity.EntityIdAsGuid(), EntityType.LogicalDevice, ValidationErrorCode.MissingSensor, guid);
			return;
		}
		if (!sensorConfigurations.ContainsKey(trigger.Entity.EntityIdAsGuid()))
		{
			if (!actuatorConfigurations.ContainsKey(trigger.Entity.EntityIdAsGuid()))
			{
				AddValidationError(trigger.Entity.EntityIdAsGuid(), EntityType.LogicalDevice, ValidationErrorCode.UnknownSensorInSensorSettings, new ErrorParameter
				{
					Key = ErrorParameterKey.SensorId,
					Value = trigger.Entity.EntityId
				}, new ErrorParameter
				{
					Key = ErrorParameterKey.ProfileId,
					Value = guid.ToString()
				});
			}
			return;
		}
		SensorConfiguration sensorConfiguration = sensorConfigurations[trigger.Entity.EntityIdAsGuid()];
		try
		{
			sensorConfiguration.CreateLinks(trigger, null, actuatorConfigurations[Guid.Empty], rule);
		}
		catch (TransformationException ex)
		{
			if (ex.Error.AffectedEntity.EntityType == EntityType.Interaction)
			{
				ex.Error.AffectedEntity.Id = guid;
			}
			else
			{
				ex.Error.ErrorParameters.Add(new ErrorParameter
				{
					Key = ErrorParameterKey.ProfileId,
					Value = guid.ToString()
				});
			}
			base.Errors.Add(ex.Error);
		}
	}

	private bool IsUpdatedHeatingTimeProfile(Guid interactionId, string triggerType, ActuatorConfiguration actuatorConfiguration)
	{
		if (configRepository.GetModifiedInteractions().Any((Interaction intr) => intr.Id == interactionId) && (triggerType == "DailyTrigger" || triggerType == "WeeklyTrigger"))
		{
			return actuatorConfiguration is RoomSetpointConfiguration;
		}
		return false;
	}

	private void CreateModeStateChange(RoomSetpointConfiguration roomSetpointCfg)
	{
		if (roomSetpointCfg == null)
		{
			return;
		}
		IEnumerable<ThermostatActuator> enumerable = configRepository.GetLogicalDevices().OfType<ThermostatActuator>().Where(delegate(ThermostatActuator ta)
		{
			Guid? locationId = ta.BaseDevice.LocationId;
			Guid? locationId2 = roomSetpointCfg.LogicalDeviceContract.BaseDevice.LocationId;
			return locationId == locationId2;
		});
		foreach (ThermostatActuator item in enumerable)
		{
			ccAutoModeChanges.Add(new GenericDeviceState
			{
				LogicalDeviceId = item.Id,
				Properties = new List<Property>
				{
					new StringProperty
					{
						Name = "OperationMode",
						Value = OperationMode.Auto.ToString()
					}
				}
			});
		}
	}

	public IEnumerable<int> GetUsedDeviceFunctions(Trigger trigger)
	{
		if (!sensorConfigurations.TryGetValue(trigger.Entity.EntityIdAsGuid(), out var value))
		{
			return new int[0];
		}
		return value.GetUsedChannels(trigger);
	}

	public IEnumerable<int> GetUsedDeviceFunctions(ActionDescription action)
	{
		if (!actuatorConfigurations.TryGetValue(new Guid(action.Target.EntityId), out var value))
		{
			return new int[0];
		}
		return value.GetUsedChannels(action);
	}

	public int? GetMaxTimePointsPerWeekday(Guid logicalDeviceId)
	{
		if (!actuatorConfigurations.ContainsKey(logicalDeviceId))
		{
			return null;
		}
		return actuatorConfigurations[logicalDeviceId].MaxTimePointsPerWeekday;
	}

	protected override void UpdateTargetEntities()
	{
		supportedDeviceIds = (from ld in configRepository.GetBaseDevices()
			where ld.ProtocolId == ProtocolId
			where ld.GetBuiltinDeviceDeviceType() != BuiltinPhysicalDeviceType.SIR
			select ld.Id).ToList();
		supportedCapabilityIds = (from ld in configRepository.GetLogicalDevices()
			where supportedDeviceIds.Contains(ld.BaseDeviceId)
			select ld.Id).ToList();
	}
}
