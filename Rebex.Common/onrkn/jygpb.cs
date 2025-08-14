using System;
using System.Threading;

namespace onrkn;

internal class jygpb : IDisposable
{
	private Action hcqcj;

	public jygpb(Action disposeAction)
	{
		if (disposeAction == null || 1 == 0)
		{
			throw new ArgumentNullException("disposeAction");
		}
		hcqcj = disposeAction;
	}

	public void Dispose()
	{
		Action action = Interlocked.Exchange(ref hcqcj, null);
		if (action != null && 0 == 0)
		{
			action();
		}
	}
}
