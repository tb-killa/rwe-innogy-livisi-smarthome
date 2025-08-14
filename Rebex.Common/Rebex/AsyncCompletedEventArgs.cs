using System;
using System.Reflection;

namespace Rebex;

public class AsyncCompletedEventArgs : EventArgs
{
	private bool palhd;

	private Exception dwndi;

	private object zjiil;

	public bool Cancelled => palhd;

	public Exception Error => dwndi;

	public object UserState => zjiil;

	public AsyncCompletedEventArgs(Exception error, bool cancelled, object userState)
	{
		dwndi = error;
		palhd = cancelled;
		zjiil = userState;
	}

	protected void RaiseExceptionIfNecessary()
	{
		if (dwndi != null && 0 == 0)
		{
			throw new TargetInvocationException("An error occurred during the operation.", dwndi);
		}
		if (palhd && 0 == 0)
		{
			throw new InvalidOperationException("Operation was cancelled.");
		}
	}
}
