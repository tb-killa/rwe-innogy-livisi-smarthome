using System;
using System.Collections.Generic;
using System.Linq;

namespace RWE.SmartHome.SHC.Core;

public class EventManager : IEventManager, IService
{
	private readonly List<EventBase> events = new List<EventBase>();

	public TEventType GetEvent<TEventType>() where TEventType : EventBase
	{
		TEventType val = events.FirstOrDefault((EventBase evt) => (object)evt.GetType() == typeof(TEventType)) as TEventType;
		if (val == null)
		{
			val = Activator.CreateInstance<TEventType>();
			events.Add(val);
		}
		return val;
	}

	public void Initialize()
	{
	}

	public void Uninitialize()
	{
	}
}
