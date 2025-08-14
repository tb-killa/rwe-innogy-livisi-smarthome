using System;
using System.Linq;

namespace RWE.SmartHome.SHC.Core;

public class Event<TPayload> : EventBase
{
	public virtual SubscriptionToken Subscribe(Action<TPayload> action, Predicate<TPayload> filter, ThreadOption threadOption, IDispatcher dispatcher)
	{
		if (filter == null)
		{
			filter = (TPayload P_0) => true;
		}
		return base.InternalSubscribe(threadOption switch
		{
			ThreadOption.PublisherThread => new Subscription<TPayload>(action, filter), 
			ThreadOption.BackgroundThread => new BackgroundSubscription<TPayload>(action, filter), 
			ThreadOption.SubscriberThread => new DispatcherSubscription<TPayload>(action, filter, dispatcher), 
			_ => new Subscription<TPayload>(action, filter), 
		});
	}

	public virtual void Publish(TPayload payload)
	{
		base.InternalPublish(payload);
	}

	public virtual void Unsubscribe(Action<TPayload> subscriber)
	{
		lock (base.Subscriptions)
		{
			ISubscription subscription = base.Subscriptions.Cast<Subscription<TPayload>>().FirstOrDefault((Subscription<TPayload> evt) => evt.Action == subscriber);
			if (subscription != null)
			{
				base.Subscriptions.Remove(subscription);
			}
		}
	}

	public virtual bool Contains(Action<TPayload> subscriber)
	{
		ISubscription subscription;
		lock (base.Subscriptions)
		{
			subscription = base.Subscriptions.Cast<Subscription<TPayload>>().FirstOrDefault((Subscription<TPayload> evt) => evt.Action == subscriber);
		}
		return subscription != null;
	}
}
