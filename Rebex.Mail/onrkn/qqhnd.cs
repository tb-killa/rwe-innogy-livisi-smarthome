using System;
using System.IO;

namespace onrkn;

internal class qqhnd
{
	private static readonly byte[] buvld = new byte[8] { 208, 207, 17, 224, 161, 177, 26, 225 };

	private static readonly byte[] bmpqr = new byte[4] { 120, 159, 62, 34 };

	public static MemoryStream zmjzm(byte[] p0, out bool p1)
	{
		p1 = false;
		int num = wsqrs(p0, out var p2, buvld, bmpqr);
		if (p2 < 0)
		{
			return null;
		}
		switch (p2)
		{
		case 0:
		{
			MemoryStream memoryStream = new MemoryStream(p0, 0, p0.Length, writable: false, publiclyVisible: false);
			memoryStream.Position = num;
			zypui zypui2 = new zypui(memoryStream, leaveOpen: false);
			try
			{
				if (zypui2.vmjtu("CONTENTS") == null || 1 == 0)
				{
					throw new uwkib("Compound file doesn't contain the 'CONTENTS' stream.");
				}
				Stream stream = zypui2.vmjtu("CONTENTS").mzhde();
				num = 0;
				byte[] array;
				for (array = new byte[stream.Length]; num < array.Length; num += stream.Read(array, num, array.Length - num))
				{
				}
				return new MemoryStream(array, 0, array.Length, writable: false, publiclyVisible: false);
			}
			finally
			{
				if (zypui2 != null && 0 == 0)
				{
					((IDisposable)zypui2).Dispose();
				}
			}
		}
		case 1:
			p1 = true;
			return new MemoryStream(p0, num, p0.Length - num, writable: false, publiclyVisible: false);
		default:
			return null;
		}
	}

	private static int wsqrs(byte[] p0, out int p1, params byte[][] p2)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0009;
		}
		goto IL_0061;
		IL_0009:
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_000e;
		}
		goto IL_0057;
		IL_000e:
		byte[] array = p2[num2];
		bool flag;
		int num3;
		if (p0.Length - num >= array.Length)
		{
			flag = true;
			num3 = 0;
			if (num3 != 0)
			{
				goto IL_0025;
			}
			goto IL_003c;
		}
		goto IL_0053;
		IL_0057:
		if (num2 < p2.Length)
		{
			goto IL_000e;
		}
		num++;
		goto IL_0061;
		IL_0025:
		if (array[num3] != p0[num + num3])
		{
			flag = false;
			if (!flag)
			{
				goto IL_0043;
			}
		}
		num3++;
		goto IL_003c;
		IL_0061:
		if (num >= p0.Length)
		{
			p1 = -1;
			return -1;
		}
		goto IL_0009;
		IL_0053:
		num2++;
		goto IL_0057;
		IL_003c:
		if (num3 < array.Length)
		{
			goto IL_0025;
		}
		goto IL_0043;
		IL_0043:
		if (flag && 0 == 0)
		{
			p1 = num2;
			return num;
		}
		goto IL_0053;
	}
}
