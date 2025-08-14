using System;
using System.Threading;

namespace onrkn;

internal class faagu : IAsyncResult
{
	private readonly IAsyncResult oquvp;

	private readonly exkzi wfmgs;

	private readonly object guxrh;

	public bool IsCompleted => oquvp.IsCompleted;

	public WaitHandle AsyncWaitHandle => oquvp.AsyncWaitHandle;

	public object AsyncState => guxrh;

	public bool CompletedSynchronously => oquvp.CompletedSynchronously;

	public exkzi ajklf => wfmgs;

	public faagu(exkzi task, object state)
	{
		if (task == null || 1 == 0)
		{
			throw new ArgumentNullException("task");
		}
		wfmgs = task;
		guxrh = state;
		oquvp = task;
	}
}
