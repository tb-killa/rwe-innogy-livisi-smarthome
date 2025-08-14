using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SmartHome.SHC.API.Configuration;

public class Trigger
{
	public Guid Id { get; private set; }

	public Guid CapabilityId { get; private set; }

	public InteractionDetails InteractionDetails { get; private set; }

	public string EventType { get; private set; }

	public IEnumerable<TriggerCondition> TriggerConditions { get; private set; }

	public int FreezeTime { get; private set; }

	public Trigger(Guid id, Guid capabilityId, InteractionDetails interactionDetails, string EventType, IEnumerable<TriggerCondition> conditions, int freezeTime)
	{
		Id = id;
		CapabilityId = capabilityId;
		InteractionDetails = interactionDetails;
		this.EventType = EventType;
		TriggerConditions = new ReadOnlyCollection<TriggerCondition>(new List<TriggerCondition>(conditions));
		FreezeTime = freezeTime;
	}
}
