using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

[rbjhl("windows")]
internal class xgwba : hflqg, eatps, dzjkq, ntowq
{
	private enum cdvsx
	{
		rmavy,
		aviok,
		vhwgw
	}

	private sealed class etvep
	{
		public CspParameters lpjit;

		public CspParameters lvvnt()
		{
			return lpjit;
		}
	}

	private sealed class xqhxe
	{
		public CspParameters tjsbc;

		public CspParameters xsgdr()
		{
			return tjsbc;
		}
	}

	public const int ehwia = -1;

	public const string gxdyv = "Sign operation requires private key.";

	public const string cgsau = "Decrypt operation requires private key.";

	public const string oyczx = "Could not acquire crypto context.";

	public const string stxox = "Unable to import public key.";

	public const string fmvps = "Could not create crypto hash object (CryptCreateHash).";

	private readonly string xhgxj = "Unable to set initial hash (CryptSetHashparam).";

	private readonly string nzcmc = "Function CryptVerifySignature failed.";

	private readonly string kohxw = "Function CryptEncrypt failed.";

	private IntPtr fiszk;

	private readonly KeyAlgorithm ayjbh;

	private readonly int yseuy;

	private readonly bool aydfk;

	private readonly bool udvhm;

	private readonly cdvsx logih;

	private IntPtr awzec;

	private string kcsop;

	private AsymmetricKeyAlgorithmId pkyra;

	private int wjkax;

	public bool porwu => udvhm;

	public string janem
	{
		get
		{
			return kcsop;
		}
		private set
		{
			kcsop = value;
		}
	}

	public AsymmetricKeyAlgorithmId bptsq
	{
		get
		{
			return pkyra;
		}
		private set
		{
			pkyra = value;
		}
	}

	public int KeySize
	{
		get
		{
			return wjkax;
		}
		private set
		{
			wjkax = value;
		}
	}

	protected override void temua(bool p0)
	{
		gcxvx(p0);
	}

	private void gcxvx(bool p0)
	{
		if (aydfk && 0 == 0)
		{
			if (logih == cdvsx.vhwgw)
			{
				pothu.xdyfe(awzec);
			}
			pothu.obsxv(fiszk, 0);
		}
	}

	private xgwba(byte[] rsaPublicKey, int keySize, Func<CspParameters> getInfo)
		: base(getInfo)
	{
		if (rsaPublicKey == null || 1 == 0)
		{
			throw new ArgumentNullException("rsaPublicKey");
		}
		if (keySize <= 0)
		{
			throw new ArgumentOutOfRangeException("keySize");
		}
		logih = cdvsx.vhwgw;
		yelkm(rsaPublicKey);
		ayjbh = KeyAlgorithm.RSA;
		bptsq = AsymmetricKeyAlgorithmId.RSA;
		janem = bptsq.ToString() + keySize;
		KeySize = keySize;
		udvhm = true;
		aydfk = true;
	}

