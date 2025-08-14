using System;
using System.Collections.Generic;
using SmartHome.SHC.API.Control;

namespace SmartHome.SHC.API;

public class PhysicalStateTransformationResult
{
	public Dictionary<Guid, CapabilityState> CapabilityStates { get; set; }

	public PhysicalStateTransformationResult()
	{
		CapabilityStates = new Dictionary<Guid, CapabilityState>();
	}
}
