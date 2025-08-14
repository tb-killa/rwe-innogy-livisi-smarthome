using System.Threading;

namespace RWE.SmartHome.SHC.HCI;

internal class AnswerContainer
{
	private ManualResetEvent processed;

	public ManualResetEvent WaitHandle { get; private set; }

	public ManualResetEvent Processed
	{
		get
		{
			lock (this)
			{
				if (processed == null)
				{
					processed = new ManualResetEvent(initialState: false);
				}
			}
			return processed;
		}
	}

	public byte[] Answer { get; set; }

	internal void MarkComplete()
	{
		lock (this)
		{
			if (processed != null)
			{
				processed.Set();
			}
		}
	}

	public AnswerContainer()
	{
		WaitHandle = new ManualResetEvent(initialState: false);
	}
}
