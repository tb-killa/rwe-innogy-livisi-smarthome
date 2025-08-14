using System;
using System.Collections.Generic;
using System.Threading;

namespace SerialAPI;

internal class BlockingQueue<T>
{
	private readonly List<T> queue;

	private readonly EventWaitHandle dataAvailableEvent;

	public BlockingQueue()
	{
		queue = new List<T>();
		dataAvailableEvent = new EventWaitHandle(initialState: false, EventResetMode.ManualReset);
	}

	public T Dequeue()
	{
		T result = default(T);
		dataAvailableEvent.WaitOne();
		lock (queue)
		{
			if (queue.Count > 0)
			{
				result = queue[0];
				queue.RemoveAt(0);
			}
			if (queue.Count == 0)
			{
				dataAvailableEvent.Reset();
			}
		}
		return result;
	}

	public void Enqueue(T container)
	{
		lock (queue)
		{
			queue.Add(container);
			dataAvailableEvent.Set();
		}
	}

	public void Remove(T container)
	{
		lock (queue)
		{
			if (queue.Count == 1)
			{
				dataAvailableEvent.Reset();
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
			dataAvailableEvent.Reset();
			queue.Clear();
		}
	}

	public void Abort()
	{
		dataAvailableEvent.Reset();
	}
}
