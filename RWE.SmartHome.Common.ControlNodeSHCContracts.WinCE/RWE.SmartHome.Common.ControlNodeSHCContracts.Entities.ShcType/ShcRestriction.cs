using System;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcType;

[Flags]
public enum ShcRestriction : long
{
	NoRestriction = 0L,
	PhysicalDeviceInclusionTo0 = 1L,
	PhysicalDeviceInclusionTo2 = 2L,
	PhysicalDeviceInclusionTo5 = 4L,
	PhysicalDeviceInclusionTo10 = 8L,
	HECSupport = 0x400L,
	All = long.MaxValue
}
