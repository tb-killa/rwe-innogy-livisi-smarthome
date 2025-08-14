using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;

public class PhysicalConfiguration
{
	public List<ValueDescription> VirtualValueDescriptions { get; set; }

	public List<Group> PartnerGroups { get; set; }

	public List<Partner> Partners { get; set; }

	public List<Link> Links { get; set; }

	public List<LemonbeatTimer> Timers { get; set; }

	public List<CalendarTask> CalendarEntries { get; set; }

	public List<LemonbeatAction> Actions { get; set; }

	public List<Calculation> Calculations { get; set; }

	public List<StateMachine> StateMachines { get; set; }

	public PhysicalConfiguration()
	{
		VirtualValueDescriptions = new List<ValueDescription>();
		PartnerGroups = new List<Group>();
		Partners = new List<Partner>();
		Links = new List<Link>();
		Timers = new List<LemonbeatTimer>();
		CalendarEntries = new List<CalendarTask>();
		Actions = new List<LemonbeatAction>();
		Calculations = new List<Calculation>();
		StateMachines = new List<StateMachine>();
	}
}
