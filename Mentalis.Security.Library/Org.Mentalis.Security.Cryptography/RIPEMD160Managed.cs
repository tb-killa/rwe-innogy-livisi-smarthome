using System;
using System.Runtime.InteropServices;

namespace Org.Mentalis.Security.Cryptography;

public sealed class RIPEMD160Managed : RIPEMD160
{
	private bool m_Disposed;

	private byte[] m_ExtraData;

	private uint[] m_HashValue;

	private ulong m_Length;

	private uint[] m_X;

	public override int InputBlockSize => 64;

	public RIPEMD160Managed()
	{
		m_X = new uint[16];
		m_HashValue = new uint[5];
		m_ExtraData = new byte[0];
		m_Disposed = false;
		Initialize();
	}

	public override void Initialize()
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		m_HashValue[0] = 1732584193u;
		m_HashValue[1] = 4023233417u;
		m_HashValue[2] = 2562383102u;
		m_HashValue[3] = 271733878u;
		m_HashValue[4] = 3285377520u;
		m_ExtraData = new byte[0];
		m_Length = 0uL;
	}

	protected override void HashCore(byte[] array, int ibStart, int cbSize)
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (cbSize != 0)
		{
			int i = 0;
			byte[] array2 = new byte[m_ExtraData.Length + cbSize];
			Array.Copy(m_ExtraData, 0, array2, 0, m_ExtraData.Length);
			Array.Copy(array, ibStart, array2, m_ExtraData.Length, cbSize);
			GCHandle gCHandle = GCHandle.Alloc(m_X, GCHandleType.Pinned);
			IntPtr destination = gCHandle.AddrOfPinnedObject();
			for (; array2.Length - i >= 64; i += 64)
			{
				Marshal.Copy(array2, i, destination, 64);
				Compress();
			}
			gCHandle.Free();
			m_ExtraData = new byte[array2.Length - i];
			Array.Copy(array2, i, m_ExtraData, 0, m_ExtraData.Length);
			Array.Clear(array2, 0, array2.Length);
			m_Length += (uint)cbSize;
		}
	}

	protected override byte[] HashFinal()
	{
		if (m_Disposed)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		CompressFinal(m_Length);
		byte[] array = new byte[20];
		GCHandle gCHandle = GCHandle.Alloc(m_HashValue, GCHandleType.Pinned);
		IntPtr source = gCHandle.AddrOfPinnedObject();
		Marshal.Copy(source, array, 0, 20);
		gCHandle.Free();
		return array;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		try
		{
			GC.SuppressFinalize(this);
		}
		catch
		{
		}
		m_Disposed = true;
	}

	~RIPEMD160Managed()
	{
		Clear();
	}

	private void Compress()
	{
		uint a = m_HashValue[0];
		uint c = m_HashValue[1];
		uint c2 = m_HashValue[2];
		uint a2 = m_HashValue[3];
		uint a3 = m_HashValue[4];
		uint a4 = m_HashValue[0];
		uint c3 = m_HashValue[1];
		uint c4 = m_HashValue[2];
		uint a5 = m_HashValue[3];
		uint a6 = m_HashValue[4];
		FF(ref a, c, ref c2, a2, a3, m_X[0], 11);
		FF(ref a3, a, ref c, c2, a2, m_X[1], 14);
		FF(ref a2, a3, ref a, c, c2, m_X[2], 15);
		FF(ref c2, a2, ref a3, a, c, m_X[3], 12);
		FF(ref c, c2, ref a2, a3, a, m_X[4], 5);
		FF(ref a, c, ref c2, a2, a3, m_X[5], 8);
		FF(ref a3, a, ref c, c2, a2, m_X[6], 7);
		FF(ref a2, a3, ref a, c, c2, m_X[7], 9);
		FF(ref c2, a2, ref a3, a, c, m_X[8], 11);
		FF(ref c, c2, ref a2, a3, a, m_X[9], 13);
		FF(ref a, c, ref c2, a2, a3, m_X[10], 14);
		FF(ref a3, a, ref c, c2, a2, m_X[11], 15);
		FF(ref a2, a3, ref a, c, c2, m_X[12], 6);
		FF(ref c2, a2, ref a3, a, c, m_X[13], 7);
		FF(ref c, c2, ref a2, a3, a, m_X[14], 9);
		FF(ref a, c, ref c2, a2, a3, m_X[15], 8);
		GG(ref a3, a, ref c, c2, a2, m_X[7], 7);
		GG(ref a2, a3, ref a, c, c2, m_X[4], 6);
		GG(ref c2, a2, ref a3, a, c, m_X[13], 8);
		GG(ref c, c2, ref a2, a3, a, m_X[1], 13);
		GG(ref a, c, ref c2, a2, a3, m_X[10], 11);
		GG(ref a3, a, ref c, c2, a2, m_X[6], 9);
		GG(ref a2, a3, ref a, c, c2, m_X[15], 7);
		GG(ref c2, a2, ref a3, a, c, m_X[3], 15);
		GG(ref c, c2, ref a2, a3, a, m_X[12], 7);
		GG(ref a, c, ref c2, a2, a3, m_X[0], 12);
		GG(ref a3, a, ref c, c2, a2, m_X[9], 15);
		GG(ref a2, a3, ref a, c, c2, m_X[5], 9);
		GG(ref c2, a2, ref a3, a, c, m_X[2], 11);
		GG(ref c, c2, ref a2, a3, a, m_X[14], 7);
		GG(ref a, c, ref c2, a2, a3, m_X[11], 13);
		GG(ref a3, a, ref c, c2, a2, m_X[8], 12);
		HH(ref a2, a3, ref a, c, c2, m_X[3], 11);
		HH(ref c2, a2, ref a3, a, c, m_X[10], 13);
		HH(ref c, c2, ref a2, a3, a, m_X[14], 6);
		HH(ref a, c, ref c2, a2, a3, m_X[4], 7);
		HH(ref a3, a, ref c, c2, a2, m_X[9], 14);
		HH(ref a2, a3, ref a, c, c2, m_X[15], 9);
		HH(ref c2, a2, ref a3, a, c, m_X[8], 13);
		HH(ref c, c2, ref a2, a3, a, m_X[1], 15);
		HH(ref a, c, ref c2, a2, a3, m_X[2], 14);
		HH(ref a3, a, ref c, c2, a2, m_X[7], 8);
		HH(ref a2, a3, ref a, c, c2, m_X[0], 13);
		HH(ref c2, a2, ref a3, a, c, m_X[6], 6);
		HH(ref c, c2, ref a2, a3, a, m_X[13], 5);
		HH(ref a, c, ref c2, a2, a3, m_X[11], 12);
		HH(ref a3, a, ref c, c2, a2, m_X[5], 7);
		HH(ref a2, a3, ref a, c, c2, m_X[12], 5);
		II(ref c2, a2, ref a3, a, c, m_X[1], 11);
		II(ref c, c2, ref a2, a3, a, m_X[9], 12);
		II(ref a, c, ref c2, a2, a3, m_X[11], 14);
		II(ref a3, a, ref c, c2, a2, m_X[10], 15);
		II(ref a2, a3, ref a, c, c2, m_X[0], 14);
		II(ref c2, a2, ref a3, a, c, m_X[8], 15);
		II(ref c, c2, ref a2, a3, a, m_X[12], 9);
		II(ref a, c, ref c2, a2, a3, m_X[4], 8);
		II(ref a3, a, ref c, c2, a2, m_X[13], 9);
		II(ref a2, a3, ref a, c, c2, m_X[3], 14);
		II(ref c2, a2, ref a3, a, c, m_X[7], 5);
		II(ref c, c2, ref a2, a3, a, m_X[15], 6);
		II(ref a, c, ref c2, a2, a3, m_X[14], 8);
		II(ref a3, a, ref c, c2, a2, m_X[5], 6);
		II(ref a2, a3, ref a, c, c2, m_X[6], 5);
		II(ref c2, a2, ref a3, a, c, m_X[2], 12);
		JJ(ref c, c2, ref a2, a3, a, m_X[4], 9);
		JJ(ref a, c, ref c2, a2, a3, m_X[0], 15);
		JJ(ref a3, a, ref c, c2, a2, m_X[5], 5);
		JJ(ref a2, a3, ref a, c, c2, m_X[9], 11);
		JJ(ref c2, a2, ref a3, a, c, m_X[7], 6);
		JJ(ref c, c2, ref a2, a3, a, m_X[12], 8);
		JJ(ref a, c, ref c2, a2, a3, m_X[2], 13);
		JJ(ref a3, a, ref c, c2, a2, m_X[10], 12);
		JJ(ref a2, a3, ref a, c, c2, m_X[14], 5);
		JJ(ref c2, a2, ref a3, a, c, m_X[1], 12);
		JJ(ref c, c2, ref a2, a3, a, m_X[3], 13);
		JJ(ref a, c, ref c2, a2, a3, m_X[8], 14);
		JJ(ref a3, a, ref c, c2, a2, m_X[11], 11);
		JJ(ref a2, a3, ref a, c, c2, m_X[6], 8);
		JJ(ref c2, a2, ref a3, a, c, m_X[15], 5);
		JJ(ref c, c2, ref a2, a3, a, m_X[13], 6);
		JJJ(ref a4, c3, ref c4, a5, a6, m_X[5], 8);
		JJJ(ref a6, a4, ref c3, c4, a5, m_X[14], 9);
		JJJ(ref a5, a6, ref a4, c3, c4, m_X[7], 9);
		JJJ(ref c4, a5, ref a6, a4, c3, m_X[0], 11);
		JJJ(ref c3, c4, ref a5, a6, a4, m_X[9], 13);
		JJJ(ref a4, c3, ref c4, a5, a6, m_X[2], 15);
		JJJ(ref a6, a4, ref c3, c4, a5, m_X[11], 15);
		JJJ(ref a5, a6, ref a4, c3, c4, m_X[4], 5);
		JJJ(ref c4, a5, ref a6, a4, c3, m_X[13], 7);
		JJJ(ref c3, c4, ref a5, a6, a4, m_X[6], 7);
		JJJ(ref a4, c3, ref c4, a5, a6, m_X[15], 8);
		JJJ(ref a6, a4, ref c3, c4, a5, m_X[8], 11);
		JJJ(ref a5, a6, ref a4, c3, c4, m_X[1], 14);
		JJJ(ref c4, a5, ref a6, a4, c3, m_X[10], 14);
		JJJ(ref c3, c4, ref a5, a6, a4, m_X[3], 12);
		JJJ(ref a4, c3, ref c4, a5, a6, m_X[12], 6);
		III(ref a6, a4, ref c3, c4, a5, m_X[6], 9);
		III(ref a5, a6, ref a4, c3, c4, m_X[11], 13);
		III(ref c4, a5, ref a6, a4, c3, m_X[3], 15);
		III(ref c3, c4, ref a5, a6, a4, m_X[7], 7);
		III(ref a4, c3, ref c4, a5, a6, m_X[0], 12);
		III(ref a6, a4, ref c3, c4, a5, m_X[13], 8);
		III(ref a5, a6, ref a4, c3, c4, m_X[5], 9);
		III(ref c4, a5, ref a6, a4, c3, m_X[10], 11);
		III(ref c3, c4, ref a5, a6, a4, m_X[14], 7);
		III(ref a4, c3, ref c4, a5, a6, m_X[15], 7);
		III(ref a6, a4, ref c3, c4, a5, m_X[8], 12);
		III(ref a5, a6, ref a4, c3, c4, m_X[12], 7);
		III(ref c4, a5, ref a6, a4, c3, m_X[4], 6);
		III(ref c3, c4, ref a5, a6, a4, m_X[9], 15);
		III(ref a4, c3, ref c4, a5, a6, m_X[1], 13);
		III(ref a6, a4, ref c3, c4, a5, m_X[2], 11);
		HHH(ref a5, a6, ref a4, c3, c4, m_X[15], 9);
		HHH(ref c4, a5, ref a6, a4, c3, m_X[5], 7);
		HHH(ref c3, c4, ref a5, a6, a4, m_X[1], 15);
		HHH(ref a4, c3, ref c4, a5, a6, m_X[3], 11);
		HHH(ref a6, a4, ref c3, c4, a5, m_X[7], 8);
		HHH(ref a5, a6, ref a4, c3, c4, m_X[14], 6);
		HHH(ref c4, a5, ref a6, a4, c3, m_X[6], 6);
		HHH(ref c3, c4, ref a5, a6, a4, m_X[9], 14);
		HHH(ref a4, c3, ref c4, a5, a6, m_X[11], 12);
		HHH(ref a6, a4, ref c3, c4, a5, m_X[8], 13);
		HHH(ref a5, a6, ref a4, c3, c4, m_X[12], 5);
		HHH(ref c4, a5, ref a6, a4, c3, m_X[2], 14);
		HHH(ref c3, c4, ref a5, a6, a4, m_X[10], 13);
		HHH(ref a4, c3, ref c4, a5, a6, m_X[0], 13);
		HHH(ref a6, a4, ref c3, c4, a5, m_X[4], 7);
		HHH(ref a5, a6, ref a4, c3, c4, m_X[13], 5);
		GGG(ref c4, a5, ref a6, a4, c3, m_X[8], 15);
		GGG(ref c3, c4, ref a5, a6, a4, m_X[6], 5);
		GGG(ref a4, c3, ref c4, a5, a6, m_X[4], 8);
		GGG(ref a6, a4, ref c3, c4, a5, m_X[1], 11);
		GGG(ref a5, a6, ref a4, c3, c4, m_X[3], 14);
		GGG(ref c4, a5, ref a6, a4, c3, m_X[11], 14);
		GGG(ref c3, c4, ref a5, a6, a4, m_X[15], 6);
		GGG(ref a4, c3, ref c4, a5, a6, m_X[0], 14);
		GGG(ref a6, a4, ref c3, c4, a5, m_X[5], 6);
		GGG(ref a5, a6, ref a4, c3, c4, m_X[12], 9);
		GGG(ref c4, a5, ref a6, a4, c3, m_X[2], 12);
		GGG(ref c3, c4, ref a5, a6, a4, m_X[13], 9);
		GGG(ref a4, c3, ref c4, a5, a6, m_X[9], 12);
		GGG(ref a6, a4, ref c3, c4, a5, m_X[7], 5);
		GGG(ref a5, a6, ref a4, c3, c4, m_X[10], 15);
		GGG(ref c4, a5, ref a6, a4, c3, m_X[14], 8);
		FFF(ref c3, c4, ref a5, a6, a4, m_X[12], 8);
		FFF(ref a4, c3, ref c4, a5, a6, m_X[15], 5);
		FFF(ref a6, a4, ref c3, c4, a5, m_X[10], 12);
		FFF(ref a5, a6, ref a4, c3, c4, m_X[4], 9);
		FFF(ref c4, a5, ref a6, a4, c3, m_X[1], 12);
		FFF(ref c3, c4, ref a5, a6, a4, m_X[5], 5);
		FFF(ref a4, c3, ref c4, a5, a6, m_X[8], 14);
		FFF(ref a6, a4, ref c3, c4, a5, m_X[7], 6);
		FFF(ref a5, a6, ref a4, c3, c4, m_X[6], 8);
		FFF(ref c4, a5, ref a6, a4, c3, m_X[2], 13);
		FFF(ref c3, c4, ref a5, a6, a4, m_X[13], 6);
		FFF(ref a4, c3, ref c4, a5, a6, m_X[14], 5);
		FFF(ref a6, a4, ref c3, c4, a5, m_X[0], 15);
		FFF(ref a5, a6, ref a4, c3, c4, m_X[3], 13);
		FFF(ref c4, a5, ref a6, a4, c3, m_X[9], 11);
		FFF(ref c3, c4, ref a5, a6, a4, m_X[11], 11);
		a5 += c2 + m_HashValue[1];
		m_HashValue[1] = m_HashValue[2] + a2 + a6;
		m_HashValue[2] = m_HashValue[3] + a3 + a4;
		m_HashValue[3] = m_HashValue[4] + a + c3;
		m_HashValue[4] = m_HashValue[0] + c + c4;
		m_HashValue[0] = a5;
	}

	private void CompressFinal(ulong length)
	{
		uint num = (uint)(length & 0xFFFFFFFFu);
		uint num2 = (uint)(length >> 32);
		Array.Clear(m_X, 0, m_X.Length);
		int num3 = 0;
		for (uint num4 = 0u; num4 < (num & 0x3F); num4++)
		{
			m_X[num4 >> 2] ^= (uint)(m_ExtraData[num3++] << (int)(8 * (num4 & 3)));
		}
		m_X[(num >> 2) & 0xF] ^= (uint)(1 << (int)(8 * (num & 3) + 7));
		if ((num & 0x3F) > 55)
		{
			Compress();
			Array.Clear(m_X, 0, m_X.Length);
		}
		m_X[14] = num << 3;
		m_X[15] = (num >> 29) | (num2 << 3);
		Compress();
	}

	private uint ROL(uint x, int n)
	{
		return (x << n) | (x >> 32 - n);
	}

	private uint F(uint x, uint y, uint z)
	{
		return x ^ y ^ z;
	}

	private uint G(uint x, uint y, uint z)
	{
		return (x & y) | (~x & z);
	}

	private uint H(uint x, uint y, uint z)
	{
		return (x | ~y) ^ z;
	}

	private uint I(uint x, uint y, uint z)
	{
		return (x & z) | (y & ~z);
	}

	private uint J(uint x, uint y, uint z)
	{
		return x ^ (y | ~z);
	}

	private void FF(ref uint a, uint b, ref uint c, uint d, uint e, uint x, int s)
	{
		a += F(b, c, d) + x;
		a = ROL(a, s) + e;
		c = ROL(c, 10);
	}

	private void GG(ref uint a, uint b, ref uint c, uint d, uint e, uint x, int s)
	{
		a += G(b, c, d) + x + 1518500249;
		a = ROL(a, s) + e;
		c = ROL(c, 10);
	}

	private void HH(ref uint a, uint b, ref uint c, uint d, uint e, uint x, int s)
	{
		a += H(b, c, d) + x + 1859775393;
		a = ROL(a, s) + e;
		c = ROL(c, 10);
	}

	private void II(ref uint a, uint b, ref uint c, uint d, uint e, uint x, int s)
	{
		a += (uint)((int)(I(b, c, d) + x) + -1894007588);
		a = ROL(a, s) + e;
		c = ROL(c, 10);
	}

	private void JJ(ref uint a, uint b, ref uint c, uint d, uint e, uint x, int s)
	{
		a += (uint)((int)(J(b, c, d) + x) + -1454113458);
		a = ROL(a, s) + e;
		c = ROL(c, 10);
	}

	private void FFF(ref uint a, uint b, ref uint c, uint d, uint e, uint x, int s)
	{
		a += F(b, c, d) + x;
		a = ROL(a, s) + e;
		c = ROL(c, 10);
	}

	private void GGG(ref uint a, uint b, ref uint c, uint d, uint e, uint x, int s)
	{
		a += G(b, c, d) + x + 2053994217;
		a = ROL(a, s) + e;
		c = ROL(c, 10);
	}

	private void HHH(ref uint a, uint b, ref uint c, uint d, uint e, uint x, int s)
	{
		a += H(b, c, d) + x + 1836072691;
		a = ROL(a, s) + e;
		c = ROL(c, 10);
	}

	private void III(ref uint a, uint b, ref uint c, uint d, uint e, uint x, int s)
	{
		a += I(b, c, d) + x + 1548603684;
		a = ROL(a, s) + e;
		c = ROL(c, 10);
	}

	private void JJJ(ref uint a, uint b, ref uint c, uint d, uint e, uint x, int s)
	{
		a += J(b, c, d) + x + 1352829926;
		a = ROL(a, s) + e;
		c = ROL(c, 10);
	}
}
