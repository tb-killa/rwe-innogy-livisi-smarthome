using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapterInterfaces;

public static class PushButtonIndexProvider
{
	public static int GetPushButtonIndex(BuiltinPhysicalDeviceType deviceType, byte keyChannelNumber)
	{
		byte b = (byte)(IsIndependentDevice(deviceType) ? 1u : 2u);
		bool flag = HasSwappedChannels(deviceType);
		int num = keyChannelNumber - b;
		if (flag)
		{
			num = 1 - num;
		}
		return num;
	}

	private static bool IsIndependentDevice(BuiltinPhysicalDeviceType deviceType)
	{
		switch (deviceType)
		{
		case BuiltinPhysicalDeviceType.WSC2:
		case BuiltinPhysicalDeviceType.BRC8:
		case BuiltinPhysicalDeviceType.ISC2:
			return true;
		case BuiltinPhysicalDeviceType.ISS2:
		case BuiltinPhysicalDeviceType.ISD2:
		case BuiltinPhysicalDeviceType.ISR2:
			return false;
		default:
			throw new ArgumentOutOfRangeException("deviceType");
		}
	}

	private static bool HasSwappedChannels(BuiltinPhysicalDeviceType deviceType)
	{
		switch (deviceType)
		{
		case BuiltinPhysicalDeviceType.WSC2:
		case BuiltinPhysicalDeviceType.ISS2:
		case BuiltinPhysicalDeviceType.ISD2:
		case BuiltinPhysicalDeviceType.ISR2:
			return true;
		case BuiltinPhysicalDeviceType.BRC8:
		case BuiltinPhysicalDeviceType.ISC2:
			return false;
		default:
			throw new ArgumentOutOfRangeException("deviceType");
		}
	}
}
