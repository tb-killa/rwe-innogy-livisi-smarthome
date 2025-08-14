using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.SerialNumber;

public class BidCosSerialNumberStrategy
{
	public static string CreateSerialNumberFromSgtin(SGTIN96 deviceSgtin)
	{
		string empty = string.Empty;
		string arg = (deviceSgtin.SerialNumber & 0xFFFFFF).ToString().PadLeft(7, '0');
		ulong num = (deviceSgtin.SerialNumber & 0xE0000000u) >> 29;
		char c = (char)(((deviceSgtin.SerialNumber & 0x1F000000) >> 24) + 65);
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
