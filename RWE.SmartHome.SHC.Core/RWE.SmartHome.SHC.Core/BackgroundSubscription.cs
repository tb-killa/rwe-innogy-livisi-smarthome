using System;
using System.Collections.Generic;
using System.Threading;

namespace RWE.SmartHome.SHC.Core;

public class BackgroundSubscription<TPayload> : Subscription<TPayload>
{
	private const int MAX_EVENT_THREADS = 3;

	private static int pendingWorkItems = 0;

	private static readonly Queue<KeyValuePair<Action<TPayload>, TPayload>> actionsQueue = new Queue<KeyValuePair<Action<TPayload>, TPayload>>();

	public BackgroundSubscription(Action<TPayload> action, Predicate<TPayload> filter)
		: base(action, filter)
	{
	}

	public override void InvokeAction(Action<TPayload> action, TPayload argument)
	{
		if (pendingWorkItems < 3)
		{
			ExecuteAction(action, argument);
			return;
		}
		lock (actionsQueue)
		{
			actionsQueue.Enqueue(new KeyValuePair<Action<TPayload>, TPayload>(action, argument));
			if (pendingWorkItems < 1)
			{
				OnActionCompleted();
			}
		}
	}

	private void ExecuteAction(Action<TPayload> action, TPayload argument)
	{
		Interlocked.Increment(ref pendingWorkItems);
		ThreadPool.QueueUserWorkItem(delegate(object c)
		{
			try
			{
				try
				{
					action((TPayload)c);
				}
				catch (Exception arg)
				{
					Console.WriteLine("Failed to publish event on background thread ({0}).", arg);
				}
				Interlocked.Decrement(ref pendingWorkItems);
				OnActionCompleted();
			}
			catch
			{
			}
		}, argument);
	}

	private void OnActionCompleted()
	{
		lock (actionsQueue)
		{
			if (actionsQueue.Count > 0)
			{
				KeyValuePair<Action<TPayload>, TPayload> keyValuePair = actionsQueue.Dequeue();
				ExecuteAction(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}
}
