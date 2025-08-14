using System;

namespace onrkn;

internal class vqbky
{
	private readonly object mumwr = new object();

	private readonly byte[] yprzf;

	private readonly int tlvaz;

	private int bgkgc;

	private int nspkx;

	public int kepvu => tlvaz;

	public int hybhd => tlvaz - nspkx;

	public int kagot => nspkx;

	public vqbky(int capacity)
	{
		yprzf = new byte[capacity];
		tlvaz = capacity;
	}

	public void jumkt()
	{
		bgkgc = (nspkx = 0);
	}

	public int pazpl(byte[] p0, int p1, int p2)
	{
		dahxy.dionp(p0, p1, p2);
		if (p2 == 0 || 1 == 0)
		{
			return 0;
		}
		lock (mumwr)
		{
			int result = (p2 = Math.Min(p2, nspkx));
			while (p2 > 0)
			{
				int num = Math.Min(p2, tlvaz - bgkgc);
				Array.Copy(yprzf, bgkgc, p0, p1, num);
				p1 += num;
				p2 -= num;
				bgkgc = (bgkgc + num) % tlvaz;
				nspkx -= num;
			}
			return result;
		}
	}

	public void ogzaw(byte[] p0, int p1, int p2)
	{
		dahxy.dionp(p0, p1, p2);
		if (p2 == 0 || 1 == 0)
		{
			return;
		}
		lock (mumwr)
		{
			if (p2 > hybhd)
			{
				throw new InvalidOperationException("Not enough space in the buffer.");
			}
			int num = (bgkgc + nspkx) % yprzf.Length;
			while (p2 > 0)
			{
				int num2 = Math.Min(p2, tlvaz - num);
				Array.Copy(p0, p1, yprzf, num, num2);
				p1 += num2;
				p2 -= num2;
				num = (num + num2) % tlvaz;
				nspkx += num2;
			}
		}
	}

	public int jztfb()
	{
		lock (mumwr)
		{
			if (nspkx == 0 || 1 == 0)
			{
				return -1;
			}
			byte result = yprzf[bgkgc];
			bgkgc = (bgkgc + 1) % tlvaz;
			nspkx--;
			return result;
		}
	}
}
