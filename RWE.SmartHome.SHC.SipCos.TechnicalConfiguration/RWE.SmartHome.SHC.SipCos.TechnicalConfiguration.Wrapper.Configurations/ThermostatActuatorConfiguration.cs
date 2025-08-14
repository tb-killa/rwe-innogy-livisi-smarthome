using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.ErrorHandling;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Channels;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Links;
using RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

public class ThermostatActuatorConfiguration : CcActuatorConfiguration
{
	private readonly bool isWrt;

	private static readonly RWE.SmartHome.SHC.Core.Scheduler.Tasks.WeekDay[] DaysOfWeek = new RWE.SmartHome.SHC.Core.Scheduler.Tasks.WeekDay[7]
	{
		RWE.SmartHome.SHC.Core.Scheduler.Tasks.WeekDay.Monday,
		RWE.SmartHome.SHC.Core.Scheduler.Tasks.WeekDay.Tuesday,
		RWE.SmartHome.SHC.Core.Scheduler.Tasks.WeekDay.Wednesday,
		RWE.SmartHome.SHC.Core.Scheduler.Tasks.WeekDay.Thursday,
		RWE.SmartHome.SHC.Core.Scheduler.Tasks.WeekDay.Friday,
		RWE.SmartHome.SHC.Core.Scheduler.Tasks.WeekDay.Saturday,
		RWE.SmartHome.SHC.Core.Scheduler.Tasks.WeekDay.Sunday
	};

	public bool ChildLock { get; private set; }

	public BaseChannel<BaseLink> AutoManualButton { get; private set; }

	public CcRotaryWheelChannel RotaryWheel { get; private set; }

	public BaseChannel<BaseLink> SoftWindowDetection { get; private set; }

	public CcSensorChannel ThermostatSensor { get; private set; }

	public CcSchedulerChannel Scheduler { get; private set; }

	public CcSetPointChannel SetPoint { get; private set; }

	public CcWindowReductionChannel WindowReduction { get; private set; }

	internal CcValveChannel InternalValve { get; set; }

	private List<LinkPartner> AutomaticWindowReductionLinks { get; set; }

	public bool IsWrt => isWrt;

	public bool HasMaster { get; set; }

	public DateTime? PhysicalDeviceTimeOfAcceptance { get; protected set; }

	public override int? MaxTimePointsPerWeekday => 8;

	public ThermostatActuatorConfiguration(LogicalDevice logicalDevice, byte[] address)
		: base(logicalDevice, address)
	{
		ChildLock = false;
		ThermostatSensor = new CcSensorChannel(1);
		Scheduler = new CcSchedulerChannel(2, 16);
		RotaryWheel = new CcRotaryWheelChannel(3, 8);
		SetPoint = new CcSetPointChannel(4, 16);
		WindowReduction = new CcWindowReductionChannel(5, 8);
		AutoManualButton = new BaseChannel<BaseLink>(6, 8);
		PhysicalDeviceTimeOfAcceptance = logicalDevice.BaseDevice.TimeOfAcceptance;
		InitializeDeviceSettings();
		switch (logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType())
		{
		case BuiltinPhysicalDeviceType.RST:
		case BuiltinPhysicalDeviceType.RST2:
			SoftWindowDetection = new BaseChannel<BaseLink>(7, 8);
			InternalValve = new CcValveChannel(8, 1);
			isWrt = false;
			break;
		case BuiltinPhysicalDeviceType.WRT:
			SoftWindowDetection = null;
			isWrt = true;
			break;
		default:
			throw new ArgumentException("PhysicalDevice.DeviceType of logicalDevice is not Rst, Rst2 or Wrt: " + logicalDevice.BaseDevice.GetBuiltinDeviceDeviceType());
		}
		AutomaticWindowReductionLinks = new List<LinkPartner>();
	}

	public override void SaveConfiguration(DeviceConfiguration configuration)
	{
		base.GlobalStatusInfo.SaveConfiguration(configuration.Channels);
		ConfigurationLink configurationLink = configuration.Channels[base.GlobalStatusInfo.ChannelIndex].Links[LinkPartner.Empty];
		configurationLink[ConfigKeys.ChildLock] = (byte)(ChildLock ? 1u : 0u);
		ThermostatSensor.SaveConfiguration(configuration.Channels);
		Scheduler.SaveConfiguration(configuration.Channels);
		SetPoint.SaveConfiguration(configuration.Channels);
		RotaryWheel.SaveConfiguration(configuration.Channels);
		WindowReduction.SaveConfiguration(configuration.Channels);
		AutoManualButton.SaveConfiguration(configuration.Channels);
		if (SoftWindowDetection != null)
		{
			SoftWindowDetection.SaveConfiguration(configuration.Channels);
		}
		if (InternalValve != null)
		{
			InternalValve.SaveConfiguration(configuration.Channels);
		}
	}

