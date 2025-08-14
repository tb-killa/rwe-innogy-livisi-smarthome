using RWE.SmartHome.SHC.CommonFunctionality.Implementations;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class SHCVersion
{
	private static ISHCVersion Instance = new SHCVersionImplementation();

	public static string HardwareVersion => Instance.HardwareVersion;

	public static string ApplicationVersion => Instance.ApplicationVersion;

	public static string OsVersion => Instance.OsVersion;

	internal static void OverrideImplementation(ISHCVersion implementation)
	{
		Instance = implementation;
	}
}
