using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;

namespace RWE.SmartHome.SHC.BusinessLogic.ConfigurationTransformation;

public class ScheduledStateChangeTask
{
	public Guid ProfileId { get; private set; }

	public TimeSpan Time { get; private set; }

	public WeekDay WeekDays { get; private set; }

	public IEnumerable<ActionDescription> StateChanges { get; private set; }

	public ScheduledStateChangeTask(Guid profileId, TimeSpan time, WeekDay weekDays, IEnumerable<ActionDescription> stateChanges)
	{
		ProfileId = profileId;
		Time = time;
		WeekDays = weekDays;
		StateChanges = stateChanges;
	}
}
