using System;
using System.Collections.Generic;
using System.Threading;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public class BlockingQueue<T>
{
	private readonly List<T> queue;

	private readonly EventWaitHandle waitHandle;

	public BlockingQueue()
	{
		queue = new List<T>();
		waitHandle = new EventWaitHandle(initialState: false, EventResetMode.ManualReset);
	}

	public T Dequeue()
	{
		T result = default(T);
		waitHandle.WaitOne();
		lock (queue)
		{
			if (queue.Count > 0)
			{
				result = queue[0];
				queue.RemoveAt(0);
			}
			if (queue.Count == 0)
			{
				waitHandle.Reset();
			}
		}
		return result;
	}

	public void Enqueue(T container)
	{
		Enqueue(container, (T f) => false);
	}

	public void Enqueue(T container, Predicate<T> replacePredicate)
	{
		lock (queue)
		{
			if (!queue.Exists(replacePredicate))
			{
				queue.Add(container);
				waitHandle.Set();
			}
		}
	}

	public void Remove(T container)
	{
		lock (queue)
		{
			if (queue.Count == 1)
			{
				waitHandle.Reset();
			}
			queue.Remove(container);
		}
	}

	public void RemoveAll(Predicate<T> match)
	{
		lock (queue)
		{
			queue.RemoveAll(match);
		}
	}

	public void Clear()
	{
		lock (queue)
		{
			waitHandle.Reset();
			queue.Clear();
		}
	}

	public void Abort()
	{
		waitHandle.Reset();
	}
}
