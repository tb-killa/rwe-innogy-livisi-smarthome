using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.CoreEvents;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices;

public class VirtualResidentHandler
{
	private IRepository configRepo;

	private IScheduler scheduler;

	private IEventManager eventManager;

	private List<Guid> virtualResidentTasks = new List<Guid>();

	private DateTime lastRefreshVRTasks = ShcDateTime.UtcNow;

	public VirtualResidentHandler(IRepository configRepo, IScheduler scheduler, IEventManager eventManager)
	{
		this.configRepo = configRepo;
		this.scheduler = scheduler;
		this.eventManager = eventManager;
		scheduler.AddSchedulerTask(new FixedTimeSchedulerTask(Guid.NewGuid(), ScheduleVRTasksRegeneration, ShcDateTime.UtcNow.TimeOfDay));
	}

	public void RefreshVirtualResidentTasks()
	{
		lastRefreshVRTasks = ShcDateTime.UtcNow;
		virtualResidentTasks.ForEach(delegate(Guid x)
		{
			scheduler.RemoveSchedulerTask(x);
		});
		virtualResidentTasks.Clear();
		IEnumerable<Rule> source = configRepo.GetInteractions().SelectMany((Interaction i) => i.Rules);
		IEnumerable<CustomTrigger> enumerable = source.Where((Rule r) => r.CustomTriggers != null).SelectMany((Rule r) => r.CustomTriggers);
		if (enumerable == null || !enumerable.Any())
		{
			return;
		}
		IEnumerable<CustomTrigger> enumerable2 = enumerable.Where((CustomTrigger t) => t.Type == VirtualResidentConstants.InitiatorCustomTriggerType && t.Properties.Any((Property p) => p.Name == VirtualResidentConstants.InitiatorId));
		Guid initiatorId = default(Guid);
		foreach (CustomTrigger initiatorCT in enumerable2)
		{
			ref Guid reference = ref initiatorId;
			reference = new Guid(initiatorCT.Properties.First((Property p) => p.Name == VirtualResidentConstants.InitiatorId).GetValueAsString());
			CustomTrigger followerCustomTrigger = enumerable.FirstOrDefault((CustomTrigger t) => t.Type == VirtualResidentConstants.FollowerCustomTriggerType && t.Properties.Any((Property p) => p.Name == VirtualResidentConstants.MasterTriggerIdProperty) && new Guid(t.Properties.First((Property p) => p.Name == VirtualResidentConstants.MasterTriggerIdProperty).GetValueAsString()) == initiatorId);
			GetInitiatorExecutionTimes(initiatorCT).ToList()?.ForEach(delegate(TimeSpan initiatorExecutionTime)
			{
				AddCustomTriggerTask(initiatorCT, initiatorExecutionTime);
				if (followerCustomTrigger != null)
				{
					TimeSpan? followerExecutionTime = GetFollowerExecutionTime(followerCustomTrigger, initiatorExecutionTime);
					if (followerExecutionTime.HasValue && followerExecutionTime.HasValue)
					{
						AddCustomTriggerTask(followerCustomTrigger, followerExecutionTime.Value);
					}
				}
			});
		}
	}

	private void ScheduleVRTasksRegeneration()
	{
		if (ShcDateTime.UtcNow.Date != lastRefreshVRTasks.Date)
		{
			RefreshVirtualResidentTasks();
			Log.Information(Module.BusinessLogic, "Virtual Resident tasks were regenerated on a daily basis.");
		}
		else
		{
			Log.Information(Module.BusinessLogic, $"Virtual Resident tasks were not regenerated because their last refresh was on {lastRefreshVRTasks} .");
		}
	}

	private TimeSpan? GetFollowerExecutionTime(CustomTrigger followerCustomTrigger, TimeSpan time)
	{
		Property property = followerCustomTrigger.Properties.FirstOrDefault((Property p) => p.Name == VirtualResidentConstants.MinDelayTimeProperty);
		Property property2 = followerCustomTrigger.Properties.FirstOrDefault((Property p) => p.Name == VirtualResidentConstants.MaxDelayTimeProperty);
		if (property != null && property is NumericProperty && property2 != null && property2 is NumericProperty)
		{
			int num = (int)(property as NumericProperty).Value.Value;
			int num2 = (int)(property2 as NumericProperty).Value.Value;
			if (num > num2)
			{
				return null;
			}
			if (time.Add(TimeSpan.FromSeconds(num2)).CompareTo(TimeSpan.FromDays(1.0)) > 0)
			{
				return null;
			}
			Random random = new Random();
			return time.Add(TimeSpan.FromSeconds(random.Next(num, num2)));
		}
		return null;
	}

	private void AddCustomTriggerTask(CustomTrigger customTrigger, TimeSpan time)
	{
		Guid guid = Guid.NewGuid();
		scheduler.AddSchedulerTask(new FixedTimeSchedulerTask(guid, delegate
		{
			FireCustomTrigger(customTrigger.Id);
		}, time));
		virtualResidentTasks.Add(guid);
	}

	private void FireCustomTrigger(Guid guid)
	{
		eventManager.GetEvent<CustomTriggerEvent>().Publish(new CustomTriggerEventArgs(guid));
	}

	private IEnumerable<TimeSpan> GetInitiatorExecutionTimes(CustomTrigger initiatorCT)
	{
		Property property = initiatorCT.Properties.FirstOrDefault((Property p) => p.Name == VirtualResidentConstants.MaxSwitchCountProperty);
		Property property2 = initiatorCT.Properties.FirstOrDefault((Property p) => p.Name == VirtualResidentConstants.StartTimeProperty);
		Property property3 = initiatorCT.Properties.FirstOrDefault((Property p) => p.Name == VirtualResidentConstants.EndTimeProperty);
		if (property2 != null && property2 is StringProperty && property3 != null && property3 is StringProperty)
		{
			decimal num = ((property != null && property is NumericProperty) ? (property as NumericProperty).Value.Value : ((decimal)VirtualResidentConstants.MaxSwitchCountMinimumValue));
			TimeSpan timeSpan = TimeSpan.Parse((property2 as StringProperty).Value);
			TimeSpan timeSpan2 = TimeSpan.Parse((property3 as StringProperty).Value);
			TimeSpan timeSpan3 = ((timeSpan2 > timeSpan) ? (timeSpan2 - timeSpan) : (TimeSpan.FromHours(24.0) - timeSpan + timeSpan2));
			if (timeSpan3.Ticks < 0)
			{
				return null;
			}
			Random random = new Random();
			int num2 = random.Next(VirtualResidentConstants.MaxSwitchCountMinimumValue, (int)num);
			TimeSpan[] array = new TimeSpan[num2];
			for (int num3 = 0; num3 < num2; num3++)
			{
				ref TimeSpan reference = ref array[num3];
				reference = timeSpan.Add(TimeSpan.FromSeconds(random.Next(0, (int)timeSpan3.TotalSeconds)));
			}
			return array.OrderBy((TimeSpan x) => x);
		}
		return null;
	}
}
