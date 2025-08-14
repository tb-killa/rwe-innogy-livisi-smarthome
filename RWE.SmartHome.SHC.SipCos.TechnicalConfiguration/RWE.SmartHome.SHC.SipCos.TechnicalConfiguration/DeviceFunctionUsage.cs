using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration;

public class DeviceFunctionUsage<T> : Dictionary<DeviceFunction, IList<T>>
{
	public bool AddUsage(Guid logicalDeviceId, int function, T usage)
	{
		return AddUsage(new DeviceFunction(logicalDeviceId, function), usage);
	}

	public bool AddUsage(DeviceFunction deviceFunction, T usage)
	{
		if (!TryGetValue(deviceFunction, out var value))
		{
			value = (base[deviceFunction] = new List<T>());
		}
		if (!value.Contains(usage))
		{
			value.Add(usage);
			return true;
		}
		return false;
	}
}
