using System;
using System.Collections;

namespace RWE.SmartHome.SHC.WebSocketsService.Client;

internal class WSFrameQueue : IDisposable
{
	private int maxCount;

	private Queue queue;

	public int Count
	{
		get
		{
			int num = 0;
			lock (queue.SyncRoot)
			{
				return queue.Count;
			}
		}
	}

	public WSFrameQueue(int maxCount)
	{
		this.maxCount = maxCount;
		queue = new Queue();
	}

	public void Dispose()
	{
		if (queue != null)
		{
			queue.Clear();
			queue = null;
		}
	}

	public void Enqueue(WSFrame wsFrame)
	{
		lock (queue.SyncRoot)
		{
			if (queue.Count == maxCount)
			{
				queue.Dequeue();
			}
			queue.Enqueue(wsFrame);
		}
	}

	public WSFrame Dequeue()
	{
		WSFrame wSFrame = null;
		lock (queue.SyncRoot)
		{
			return (WSFrame)queue.Dequeue();
		}
	}

	public WSFrame Peek()
	{
		WSFrame wSFrame = null;
		lock (queue.SyncRoot)
		{
			return (WSFrame)queue.Peek();
		}
	}

	public void Poke(WSFrame wsFrame)
	{
		lock (queue.SyncRoot)
		{
			if (queue.Count > maxCount)
			{
				queue.Dequeue();
			}
			int count = queue.Count;
			queue.Enqueue(wsFrame);
			while (count > 0)
			{
				queue.Enqueue(queue.Dequeue());
			}
		}
	}

	public void Clear()
	{
		lock (queue.SyncRoot)
		{
			queue.Clear();
		}
	}
}
