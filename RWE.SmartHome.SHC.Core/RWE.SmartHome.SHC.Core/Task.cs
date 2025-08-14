using System;
using System.Threading;

namespace RWE.SmartHome.SHC.Core;

public abstract class Task : ITask
{
	private readonly ManualResetEvent waitHandle;

	private readonly Thread thread;

	public int ManagedThreadId => thread.ManagedThreadId;

	public WaitHandle WaitHandle => waitHandle;

	public string Name
	{
		get
		{
			return thread.Name;
		}
		set
		{
			thread.Name = value;
		}
	}

	protected Task()
	{
		waitHandle = new ManualResetEvent(initialState: false);
		thread = new Thread(Work);
	}

	public virtual void Start()
	{
		thread.Start();
	}

	public abstract void Stop();

	public void Join()
	{
		Join(-1);
	}

	public bool Join(int milliSecondsTimeout)
	{
		if (Thread.CurrentThread.ManagedThreadId == thread.ManagedThreadId)
		{
			return false;
		}
		return thread.Join(milliSecondsTimeout);
	}

	protected abstract void Run();

	public override int GetHashCode()
	{
		return thread.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		return thread.Equals(obj);
	}

	private void Work()
	{
		try
		{
			Run();
		}
		catch (Exception arg)
		{
			Console.WriteLine("Thread [{0}] aborted and threw exception: {1}", Name, arg);
		}
		finally
		{
			waitHandle.Set();
		}
	}
}
