using System;
using System.Collections.Generic;
using System.Linq;

namespace RWE.SmartHome.SHC.Core;

public abstract class EventBase
{
	private readonly List<ISubscription> subscriptions = new List<ISubscription>();

	protected ICollection<ISubscription> Subscriptions => subscriptions;

	protected virtual SubscriptionToken InternalSubscribe(ISubscription subscription)
	{
		subscription.SubscriptionToken = new SubscriptionToken();
		lock (Subscriptions)
		{
			Subscriptions.Add(subscription);
		}
		return subscription.SubscriptionToken;
	}

	protected virtual void InternalPublish(params object[] arguments)
	{
		IEnumerable<Action<object[]>> enumerable = PruneAndReturnStrategies();
		foreach (Action<object[]> item in enumerable)
		{
			try
			{
				item(arguments);
			}
			catch (Exception arg)
			{
				Console.WriteLine("Failed to publish event {0} ({1}).", GetType(), arg);
			}
		}
	}

	public virtual void Unsubscribe(SubscriptionToken token)
	{
		lock (Subscriptions)
		{
			ISubscription subscription = Subscriptions.FirstOrDefault((ISubscription evt) => evt.SubscriptionToken == token);
			if (subscription != null)
			{
				Subscriptions.Remove(subscription);
			}
		}
	}

	public virtual bool Contains(SubscriptionToken token)
	{
		lock (Subscriptions)
		{
			ISubscription subscription = Subscriptions.FirstOrDefault((ISubscription evt) => evt.SubscriptionToken == token);
			return subscription != null;
		}
	}

	private IEnumerable<Action<object[]>> PruneAndReturnStrategies()
	{
		List<Action<object[]>> list = new List<Action<object[]>>();
		lock (Subscriptions)
		{
			for (int num = Subscriptions.Count - 1; num >= 0; num--)
			{
				Action<object[]> executionStrategy = subscriptions[num].GetExecutionStrategy();
				if (executionStrategy == null)
				{
					subscriptions.RemoveAt(num);
				}
				else
				{
					list.Add(executionStrategy);
				}
			}
			return list;
		}
	}
}
