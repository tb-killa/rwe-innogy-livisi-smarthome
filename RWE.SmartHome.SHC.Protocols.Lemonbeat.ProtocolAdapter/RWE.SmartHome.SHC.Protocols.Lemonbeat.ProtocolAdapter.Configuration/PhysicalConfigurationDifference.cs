using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calendar;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.StateMachines;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;

public class PhysicalConfigurationDifference
{
	public ConfigurationDifference<Group> PartnerGroups { get; set; }

	public ConfigurationDifference<Partner> Partners { get; set; }

	public ConfigurationDifference<Link> Links { get; set; }

	public ConfigurationDifference<LemonbeatTimer> Timers { get; set; }

	public ConfigurationDifference<CalendarTask> CalendarEntries { get; set; }

	public ConfigurationDifference<LemonbeatAction> Actions { get; set; }

	public ConfigurationDifference<Calculation> Calculations { get; set; }

	public ConfigurationDifference<StateMachine> StateMachines { get; set; }

	public ConfigurationDifference<ValueDescription> VirtualValueDescriptions { get; set; }

	public bool IsEmpty()
	{
		if ((Partners == null || Partners.IsEmpty) && (PartnerGroups == null || PartnerGroups.IsEmpty) && (Links == null || Links.IsEmpty) && (Timers == null || Timers.IsEmpty) && (CalendarEntries == null || CalendarEntries.IsEmpty) && (Actions == null || Actions.IsEmpty) && (Calculations == null || Calculations.IsEmpty) && (StateMachines == null || StateMachines.IsEmpty))
		{
			if (VirtualValueDescriptions != null)
			{
				return VirtualValueDescriptions.IsEmpty;
			}
			return true;
		}
		return false;
	}
}
