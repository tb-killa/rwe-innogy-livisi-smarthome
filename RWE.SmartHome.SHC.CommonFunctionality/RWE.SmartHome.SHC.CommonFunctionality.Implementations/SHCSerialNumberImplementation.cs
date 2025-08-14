using System;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;
using SHCWrapper.Misc;

namespace RWE.SmartHome.SHC.CommonFunctionality.Implementations;

internal class SHCSerialNumberImplementation : ISHCSerialNumber
{
	private string serial;

	public string SerialNumber()
	{
		if (Environment.OSVersion.Platform == PlatformID.WinCE)
		{
			return serial ?? (serial = CreateSerial());
		}
		return string.Empty;
	}

	private static string CreateSerial()
	{
		SGTIN96 sGTIN = SGTIN96.Create(SGTIN.GetSGTIN());
		if (sGTIN.ItemReference != 91410)
		{
			return sGTIN.SerialNumber.ToString().PadLeft(12, '0');
		}
		if (sGTIN.SerialNumber > 9999999 || sGTIN.ItemReference > 99999)
		{
			return "000000000000";
		}
		return ((ulong)((double)sGTIN.ItemReference * Math.Pow(10.0, 7.0)) + sGTIN.SerialNumber).ToString().PadLeft(12, '0');
	}
}
