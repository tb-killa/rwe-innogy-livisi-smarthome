using System;

namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public static class TimeConverter
{
	public static ushort ConvertMsToUshort(int timeSpan)
	{
		if (timeSpan <= 0)
		{
			return 0;
		}
		int num = timeSpan / 100;
		ushort result = 0;
		for (int i = 0; i < 32; i++)
		{
			int num2 = (int)Math.Pow(2.0, i);
			double num3 = (double)num / (double)num2;
			if (num3 < Math.Pow(2.0, 11.0))
			{
				result = (ushort)((int)num3 << 5);
				result |= (ushort)i;
				break;
			}
		}
		return result;
	}
}
