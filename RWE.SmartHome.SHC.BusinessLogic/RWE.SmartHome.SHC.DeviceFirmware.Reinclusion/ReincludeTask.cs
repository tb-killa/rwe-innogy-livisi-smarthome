using System;
using RWE.SmartHome.SHC.Core.Scheduler;

namespace RWE.SmartHome.SHC.DeviceFirmware.Reinclusion;

internal class ReincludeTask : SchedulerTaskBase
{
	public DateTime DueTime { get; set; }

	public ReincludeTask(Guid id, Action taskAction, bool runOnce)
		: base(id, taskAction, runOnce)
	{
	}

	public override bool ShouldExecute(DateTime now)
	{
		return now >= DueTime;
	}
}
