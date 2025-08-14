using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Helpers.ISR2Calibration;

public class ISR2StateManager
{
	private readonly IDictionary<Guid, ISR2State> rollerShutterStates = new Dictionary<Guid, ISR2State>();

	private readonly object sync = new object();

	public ISR2State GetStateForId(Guid id)
	{
		lock (sync)
		{
			if (!rollerShutterStates.ContainsKey(id))
			{
				rollerShutterStates.Add(id, new ISR2State());
			}
			return rollerShutterStates[id];
		}
	}
}
