using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using onrkn;

namespace Rebex.Security.Cryptography;

public class DiffieHellmanCryptoServiceProvider : DiffieHellman, hibhk
{
	private static IntPtr muqvx;

	private int mfpvp;

	private DiffieHellmanParameters etwdw;

	private bool fkqys;

	public override string SignatureAlgorithm
	{
		get
		{
			throw new NotSupportedException("Diffie-Hellman does not support signatures.");
		}
	}

	public override string KeyExchangeAlgorithm => "Diffie-Hellman";

	private static void oevbf()
	{
		if (muqvx == IntPtr.Zero && 0 == 0)
		{
			muqvx = gmetq.gdehj(p0: true);
		}
	}

	public DiffieHellmanCryptoServiceProvider()
		: this(1024)
	{
	}

	internal static int mdswo()
	{
		if (dahxy.xzevd && 0 == 0)
		{
			return 0;
		}
		if (gmetq.bzdqw() && 0 == 0)
		{
			if (Environment.OSVersion.Version.Major <= 5)
			{
				return 2048;
			}
			return 4096;
		}
		return 1024;
	}

	public DiffieHellmanCryptoServiceProvider(int keySize)
	{
		if (dahxy.xzevd && 0 == 0)
		{
			throw new CryptographicException("Diffie-Hellman CSP is not supported on this platform.");
		}
		mfpvp = mdswo();
		KeySizeValue = keySize;
		if (!fqzsi(keySize) || 1 == 0)
		{
			throw new CryptographicException("Unsupported key size ({0}).", keySize.ToString());
		}
		oevbf();
		LegalKeySizesValue = new KeySizes[1]
		{
			new KeySizes(512, mfpvp, 64)
		};
	}

	internal static bool fqzsi(int p0)
	{
		if ((p0 % 64 == 0 || 1 == 0) && p0 >= 512)
		{
			return p0 <= mdswo();
		}
		return false;
	}

	private void iidbl()
	{
		if (!fkqys)
		{
			int keySizeValue = KeySizeValue;
			IntPtr p = IntPtr.Zero;
			if (pothu.peviy(muqvx, 43522, (uint)((keySizeValue << 16) | 1), out p) == 0 || 1 == 0)
			{
				throw new CryptographicException(brgjd.edcru("Unable to generate key (0x{0:X8}).", Marshal.GetLastWin32Error()));
			}
			etwdw = aaxhc(p);
			etwdw.Y = ojluq(p);
			if (pothu.xdyfe(p) == 0 || 1 == 0)
			{
				throw new CryptographicException(brgjd.edcru("Unable to destroy key (0x{0:X8}).", Marshal.GetLastWin32Error()));
			}
			fkqys = true;
		}
	}

	private static byte[] doagb(IntPtr p0, int p1)
	{
		int p2 = 0;
		if (pothu.rrqio(p0, IntPtr.Zero, p1, 0, IntPtr.Zero, ref p2) == 0 || 1 == 0)
		{
			throw new CryptographicException(brgjd.edcru("Unable to get key blob (0x{0:X8}).", Marshal.GetLastWin32Error()));
		}
		samhn samhn = new samhn(p2);
		try
		{
			if (pothu.rrqio(p0, IntPtr.Zero, p1, 0, samhn.inyna(), ref p2) == 0 || 1 == 0)
			{
				throw new CryptographicException(brgjd.edcru("Unable to get key blob (0x{0:X8}).", Marshal.GetLastWin32Error()));
			}
			return samhn.hsdhr();
		}
		finally
		{
			if (samhn != null && 0 == 0)
			{
				((IDisposable)samhn).Dispose();
			}
		}
	}

