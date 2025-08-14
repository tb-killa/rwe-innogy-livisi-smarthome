using System;

namespace onrkn;

internal class nzvsl
{
	public readonly int fybsd;

	public readonly byte[] afnnf;

	public readonly sjhqe emwzx;

	public int ujzza;

	public nzvsl(int maximumCapacityBits, sjhqe logger)
	{
		fybsd = 1 << maximumCapacityBits;
		afnnf = new byte[fybsd];
		emwzx = logger;
	}

	public void bttag(byte p0)
	{
		if (ujzza == fybsd)
		{
			ujzza = 0;
		}
		afnnf[ujzza++] = p0;
	}

	public void pjccs(byte[] p0, int p1, int p2)
	{
		if (p2 >= fybsd)
		{
			ujzza = 0;
			Array.Copy(p0, p1 + p2 - fybsd, afnnf, 0, fybsd);
		}
		else
		{
			twzrv(p0, p1, p2);
		}
	}

	private bool twzrv(byte[] p0, int p1, int p2)
	{
		int num = fybsd - ujzza;
		if (p2 <= num)
		{
			Array.Copy(p0, p1, afnnf, ujzza, p2);
			ujzza += p2;
			return false;
		}
		Array.Copy(p0, p1, afnnf, ujzza, num);
		ujzza = p2 - num;
		Array.Copy(p0, p1 + num, afnnf, 0, ujzza);
		return true;
	}

	public void ktrjw(int p0, int p1, byte[] p2, int p3)
	{
		p0 = ujzza - p0;
		if (p0 < 0)
		{
			p0 += fybsd;
		}
		while (p1 > 0)
		{
			if (p0 >= ujzza)
			{
				int num = fybsd - p0;
				if (p1 <= num)
				{
					Array.Copy(afnnf, p0, p2, p3, p1);
					Array.Copy(p2, p3, afnnf, ujzza, p1);
					ujzza += p1;
					break;
				}
				Array.Copy(afnnf, p0, p2, p3, num);
				Array.Copy(p2, p3, afnnf, ujzza, num);
				ujzza += num;
				p3 += num;
				p1 -= num;
				if (p1 <= ujzza)
				{
					Array.Copy(afnnf, 0, p2, p3, p1);
					twzrv(p2, p3, p1);
					break;
				}
				int num2 = ujzza;
				Array.Copy(afnnf, 0, p2, p3, num2);
				p0 = ((twzrv(p2, p3, num2) && 0 == 0) ? num2 : 0);
				p3 += num2;
				p1 -= num2;
			}
			else
			{
				int num3 = ujzza - p0;
				if (p1 <= num3)
				{
					Array.Copy(afnnf, p0, p2, p3, p1);
					twzrv(p2, p3, p1);
					break;
				}
				Array.Copy(afnnf, p0, p2, p3, num3);
				if (twzrv(p2, p3, num3) && 0 == 0)
				{
					p0 += num3;
				}
				p3 += num3;
				p1 -= num3;
			}
		}
	}

	public void lvado(int p0, int p1)
	{
		int num = ujzza - p0;
		if (p0 < 3 || p1 < 3)
		{
			if (p1 == 1)
			{
				if (num >= 0)
				{
					afnnf[ujzza++] = afnnf[num];
				}
				else
				{
					afnnf[ujzza++] = afnnf[num + fybsd];
				}
				return;
			}
			if (num >= 0)
			{
				afnnf[ujzza++] = afnnf[num];
				afnnf[ujzza++] = afnnf[num + 1];
			}
			else
			{
				afnnf[ujzza++] = afnnf[num + fybsd];
				if (num == -1)
				{
					afnnf[ujzza++] = afnnf[0];
				}
				else
				{
					afnnf[ujzza++] = afnnf[num + 1 + fybsd];
				}
			}
			if (p1 == 2)
			{
				return;
			}
			p0 += 2;
			p1 -= 2;
		}
		while (true)
		{
			if (num >= 0)
			{
				if (p1 <= p0)
				{
					Array.Copy(afnnf, num, afnnf, ujzza, p1);
					ujzza += p1;
					return;
				}
				Array.Copy(afnnf, num, afnnf, ujzza, p0);
				ujzza += p0;
				p1 -= p0;
				p0 <<= 1;
			}
			else
			{
				if (num + p1 <= 0)
				{
					break;
				}
				Array.Copy(afnnf, num + fybsd, afnnf, ujzza, -num);
				ujzza -= num;
				p1 += num;
				num = 0;
			}
		}
		Array.Copy(afnnf, num + fybsd, afnnf, ujzza, p1);
		ujzza += p1;
	}

	public void dmrvp(byte[] p0, int p1, int p2)
	{
		if ((p2 != 0) ? true : false)
		{
			int num = ujzza - p2;
			if (num >= 0)
			{
				Array.Copy(afnnf, num, p0, p1, p2);
				return;
			}
			Array.Copy(afnnf, fybsd + num, p0, p1, -num);
			Array.Copy(afnnf, 0, p0, p1 - num, ujzza);
		}
	}
}
