using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using SmartHome.SHC.API;

namespace RWE.SmartHome.SHC.CoreApiConverters;

public static class ApplicationTokenConverters
{
	public static AddInConfiguration ToApiAddInConfiguration(this ApplicationTokenEntry coreTokenEntry)
	{
		AddInConfiguration addInConfiguration = new AddInConfiguration();
		List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
		if (coreTokenEntry.Parameters != null)
		{
			coreTokenEntry.Parameters.ForEach(delegate(ApplicationParameter p)
			{
				parameters.Add(new KeyValuePair<string, string>(p.Key, p.Value));
			});
		}
		addInConfiguration.Parameters = parameters;
		return addInConfiguration;
	}
}
