using System;
using System.Collections.Generic;
using System.Threading;

namespace RWE.SmartHome.SHC.Core;

public class Dispatcher : Task, IDispatcher
{
	private volatile bool endLoop;

	private readonly object syncRoot = new object();

	private readonly Queue<IExecutable> queue = new Queue<IExecutable>();

	private readonly ManualResetEvent signal = new ManualResetEvent(initialState: false);

	public override void Stop()
	{
		lock (syncRoot)
		{
			endLoop = true;
			signal.Set();
		}
	}

	public void Dispatch(IExecutable executable)
	{
		lock (syncRoot)
		{
			if (!endLoop)
			{
				queue.Enqueue(executable);
				signal.Set();
			}
		}
	}

	protected override void Run()
	{
		try
		{
			while (!endLoop)
			{
				signal.WaitOne();
				if (endLoop)
				{
					continue;
				}
				IExecutable executable;
				lock (syncRoot)
				{
					executable = queue.Dequeue();
					if (queue.Count == 0)
					{
						signal.Reset();
					}
				}
				try
				{
					executable.Execute();
				}
				catch (Exception ex)
				{
					Console.WriteLine("An exception occured while interpreting an event. Keeping the Dispatcher alive. Exception: " + ex.ToString());
				}
			}
		}
		finally
		{
			lock (syncRoot)
			{
				signal.Close();
			}
		}
	}
}