	internal override IEnumerable<LinkPartner> CreateLinks(LinkPartner sensor, ActionDescription action, ProfileAction switchEvent, ProfileAction aboveEvent, ProfileAction belowEvent, int? comparisonValuePercent, Rule rule)
	{
		List<LinkPartner> list = new List<LinkPartner>();
		SwitchAction switchAction = Convert(switchEvent);
		SwitchAction aboveAction = Convert(aboveEvent);
		SwitchAction belowAction = Convert(belowEvent);
		if (SetPoint.AddLink(sensor, action, switchAction, aboveAction, belowAction, rule.Id, base.PhysicalDeviceId))
		{
			list.Add(CreateLinkPartner(SetPoint.ChannelIndex));
		}
		if (Scheduler.AddLink(sensor, action, switchAction, aboveAction, belowAction))
		{
			list.Add(CreateLinkPartner(Scheduler.ChannelIndex));
		}
		if ((IsWrt || !HasMaster) && WindowReduction.AddLink(sensor, action, switchAction, aboveAction, belowAction, comparisonValuePercent, rule.Id, base.PhysicalDeviceId))
		{
			list.Add(CreateLinkPartner(WindowReduction.ChannelIndex));
			RemoveAutomaticWindowReductionLinks();
		}
		return list;
	}

	private void RemoveAutomaticWindowReductionLinks()
	{
		if (SoftWindowDetection != null)
		{
			SoftWindowDetection.Links.Clear();
		}
		if (AutomaticWindowReductionLinks.Count > 0)
		{
			AutomaticWindowReductionLinks.ForEach(delegate(LinkPartner link)
			{
				WindowReduction.Links.Remove(link);
			});
			AutomaticWindowReductionLinks.Clear();
		}
	}

	public override bool AddDeviceSetpoint(TimeSpan timeOfDay, byte weekdays, ActionDescription action)
	{
		Parameter parameter = action.Data.SingleOrDefault((Parameter p) => p.Name == "PointTemperature");
		if (parameter == null || !(parameter.Value is ConstantNumericBinding))
		{
			return false;
		}
		decimal value = (parameter.Value as ConstantNumericBinding).Value;
		for (byte b = 0; b < 7; b++)
		{
			if ((weekdays & (1 << (int)b)) != 0)
			{
				if (Scheduler.Setpoints[b].Count > 7)
				{
					return false;
				}
				Scheduler.Setpoints[b].Add(new SchedulerSetpoint
				{
					Time = timeOfDay,
					TemperatureInCentigrades = value
				});
			}
		}
		return true;
	}

	public override IEnumerable<int> GetUsedChannels(ActionDescription action)
	{
		List<int> list = new List<int>();
		if (action.Data.Any((Parameter p) => p.Name == "PointTemperature"))
		{
			list.Add(SetPoint.ChannelIndex);
		}
		if (action.Data.Any((Parameter p) => p.Name == "OperationMode"))
		{
			list.Add(Scheduler.ChannelIndex);
		}
		if (action.ActionType == "WindowReduction")
		{
			list.Add(WindowReduction.ChannelIndex);
		}
		return list;
	}

	public override void InitializeConfiguration(IList<byte[]> shcAddresses, IDictionary<Guid, SensorConfiguration> sensors, IDictionary<Guid, ActuatorConfiguration> actuators)
	{
		base.InitializeConfiguration(shcAddresses, sensors, actuators);
	}

	private void InitializeDeviceSettings()
	{
		ThermostatActuator thermostatActuator = base.LogicalDeviceContract as ThermostatActuator;
		ThermostatSensor.DisplayMode = GetDisplayMode(base.LogicalDeviceContract.BaseDevice);
		ChildLock = thermostatActuator.ChildLock;
		RotaryWheel.RotaryMinCentigrade = thermostatActuator.MinTemperature;
		RotaryWheel.RotaryMaxCentigrade = thermostatActuator.MaxTemperature;
		WindowReduction.WindowOpenTemperatureCentigrade = thermostatActuator.WindowOpenTemperature;
	}

	public override void LinkWithOtherDevice(CcActuatorConfiguration otherDevice)
	{
		if (otherDevice is ThermostatActuatorConfiguration thermostatActuatorConfiguration)
		{
			LinkWithThermostatDevice(thermostatActuatorConfiguration);
			if (base.PhysicalAddress == thermostatActuatorConfiguration.PhysicalAddress)
			{
				LinkValveWithDeviceSensor(thermostatActuatorConfiguration);
			}
		}
	}

