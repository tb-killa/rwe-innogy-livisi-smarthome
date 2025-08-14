using System.Collections.Generic;
using System.Threading;

namespace WebSocketLibrary.Managers.Frames;

public class BlockingBuffer
{
	private readonly Queue<ReceiverBuffer> buffers = new Queue<ReceiverBuffer>();

	private readonly ManualResetEvent semaphore = new ManualResetEvent(initialState: false);

	private readonly object sync = new object();

	private bool isReleased;

	public void AddBuffer(ReceiverBuffer buffer)
	{
		lock (sync)
		{
			buffers.Enqueue(buffer);
			semaphore.Set();
		}
	}

	public ReceiverBuffer GetBufferWhenExists()
	{
		while (!isReleased)
		{
			semaphore.WaitOne();
			lock (sync)
			{
				if (buffers.Count > 0 || isReleased)
				{
					ReceiverBuffer result = ((buffers.Count > 0) ? buffers.Dequeue() : null);
					if (buffers.Count <= 0)
					{
						semaphore.Reset();
					}
					return result;
				}
			}
		}
		return null;
	}

	public void ReleaseLock()
	{
		isReleased = true;
		semaphore.Set();
	}
}
