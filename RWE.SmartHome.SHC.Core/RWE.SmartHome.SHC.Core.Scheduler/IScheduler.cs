using System;
using System.Collections;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.Core.Scheduler;

public interface IScheduler : IEnumerable<ISchedulerTask>, IEnumerable
{
	bool IsRunning { get; }

	int Count { get; }

	void AddSchedulerTask(ISchedulerTask schedulerTask);

	void RemoveSchedulerTask(Guid taskId);
}
