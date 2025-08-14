using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.CoreApiConverters;
using SmartHome.SHC.API;

namespace RWE.SmartHome.SHC.ApplicationsHost;

public static class AppTokenExtensions
{
	public static AddInConfiguration GetAddInConfiguration(this ApplicationTokenEntry entry)
	{
		return entry.ToApiAddInConfiguration();
	}

	public static List<Property> GetApplicationStateFromToken(this ApplicationTokenEntry entry)
	{
		List<Property> properties = new List<Property>
		{
			new BooleanProperty
			{
				Name = "EnabledByUser",
				Value = entry.IsEnabledByUser
			},
			new BooleanProperty
			{
				Name = "EnabledByWebshop",
				Value = entry.IsEnabledByWebshop
			},
			new BooleanProperty
			{
				Name = "UpdateAvailable",
				Value = entry.UpdateAvailable
			},
			new BooleanProperty
			{
				Name = "ActiveOnShc",
				Value = entry.ActiveOnShc
			},
			new StringProperty
			{
				Name = "FullVersion",
				Value = entry.Version
			}
		};
		entry.Parameters.ForEach(delegate(ApplicationParameter p)
		{
			properties.Add(new StringProperty(p.Key, p.Value));
		});
		return properties;
	}
}
