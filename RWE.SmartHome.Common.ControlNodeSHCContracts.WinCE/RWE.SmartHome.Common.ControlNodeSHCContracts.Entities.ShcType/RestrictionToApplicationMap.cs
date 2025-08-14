using System.Collections.Generic;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcType;

public static class RestrictionToApplicationMap
{
	public static readonly Dictionary<ShcRestriction, List<string>> RestrictionToApplications = new Dictionary<ShcRestriction, List<string>>
	{
		{
			ShcRestriction.PhysicalDeviceInclusionTo0,
			new List<string> { "sh://DeviceContingent.RWE" }
		},
		{
			ShcRestriction.PhysicalDeviceInclusionTo2,
			new List<string> { "sh://DeviceContingent.RWE" }
		},
		{
			ShcRestriction.PhysicalDeviceInclusionTo5,
			new List<string> { "sh://DeviceContingent.RWE" }
		},
		{
			ShcRestriction.PhysicalDeviceInclusionTo10,
			new List<string> { "sh://DeviceContingent.RWE" }
		}
	};
}
