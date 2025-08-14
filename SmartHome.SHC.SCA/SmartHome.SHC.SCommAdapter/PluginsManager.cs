using System;
using System.Collections.Generic;
using Rebex.Security.Cryptography;

namespace SmartHome.SHC.SCommAdapter;

public static class PluginsManager
{
	private const string EllipticCurveAlgorithmName = "EllipticCurveAlgorithm";

	private const string Curve25519Name = "Curve25519";

	private static readonly List<string> registeredAddins = new List<string>();

	public static void RegisterEllipticCurveAlgorithm()
	{
		RegisterAsymetricPluggin(EllipticCurveAlgorithm.Create, "EllipticCurveAlgorithm");
	}

	public static void RegisterCurve25519Algorithm()
	{
		RegisterAsymetricPluggin(Curve25519.Create, "Curve25519");
	}

	private static void RegisterAsymetricPluggin(Func<string, object> algorithmFactory, string algorithmName)
	{
		try
		{
			if (!registeredAddins.Contains(algorithmName))
			{
				AsymmetricKeyAlgorithm.Register(algorithmFactory);
				registeredAddins.Add(algorithmName);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("Register of Rebex pluggin failed: {0}", ex.Message);
		}
	}
}
