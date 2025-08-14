using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.TaskList;

public class DeviceConfigurationStates
{
	private static readonly object sync = new object();

	private List<Guid> ids = new List<Guid>();

	public void ClearAllStates()
	{
		lock (sync)
		{
			ids.Clear();
		}
	}

	public void ReceivedNewConfigState(Guid deviceId)
	{
		lock (sync)
		{
			ids.Add(deviceId);
		}
	}

	public bool IsReceivedConfigState(Guid deviceId)
	{
		lock (sync)
		{
			return ids.Contains(deviceId);
		}
	}
}
