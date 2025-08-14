using System;
using System.Collections.Generic;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.Actions;
using SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions;

namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;

public class TransformationResult
{
	public List<LemonbeatTimer> Timers { get; private set; }

	public List<CalendarTask> Calendars { get; private set; }

	public List<LemonbeatAction> Actions { get; private set; }

	public List<Calculation> Calculations { get; private set; }

	public List<PartnerCalculationAllocation> PartnerCalculationAllocations { get; private set; }

	public List<TriggerCalculations> TriggerCalculations { get; private set; }

	public List<ConfigurationError> Errors { get; private set; }

	public List<StateMachine> StateMachines { get; private set; }

	public List<ValueDescription> ValueDescriptions { get; private set; }

	public List<Guid> ProcessedTimeInteractions { get; private set; }

	public TransformationResult()
	{
		Timers = new List<LemonbeatTimer>();
		Calendars = new List<CalendarTask>();
		Actions = new List<LemonbeatAction>();
		Calculations = new List<Calculation>();
		PartnerCalculationAllocations = new List<PartnerCalculationAllocation>();
		TriggerCalculations = new List<TriggerCalculations>();
		StateMachines = new List<StateMachine>();
		ValueDescriptions = new List<ValueDescription>();
		Errors = new List<ConfigurationError>();
		ProcessedTimeInteractions = new List<Guid>();
	}
}
