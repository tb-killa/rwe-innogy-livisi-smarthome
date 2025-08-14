using System;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public class RandomNumberGenerator : IRandomNumberGenerator
{
	private readonly Random random;

	public RandomNumberGenerator()
	{
		random = new Random();
	}

	public int Next(int min, int max)
	{
		return random.Next(min, max);
	}
}
