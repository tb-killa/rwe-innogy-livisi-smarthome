using System;

namespace onrkn;

internal static class jtedy
{
	public static void yvsky<T>(ref T[] p0, int p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("array");
		}
		if (p1 <= 0)
		{
			throw new ArgumentOutOfRangeException("newSize");
		}
		if (p0.Length != p1)
		{
			T[] array = new T[p1];
			int length = Math.Min(p0.Length, p1);
			Array.Copy(p0, 0, array, 0, length);
			p0 = array;
		}
	}

	public static nxtme<T> puqqk<T>(this T[] p0, int p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("array");
		}
		return new nxtme<T>(p0, p1);
	}

	public static nxtme<T> liutv<T>(this T[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("array");
		}
		return new nxtme<T>(p0);
	}

	public static nxtme<T> plhfl<T>(this T[] p0, int p1, int p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("array");
		}
		return new nxtme<T>(p0, p1, p2);
	}

	public static nxtme<T> xtagy<T>(this T[] p0)
	{
		return p0.liutv();
	}

	public static nxtme<T> lafmy<T>(this T[] p0, int p1)
	{
		return p0.puqqk(p1);
	}

	public static nxtme<T> myshu<T>(this T[] p0, int p1, int p2)
	{
		return p0.plhfl(p1, p2);
	}

	public static nxtme<T> pagxw<T>(this T[] p0)
	{
		return p0.liutv();
	}

	public static nxtme<T> rotod<T>(this T[] p0, int p1)
	{
		return p0.puqqk(p1);
	}

	public static nxtme<T> pynmq<T>(this T[] p0, int p1, int p2)
	{
		return p0.plhfl(p1, p2);
	}

	public static int babpw<T>(this T[] p0, params T[] p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("array");
		}
		return p0.pbueu(0, p0.Length, p1);
	}

	public static int pbueu<T>(this T[] p0, int p1, int p2, params T[] p3)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("array");
		}
		if (p3 == null || 1 == 0)
		{
			throw new ArgumentNullException("values");
		}
		if (p3.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("At least one argument is required.", "values");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0055;
		}
		goto IL_0076;
		IL_0055:
		T value = p3[num];
		int num2 = Array.IndexOf(p0, value, p1, p2);
		if (num2 >= 0)
		{
			return num2;
		}
		num++;
		goto IL_0076;
		IL_0076:
		if (num < p3.Length)
		{
			goto IL_0055;
		}
		return -1;
	}
}
