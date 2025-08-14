using System.IO;

namespace onrkn;

internal class vmzeg : xlbfv
{
	private readonly Stream bqwli;

	private readonly bool smvgn;

	private bool uaeak;

	public vmzeg(Stream inner, bool ownsInner)
	{
		bqwli = inner;
		smvgn = ownsInner;
	}

	protected override void julnt()
	{
		uaeak = true;
		base.julnt();
		if (smvgn && 0 == 0)
		{
			bqwli.Close();
		}
		else
		{
			bqwli.Flush();
		}
	}

	public override void Flush()
	{
		bqwli.Flush();
	}

	protected override void iabst(byte[] p0, int p1, int p2, bool p3)
	{
		if ((p3 ? true : false) || uaeak)
		{
			for (; p2 > 0; p2--)
			{
				switch (p0[p1 + p2 - 1])
				{
				case 61:
					p2--;
					p3 = false;
					break;
				case 9:
				case 32:
					continue;
				}
				break;
			}
		}
		int num = 0;
		for (int i = p1; i < p1 + p2 - 2; i++)
		{
			if (p0[i] != 61)
			{
				continue;
			}
			int num2 = kgbvh.bvuap(p0[i + 1]);
			int num3 = kgbvh.bvuap(p0[i + 2]);
			if (num2 >= 0 && num3 >= 0)
			{
				if (i > num)
				{
					bqwli.Write(p0, num, i - num);
				}
				bqwli.WriteByte((byte)(16 * num2 + num3));
				num = i + 3;
				i += 2;
			}
		}
		bqwli.Write(p0, num, p1 + p2 - num);
		if (p3 && 0 == 0)
		{
			bqwli.WriteByte(13);
			bqwli.WriteByte(10);
		}
	}
}
