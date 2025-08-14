using System;
using System.Collections.Generic;
using System.Threading;

namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public class WaitingMaxItemCollection<T>
{
	private readonly List<T> items;

	private readonly int maximumCount;

	private readonly ManualResetEvent waitObject = new ManualResetEvent(initialState: true);

	private readonly ManualResetEvent queueEmpty = new ManualResetEvent(initialState: true);

	public int Count => items.Count;

	public T this[int index] => items[index];

	public WaitingMaxItemCollection(int maximumCount)
	{
		this.maximumCount = maximumCount;
		items = new List<T>(maximumCount);
	}

	public void Wait(Action check)
	{
		check();
		while (!waitObject.WaitOne(300, exitContext: false))
		{
			check();
		}
	}

	public void WaitUntilEmpty(Action check)
	{
		check();
		while (!queueEmpty.WaitOne(300, exitContext: false))
		{
			check();
		}
	}

	public void Wait()
	{
		waitObject.WaitOne();
	}

	public bool Contains(T item)
	{
		return items.Contains(item);
	}

	public void Add(T item)
	{
		if (items.Count >= maximumCount)
		{
			throw new InvalidOperationException("Collection is full.");
		}
		items.Add(item);
		if (items.Count == maximumCount)
		{
			waitObject.Reset();
		}
		queueEmpty.Reset();
	}

	public void Remove(T item)
	{
		items.Remove(item);
		SetEvents();
	}

	public void RemoveAll(Predicate<T> match)
	{
		items.RemoveAll(match);
		SetEvents();
	}

	public void RemoveAt(int index)
	{
		items.RemoveAt(index);
		SetEvents();
	}

	private void SetEvents()
	{
		if (items.Count < maximumCount)
		{
			waitObject.Set();
		}
		if (items.Count == 0)
		{
			queueEmpty.Set();
		}
	}
}
