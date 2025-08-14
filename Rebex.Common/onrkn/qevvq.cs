using System;
using System.IO;

namespace onrkn;

internal static class qevvq
{
	private class ndhtp : wqywh<byte>
	{
		private readonly Stream ficuq;

		private readonly int kmmuk;

		public ndhtp(Stream input, int segmentSize)
		{
			ficuq = input;
			kmmuk = segmentSize;
		}

		public void vmdbf(Action<ArraySegment<byte>> p0)
		{
			byte[] array = new byte[kmmuk];
			bool flag = false;
			do
			{
				int num = 0;
				int num2 = kmmuk;
				int num3 = 0;
				if (num3 != 0)
				{
					goto IL_0022;
				}
				goto IL_0054;
				IL_0054:
				if (num2 > 0)
				{
					goto IL_0022;
				}
				goto IL_0058;
				IL_0022:
				int num4 = ficuq.Read(array, num, num2);
				if (num4 == 0 || 1 == 0)
				{
					flag = true;
					if (flag)
					{
						goto IL_0058;
					}
				}
				num += num4;
				num2 -= num4;
				num3 += num4;
				goto IL_0054;
				IL_0058:
				if (num3 > 0)
				{
					p0(new ArraySegment<byte>(array, 0, num3));
				}
			}
			while (!flag);
		}
	}

	public const string pvqgs = "Stream is not readable.";

	public const string fcygp = "Stream is not writable.";

	public const int axdjs = 65536;

	public static void alskc(this Stream p0, Stream p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new NullReferenceException("source");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("destination");
		}
		if (!p0.CanRead || 1 == 0)
		{
			throw new NotSupportedException("Stream is not readable.");
		}
		if (!p1.CanWrite || 1 == 0)
		{
			throw new NotSupportedException("Stream is not writable.");
		}
		byte[] array = new byte[65536];
		int num = 0;
		if (num != 0)
		{
			goto IL_0075;
		}
		goto IL_007e;
		IL_0075:
		p1.Write(array, 0, num);
		goto IL_007e;
		IL_007e:
		if ((num = p0.Read(array, 0, array.Length)) == 0 || 1 == 0)
		{
			return;
		}
		goto IL_0075;
	}

	public static void xnred(this Stream p0, byte[] p1)
	{
		p0.Write(p1, 0, p1.Length);
	}

	public static wqywh<byte> tloyl(this Stream p0, int p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new NullReferenceException("stream");
		}
		if (!p0.CanRead || 1 == 0)
		{
			throw new NotSupportedException("Stream is not readable.");
		}
		object obj = p0 as wqywh<byte>;
		if (obj == null || 1 == 0)
		{
			obj = new ndhtp(p0, p1);
		}
		return (wqywh<byte>)obj;
	}
}
