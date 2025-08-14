using System;
using System.Diagnostics;
using System.IO;

namespace onrkn;

internal class euduf : xaxit
{
	private const int bsybx = -1;

	private readonly Stream nlpwc;

	private readonly bool hohjo;

	private long pvsts;

	private long hwwpa;

	public override bool CanRead => nlpwc.CanRead;

	public override bool CanSeek => nlpwc.CanSeek;

	public override bool CanWrite => nlpwc.CanWrite;

	public override long Length => nlpwc.Length;

	public override long Position
	{
		get
		{
			return pvsts;
		}
		set
		{
			nlpwc.Position = value;
			pvsts = nlpwc.Position;
		}
	}

	internal Stream plsjz => nlpwc;

	public euduf(Stream innerStream, bool ownInnerStream)
	{
		if (innerStream == null || 1 == 0)
		{
			throw new ArgumentNullException("innerStream");
		}
		nlpwc = innerStream;
		pvsts = ((nlpwc.CanSeek ? true : false) ? nlpwc.Position : 0);
		hwwpa = -1L;
		hohjo = ownInnerStream;
	}

	public override int ReadByte()
	{
		if (!qauuk(pvsts) || 1 == 0)
		{
			return -1;
		}
		int num = nlpwc.ReadByte();
		if (num < 0)
		{
			hwwpa = pvsts;
		}
		else
		{
			pvsts++;
		}
		return num;
	}

	public override void Flush()
	{
		nlpwc.Flush();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		if (!CanSeek || 1 == 0)
		{
			if (((origin != SeekOrigin.Begin) ? true : false) || hwwpa == -1 || offset < hwwpa)
			{
				throw new NotSupportedException();
			}
			pvsts = offset;
			return pvsts;
		}
		ocwfj<long, SeekOrigin, long> ocwfj2 = sooud(offset, origin);
		nlpwc.Seek(ocwfj2.shrdw, ocwfj2.vqhck);
		pvsts = ocwfj2.anmqs;
		return pvsts;
	}

	public override void SetLength(long value)
	{
		nlpwc.SetLength(value);
		hwwpa = value;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (!qauuk(pvsts) || 1 == 0)
		{
			return 0;
		}
		int num = nlpwc.Read(buffer, offset, count);
		if (num <= 0)
		{
			hwwpa = pvsts;
		}
		else
		{
			pvsts += num;
		}
		return num;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		nlpwc.Write(buffer, offset, count);
		pvsts += count;
		if (hwwpa > -1 && pvsts > hwwpa)
		{
			hwwpa = pvsts;
		}
	}

	protected override void julnt()
	{
		base.julnt();
		if (hohjo && 0 == 0)
		{
			((IDisposable)nlpwc).Dispose();
		}
	}

	[Conditional("DEBUG")]
	private void hytto()
	{
		if (CanSeek ? true : false)
		{
			qauuk(pvsts);
		}
	}

	private bool qauuk(long p0)
	{
		if (!CanSeek || false || p0 > Length)
		{
			if (!CanSeek || 1 == 0)
			{
				if (hwwpa != -1)
				{
					return p0 <= hwwpa;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	private ocwfj<long, SeekOrigin, long> sooud(long p0, SeekOrigin p1)
	{
		long num = awawh(p0, p1);
		if (!qauuk(num) || 1 == 0)
		{
			return ujajv.ykzss(0L, SeekOrigin.End, num);
		}
		return ujajv.ykzss(num, SeekOrigin.Begin, num);
	}

	private long awawh(long p0, SeekOrigin p1)
	{
		long num = pvsts;
		switch (p1)
		{
		case SeekOrigin.Begin:
			num = p0;
			break;
		case SeekOrigin.Current:
			num += p0;
			break;
		case SeekOrigin.End:
			num = Length + p0;
			break;
		}
		return num;
	}
}
