namespace RWE.SmartHome.SHC.wMBusProtocol;

public static class BCDConverter
{
	public static int ConvertFromBcdSimple(byte[] code)
	{
		int num = 0;
		for (int num2 = code.Length - 1; num2 >= 0; num2--)
		{
			byte b = code[num2];
			int num3 = b >> 4;
			int num4 = b & 0xF;
			num = num * 100 + num3 * 10 + num4;
		}
		return num;
	}

	public static int ConvertFromBcd(byte[] code)
	{
		bool flag = true;
		bool flag2 = false;
		int num = 0;
		for (int num2 = code.Length - 1; num2 >= 0; num2--)
		{
			byte b = code[num2];
			int num3 = b >> 4;
			int num4 = b & 0xF;
			if (flag)
			{
				if (num3 == 14 || num3 == 15)
				{
					num3 = 15 - num3;
					num4 = 9 - num4;
					flag2 = true;
				}
			}
			else if (flag2)
			{
				num3 = 9 - num3;
				num4 = 9 - num4;
			}
			flag = false;
			num = num * 100 + num3 * 10 + num4;
		}
		if (flag2)
		{
			num++;
			num *= -1;
		}
		return num;
	}

	public static byte[] ConvertToBcd(long number)
	{
		if (number <= 0)
		{
			return ConvertToBcdNegative(number);
		}
		return ConvertToBcdPositive(number);
	}

	private static byte[] ConvertToBcdPositive(long number)
	{
		int length = number.ToString().Length;
		int num = length / 2 + ((length % 2 != 0) ? 1 : 0);
		byte[] array = new byte[num];
		long num2 = number;
		for (int i = 0; i < num; i++)
		{
			long num3 = num2 % 10;
			num2 /= 10;
			long num4 = num2 % 10;
			num2 /= 10;
			array[i] = (byte)(num3 + num4 * 16);
		}
		return array;
	}

	private static byte[] ConvertToBcdNegative(long number)
	{
		long num = number * -1;
		num--;
		int length = num.ToString().Length;
		int num2 = length / 2 + ((length % 2 != 0) ? 1 : 0);
		byte[] array = new byte[num2];
		long num3 = num;
		for (int i = 0; i < num2; i++)
		{
			long num4;
			long num5;
			if (i == num2 - 1)
			{
				num4 = 9 - num3 % 10;
				num3 /= 10;
				num5 = 15 - num3 % 16;
				num3 /= 16;
			}
			else
			{
				num4 = 9 - num3 % 10;
				num3 /= 10;
				num5 = 9 - num3 % 10;
				num3 /= 10;
			}
			array[i] = (byte)(num4 + num5 * 16);
		}
		return array;
	}
}
