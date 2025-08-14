using System;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class SerialForDisplay
{
	public static string GetSerialForDisplay(this SGTIN96 sgtin)
	{
		return FromSgtin(sgtin);
	}

	public static string FromSgtin(byte[] sgtin)
	{
		return FromSgtin(SGTIN96.Create(sgtin));
	}

	public static string FromSgtin(string sgtin)
	{
		return FromSgtin(SGTIN96.Create(sgtin));
	}

	private static string FromSgtin(SGTIN96 sgtin)
	{
		if (sgtin.CompanyPrefix == 4051495 && (sgtin.ItemReference == 91419 || sgtin.ItemReference == 97510))
		{
			return FromBidCosDevice(sgtin);
		}
		if (sgtin.SerialNumber > 9999999 || sgtin.ItemReference > 99999)
		{
			return "000000000000";
		}
		return ((ulong)((double)sgtin.ItemReference * Math.Pow(10.0, 7.0)) + sgtin.SerialNumber).ToString().PadLeft(12, '0');
	}

	public static string FromBidCosDevice(SGTIN96 sgtin)
	{
		string arg = (sgtin.SerialNumber & 0xFFFFFF).ToString().PadLeft(7, '0');
		ulong num = (sgtin.SerialNumber & 0xE0000000u) >> 29;
		char c = (char)(((sgtin.SerialNumber & 0x1F000000) >> 24) + 65);
		string arg2 = string.Empty;
		ulong num2 = num;
		if ((long)num2 <= 2L && (long)num2 >= 0L)
		{
			switch (num2)
			{
			case 0uL:
				arg2 = "RW";
				break;
			case 1uL:
				arg2 = "WE";
				break;
			case 2uL:
				arg2 = "EQ";
				break;
			}
		}
		return $"{c}{arg2}{arg}";
	}
}
