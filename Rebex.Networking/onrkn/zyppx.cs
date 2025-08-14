using System;
using System.Text;

namespace onrkn;

internal class zyppx
{
	public const string ykxuf = "Not enough data.";

	public const string qwipr = "Data too long.";

	public const string hmeob = "Invalid data.";

	private readonly Encoding bspog;

	private readonly byte[] lrled;

	private int xawjm;

	private int tgsxs;

	public int lkjla => xawjm;

	public int lheoy => tgsxs;

	public byte[] dysab()
	{
		return lrled;
	}

	public zyppx(byte[] buffer, int offset, int count, Encoding encoding)
	{
		lrled = buffer;
		xawjm = offset;
		tgsxs = count;
		bspog = encoding;
	}

	public byte sfolp()
	{
		if (tgsxs == 0 || 1 == 0)
		{
			throw new InvalidOperationException("Not enough data.");
		}
		byte result = lrled[xawjm];
		xawjm++;
		tgsxs--;
		return result;
	}

	public bool qxurr()
	{
		byte b = sfolp();
		return b != 0;
	}

	public int rvfya()
	{
		return (int)fiswn();
	}

	public uint fiswn()
	{
		if (tgsxs < 4)
		{
			throw new InvalidOperationException("Not enough data.");
		}
		uint result = (uint)(lrled[xawjm] * 16777216 + lrled[xawjm + 1] * 65536 + lrled[xawjm + 2] * 256 + lrled[xawjm + 3]);
		xawjm += 4;
		tgsxs -= 4;
		return result;
	}

	public long skgcn()
	{
		return (long)rgaur();
	}

	public ulong rgaur()
	{
		uint num = fiswn();
		uint num2 = fiswn();
		return ((ulong)num << 32) + num2;
	}

	public byte[] jvszb(int p0)
	{
		if (tgsxs < p0)
		{
			throw new InvalidOperationException("Not enough data.");
		}
		byte[] array = new byte[p0];
		Array.Copy(lrled, xawjm, array, 0, p0);
		xawjm += p0;
		tgsxs -= p0;
		return array;
	}

	public byte[] rypuc(bool p0)
	{
		uint num = fiswn();
		if (num > 65535)
		{
			throw new InvalidOperationException("Data too long.");
		}
		if (tgsxs < num)
		{
			throw new InvalidOperationException("Not enough data.");
		}
		if (num == 0 || 1 == 0)
		{
			return new byte[0];
		}
		if (!p0 || 1 == 0)
		{
			return jvszb((int)num);
		}
		if (lrled[xawjm] >= 128)
		{
			throw new InvalidOperationException("Invalid data.");
		}
		if (lrled[xawjm] == 0 || 1 == 0)
		{
			num--;
			xawjm++;
			tgsxs--;
		}
		return jvszb((int)num);
	}

	public byte[] tebzf()
	{
		uint num = fiswn();
		if (num > 65536)
		{
			throw new InvalidOperationException("Data too long.");
		}
		if (tgsxs < num)
		{
			throw new InvalidOperationException("Not enough data.");
		}
		return jvszb((int)num);
	}

	public string mdsgo()
	{
		byte[] array = tebzf();
		string text = bspog.GetString(array, 0, array.Length);
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (num != 0)
		{
			goto IL_0024;
		}
		goto IL_0064;
		IL_0024:
		char c = text[num];
		if (c < ' ')
		{
			switch (c)
			{
			case '\t':
			case '\n':
			case '\r':
				break;
			default:
				goto IL_0060;
			}
		}
		stringBuilder.Append(c);
		goto IL_0060;
		IL_0060:
		num++;
		goto IL_0064;
		IL_0064:
		if (num < text.Length)
		{
			goto IL_0024;
		}
		return stringBuilder.ToString();
	}

	public string[] dxxld()
	{
		string text = mdsgo();
		if (text.Length == 0 || 1 == 0)
		{
			return new string[0];
		}
		return text.TrimEnd(',').Split(',');
	}

	public byte[] tblgh()
	{
		return jvszb(tgsxs);
	}
}
