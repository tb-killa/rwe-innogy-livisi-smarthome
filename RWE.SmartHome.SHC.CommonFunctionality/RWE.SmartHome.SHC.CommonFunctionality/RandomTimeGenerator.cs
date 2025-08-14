using System;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class RandomTimeGenerator
{
	public static TimeSpan GenerateTimeBetween(string startTime, string endTime)
	{
		string[] array = startTime.Split(':');
		string[] array2 = endTime.Split(':');
		int num = int.Parse(array[0]) * 3600 + int.Parse(array[1]) * 60;
		int num2 = int.Parse(array2[0]) * 3600 + int.Parse(array2[1]) * 60;
		int maxValue = ((num2 <= num) ? (num2 + 86400 - num) : (num2 - num));
		Random random = new Random();
		int num3 = random.Next(maxValue);
		return TimeSpan.FromSeconds(num + num3);
	}
}
