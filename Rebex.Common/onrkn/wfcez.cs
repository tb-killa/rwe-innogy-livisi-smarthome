using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class wfcez
{
	public const string xgskq = "ManagedRebexAesGcm";

	public const string yojny = "NetStandardAesGcm";

	public const string spamt = "CngAesGcm";

	private const string mjzww = "Specified provider is not available.";

	private static readonly Dictionary<string, bhfih> ajdsv;

	private static readonly string ldzfm;

	static wfcez()
	{
		ajdsv = new Dictionary<string, bhfih>();
		ldzfm = "ManagedRebexAesGcm";
		ajdsv.Add("ManagedRebexAesGcm", nebdz.pqtvi(SymmetricKeyAlgorithmId.AES));
	}

	public static bool riopy(int p0, int p1)
	{
		if (p0 != 16)
		{
			return false;
		}
		if (p1 < 8 || p1 > 16)
		{
			return false;
		}
		if (CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
		{
			return CryptoHelper.wyrkl;
		}
		return true;
	}

	public static fhryo stfbw(byte[] p0, int p1, int p2)
	{
		prjlw p3 = mzjjf(p2, p1);
		bhfih bhfih2 = jiqzk(ldzfm);
		return bhfih2.nkjyk(p0, p3);
	}

	public static gajry usflo(byte[] p0, int p1, int p2)
	{
		prjlw p3 = mzjjf(p2, p1);
		bhfih bhfih2 = jiqzk(ldzfm);
		return bhfih2.smvfj(p0, p3);
	}

	private static bhfih jiqzk(string p0)
	{
		bhfih bhfih2 = yrtwh(p0);
		if (bhfih2 == null || 1 == 0)
		{
			throw new CryptographicException("Specified provider is not available.");
		}
		return bhfih2;
	}

	internal static bhfih yrtwh(string p0)
	{
		if (string.IsNullOrEmpty(p0) && 0 == 0)
		{
			throw new ArgumentException("Value cannot be null or empty.", "providerKey");
		}
		if (!eddns(p0) || 1 == 0)
		{
			return null;
		}
		ajdsv.TryGetValue(p0, out var value);
		return value;
	}

	private static bool eddns(string p0)
	{
		if (!(p0 == "CngAesGcm") || 1 == 0)
		{
			return !CryptoHelper.UseFipsAlgorithmsOnly;
		}
		return true;
	}

	private static prjlw mzjjf(int p0, int p1)
	{
		prjlw prjlw2 = new prjlw();
		prjlw2.sopyo = p0;
		prjlw2.lvrxy = p1;
		return prjlw2;
	}

	public static bool ccwen(string p0)
	{
		return yrtwh(p0) != null;
	}
}
