using System;
using System.IO;
using System.Text;

namespace onrkn;

internal abstract class rnsvi : Stream
{
	internal static readonly byte[] khisd = new byte[64]
	{
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255
	};

	private byte[] jcrvd;

	public rnsvi()
	{
		jcrvd = new byte[4];
	}

	public override void Flush()
	{
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		switch (origin)
		{
		case SeekOrigin.Begin:
			Position = offset;
			break;
		case SeekOrigin.Current:
			Position += offset;
			break;
		case SeekOrigin.End:
			Position = Length + offset;
			break;
		default:
			throw hifyx.nztrs("origin", origin, "Argument is out of range of valid values.");
		}
		return Position;
	}

	internal void hvees(byte[] p0)
	{
		hxmiq(p0, 0, p0.Length);
	}

	internal void hxmiq(byte[] p0, int p1, int p2)
	{
		if (!mdqyq(p0, p1, p2) || 1 == 0)
		{
			throw new uwkib("Not enough data in stream.");
		}
	}

	internal bool mdqyq(byte[] p0, int p1, int p2)
	{
		while (p2 > 0)
		{
			int num = Read(p0, p1, p2);
			if (num <= 0)
			{
				return false;
			}
			p1 += num;
			p2 -= num;
		}
		return true;
	}

	internal bool gkpcu(byte[] p0, int p1, int p2, out int p3)
	{
		p3 = 0;
		while (p2 > 0)
		{
			int num = Read(p0, p1, p2);
			if (num <= 0)
			{
				return false;
			}
			p3 += num;
			p1 += num;
			p2 -= num;
		}
		return true;
	}

	internal byte[] uxseg()
	{
		byte[] array = new byte[(int)(Length - Position)];
		if (!mdqyq(array, 0, array.Length) || 1 == 0)
		{
			throw new InvalidOperationException("Unable to read the whole stream.");
		}
		return array;
	}

	internal ushort gskva()
	{
		hxmiq(jcrvd, 0, 2);
		return (ushort)(jcrvd[0] | (jcrvd[1] << 8));
	}

	internal uint pfeoc()
	{
		hxmiq(jcrvd, 0, 4);
		return (uint)(jcrvd[0] | (jcrvd[1] << 8) | (jcrvd[2] << 16) | (jcrvd[3] << 24));
	}

	internal ulong hrtyl()
	{
		return (ulong)(pfeoc() | ((long)wfljb() << 32));
	}

	internal short ecwzy()
	{
		hxmiq(jcrvd, 0, 2);
		return (short)(jcrvd[0] | (jcrvd[1] << 8));
	}

	internal int wfljb()
	{
		hxmiq(jcrvd, 0, 4);
		return jcrvd[0] | (jcrvd[1] << 8) | (jcrvd[2] << 16) | (jcrvd[3] << 24);
	}

	internal long sftyl()
	{
		return pfeoc() | ((long)wfljb() << 32);
	}

	internal string bccdk(int p0, Encoding p1)
	{
		if (jcrvd.Length < p0)
		{
			jcrvd = new byte[p0];
		}
		hxmiq(jcrvd, 0, p0);
		return p1.GetString(jcrvd, 0, p0);
	}

	internal void wukmn(int p0)
	{
		jcrvd[0] = (byte)p0;
		Write(jcrvd, 0, 1);
	}

	internal void ttjsz(ushort p0)
	{
		jcrvd[0] = (byte)p0;
		jcrvd[1] = (byte)(p0 >> 8);
		Write(jcrvd, 0, 2);
	}

	internal void iizfz(uint p0)
	{
		jcrvd[0] = (byte)p0;
		jcrvd[1] = (byte)(p0 >> 8);
		jcrvd[2] = (byte)(p0 >> 16);
		jcrvd[3] = (byte)(p0 >> 24);
		Write(jcrvd, 0, 4);
	}

	internal void fptyf(int p0)
	{
		jcrvd[0] = (byte)p0;
		jcrvd[1] = (byte)(p0 >> 8);
		jcrvd[2] = (byte)(p0 >> 16);
		jcrvd[3] = (byte)(p0 >> 24);
		Write(jcrvd, 0, 4);
	}

	internal void guutg(long p0)
	{
		fptyf((int)p0);
		fptyf((int)(p0 >> 32));
	}

	internal void jxrks(int p0)
	{
		if (jcrvd.Length < p0)
		{
			jcrvd = new byte[p0];
		}
		else
		{
			Array.Clear(jcrvd, 0, p0);
		}
		Write(jcrvd, 0, p0);
	}

	internal void fmgjd(int p0)
	{
		while (p0 > 0)
		{
			int num = Math.Min(p0, khisd.Length);
			Write(khisd, 0, num);
			p0 -= num;
		}
	}
}
