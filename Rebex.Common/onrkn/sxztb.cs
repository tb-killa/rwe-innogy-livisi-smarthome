using System;

namespace onrkn;

internal class sxztb<T0>
{
	private class webpn : rhzda<T0[]>
	{
		public readonly int whntk;

		public webpn(int arrayLength, int bucketSize)
			: base(bucketSize)
		{
			whntk = arrayLength;
		}

		protected override T0[] qbuja()
		{
			return new T0[whntk];
		}
	}

	public const int lxtca = 1048576;

	public const int rmkfp = 50;

	private const int kdjck = 4;

	private const int jsdkk = 16;

	private static readonly rwfyw<T0[]> jeztc;

	private readonly webpn[] pjyum;

	private static readonly rwfyw<sxztb<T0>> wlrkm;

	private static Func<T0[]> klhwj;

	private static Func<sxztb<T0>> ntllv;

	public static sxztb<T0> ahblv => wlrkm.avlfd;

	public sxztb()
		: this(1048576, 50)
	{
	}

	public sxztb(int maxLength, int bucketSize)
	{
		if (bucketSize <= 0)
		{
			throw new ArgumentOutOfRangeException("bucketSize", "Bucket size must be a positive value.");
		}
		if (maxLength < 0)
		{
			throw new ArgumentOutOfRangeException("maxLength", "Maximum length must be a non-negative value.");
		}
		if (maxLength < 16)
		{
			maxLength = 16;
		}
		int num = ocuft(maxLength) + 1;
		pjyum = new webpn[num];
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0050;
		}
		goto IL_0073;
		IL_0050:
		int arrayLength = zmyvj(num2);
		pjyum[num2] = new webpn(arrayLength, bucketSize);
		num2++;
		goto IL_0073;
		IL_0073:
		if (num2 >= num)
		{
			return;
		}
		goto IL_0050;
	}

	public static int zmyvj(int p0)
	{
		int num = 4 + p0;
		if (num == 31)
		{
			return int.MaxValue;
		}
		if (num > 31)
		{
			throw new ArgumentOutOfRangeException();
		}
		return 1 << 4 + p0;
	}

	public static int ocuft(int p0)
	{
		if (p0 == 0 || 1 == 0)
		{
			return 0;
		}
		return Math.Max(0, gqjpe(p0) - 4);
	}

	public virtual T0[] vfhlp(int p0)
	{
		if (p0 < 0)
		{
			throw new ArgumentException("Minimal size must be greater or equal to zero.", "minSize");
		}
		if (p0 == 0 || 1 == 0)
		{
			return jeztc.avlfd;
		}
		int num = ocuft(p0);
		if (num >= pjyum.Length)
		{
			return new T0[p0];
		}
		if ((pjyum[num].myplq(out var p1) ? true : false) || (num + 1 < pjyum.Length && pjyum[num + 1].myplq(out p1)))
		{
			return p1;
		}
		return pjyum[num].tbdcs();
	}

	public ihlqx<T0[]> uzkcf(int p0)
	{
		T0[] value = vfhlp(p0);
		return new ihlqx<T0[]>(value, uqydw);
	}

	public virtual void uqydw(T0[] p0)
	{
		if (p0.Length == 0 || 1 == 0)
		{
			return;
		}
		int num = ocuft(p0.Length);
		if (num < pjyum.Length)
		{
			webpn webpn = pjyum[num];
			if (webpn.whntk == p0.Length)
			{
				webpn.wkkog(p0);
			}
		}
	}

	public static sxztb<T0> adgoe(int p0, int p1)
	{
		return new sxztb<T0>(p0, p1);
	}

	private static int gqjpe(int p0)
	{
		uint num = (uint)(p0 - 1);
		int num2 = 0;
		if (num >= 65536)
		{
			num >>= 16;
			num2 = 16;
		}
		if (num >= 256)
		{
			num >>= 8;
			num2 += 8;
		}
		if (num >= 16)
		{
			num >>= 4;
			num2 += 4;
		}
		if (num >= 4)
		{
			num >>= 2;
			num2 += 2;
		}
		if (num >= 2)
		{
			num >>= 1;
			num2++;
		}
		return num2 + (int)num;
	}

	static sxztb()
	{
		if (klhwj == null || 1 == 0)
		{
			klhwj = pfgxm;
		}
		jeztc = new rwfyw<T0[]>(klhwj);
		if (ntllv == null || 1 == 0)
		{
			ntllv = wpxuk;
		}
		wlrkm = new rwfyw<sxztb<T0>>(ntllv);
	}

	private static T0[] pfgxm()
	{
		return new T0[0];
	}

	private static sxztb<T0> wpxuk()
	{
		return new sxztb<T0>();
	}
}