	private static byte[] ojluq(IntPtr p0)
	{
		byte[] buffer = doagb(p0, 6);
		MemoryStream memoryStream = new MemoryStream(buffer);
		try
		{
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.ASCII);
			try
			{
				if (binaryReader.ReadByte() != 6)
				{
					throw new CryptographicException("Unsupported key blob format.");
				}
				if (binaryReader.ReadByte() != 2)
				{
					throw new CryptographicException("Unsupported key blob format.");
				}
				binaryReader.ReadInt16();
				if (binaryReader.ReadInt32() != 43522)
				{
					throw new CryptographicException("Unsupported key blob format.");
				}
				if (binaryReader.ReadInt32() != 826819584)
				{
					throw new CryptographicException("Unsupported key blob format.");
				}
				int num = binaryReader.ReadInt32();
				if (num == 0 || 1 == 0)
				{
					throw new CryptographicException("Unsupported key blob format.");
				}
				byte[] array = new byte[num / 8];
				if (array.Length != binaryReader.Read(array, 0, array.Length))
				{
					throw new CryptographicException("Unsupported key blob format.");
				}
				Array.Reverse(array);
				return jlfbq.cnbay(array);
			}
			finally
			{
				if (binaryReader != null && 0 == 0)
				{
					((IDisposable)binaryReader).Dispose();
				}
			}
		}
		finally
		{
			if (memoryStream != null && 0 == 0)
			{
				((IDisposable)memoryStream).Dispose();
			}
		}
	}

	private static DiffieHellmanParameters aaxhc(IntPtr p0)
	{
		byte[] buffer = doagb(p0, 7);
		MemoryStream memoryStream = new MemoryStream(buffer);
		try
		{
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.ASCII);
			try
			{
				if (binaryReader.ReadByte() != 7)
				{
					throw new CryptographicException("Unsupported key blob format.");
				}
				if (binaryReader.ReadByte() != 2)
				{
					throw new CryptographicException("Unsupported key blob format.");
				}
				binaryReader.ReadInt16();
				if (binaryReader.ReadInt32() != 43522)
				{
					throw new CryptographicException("Unsupported key blob format.");
				}
				if (binaryReader.ReadInt32() != 843596800)
				{
					throw new CryptographicException("Unsupported key blob format.");
				}
				int num = binaryReader.ReadInt32();
				byte[] array = new byte[num / 8];
				byte[] array2 = new byte[num / 8];
				byte[] array3 = new byte[num / 8];
				if (array.Length != binaryReader.Read(array, 0, array.Length))
				{
					throw new CryptographicException("Unsupported key blob format.");
				}
				if (array2.Length != binaryReader.Read(array2, 0, array2.Length))
				{
					throw new CryptographicException("Unsupported key blob format.");
				}
				if (array3.Length != binaryReader.Read(array3, 0, array3.Length))
				{
					throw new CryptographicException("Unsupported key blob format.");
				}
				Array.Reverse(array);
				Array.Reverse(array2);
				Array.Reverse(array3);
				return new DiffieHellmanParameters
				{
					P = array,
					G = array2,
					X = array3
				};
			}
			finally
			{
				if (binaryReader != null && 0 == 0)
				{
					((IDisposable)binaryReader).Dispose();
				}
			}
		}
		finally
		{
			if (memoryStream != null && 0 == 0)
			{
				((IDisposable)memoryStream).Dispose();
			}
		}
	}

	private static samhn jbipz(DiffieHellmanParameters p0)
	{
		byte[] array = (byte[])p0.P.Clone();
		byte[] array2 = (byte[])p0.G.Clone();
		byte[] array3 = (byte[])p0.X.Clone();
		Array.Reverse(array);
		Array.Reverse(array2);
		Array.Reverse(array3);
		MemoryStream memoryStream = new MemoryStream();
		try
		{
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.ASCII);
			try
			{
				binaryWriter.Write((byte)7);
				binaryWriter.Write((byte)2);
				binaryWriter.Write((short)0);
				binaryWriter.Write(43522);
				binaryWriter.Write(843596800);
				binaryWriter.Write(array.Length * 8);
				binaryWriter.Write(array, 0, array.Length);
				binaryWriter.Write(array2, 0, array2.Length);
				if (array2.Length < array.Length)
				{
					int num = array.Length - array2.Length;
					binaryWriter.Write(new byte[num], 0, num);
				}
				binaryWriter.Write(array3, 0, array3.Length);
				if (array3.Length < array.Length)
				{
					int num2 = array.Length - array3.Length;
					binaryWriter.Write(new byte[num2], 0, num2);
				}
				binaryWriter.Flush();
				return samhn.ojtyh(memoryStream.ToArray());
			}
			finally
			{
				if (binaryWriter != null && 0 == 0)
				{
					((IDisposable)binaryWriter).Dispose();
				}
			}
		}
		finally
		{
			if (memoryStream != null && 0 == 0)
			{
				((IDisposable)memoryStream).Dispose();
			}
		}
	}

	public override byte[] GetPublicKey()
	{
		iidbl();
		return (byte[])etwdw.Y.Clone();
	}

	private byte[] jmmov(byte[] p0)
	{
		return cfqal(p0, p1: false);
	}

	byte[] hibhk.ovrid(byte[] p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in jmmov
		return this.jmmov(p0);
	}

	public override byte[] GetSharedSecretKey(byte[] otherPublicKey)
	{
		return cfqal(otherPublicKey, p1: true);
	}

	private byte[] cfqal(byte[] p0, bool p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("otherPublicKey");
		}
		iidbl();
		DiffieHellmanParameters p2 = ExportParameters(includePrivateParameters: true);
		byte[] array = jlfbq.cnbay(p0);
		byte[] p3 = p2.P;
		if (array.Length > p3.Length)
		{
			throw new CryptographicException("Public key is longer than the prime modulus.");
		}
		if (array.Length == p3.Length && array[0] > p3[0])
		{
			throw new CryptographicException("Public key is bigger than the prime modulus.");
		}
		p2.G = array;
		IntPtr p4 = IntPtr.Zero;
		samhn samhn = jbipz(p2);
		try
		{
			if (pothu.lwvkz(muqvx, samhn.inyna(), samhn.enukg, IntPtr.Zero, 0, out p4) == 0 || 1 == 0)
			{
				throw new CryptographicException(brgjd.edcru("Unable to import key blob (0x{0:X8}).", Marshal.GetLastWin32Error()));
			}
		}
		finally
		{
			if (samhn != null && 0 == 0)
			{
				((IDisposable)samhn).Dispose();
			}
		}
		byte[] array2 = ojluq(p4);
		if (pothu.xdyfe(p4) == 0 || 1 == 0)
		{
			throw new CryptographicException(brgjd.edcru("Unable to destroy key (0x{0:X8}).", Marshal.GetLastWin32Error()));
		}
		if (p1 && 0 == 0 && array2[0] >= 128)
		{
			byte[] array3 = new byte[array2.Length + 1];
			array2.CopyTo(array3, 1);
			array2 = array3;
		}
		return array2;
	}

	private static samhn nwbhp(byte[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		samhn samhn = new samhn(IntPtr.Size * 2 + p0.Length);
		IntPtr intPtr = new IntPtr(samhn.inyna().ToInt64() + IntPtr.Size * 2);
		samhn.fpzdi(0, p0.Length);
		samhn.qurik(IntPtr.Size, intPtr);
		Marshal.Copy(p0, 0, intPtr, p0.Length);
		return samhn;
	}

	public override void ImportParameters(DiffieHellmanParameters parameters)
	{
		parameters = DiffieHellman.qftrp(parameters, p1: true);
		KeySizeValue = parameters.P.Length * 8;
		if (((KeySizeValue % 64 != 0) ? true : false) || KeySizeValue < 512 || KeySizeValue > mfpvp)
		{
			throw new CryptographicException("Unsupported key size ({0}).", KeySizeValue.ToString());
		}
		if ((parameters.P[0] & 0x80) == 0 || 1 == 0)
		{
			throw new CryptographicException("Unsupported prime modulus size.");
		}
		if (parameters.G == null || 1 == 0)
		{
			throw new CryptographicException("Missing G parameter.", "parameters");
		}
		if (parameters.G.Length > parameters.P.Length)
		{
			throw new CryptographicException("Generator is longer than the prime modulus.");
		}
		oevbf();
		IntPtr p = IntPtr.Zero;
		try
		{
			if (parameters.X != null && 0 == 0)
			{
				if (parameters.X.Length > parameters.P.Length)
				{
					throw new CryptographicException("Private key is longer than the prime modulus.");
				}
				samhn samhn = jbipz(parameters);
				try
				{
					if (pothu.lwvkz(muqvx, samhn.inyna(), samhn.enukg, IntPtr.Zero, 0, out p) == 0 || 1 == 0)
					{
						throw new CryptographicException(brgjd.edcru("Unable to import key blob (0x{0:X8}).", Marshal.GetLastWin32Error()));
					}
				}
				finally
				{
					if (samhn != null && 0 == 0)
					{
						((IDisposable)samhn).Dispose();
					}
				}
				etwdw = parameters;
			}
			else
			{
				byte[] array = (byte[])parameters.P.Clone();
				Array.Reverse(array);
				byte[] array2 = new byte[parameters.P.Length];
				parameters.G.CopyTo(array2, array2.Length - parameters.G.Length);
				Array.Reverse(array2);
				if (pothu.peviy(muqvx, 43522, (uint)((KeySizeValue << 16) | 0x40 | 1), out p) == 0 || 1 == 0)
				{
					throw new CryptographicException(brgjd.edcru("Unable to generate key (0x{0:X8}).", Marshal.GetLastWin32Error()));
				}
				samhn samhn2 = nwbhp(array2);
				try
				{
					if (pothu.sfbuy(p, 12, samhn2.inyna(), 0) == 0 || 1 == 0)
					{
						throw new CryptographicException(brgjd.edcru("Unable to set key parameters (0x{0:X8}).", Marshal.GetLastWin32Error()));
					}
				}
				finally
				{
					if (samhn2 != null && 0 == 0)
					{
						((IDisposable)samhn2).Dispose();
					}
				}
				samhn samhn3 = nwbhp(array);
				try
				{
					if (pothu.sfbuy(p, 11, samhn3.inyna(), 0) == 0 || 1 == 0)
					{
						throw new CryptographicException(brgjd.edcru("Unable to set key parameters (0x{0:X8}).", Marshal.GetLastWin32Error()));
					}
				}
				finally
				{
					if (samhn3 != null && 0 == 0)
					{
						((IDisposable)samhn3).Dispose();
					}
				}
				if (pothu.sfbuy(p, 14, IntPtr.Zero, 0) == 0 || 1 == 0)
				{
					throw new CryptographicException(brgjd.edcru("Unable to set key parameters (0x{0:X8}).", Marshal.GetLastWin32Error()));
				}
				etwdw = aaxhc(p);
			}
			etwdw.Y = ojluq(p);
			fkqys = true;
		}
		finally
		{
			if (p != IntPtr.Zero && 0 == 0 && (pothu.xdyfe(p) == 0 || 1 == 0))
			{
				throw new CryptographicException(brgjd.edcru("Unable to destroy key (0x{0:X8}).", Marshal.GetLastWin32Error()));
			}
		}
	}

	public override DiffieHellmanParameters ExportParameters(bool includePrivateParameters)
	{
		iidbl();
		DiffieHellmanParameters result = new DiffieHellmanParameters
		{
			P = (byte[])etwdw.P.Clone(),
			G = (byte[])etwdw.G.Clone(),
			Y = (byte[])etwdw.Y.Clone()
		};
		if (includePrivateParameters && 0 == 0)
		{
			result.X = (byte[])etwdw.X.Clone();
		}
		return result;
	}

	protected override void Dispose(bool disposing)
	{
	}
}
