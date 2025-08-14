using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper.Configurations;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration;

public class CustomTriggerProcessor
{
	internal class TimeScheduleDetails
	{
		public TimeSpan StartTime { get; set; }

		public byte DaysOfWeek { get; set; }
	}

	private IRepository configRepository;

	public CustomTriggerProcessor(IRepository configRepo)
	{
		configRepository = configRepo;
	}

	public bool ProcessCustomTriggers(CustomTrigger trigger, ActionDescription action, ActuatorConfiguration actuatorConfiguration)
	{
		if (!SupportedTimeTrigger(trigger))
		{
			return false;
		}
		return TryScheduleAction(action, GetScheduleDetails(trigger), actuatorConfiguration);
	}

	private bool TryScheduleAction(ActionDescription action, List<TimeScheduleDetails> scheduleDetails, ActuatorConfiguration actuatorConfiguration)
	{
		foreach (TimeScheduleDetails scheduleDetail in scheduleDetails)
		{
			if (!actuatorConfiguration.AddDeviceSetpoint(scheduleDetail.StartTime, scheduleDetail.DaysOfWeek, action))
			{
				return false;
			}
		}
		return true;
	}

	private bool SupportedTimeTrigger(CustomTrigger ct)
	{
		if (IsCustomTriggerValid(ct))
		{
			Guid id = ct.Entity.EntityIdAsGuid();
			LogicalDevice logicalDevice = configRepository.GetLogicalDevice(id);
			if (logicalDevice.DeviceType != "Calendar")
			{
				return false;
			}
			if (ct.Properties == null || !ct.Properties.Any())
			{
				return false;
			}
			decimal? decimalValue = ct.Properties.GetDecimalValue("RecurrenceInterval");
			if (!(ct.Type == "DailyTrigger"))
			{
				if (ct.Type == "WeeklyTrigger")
				{
					if (decimalValue.HasValue)
					{
						return decimalValue.Value < 2m;
					}
					return true;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool IsCustomTriggerValid(CustomTrigger ct)
	{
		if (ct != null && ct.Entity != null)
		{
			return IsLinkValid(ct.Entity);
		}
		Log.Error(Module.TechnicalConfiguration, "CustomTrigger null or has invalid link.");
		return false;
	}

	private bool IsLinkValid(LinkBinding link)
	{
		switch (link.LinkType)
		{
		case EntityType.LogicalDevice:
		{
			LogicalDevice logicalDevice = configRepository.GetLogicalDevice(link.EntityIdAsGuid());
			return logicalDevice != null;
		}
		case EntityType.BaseDevice:
		{
			BaseDevice baseDevice = configRepository.GetBaseDevice(link.EntityIdAsGuid());
			return baseDevice != null;
		}
		default:
			return false;
		}
	}

	internal List<TimeScheduleDetails> GetScheduleDetails(CustomTrigger ct)
	{
		List<TimeSpan> list = new List<TimeSpan>();
		byte dayOfWeek = 127;
		list = (from p in ct.Properties.OfType<StringProperty>()
			where p.Name == "StartTime"
			select TimeSpan.Parse(p.Value)).ToList();
		if (ct.Type == "WeeklyTrigger")
		{
			dayOfWeek = Convert.ToByte(ct.Properties.GetDecimalValue("DayOfWeek"));
		}
		return list.Select((TimeSpan st) => new TimeScheduleDetails
		{
			StartTime = st,
			DaysOfWeek = dayOfWeek
		}).ToList();
	}
}
