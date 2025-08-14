using System;
using System.IO;

namespace onrkn;

internal class mtlib
{
	private int mxlun;

	private readonly byte[] mqgdr;

	private readonly Stream kotnf;

	private bool tilmx;

	private bool krpap;

	private int ktflw;

	private bool twnfg;

	private int nclxu;

	public bool kqoib
	{
		get
		{
			if (mxlun < mqgdr.Length)
			{
				return false;
			}
			return tilmx;
		}
	}

	public bool zpiat
	{
		get
		{
			if (kqoib && 0 == 0)
			{
				return nclxu != 3338;
			}
			return false;
		}
	}

	public mtlib(Stream inner, bool stuffing)
	{
		kotnf = inner;
		mqgdr = new byte[4096];
		mxlun = mqgdr.Length;
		ktflw = -1;
		twnfg = stuffing;
	}

	public int lfdxc(byte[] p0, int p1, int p2)
	{
		int i = 0;
		try
		{
			int num;
			for (; p2 > 0; i += num, p1 += num, p2 -= num)
			{
				num = 0;
				if (mxlun < mqgdr.Length)
				{
					num = Math.Min(mqgdr.Length - mxlun, p2);
					Array.Copy(mqgdr, mxlun, p0, p1, num);
					mxlun += num;
				}
				if (num < p2 && ktflw >= 0)
				{
					p0[p1 + num] = (byte)ktflw;
					ktflw = -1;
					num++;
				}
				if (num < p2)
				{
					int count = Math.Min(p2 - num, mxlun);
					count = kotnf.Read(p0, p1 + num, count);
					if ((num == 0 || 1 == 0) && (count == 0 || 1 == 0))
					{
						tilmx = true;
						return i;
					}
					num += count;
				}
				if (!twnfg)
				{
					continue;
				}
				int num2 = 0;
				if (num2 != 0)
				{
					goto IL_00e1;
				}
				goto IL_015b;
				IL_00e1:
				switch (p0[p1 + num2])
				{
				case 46:
					if (krpap && 0 == 0)
					{
						byte b = p0[p1 + num - 1];
						Array.Copy(p0, p1 + num2, p0, p1 + num2 + 1, num - num2 - 1);
						mxlun--;
						mqgdr[mxlun] = b;
					}
					krpap = false;
					break;
				case 10:
					krpap = true;
					break;
				default:
					krpap = false;
					break;
				}
				num2++;
				goto IL_015b;
				IL_015b:
				if (num2 >= num)
				{
					continue;
				}
				goto IL_00e1;
			}
			ktflw = kotnf.ReadByte();
			if (ktflw < 0)
			{
				tilmx = true;
			}
			return i;
		}
		finally
		{
			if (i > 1)
			{
				nclxu = ((nclxu & 0xFF) << 8) + p0[p1 - 2];
			}
			nclxu = ((nclxu & 0xFF) << 8) + p0[p1 - 1];
		}
	}
}
