using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;

internal class ConfigurationInformation
{
	private PhysicalConfiguration current;

	private PhysicalConfiguration target;

	private bool isDifferenceDirty;

	private PhysicalConfigurationDifference difference;

	private readonly object syncRoot = new object();

	public PhysicalConfiguration Current
	{
		get
		{
			return current;
		}
		set
		{
			lock (syncRoot)
			{
				current = value;
				isDifferenceDirty = true;
			}
		}
	}

	public PhysicalConfiguration Target
	{
		get
		{
			return target;
		}
		set
		{
			lock (syncRoot)
			{
				target = value;
				isDifferenceDirty = true;
			}
		}
	}

	public PhysicalConfigurationDifference DifferencesToBeDeployed
	{
		get
		{
			lock (syncRoot)
			{
				if (isDifferenceDirty)
				{
					difference = GetDifferences();
					isDifferenceDirty = false;
				}
				return difference;
			}
		}
	}

	public void UpdateCurrentConfiguration(List<ServiceType> deployedServices, PhysicalConfigurationDifference difference)
	{
		lock (syncRoot)
		{
			using (List<ServiceType>.Enumerator enumerator = deployedServices.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current)
					{
					case ServiceType.PartnerInformation:
						Current.Partners.ApplyDifference(difference.Partners);
						Current.PartnerGroups.ApplyDifference(difference.PartnerGroups);
						break;
					case ServiceType.Timer:
						Current.Timers.ApplyDifference(difference.Timers);
						break;
					case ServiceType.Calendar:
						Current.CalendarEntries.ApplyDifference(difference.CalendarEntries);
						break;
					case ServiceType.Action:
						Current.Actions.ApplyDifference(difference.Actions);
						break;
					case ServiceType.Calculation:
						Current.Calculations.ApplyDifference(difference.Calculations);
						break;
					case ServiceType.Statemachine:
						Current.StateMachines.ApplyDifference(difference.StateMachines);
						break;
					case ServiceType.ValueDescription:
						Current.VirtualValueDescriptions.ApplyDifference(difference.VirtualValueDescriptions);
						break;
					default:
						throw new ArgumentOutOfRangeException("service");
					}
				}
			}
			isDifferenceDirty = true;
		}
	}

	private PhysicalConfigurationDifference GetDifferences()
	{
		lock (syncRoot)
		{
			if (Current != null && Target != null)
			{
				PhysicalConfigurationDifference physicalConfigurationDifference = new PhysicalConfigurationDifference();
				physicalConfigurationDifference.Partners = Current.Partners.CalculateDifference(Target.Partners);
				physicalConfigurationDifference.PartnerGroups = Current.PartnerGroups.CalculateDifference(Target.PartnerGroups);
				physicalConfigurationDifference.Links = Current.Links.CalculateDifference(Target.Links);
				physicalConfigurationDifference.Timers = Current.Timers.CalculateDifference(Target.Timers);
				physicalConfigurationDifference.CalendarEntries = Current.CalendarEntries.CalculateDifference(Target.CalendarEntries);
				physicalConfigurationDifference.Actions = Current.Actions.CalculateDifference(Target.Actions);
				physicalConfigurationDifference.Calculations = Current.Calculations.CalculateDifference(Target.Calculations);
				physicalConfigurationDifference.StateMachines = Current.StateMachines.CalculateDifference(Target.StateMachines);
				physicalConfigurationDifference.VirtualValueDescriptions = current.VirtualValueDescriptions.CalculateDifference(Target.VirtualValueDescriptions);
				return physicalConfigurationDifference;
			}
			return new PhysicalConfigurationDifference();
		}
	}
}
