using System;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public class RweSerialNumberStrategy
{
	public static string CreateSerialNumberFromSgtin(SGTIN96 sgtin)
	{
		string empty = string.Empty;
		_ = sgtin.SerialNumber;
		_ = 9999999;
		_ = sgtin.ItemReference;
		_ = 99999;
		return ((ulong)((double)sgtin.ItemReference * Math.Pow(10.0, 7.0)) + sgtin.SerialNumber).ToString().PadLeft(12, '0');
	}
}
