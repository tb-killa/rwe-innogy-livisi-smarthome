using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Persistence;

public class PhysicalConfigurationEntity
{
	public Guid DeviceId { get; set; }

	public List<Group> PartnerGroups { get; set; }

	public List<PartnerEntity> Partners { get; set; }

	public List<Link> Links { get; set; }

	public List<LemonbeatTimer> Timers { get; set; }

	public List<CalendarTask> CalendarEntries { get; set; }

	public List<LemonbeatAction> Actions { get; set; }

	public List<Calculation> Calculations { get; set; }

	public List<StateMachine> StateMachines { get; set; }

	public List<ValueDescription> VirtualValueDescriptions { get; set; }
}
