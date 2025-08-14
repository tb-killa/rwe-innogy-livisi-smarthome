using System;
using System.Collections.Generic;
using System.Linq;

namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public class PriorityQueue<TValue, TPriority>
{
	private readonly IComparer<TPriority> comparer;

	private readonly Predicate<TValue> priorityBoost = (TValue TValue) => false;

	private readonly object syncRoot = new object();

	private readonly List<PriorityQueueItem<TValue, TPriority>> internalList = new List<PriorityQueueItem<TValue, TPriority>>();

	public int Count
	{
		get
		{
			lock (syncRoot)
			{
				return internalList.Count;
			}
		}
	}

	public PriorityQueue(IComparer<TPriority> comparer)
	{
		if (comparer == null)
		{
			throw new ArgumentNullException("comparer");
		}
		this.comparer = comparer;
	}

	public PriorityQueue(IComparer<TPriority> comparer, Predicate<TValue> priorityBoost)
	{
		if (comparer == null)
		{
			throw new ArgumentNullException("comparer");
		}
		this.priorityBoost = priorityBoost;
		this.comparer = comparer;
	}

	public PriorityQueue()
	{
		comparer = Comparer<TPriority>.Default;
	}

	public virtual void Enqueue(PriorityQueueItem<TValue, TPriority> item)
	{
		lock (syncRoot)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			bool flag = false;
			int count = internalList.Count;
			for (int i = 0; i < count; i++)
			{
				TPriority priority = internalList[i].Priority;
				int num = comparer.Compare(priority, item.Priority);
				if (!priorityBoost(internalList[i].Value) && (priorityBoost(item.Value) || num < 0))
				{
					internalList.Insert(i, item);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				internalList.Add(item);
			}
		}
	}

	public virtual void Enqueue(TValue value, TPriority priority)
	{
		PriorityQueueItem<TValue, TPriority> item = new PriorityQueueItem<TValue, TPriority>(value, priority);
		Enqueue(item);
	}

	public PriorityQueueItem<TValue, TPriority> DequeueItem()
	{
		lock (syncRoot)
		{
			int count = internalList.Count;
			if (count <= 0)
			{
				throw new InvalidOperationException("Queue empty.");
			}
			PriorityQueueItem<TValue, TPriority> result = internalList[0];
			internalList.RemoveAt(0);
			return result;
		}
	}

	public TValue Dequeue()
	{
		return DequeueItem().Value;
	}

	public TValue Peek()
	{
		lock (syncRoot)
		{
			if (internalList.Count <= 0)
			{
				throw new InvalidOperationException("Queue empty.");
			}
			return internalList[0].Value;
		}
	}

	public PriorityQueueItem<TValue, TPriority> PeekItem()
	{
		lock (syncRoot)
		{
			if (internalList.Count <= 0)
			{
				throw new InvalidOperationException("Queue empty.");
			}
			return internalList[0];
		}
	}

	public void Remove(TValue value)
	{
		lock (syncRoot)
		{
			for (int i = 0; i < internalList.Count; i++)
			{
				if (internalList[i].Value.Equals(value))
				{
					internalList.RemoveAt(i);
					break;
				}
			}
		}
	}

	public List<TValue> Remove(Predicate<TValue> predicate)
	{
		List<PriorityQueueItem<TValue, TPriority>> list;
		lock (syncRoot)
		{
			list = internalList.FindAll((PriorityQueueItem<TValue, TPriority> item) => predicate(item.Value));
			list.ForEach(delegate(PriorityQueueItem<TValue, TPriority> item)
			{
				internalList.Remove(item);
			});
		}
		return list.Select((PriorityQueueItem<TValue, TPriority> item) => item.Value).ToList();
	}

	public bool Contains(Predicate<TValue> predicate)
	{
		return internalList.Any((PriorityQueueItem<TValue, TPriority> priorityQueueItem) => predicate(priorityQueueItem.Value));
	}
}
