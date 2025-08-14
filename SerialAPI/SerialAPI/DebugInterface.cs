using System;
using System.Collections.Generic;

namespace SerialAPI;

public abstract class DebugInterface
{
	public delegate void ReceiveDebugInfo(string data);

	public delegate void WatchDogCall();

	public delegate void SequenceCountProblem(byte[] Address, uint OldSequenceCount, uint NewSequenceCount);

	private List<DebugInterface> parrents = new List<DebugInterface>();

	private DateTime m_last = DateTime.Now;

	public event ReceiveDebugInfo ReceiveDebugData;

	public event WatchDogCall ReceiveWatchDogCall;

	public event SequenceCountProblem ReceiveSequenceProblem;

	protected void debug(string str)
	{
	}

	protected void WatchDog()
	{
		if (parrents.Count != 0)
		{
			foreach (DebugInterface parrent in parrents)
			{
				parrent.WatchDog();
			}
			return;
		}
		if (this.ReceiveWatchDogCall != null)
		{
			this.ReceiveWatchDogCall();
		}
	}

	protected void SequenceProblem(byte[] Address, uint OldSequenceCount, uint NewSequenceCount)
	{
		if (parrents.Count != 0)
		{
			foreach (DebugInterface parrent in parrents)
			{
				parrent.SequenceProblem(Address, OldSequenceCount, NewSequenceCount);
			}
			return;
		}
		if (this.ReceiveSequenceProblem != null)
		{
			this.ReceiveSequenceProblem(Address, OldSequenceCount, NewSequenceCount);
		}
	}

	protected void hookup(DebugInterface other)
	{
		if (!other.parrents.Contains(this))
		{
			other.parrents.Add(this);
		}
	}
}
