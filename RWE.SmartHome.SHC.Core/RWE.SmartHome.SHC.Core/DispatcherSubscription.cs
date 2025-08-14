using System;

namespace RWE.SmartHome.SHC.Core;

public class DispatcherSubscription<TPayload> : Subscription<TPayload>
{
	private readonly IDispatcher dispatcher;

	public DispatcherSubscription(Action<TPayload> action, Predicate<TPayload> filter, IDispatcher dispatcher)
		: base(action, filter)
	{
		if (dispatcher == null)
		{
			throw new ArgumentNullException("dispatcher");
		}
		this.dispatcher = dispatcher;
	}

	public override void InvokeAction(Action<TPayload> action, TPayload argument)
	{
		dispatcher.Dispatch(new Executable<TPayload>
		{
			Action = action,
			Argument = argument
		});
	}
}
