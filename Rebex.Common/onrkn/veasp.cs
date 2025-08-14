using System;
using System.IO;

namespace onrkn;

internal class veasp : Stream
{
	public enum hnzas
	{
		syqha,
		hjipd,
		ructu
	}

	private sealed class rcmrv
	{
		public long wkmpn;

		public Exception hzfnq()
		{
			return hifyx.nztrs("value", wkmpn, "Argument is out of range of valid values.");
		}
	}

	private sealed class mcmqs
	{
		public int boqxu;

		public int lerlj;

		public veasp vruyy;

		public byte[] qmvxg;

		public int qzwqy;

		public void pdibj()
		{
			lerlj = vruyy.ypfmo.Read(qmvxg, qzwqy, boqxu);
		}
	}

	private sealed class cpmep
	{
		public veasp apzfw;

		public byte[] stqig;

		public int qvgiv;

		public int zrxmj;

		public void ymaxn()
		{
			apzfw.ypfmo.Write(stqig, qvgiv, zrxmj);
		}
	}

	private const int uexek = 1024;

	private const int uiqsk = -1;

	private long tzkle;

	private opjbe aumck;

	private long nzpan;

	private long lmthq;

	private long piafi;

	private hnzas porxs;

	private static Func<Exception> glcmq;

	public opjbe ypfmo
	{
		get
		{
			return aumck;
		}
		private set
		{
			aumck = value;
		}
	}

	public long lvwjj
	{
		get
		{
			return nzpan;
		}
		private set
		{
			nzpan = value;
		}
	}

	public long brshv
	{
		get
		{
			return lmthq;
		}
		private set
		{
			lmthq = value;
		}
	}

	public long kjnyb
	{
		get
		{
			return piafi;
		}
		private set
		{
			piafi = value;
		}
	}

	public hnzas ritsx
	{
		get
		{
			return porxs;
		}
		set
		{
			porxs = value;
		}
	}

	public override bool CanRead => ypfmo.CanRead;

	public override bool CanSeek => ypfmo.CanSeek;

	public override bool CanWrite => ypfmo.CanWrite;

	public override long Length => kjnyb;

	public override long Position
	{
		get
		{
			return tzkle;
		}
		set
		{
			rcmrv rcmrv = new rcmrv();
			rcmrv.wkmpn = value;
			jdgun(rcmrv.wkmpn, rcmrv.hzfnq);
			if (rcmrv.wkmpn < 0)
			{
				tzkle = 0L;
			}
			else if (rcmrv.wkmpn > kjnyb)
			{
				tzkle = kjnyb;
			}
			else
			{
				tzkle = rcmrv.wkmpn;
			}
		}
	}

	public veasp(opjbe chunkedMemoryStream, long startIndex, long count, hnzas type)
	{
		if (chunkedMemoryStream == null || 1 == 0)
		{
			throw new ArgumentNullException("chunkedMemoryStream");
		}
		if (startIndex < 0 || startIndex > chunkedMemoryStream.Length - 1)
		{
			throw new ArgumentOutOfRangeException("startIndex");
		}
		lvwjj = count - 1;
		if (lvwjj >= chunkedMemoryStream.Length)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (type == hnzas.syqha || 1 == 0)
		{
			throw new ArgumentException("type");
		}
		brshv = startIndex;
		kjnyb = count;
		ritsx = type;
		ypfmo = chunkedMemoryStream;
		Position = 0L;
	}

	public override void Flush()
	{
		ypfmo.Flush();
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
		}
		return Position;
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		mcmqs mcmqs = new mcmqs();
		mcmqs.qmvxg = buffer;
		mcmqs.qzwqy = offset;
		mcmqs.vruyy = this;
		long num = kjnyb - Position;
		mcmqs.boqxu = ((num > int.MaxValue) ? count : Math.Min(Convert.ToInt32(num), count));
		mcmqs.lerlj = 0;
		axndw(mcmqs.pdibj);
		Position += mcmqs.lerlj;
		return mcmqs.lerlj;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		cpmep cpmep = new cpmep();
		cpmep.stqig = buffer;
		cpmep.qvgiv = offset;
		cpmep.zrxmj = count;
		cpmep.apzfw = this;
		long num = Position + cpmep.zrxmj;
		if (glcmq == null || 1 == 0)
		{
			glcmq = tlued;
		}
		jdgun(num, glcmq);
		axndw(cpmep.ymaxn);
		Position = num;
	}

	public virtual byte[] zfpbp()
	{
		byte[] array = new byte[Length];
		long num = Length;
		int num2 = 0;
		Position = 0L;
		int num3 = -1;
		if (num3 == 0)
		{
			goto IL_0024;
		}
		goto IL_005a;
		IL_0024:
		int count = ((num > int.MaxValue) ? 1024 : Math.Min(Convert.ToInt32(num), 1024));
		num3 = Read(array, num2, count);
		num -= num3;
		num2 += num3;
		goto IL_005a;
		IL_005a:
		if (num <= 0 || num3 == 0 || 1 == 0)
		{
			return array;
		}
		goto IL_0024;
	}

	public virtual void vfxkn(Stream p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("");
		}
		byte[] buffer = new byte[1024];
		long num = Length;
		Position = 0L;
		int num2 = -1;
		if (num2 == 0)
		{
			goto IL_0038;
		}
		goto IL_0071;
		IL_0038:
		int count = ((num > int.MaxValue) ? 1024 : Math.Min(Convert.ToInt32(num), 1024));
		num2 = Read(buffer, 0, count);
		num -= num2;
		p0.Write(buffer, 0, num2);
		goto IL_0071;
		IL_0071:
		if (num <= 0 || num2 == 0 || 1 == 0)
		{
			return;
		}
		goto IL_0038;
	}

	public virtual byte rifgn(long p0)
	{
		if (p0 < 0 || p0 > lvwjj)
		{
			throw new ArgumentOutOfRangeException("indexToRead");
		}
		long p1 = brshv + p0;
		return ypfmo.oidsn(p1);
	}

	private void axndw(Action p0)
	{
		long position = ypfmo.Position;
		try
		{
			ypfmo.Position = jmqmr();
			p0();
		}
		finally
		{
			ypfmo.Position = position;
		}
	}

	private long jmqmr()
	{
		return brshv + Position;
	}

	private void jdgun(long p0, Func<Exception> p1)
	{
		if (ritsx == hnzas.hjipd || (p0 >= 0 && p0 <= kjnyb))
		{
			return;
		}
		throw p1();
	}

	private static Exception tlued()
	{
		return new InvalidOperationException("Cannot write outside bounds of the view.");
	}
}
