using System;

namespace RWE.SmartHome.SHC.Core;

public class Subscription<TPayload> : ISubscription
{
	private readonly Action<TPayload> action;

	private readonly Predicate<TPayload> filter;

	public SubscriptionToken SubscriptionToken { get; set; }

	public Action<TPayload> Action => action;

	public Predicate<TPayload> Filter => filter;

	public Subscription(Action<TPayload> action, Predicate<TPayload> filter)
	{
		if (action == null)
		{
			throw new ArgumentNullException("action");
		}
		if (filter == null)
		{
			throw new ArgumentNullException("filter");
		}
		this.action = action;
		this.filter = filter;
	}

	public virtual Action<object[]> GetExecutionStrategy()
	{
		if (action != null && filter != null)
		{
			return delegate(object[] arguments)
			{
				TPayload val = default(TPayload);
				if (arguments != null && arguments.Length > 0 && arguments[0] != null)
				{
					val = (TPayload)arguments[0];
				}
				if (filter(val))
				{
					InvokeAction(action, val);
				}
			};
		}
		return null;
	}

	public virtual void InvokeAction(Action<TPayload> action, TPayload argument)
	{
		action(argument);
	}
}
