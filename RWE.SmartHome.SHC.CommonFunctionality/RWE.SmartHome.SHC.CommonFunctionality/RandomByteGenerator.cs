using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public sealed class RandomByteGenerator
{
	private class Inner
	{
		internal static readonly RandomByteGenerator instance = new RandomByteGenerator();
	}

	private const string Container = "shc";

	private const string Provider = "SHC Trusted Platform Module Cryptographic Service Provider";

	private const int CspProviderType = 1;

	private readonly RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider(new CspParameters(1, "SHC Trusted Platform Module Cryptographic Service Provider", "shc"));

	public static RandomByteGenerator Instance => Inner.instance;

	public byte[] GenerateRandomByteSequence(uint length)
	{
		byte[] array = new byte[length];
		try
		{
			csp.GetBytes(array);
			return array;
		}
		catch (CryptographicException)
		{
			Log("[RandomByteGenerator] Unable to generate random byte sequence.");
			throw;
		}
	}

	[Conditional("Verbose")]
	private static void LogIfVerbose(string message)
	{
		Console.WriteLine(message);
	}

	private static void Log(string message)
	{
		Console.WriteLine(message);
	}
}
