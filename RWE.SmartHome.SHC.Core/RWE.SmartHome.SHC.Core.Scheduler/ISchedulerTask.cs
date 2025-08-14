using System;

namespace RWE.SmartHome.SHC.Core.Scheduler;

public interface ISchedulerTask
{
	Guid TaskId { get; }

	bool RunOnce { get; }

	Action TaskAction { get; }

	bool ShouldExecute(DateTime now);
}
