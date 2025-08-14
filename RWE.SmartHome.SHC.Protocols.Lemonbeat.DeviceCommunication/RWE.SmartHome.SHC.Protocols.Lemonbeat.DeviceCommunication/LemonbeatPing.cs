using System;
using System.Net;
using System.Runtime.InteropServices;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication;

public class LemonbeatPing
{
	[DllImport("PingDLL.dll")]
	private static extern int shcping6(byte[] address);

	public static bool Ping(IPAddress address)
	{
		int num;
		try
		{
			num = shcping6(address.GetAddressBytes());
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Unable to invoke ping, exception: " + ex.ToString());
			return false;
		}
		if (num < 0)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Ping to " + address.ToString() + " failed, error: " + -num);
			return false;
		}
		if (num == 0)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, $"Ping to address {address.ToString()} timed out");
			return false;
		}
		return true;
	}
}
