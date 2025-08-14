using System;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler;

public class Promise
{
	public static T GetFirstValue<T, R>(T defaultValue, R parameter, Func<T, bool> conditionCallback, params Func<R, T>[] functions)
	{
		foreach (Func<R, T> func in functions)
		{
			T val = func(parameter);
			if (conditionCallback(val))
			{
				return val;
			}
		}
		return defaultValue;
	}
}
