using RWE.SmartHome.SHC.CommonFunctionality.Implementations;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class SHCSerialNumber
{
	private static ISHCSerialNumber Instance = new SHCSerialNumberImplementation();

	internal static void OverrideImplementation(ISHCSerialNumber implementation)
	{
		Instance = implementation;
	}

	public static string SerialNumber()
	{
		return Instance.SerialNumber();
	}
}
