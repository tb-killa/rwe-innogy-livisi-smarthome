using System;
using System.Runtime.InteropServices;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class Ntp
{
	private const string ModuleName = "CommonFunctionality";

	private static readonly DateTime MIN_VALID_SYSTEM_TIME = new DateTime(2016, 2, 20);

	public static bool AssertSystemTimeValidity()
	{
		return DateTime.UtcNow >= MIN_VALID_SYSTEM_TIME;
	}

	[DllImport("CommonFunctionalityNative.dll", CharSet = CharSet.Unicode)]
	private static extern bool CheckTimeServerConnectivity();

	[DllImport("CommonFunctionalityNative.dll", CharSet = CharSet.Unicode)]
	public static extern bool ForceTimeSync();
}
