using System;
using System.Diagnostics;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal abstract class vifpo : IDisposable
{
	public const int okcmg = 32;

	public const int momon = 8;

	public const int zuums = 12;

	public const int zqykz = 3;

	public const int vseks = 16;

	public const int inagt = 64;

	public const int abxde = 16;

	public const int kdzvq = 64;

	public const uint sxjyu = 1634760805u;

	public const uint rmwbm = 857760878u;

	public const uint qcxrv = 2036477234u;

	public const uint inksr = 1797285236u;

	public const string xpqya = "Unable to create ChaCha20 cipher. Avx2 intrinsics are not supported on this platform.";

	public const string gqdkj = "Unable to create ChaCha20 cipher. Arm AdvSimd intrinsics are not supported on this platform.";

	public const string gcahq = "Key must have 32 bytes.";

	public const string damqd = "Nonce must have 12 bytes.";

	public const string qwldk = "Algorithm not supported in FIPS-compliant mode.";

	private bool pelxc;

	public virtual int iumzx => 64;

	public virtual int tyckw => 64;

	protected vifpo()
	{
		pelxc = false;
	}

	public abstract int ivxhj(byte[] p0, int p1, int p2, byte[] p3, int p4);

	public byte[] qzvjn()
	{
		byte[] array = new byte[32];
		iohsh(array, IntPtr.Zero);
		return array;
	}

	public void iohsh(nxtme<byte> p0, IntPtr p1)
	{
		if (p0.hvvsm != 32)
		{
			throw new ArgumentException("keyStorage");
		}
		rxgtt(p0, p1);
	}

	public void vpvaw(byte[] p0, int p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("nonce");
		}
		if (p0.Length != 12)
		{
			throw new ArgumentException("nonce");
		}
		maivd(p0, p1);
	}

	public void Dispose()
	{
		if (!pelxc)
		{
			cldjt(p0: true);
			pelxc = true;
		}
	}

	protected virtual void cldjt(bool p0)
	{
	}

	public static byte[] kcayq(byte[] p0, int p1, byte[] p2, byte[] p3, zssmc p4 = zssmc.krmma)
	{
		byte[] array = new byte[p3.Length];
		vifpo vifpo2 = nukst(p0, p2, p1);
		try
		{
			vifpo2.ivxhj(p3, 0, p3.Length, array, 0);
			return array;
		}
		finally
		{
			if (vifpo2 != null && 0 == 0)
			{
				((IDisposable)vifpo2).Dispose();
			}
		}
	}

	public static byte[] ifygs(byte[] p0, byte[] p1, zssmc p2 = zssmc.krmma)
	{
		vifpo vifpo2 = nukst(p0, p1, 0, p2);
		try
		{
			return vifpo2.qzvjn();
		}
		finally
		{
			if (vifpo2 != null && 0 == 0)
			{
				((IDisposable)vifpo2).Dispose();
			}
		}
	}

	public static vifpo nukst(byte[] p0, byte[] p1, int p2, zssmc p3 = zssmc.krmma)
	{
		return p3 switch
		{
			zssmc.krmma => new qtllz(p0, p1, p2), 
			zssmc.iiuux => new qtllz(p0, p1, p2), 
			zssmc.bndah => throw new CryptographicException("Unable to create ChaCha20 cipher. Avx2 intrinsics are not supported on this platform."), 
			zssmc.ajotc => throw new CryptographicException("Unable to create ChaCha20 cipher. Arm AdvSimd intrinsics are not supported on this platform."), 
			_ => throw new ArgumentOutOfRangeException("implementationType"), 
		};
	}

	protected abstract void maivd(byte[] p0, int p1);

	protected abstract void rxgtt(nxtme<byte> p0, IntPtr p1);

	[Conditional("DEBUG")]
	protected void vxrbs()
	{
		if (pelxc && 0 == 0)
		{
			throw new ObjectDisposedException("Chacha20TransformBase");
		}
	}

	protected static void uuala(byte[] p0, byte[] p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("key");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("nonce");
		}
		if (p0.Length != 32)
		{
			throw new ArgumentException("key", "Key must have 32 bytes.");
		}
		if (p1.Length != 12)
		{
			throw new ArgumentException("nonce", "Nonce must have 12 bytes.");
		}
		if (CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
		{
			throw new CryptographicException("Algorithm not supported in FIPS-compliant mode.");
		}
	}

	protected static void tgvgr(byte[] p0, int p1, int p2, byte[] p3, int p4)
	{
		dahxy.valft(p0, "inputBuffer", p1, "inputOffset", p2, "inputCount");
		dahxy.dionp(p3, p4, p2);
	}
}
