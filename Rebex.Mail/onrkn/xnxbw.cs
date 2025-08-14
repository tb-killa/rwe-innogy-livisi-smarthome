using System;
using System.Collections.Generic;
using System.IO;

namespace onrkn;

internal class xnxbw : xxolr
{
	private int ipoop;

	private long wlrng;

	private long fjhhi;

	private List<int> mjutf;

	private int xjbzp;

	internal override int zqmsy
	{
		get
		{
			if (mjutf.Count != 0 && 0 == 0)
			{
				return mjutf[0];
			}
			return -2;
		}
	}

	public override bool CanRead => false;

	public override bool CanSeek => true;

	public override bool CanWrite => true;

	public override long Length => fjhhi;

	public override long Position
	{
		get
		{
			return wlrng;
		}
		set
		{
			if (value < 0)
			{
				throw hifyx.nztrs("value", value, "Argument is out of range of valid values.");
			}
			if (value == fjhhi)
			{
				SetLength(value + 1);
			}
			else if (value > fjhhi)
			{
				SetLength(value);
			}
			if (fjhhi != 0)
			{
				xjbzp = (int)(value / base.cqgnw);
				ipoop = (int)(value & (base.cqgnw - 1));
				wlrng = value;
				base.mlerv.Seek(wftjv(mjutf[xjbzp]) + ipoop, SeekOrigin.Begin);
			}
		}
	}

	internal void cgrwp()
	{
		if (mjutf.Count == 0 || 1 == 0)
		{
			mjutf.Add(base.lexyq.Count);
			hejrp(-2);
			return;
		}
		int index = mjutf[mjutf.Count - 1];
		mjutf.Add(base.lexyq.Count);
		base.lexyq[index] = base.lexyq.Count;
		hejrp(-2);
	}

	internal xnxbw(duqmg owner, bool isShortStream)
		: base(owner, isShortStream)
	{
		ipoop = base.cqgnw;
		mjutf = new List<int>();
		xjbzp = -1;
	}

	public override void Flush()
	{
		base.mlerv.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new InvalidOperationException("Stream is not readable.");
	}

	public override void SetLength(long value)
	{
		if (value < fjhhi)
		{
			throw new NotSupportedException("Setting length to a smaller value than actual value is unsupported.");
		}
		if (value != fjhhi)
		{
			int num = (int)(value / base.cqgnw);
			for (int i = mjutf.Count; i <= num; i++)
			{
				cgrwp();
			}
		}
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (buffer == null || 1 == 0)
		{
			throw new ArgumentNullException("buffer", "Buffer cannot be null.");
		}
		if (offset < 0 || offset > buffer.Length)
		{
			throw hifyx.nztrs("offset", offset, "Offset is out of range of valid values.");
		}
		if (count < 0 || offset + count > buffer.Length)
		{
			throw hifyx.nztrs("count", count, "Count is out of range of valid values.");
		}
		while (count > 0)
		{
			if (ipoop == base.cqgnw)
			{
				ipoop = 0;
				xjbzp++;
				if (xjbzp == mjutf.Count)
				{
					cgrwp();
				}
				long num = wftjv(mjutf[xjbzp]);
				if (num + count > int.MaxValue)
				{
					throw new uwkib("The total file length limit of 2GB has been exceeded.");
				}
				base.mlerv.Seek(num, SeekOrigin.Begin);
			}
			int num2 = Math.Min(count, base.cqgnw - ipoop);
			base.mlerv.Write(buffer, offset, num2);
			offset += num2;
			count -= num2;
			ipoop += num2;
			wlrng += num2;
			if (wlrng > fjhhi)
			{
				fjhhi = wlrng;
			}
		}
	}
}
