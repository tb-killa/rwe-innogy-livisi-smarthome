namespace WebSocketLibrary.Managers.Frames;

public class ReceiverBuffersManager
{
	private readonly BlockingBuffer waitingBuffer = new BlockingBuffer();

	private readonly BlockingBuffer readingBuffer = new BlockingBuffer();

	private bool isStopped;

	public void StopWaiting()
	{
		isStopped = true;
		waitingBuffer.ReleaseLock();
		readingBuffer.ReleaseLock();
	}

	public void AddWaitingBuffer(ReceiverBuffer buffer)
	{
		waitingBuffer.AddBuffer(buffer);
	}

	public void AddReadyBuffer(ReceiverBuffer buffer)
	{
		readingBuffer.AddBuffer(buffer);
	}

	public ReceiverBuffer GetWaitingBuffer()
	{
		return waitingBuffer.GetBufferWhenExists();
	}

	public ReceiverBuffer GetReadyBuffer()
	{
		return readingBuffer.GetBufferWhenExists();
	}
}
