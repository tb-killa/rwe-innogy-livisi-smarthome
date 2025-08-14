using System;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core.Scheduler.Tasks;

public class FixDateFromPersistenceSchedulerTask : SchedulerTaskBase
{
	private DateTime executionTime;

	public override Action TaskAction
	{
		get
		{
			Action action = base.TaskAction;
			return delegate
			{
				try
				{
					action();
				}
				catch (Exception ex)
				{
					Log.Exception(Module.Core, ex, "Error occured in executing FixDateFromPersistenceSchedulerTask Action");
				}
			};
		}
	}

	public FixDateFromPersistenceSchedulerTask(Guid id, Action taskAction, DateTime stopBackendRequestsDate)
		: base(id, taskAction, runOnce: false)
	{
		executionTime = stopBackendRequestsDate;
	}

	public override bool ShouldExecute(DateTime now)
	{
		return ShcDateTime.Now >= executionTime;
	}
}
