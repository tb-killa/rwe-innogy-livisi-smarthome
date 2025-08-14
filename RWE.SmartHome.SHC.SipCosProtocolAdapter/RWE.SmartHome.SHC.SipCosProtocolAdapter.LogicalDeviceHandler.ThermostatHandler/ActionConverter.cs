using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control.ActuatorStates;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;
using RWE.SmartHome.SHC.DomainModel.Actions;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler.ThermostatHandler;

public class ActionConverter
{
	private readonly IRepository cfgRepo;

	private readonly TemperatureActuatorStateCache stateCache;

	private readonly BuiltinPhysicalDeviceType[] ccDevices = new BuiltinPhysicalDeviceType[3]
	{
		BuiltinPhysicalDeviceType.WRT,
		BuiltinPhysicalDeviceType.RST,
		BuiltinPhysicalDeviceType.RST2
	};

	public ActionConverter(IRepository cfgRepo, TemperatureActuatorStateCache stateCache)
	{
		this.cfgRepo = cfgRepo;
		this.stateCache = stateCache;
	}

	public IEnumerable<SwitchSettings> CreateSwitchSettings(ActionContext ac, ThermostatActuator thermostat, OperationMode? opMode, decimal? targetTemp)
	{
		List<SwitchSettings> list = new List<SwitchSettings>();
		if (opMode.HasValue)
		{
			list.AddRange(CreateSchedulerModeLocationDec(thermostat.BaseDevice.LocationId, opMode.Value));
		}
		if (targetTemp.HasValue && !IgnoreStateSetting(ac, thermostat, targetTemp.Value))
		{
			list.Add(CreateSetpointLocationDec(targetTemp.Value, ac.Type, thermostat.BaseDevice.LocationId));
		}
		return list;
	}

	private SwitchSettingsDirectExecution CreateSetpointDec(decimal targetTempDegrees)
	{
		return CreateDecForChannel(ClimateControlChannelIndex.SetpointChannel, (byte)Math.Round(targetTempDegrees * 2m));
	}

	private SwitchSettingsDirectExecution CreateSchedulerModeDec(OperationMode opMode)
	{
		return CreateDecForChannel(ClimateControlChannelIndex.SchedulerChannel, (byte)opMode);
	}

	private SwitchSettings CreateSetpointLocationDec(decimal targetTemp, ContextType contextType, Guid? locationId)
	{
		SwitchSettings switchSettings = CreateSetpointDec(targetTemp);
		if (locationId.HasValue && contextType == ContextType.ClientRequest)
		{
			BaseDevice baseDevice = GetLocationWRTs(locationId.Value).FirstOrDefault();
			if (baseDevice != null)
			{
				switchSettings.DeviceId = baseDevice.Id;
			}
		}
		return switchSettings;
	}

	private IEnumerable<BaseDevice> GetLocationWRTs(Guid locationId)
	{
		return from bd in cfgRepo.GetBaseDevices()
			where bd.LocationId.HasValue && bd.LocationId.Value == locationId && bd.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.WRT
			orderby bd.TimeOfAcceptance
			select bd;
	}

	private IEnumerable<SwitchSettings> CreateSchedulerModeLocationDec(Guid? locationId, OperationMode opMode)
	{
		IEnumerable<SwitchSettings> result = Enumerable.Empty<SwitchSettings>();
		if (locationId.HasValue)
		{
			IEnumerable<BaseDevice> source = (from m in cfgRepo.GetBaseDevices()
				where m.LocationId.HasValue && m.LocationId.Value == locationId.Value
				select m).Where(IsCCDevice);
			if (source.Any((BaseDevice m) => m.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.WRT))
			{
				source = source.Where((BaseDevice m) => m.GetBuiltinDeviceDeviceType() == BuiltinPhysicalDeviceType.WRT);
			}
			result = source.Select((Func<BaseDevice, SwitchSettings>)((BaseDevice m) => CreateDecForChannel(ClimateControlChannelIndex.SchedulerChannel, (byte)opMode, m.Id))).ToList();
		}
		return result;
	}

	private bool IsCCDevice(BaseDevice device)
	{
		return ccDevices.Contains(device.GetBuiltinDeviceDeviceType());
	}

	private SwitchSettingsDirectExecution CreateDecForChannel(ClimateControlChannelIndex channel, byte decValue)
	{
		return new SwitchSettingsDirectExecution(RampMode.RampStart, 0, (byte)channel, decValue, 0);
	}

	private SwitchSettingsDirectExecution CreateDecForChannel(ClimateControlChannelIndex channel, byte decValue, Guid deviceId)
	{
		SwitchSettingsDirectExecution switchSettingsDirectExecution = new SwitchSettingsDirectExecution(RampMode.RampStart, 0, (byte)channel, decValue, 0);
		switchSettingsDirectExecution.DeviceId = deviceId;
		return switchSettingsDirectExecution;
	}

	private bool IsVrccSync(ActionContext ac)
	{
		if (ac != null)
		{
			return ac.Type == ContextType.ClimateControlSync;
		}
		return false;
	}

	private bool IsWrContext(ThermostatActuator thermostat)
	{
		bool flag = false;
		IEnumerable<ThermostatActuator> source = cfgRepo.GetLogicalDevices().OfType<ThermostatActuator>().Where(delegate(ThermostatActuator ld)
		{
			Guid? locationId = ld.BaseDevice.LocationId;
			Guid? locationId2 = thermostat.BaseDevice.LocationId;
			return locationId == locationId2;
		});
		if (source.Any((ThermostatActuator rt) => rt.BaseDevice.DeviceType == BuiltinPhysicalDeviceType.WRT.ToString()) && thermostat.BaseDevice.DeviceType != BuiltinPhysicalDeviceType.WRT.ToString())
		{
			return false;
		}
		return source.Any((ThermostatActuator wrt) => stateCache.GetPhysicalState(wrt.Id).IsWindowReduction == true);
	}

	private bool IgnoreStateSetting(ActionContext ac, ThermostatActuator targetDevice, decimal targetTemp)
	{
		if (targetDevice != null && targetTemp == targetDevice.WindowOpenTemperature && IsVrccSync(ac) && IsWrContext(targetDevice))
		{
			return true;
		}
		return false;
	}
}
