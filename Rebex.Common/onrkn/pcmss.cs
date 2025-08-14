using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class pcmss : HashAlgorithm
{
	private IntPtr vyamy;

	private bool cgqxp;

	private int xxlhk;

	private int ohxlr;

	private static bool? pfscz;

	public static bool ftkxd
	{
		get
		{
			if (!pfscz.HasValue || 1 == 0)
			{
				bool value = false;
				int p;
				IntPtr intPtr = gmetq.cskhq(out p);
				if (intPtr != IntPtr.Zero && 0 == 0 && pothu.fclhh(intPtr, 32780, IntPtr.Zero, 0, out var p2) != 0 && 0 == 0)
				{
					value = true;
					pothu.sxrxx(p2);
				}
				pfscz = value;
			}
			return pfscz.Value;
		}
	}

	public pcmss(HashingAlgorithmId algorithm)
	{
		switch (algorithm)
		{
		case HashingAlgorithmId.SHA256:
			xxlhk = 32780;
			ohxlr = 32;
			break;
		case HashingAlgorithmId.SHA384:
			xxlhk = 32781;
			ohxlr = 48;
			break;
		case HashingAlgorithmId.SHA512:
			xxlhk = 32782;
			ohxlr = 64;
			break;
		default:
			throw new CryptographicException("Unsupported hash algorithm.");
		}
		cgqxp = false;
		Initialize();
	}

	public override void Initialize()
	{
		if (cgqxp && 0 == 0)
		{
			throw new ObjectDisposedException(null, "Object was disposed.");
		}
		if (vyamy != IntPtr.Zero && 0 == 0)
		{
			pothu.sxrxx(vyamy);
		}
		int p;
		IntPtr intPtr = gmetq.cskhq(out p);
		if (intPtr == IntPtr.Zero && 0 == 0)
		{
			throw new CryptographicException(brgjd.edcru("Unable to acquire RSA_AES CSP context ({0}).", p));
		}
		if (pothu.fclhh(intPtr, xxlhk, IntPtr.Zero, 0, out vyamy) == 0 || 1 == 0)
		{
			throw new CryptographicException(brgjd.edcru("Unable to use SHA-2 (0x{0:X}).", Marshal.GetLastWin32Error()));
		}
		HashSizeValue = ohxlr * 8;
	}

	protected override void HashCore(byte[] array, int ibStart, int cbSize)
	{
		if (cgqxp && 0 == 0)
		{
			throw new ObjectDisposedException(null, "Object was disposed.");
		}
		if ((cbSize != 0) ? true : false)
		{
			if (ibStart > 0)
			{
				byte[] sourceArray = array;
				array = new byte[cbSize];
				Array.Copy(sourceArray, ibStart, array, 0, cbSize);
			}
			if (pothu.ufoji(vyamy, array, cbSize, 0) == 0 || 1 == 0)
			{
				throw new CryptographicException(brgjd.edcru("Unable to hash data (0x{0:X}).", Marshal.GetLastWin32Error()));
			}
		}
	}

	protected override byte[] HashFinal()
	{
		if (cgqxp && 0 == 0)
		{
			throw new ObjectDisposedException(null, "Object was disposed.");
		}
		byte[] array = new byte[ohxlr];
		int p = array.Length;
		if (pothu.ovhzp(vyamy, 2, array, ref p, 0) == 0 || 1 == 0)
		{
			throw new CryptographicException(brgjd.edcru("Unable to get the hash value (0x{0:X}).", Marshal.GetLastWin32Error()));
		}
		return array;
	}

	protected override void Dispose(bool disposing)
	{
		if (cgqxp)
		{
			return;
		}
		cgqxp = true;
		if (vyamy == IntPtr.Zero)
		{
			return;
		}
		try
		{
			pothu.sxrxx(vyamy);
		}
		catch
		{
		}
		finally
		{
			vyamy = IntPtr.Zero;
		}
	}

	~pcmss()
	{
		Clear();
	}
}
