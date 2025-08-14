using System;
using System.IO;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal sealed class mecsr : HashAlgorithm, IHashTransform, IDisposable
{
	private const uint bknit = 3988292384u;

	private static readonly uint[] ihezx;

	private uint cvomc;

	public long hxtqz
	{
		get
		{
			return ~cvomc;
		}
		set
		{
			cvomc = (uint)(~value);
		}
	}

	public override int HashSize => 32;

	static mecsr()
	{
		ihezx = new uint[256];
		int num = 0;
		if (num != 0)
		{
			goto IL_0011;
		}
		goto IL_0050;
		IL_0011:
		uint num2 = (uint)num;
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_0018;
		}
		goto IL_0037;
		IL_0018:
		uint num4 = num2 & 1;
		num2 >>= 1;
		if (num4 != 0 && 0 == 0)
		{
			num2 ^= 0xEDB88320u;
		}
		num3++;
		goto IL_0037;
		IL_0037:
		if (num3 < 8)
		{
			goto IL_0018;
		}
		ihezx[num] = num2;
		num++;
		goto IL_0050;
		IL_0050:
		if (num >= 256)
		{
			return;
		}
		goto IL_0011;
	}

	public mecsr()
	{
		Initialize();
	}

	public byte[] GetHash()
	{
		return HashFinal();
	}

	public override void Initialize()
	{
		cvomc = uint.MaxValue;
	}

	public void xmdwo(byte p0)
	{
		cvomc = (cvomc >> 8) ^ ihezx[p0 ^ (cvomc & 0xFF)];
	}

	public void Process(byte[] data, int offset, int count)
	{
		HashCore(data, offset, count);
	}

	public void zeeih(byte[] p0)
	{
		Process(p0, 0, p0.Length);
	}

	public void cvnqk(Stream p0)
	{
		byte[] array = new byte[1024];
		while (true)
		{
			int num = p0.Read(array, 0, array.Length);
			if (num == 0 || 1 == 0)
			{
				break;
			}
			Process(array, 0, num);
		}
	}

	public void borav(Stream p0, long p1)
	{
		byte[] array = new byte[1024];
		while (p1 > 0)
		{
			int count = (int)Math.Min(p1, array.Length);
			count = p0.Read(array, 0, count);
			if (count == 0 || 1 == 0)
			{
				break;
			}
			Process(array, 0, count);
			p1 -= count;
		}
	}

	public void Reset()
	{
		Initialize();
	}

	protected override void HashCore(byte[] data, int offset, int count)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_003c;
		IL_0006:
		cvomc = (cvomc >> 8) ^ ihezx[data[offset++] ^ (cvomc & 0xFF)];
		num++;
		goto IL_003c;
		IL_003c:
		if (num >= count)
		{
			return;
		}
		goto IL_0006;
	}

	protected override byte[] HashFinal()
	{
		byte[] array = new byte[4];
		long num = hxtqz;
		jlfbq.lyicr(array, 0, (int)num);
		return array;
	}
}
