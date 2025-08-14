using System;
using Rebex.Security.Cryptography;

namespace onrkn;

internal abstract class vclqf : IHashTransform, IDisposable
{
	protected uint tfugy;

	protected uint xpwzo;

	protected uint jvpsp;

	protected uint mvysi;

	protected uint[] nfnsv;

	protected byte[] tjjwm;

	protected int chnzv;

	protected long wtsuv;

	public int HashSize => 128;

	public vclqf()
	{
		nfnsv = new uint[16];
		tjjwm = new byte[64];
		Reset();
	}

	protected abstract void pypmh(byte[] p0, int p1);

	public void Reset()
	{
		tfugy = 1732584193u;
		xpwzo = 4023233417u;
		jvpsp = 2562383102u;
		mvysi = 271733878u;
		chnzv = 0;
		wtsuv = 0L;
	}

	public void Process(byte[] buffer, int offset, int count)
	{
		if (count == 0 || 1 == 0)
		{
			return;
		}
		wtsuv += count << 3;
		if (chnzv > 0)
		{
			int num = tjjwm.Length - chnzv;
			if (count < num)
			{
				Array.Copy(buffer, offset, tjjwm, chnzv, count);
				chnzv += count;
				return;
			}
			Array.Copy(buffer, offset, tjjwm, chnzv, num);
			offset += num;
			count -= num;
			pypmh(tjjwm, 0);
			chnzv = 0;
		}
		while (count >= tjjwm.Length)
		{
			pypmh(buffer, offset);
			offset += tjjwm.Length;
			count -= tjjwm.Length;
		}
		if (count > 0)
		{
			Array.Copy(buffer, offset, tjjwm, 0, count);
			chnzv += count;
		}
	}

	public byte[] GetHash()
	{
		tjjwm[chnzv++] = 128;
		if (chnzv > 56)
		{
			Array.Clear(tjjwm, chnzv, tjjwm.Length - chnzv);
			pypmh(tjjwm, 0);
			Array.Clear(tjjwm, 0, 56);
		}
		else if (chnzv < 56)
		{
			Array.Clear(tjjwm, chnzv, 56 - chnzv);
		}
		fjllu(tjjwm, 56, (uint)wtsuv);
		fjllu(tjjwm, 60, (uint)(wtsuv >> 32));
		pypmh(tjjwm, 0);
		byte[] array = new byte[16];
		fjllu(array, 0, tfugy);
		fjllu(array, 4, xpwzo);
		fjllu(array, 8, jvpsp);
		fjllu(array, 12, mvysi);
		return array;
	}

	protected static void fjllu(byte[] p0, int p1, uint p2)
	{
		p0[p1] = (byte)p2;
		p0[p1 + 1] = (byte)(p2 >> 8);
		p0[p1 + 2] = (byte)(p2 >> 16);
		p0[p1 + 3] = (byte)(p2 >> 24);
	}

	protected static uint bbjej(byte[] p0, int p1)
	{
		if (BitConverter.IsLittleEndian && 0 == 0)
		{
			return BitConverter.ToUInt32(p0, p1);
		}
		return (uint)(p0[p1] | (p0[p1 + 1] << 8) | (p0[p1 + 2] << 16) | (p0[p1 + 3] << 24));
	}

	public static uint prlgj(uint p0, int p1)
	{
		return (p0 << p1) | (p0 >> 32 - p1);
	}

	public void Dispose()
	{
		Reset();
	}
}