	public xgwba(IntPtr provider, Func<CspParameters> getInfo, KeyAlgorithm keyAlgorithm, int keySpec, bool ownsHandle)
		: base(getInfo)
	{
		fiszk = provider;
		ayjbh = keyAlgorithm;
		yseuy = keySpec;
		aydfk = ownsHandle;
		logih = cdvsx.aviok;
		janem = ayjbh.ToString();
		KeySize = -1;
		bptsq = bpkgq.frvei(ayjbh, p1: true);
		if ((keyAlgorithm != KeyAlgorithm.RSA) ? true : false)
		{
			return;
		}
		string text = gmetq.uggao(provider);
		string key;
		if ((key = text) != null && 0 == 0)
		{
			if (fnfqw.sflbm == null || 1 == 0)
			{
				fnfqw.sflbm = new Dictionary<string, int>(7)
				{
					{ "Microsoft Base Cryptographic Provider v1.0", 0 },
					{ "Microsoft RSA Signature Cryptographic Provider", 1 },
					{ "Microsoft RSA SChannel Strong Cryptographic Provider", 2 },
					{ "Microsoft Enhanced Cryptographic Provider v1.0", 3 },
					{ "Microsoft Strong Cryptographic Provider", 4 },
					{ "Microsoft Enhanced RSA and AES Cryptographic Provider", 5 },
					{ "Microsoft Enhanced RSA and AES Cryptographic Provider (Prototype)", 6 }
				};
			}
			if (fnfqw.sflbm.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
					udvhm = false;
					return;
				}
			}
		}
		udvhm = true;
	}

	public static xgwba igivl(RSAParameters p0)
	{
		PublicKeyInfo p1 = new PublicKeyInfo(p0);
		return vxfno(p1);
	}

	public static xgwba vxfno(PublicKeyInfo p0)
	{
		etvep etvep = new etvep();
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("publicKeyInfo");
		}
		byte[] rsaPublicKey = new poojt().xopmb(p0);
		etvep.lpjit = new CspParameters
		{
			ProviderType = 24
		};
		return new xgwba(rsaPublicKey, p0.GetKeySize(), etvep.lvvnt);
	}

	public static jayrg jwzup(CspParameters p0, KeyAlgorithm p1, bool p2, bool p3, out int p4)
	{
		xqhxe xqhxe = new xqhxe();
		xqhxe.tjsbc = p0;
		p4 = 0;
		IntPtr p5 = IntPtr.Zero;
		int keyNumber = xqhxe.tjsbc.KeyNumber;
		int providerType = xqhxe.tjsbc.ProviderType;
		string providerName = xqhxe.tjsbc.ProviderName;
		if (!providerName.StartsWith("Microsoft ", StringComparison.Ordinal) || 1 == 0)
		{
			return null;
		}
		int p6 = providerType;
		string p7 = providerName;
		KeyAlgorithm keyAlgorithm;
		switch (p6)
		{
		case 0:
			throw new CryptographicException("Not a CryptoAPI key.");
		case 24:
			keyAlgorithm = KeyAlgorithm.RSA;
			if (keyAlgorithm == KeyAlgorithm.RSA)
			{
				break;
			}
			goto case 1;
		case 1:
		case 2:
		case 12:
			keyAlgorithm = KeyAlgorithm.RSA;
			gmetq.qjkxo(ref p7, ref p6);
			break;
		case 13:
			keyAlgorithm = KeyAlgorithm.DSA;
			if (keyAlgorithm != KeyAlgorithm.RSA)
			{
				break;
			}
			goto default;
		default:
			return null;
		}
		uint num = 0u;
		if (p2 && 0 == 0)
		{
			num |= 0x40;
		}
		if ((xqhxe.tjsbc.Flags & CspProviderFlags.UseMachineKeyStore) != CspProviderFlags.NoFlags && 0 == 0)
		{
			num |= 0x20;
		}
		if (pothu.qfori(out p5, xqhxe.tjsbc.KeyContainerName, p7, p6, num) == 0 || 1 == 0)
		{
			p4 = Marshal.GetLastWin32Error();
			if (p4 == -2146893802)
			{
				if (pothu.qfori(out p5, xqhxe.tjsbc.KeyContainerName, providerName, providerType, num) == 0 || 1 == 0)
				{
					p4 = Marshal.GetLastWin32Error();
				}
				else
				{
					p4 = 0;
				}
			}
		}
		if (((p4 != 0) ? true : false) || p5 == IntPtr.Zero)
		{
			if (!p3 || 1 == 0)
			{
				return null;
			}
			if (p4 == -2146893790)
			{
				throw new CryptographicException("CSP needs to display UI to operate.");
			}
			throw new CryptographicException(brgjd.edcru("Unable to acquire private key handle (0x{0:X8}). Suggested provider: '{1}'.", p4, providerName));
		}
		Func<CspParameters> getInfo = xqhxe.xsgdr;
		return new xgwba(p5, getInfo, keyAlgorithm, keyNumber, ownsHandle: true);
	}

	public override bool vmedb(mrxvh p0)
	{
		switch (p0.hqtwc)
		{
		case goies.gbwxv:
			if ((ayjbh != KeyAlgorithm.RSA) ? true : false)
			{
				break;
			}
			goto default;
		case goies.lfkki:
			if (ayjbh == KeyAlgorithm.RSA)
			{
				break;
			}
			goto default;
		default:
			return false;
		}
		if (ayjbh == KeyAlgorithm.DSA)
		{
			if (p0.faqqk != SignatureHashAlgorithm.SHA1)
			{
				return false;
			}
		}
		else
		{
			switch (p0.faqqk)
			{
			case SignatureHashAlgorithm.SHA256:
			case SignatureHashAlgorithm.SHA384:
			case SignatureHashAlgorithm.SHA512:
				return udvhm;
			}
			if (pfihk(p0.faqqk) == 0 || 1 == 0)
			{
				return false;
			}
		}
		return true;
	}

	public override byte[] rypyi(byte[] p0, mrxvh p1)
	{
		if (logih != cdvsx.aviok)
		{
			throw new CryptographicException("Sign operation requires private key.");
		}
		SignatureHashAlgorithm faqqk = p1.faqqk;
		int num = pfihk(faqqk);
		jrrym(p1, faqqk, num);
		IntPtr p2 = IntPtr.Zero;
		try
		{
			if (pothu.fclhh(fiszk, num, IntPtr.Zero, 0, out p2) == 0 || 1 == 0)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				string text = gmetq.uggao(fiszk);
				throw new CryptographicException(brgjd.edcru("Unable to create {0} hash using {1} (0x{2:X8}).", faqqk, text, lastWin32Error));
			}
			if (pothu.bmpgj(p2, 2, p0, 0) == 0 || 1 == 0)
			{
				throw new CryptographicException("Unable to set hash parameter.");
			}
			int p3 = 0;
			if (pothu.kqejz(p2, yseuy, IntPtr.Zero, 0, null, ref p3) == 0 || 1 == 0)
			{
				int lastWin32Error2 = Marshal.GetLastWin32Error();
				throw new CryptographicException(brgjd.edcru("Unable to sign hash (0x{0:X8}).", lastWin32Error2));
			}
			if (p3 == 0 || false || (ayjbh == KeyAlgorithm.DSA && p3 != 40))
			{
				throw new CryptographicException("Unable to determine signature length.");
			}
			byte[] array = new byte[p3];
			if (pothu.kqejz(p2, yseuy, IntPtr.Zero, 0, array, ref p3) == 0 || 1 == 0)
			{
				int lastWin32Error3 = Marshal.GetLastWin32Error();
				throw new CryptographicException(brgjd.edcru("Unable to sign hash (0x{0:X8}).", lastWin32Error3));
			}
			KeyAlgorithm keyAlgorithm = ayjbh;
			if (keyAlgorithm == KeyAlgorithm.DSA)
			{
				Array.Reverse(array, 0, 20);
				Array.Reverse(array, 20, 20);
			}
			else
			{
				Array.Reverse(array, 0, array.Length);
			}
			return array;
		}
		finally
		{
			if (p2 != IntPtr.Zero && 0 == 0)
			{
				pothu.sxrxx(p2);
			}
		}
	}

	public virtual bool cbzmp(byte[] p0, byte[] p1, mrxvh p2)
	{
		SignatureHashAlgorithm faqqk = p2.faqqk;
		int p3 = pfihk(faqqk);
		jrrym(p2, faqqk, p3);
		IntPtr p4 = IntPtr.Zero;
		try
		{
			int p5 = pothu.fclhh(fiszk, pfihk(p2.faqqk), IntPtr.Zero, pothu.qnnwg, out p4);
			pothu.gqrne(p5, "Could not create crypto hash object (CryptCreateHash).");
			int p6 = pothu.bmpgj(p4, 2, p0, pothu.qnnwg);
			pothu.gqrne(p6, xhgxj);
			byte[] array = jlfbq.ukqqp(p1, 0, p1.Length);
			Array.Reverse(array, 0, array.Length);
			int num = pothu.gciab(p4, array, array.Length, awzec, IntPtr.Zero, pothu.qnnwg);
			int lastWin32Error = Marshal.GetLastWin32Error();
			if ((num == 0 || 1 == 0) && lastWin32Error != -2146893818 && lastWin32Error != 87 && lastWin32Error != 8)
			{
				pothu.gqrne(num, nzcmc);
			}
			return num != 0;
		}
		finally
		{
			if (p4 != IntPtr.Zero && 0 == 0)
			{
				pothu.sxrxx(p4);
			}
		}
	}

	private static int pfihk(SignatureHashAlgorithm p0)
	{
		return p0 switch
		{
			SignatureHashAlgorithm.MD5 => 32771, 
			SignatureHashAlgorithm.SHA1 => 32772, 
			SignatureHashAlgorithm.SHA256 => 32780, 
			SignatureHashAlgorithm.SHA384 => 32781, 
			SignatureHashAlgorithm.SHA512 => 32782, 
			SignatureHashAlgorithm.MD5SHA1 => 32776, 
			_ => 0, 
		};
	}

	public override bool knvjq(jyamo p0)
	{
		if (ayjbh != KeyAlgorithm.RSA && 0 == 0)
		{
			return false;
		}
		switch (p0.vmeor)
		{
		case xdgzn.bntzq:
			if (!dahxy.kxxtc || 1 == 0)
			{
				return false;
			}
			if (p0.fbcyx == HashingAlgorithmId.SHA1 && p0.bablj == HashingAlgorithmId.SHA1)
			{
				return jlfbq.uqqcs(p0.blonh);
			}
			return false;
		case xdgzn.ctbmq:
			return true;
		default:
			return false;
		}
	}

	public override byte[] lhhds(byte[] p0, jyamo p1)
	{
		if (logih != cdvsx.aviok)
		{
			throw new CryptographicException("Decrypt operation requires private key.");
		}
		int p2 = yxrmt(p1);
		IntPtr p3 = IntPtr.Zero;
		try
		{
			if (pothu.pmwpt(fiszk, 1u, out p3) == 0 || 1 == 0)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (pothu.pmwpt(fiszk, 2u, out p3) == 0 || 1 == 0)
				{
					throw new CryptographicException(brgjd.edcru("Unable to get the key (0x{0:X8}).", lastWin32Error));
				}
				throw new CryptographicException("Key is only allowed to be used for signing, not for decryption.");
			}
			int p4 = p0.Length;
			byte[] array = (byte[])p0.Clone();
			Array.Reverse(array, 0, p4);
			if (pothu.iukzl(p3, IntPtr.Zero, 1, p2, array, ref p4) == 0 || 1 == 0)
			{
				int lastWin32Error2 = Marshal.GetLastWin32Error();
				throw new CryptographicException(brgjd.edcru("Unable to decrypt data (0x{0:X8}).", lastWin32Error2));
			}
			return jlfbq.ukqqp(array, 0, p4);
		}
		finally
		{
			if (p3 != IntPtr.Zero && 0 == 0)
			{
				pothu.xdyfe(p3);
			}
		}
	}

	public byte[] sfbms(byte[] p0, jyamo p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("rgb");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("paddingInfo");
		}
		int p2 = yxrmt(p1);
		int p3 = 0;
		int p4 = pothu.ekfve(awzec, IntPtr.Zero, 1, p2, IntPtr.Zero, ref p3, p0.Length);
		pothu.gqrne(p4, kohxw);
		byte[] array = new byte[p3];
		Array.Copy(p0, array, p0.Length);
		rnqdw rnqdw2 = new rnqdw(array);
		int p5 = p0.Length;
		try
		{
			int p6 = pothu.ekfve(awzec, IntPtr.Zero, 1, p2, rnqdw2.peara(), ref p5, p3);
			pothu.gqrne(p6, kohxw);
		}
		finally
		{
			rnqdw2.fbdzt();
		}
		Array.Reverse(array, 0, array.Length);
		return array;
	}

	public override PrivateKeyInfo jbbgs(bool p0)
	{
		CspParameters cspParameters = iqqfj();
		cspParameters.Flags |= (CspProviderFlags)((p0 ? true : false) ? 64 : 0);
		switch (ayjbh)
		{
		case KeyAlgorithm.RSA:
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(cspParameters);
			try
			{
				mdxtm.azcgf(rSACryptoServiceProvider.PublicOnly, rSACryptoServiceProvider, p0);
				RSAParameters parameters2 = rSACryptoServiceProvider.ExportParameters(includePrivateParameters: true);
				return new PrivateKeyInfo(parameters2);
			}
			finally
			{
				if (rSACryptoServiceProvider != null && 0 == 0)
				{
					((IDisposable)rSACryptoServiceProvider).Dispose();
				}
			}
		}
		case KeyAlgorithm.DSA:
		{
			DSACryptoServiceProvider dSACryptoServiceProvider = new DSACryptoServiceProvider(cspParameters);
			try
			{
				mdxtm.azcgf(dSACryptoServiceProvider.PublicOnly, dSACryptoServiceProvider, p0);
				DSAParameters parameters = dSACryptoServiceProvider.ExportParameters(includePrivateParameters: true);
				return new PrivateKeyInfo(parameters);
			}
			finally
			{
				if (dSACryptoServiceProvider != null && 0 == 0)
				{
					((IDisposable)dSACryptoServiceProvider).Dispose();
				}
			}
		}
		default:
			throw new CryptographicException("Export not supported for this private key.");
		}
	}

	private void yelkm(byte[] p0)
	{
		string p1 = null;
		int p2 = 1;
		gmetq.qjkxo(ref p1, ref p2);
		int p3 = pothu.qfori(out fiszk, null, p1, p2, 4026531840u);
		pothu.gqrne(p3, "Could not acquire crypto context.");
		rnqdw rnqdw2 = new rnqdw(p0);
		try
		{
			int p4 = pothu.lwvkz(fiszk, rnqdw2.peara(), rnqdw2.hrzld, IntPtr.Zero, pothu.qnnwg, out awzec);
			pothu.gqrne(p4, "Unable to import public key.");
		}
		finally
		{
			rnqdw2.fbdzt();
		}
	}

	private int yxrmt(jyamo p0)
	{
		if (ayjbh != KeyAlgorithm.RSA && 0 == 0)
		{
			throw new CryptographicException("Decryption is only supported with RSA algorithm.");
		}
		int num = 0;
		switch (p0.vmeor)
		{
		case xdgzn.bntzq:
			if (!dahxy.kxxtc || 1 == 0)
			{
				throw new CryptographicException("RSA/OAEP is not supported on this platform.");
			}
			if (p0.fbcyx != HashingAlgorithmId.SHA1)
			{
				throw new CryptographicException(string.Concat("RSA/OAEP with ", p0.fbcyx, " is not supported for this key."));
			}
			if (p0.bablj != p0.fbcyx)
			{
				throw new CryptographicException("RSA/OAEP with mismatched hash algorithms is not supported for this key.");
			}
			if (!jlfbq.uqqcs(p0.blonh) || 1 == 0)
			{
				throw new CryptographicException("RSA/OAEP with input parameter is not supported for this key.");
			}
			num |= 0x40;
			break;
		default:
			throw new CryptographicException("Padding scheme is not supported for this algorithm.");
		case xdgzn.ctbmq:
			break;
		}
		return num;
	}

	private void jrrym(mrxvh p0, SignatureHashAlgorithm p1, int p2)
	{
		if (p2 == 0 || 1 == 0)
		{
			throw new CryptographicException("Unsupported signature hash algorithm.");
		}
		switch (p0.hqtwc)
		{
		case goies.gbwxv:
			if (ayjbh == KeyAlgorithm.RSA || 1 == 0)
			{
				goto default;
			}
			break;
		case goies.lfkki:
			if (ayjbh == KeyAlgorithm.RSA)
			{
				break;
			}
			goto default;
		default:
			throw new CryptographicException("Padding scheme is not supported for this algorithm.");
		}
	}
}