	public override void LinkWithMasterDevice(CcActuatorConfiguration masterDevice)
	{
		if (masterDevice is ThermostatActuatorConfiguration thermostatActuatorConfiguration)
		{
			LinkWithThermostatDevice(thermostatActuatorConfiguration);
			LinkValveWithDeviceSensor(thermostatActuatorConfiguration);
		}
	}

	private void LinkValveWithDeviceSensor(ThermostatActuatorConfiguration thermostatActuator)
	{
		if (InternalValve != null)
		{
			LinkPartner linkPartner = thermostatActuator.CreateLinkPartner(thermostatActuator.ThermostatSensor.ChannelIndex);
			InternalValve.AddLink(linkPartner, new BaseLink());
		}
	}

	private void LinkWithThermostatDevice(ThermostatActuatorConfiguration thermostatDevice)
	{
		CreateRoomLinkBidirectional(thermostatDevice, RotaryWheel, CcLinkType.SetPoint);
		CreateRoomLinkBidirectional(thermostatDevice, AutoManualButton, CcLinkType.Scheduler);
		if (SoftWindowDetection != null)
		{
			CreateRoomLinkBidirectional(thermostatDevice, SoftWindowDetection, CcLinkType.WindowReduction);
		}
	}

	private void CreateRoomLinkBidirectional(ThermostatActuatorConfiguration thermostatDevice, BaseChannel<BaseLink> channel, CcLinkType linkType)
	{
		LinkPartner linkPartner = thermostatDevice.CreateRoomLink(CreateLinkPartner(channel.ChannelIndex), linkType);
		channel.AddLink(linkPartner, new BaseLink());
	}

	private LinkPartner CreateRoomLink(LinkPartner partner, CcLinkType type)
	{
		byte channelIndex;
		switch (type)
		{
		case CcLinkType.Scheduler:
			channelIndex = Scheduler.ChannelIndex;
			Scheduler.AddLink(partner, new ActuatorLink());
			break;
		case CcLinkType.SetPoint:
			channelIndex = SetPoint.ChannelIndex;
			SetPoint.AddLink(partner, new SetPointLink());
			break;
		case CcLinkType.WindowReduction:
			channelIndex = WindowReduction.ChannelIndex;
			WindowReduction.AddLink(partner, new WindowReductionLink
			{
				OpenTemperatureCentigrade = WindowReduction.WindowOpenTemperatureCentigrade
			});
			AutomaticWindowReductionLinks.Add(partner);
			break;
		default:
			throw new IncompatibleLinkTypeException();
		}
		return CreateLinkPartner(channelIndex);
	}

	public void CreateInternalLinks(IEnumerable<ActuatorConfiguration> actuatorConfigurations)
	{
		AutoManualButton.Links[CreateLinkPartner(Scheduler.ChannelIndex)] = new BaseLink();
		Scheduler.Links[CreateLinkPartner(AutoManualButton.ChannelIndex)] = new ActuatorLink();
		RotaryWheel.Links[CreateLinkPartner(SetPoint.ChannelIndex)] = new BaseLink();
		SetPoint.Links[CreateLinkPartner(RotaryWheel.ChannelIndex)] = new SetPointLink();
		if (SoftWindowDetection != null)
		{
			SoftWindowDetection.Links[CreateLinkPartner(WindowReduction.ChannelIndex)] = new BaseLink();
			WindowReduction.Links[CreateLinkPartner(SoftWindowDetection.ChannelIndex)] = new WindowReductionLink();
		}
		if (!isWrt && InternalValve != null)
		{
			InternalValve.Links[CreateLinkPartner(ThermostatSensor.ChannelIndex)] = new ActuatorLink();
		}
	}

	private static SwitchAction Convert(ProfileAction profileAction)
	{
		return profileAction switch
		{
			ProfileAction.On => SwitchAction.On, 
			ProfileAction.Off => SwitchAction.Off, 
			ProfileAction.Toggle => SwitchAction.Toggle, 
			ProfileAction.NoAction => SwitchAction.Default, 
			_ => throw new ArgumentOutOfRangeException("profileAction"), 
		};
	}

	private LinkPartner CreateLinkPartner(byte channelIndex)
	{
		return new LinkPartner(base.PhysicalDeviceId, base.PhysicalAddress, channelIndex);
	}

	private static ThermostateDisplayMode GetDisplayMode(BaseDevice ccDevice)
	{
		if (ccDevice != null)
		{
			string stringValue = ccDevice.Properties.GetStringValue("DisplayCurrentTemperature");
			if (stringValue == "CurrentTemperature")
			{
				return ThermostateDisplayMode.CurrentTemperature;
			}
		}
		return ThermostateDisplayMode.TargetTemperatue;
	}
}
