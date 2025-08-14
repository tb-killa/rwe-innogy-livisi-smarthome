using System;
using System.Collections.Generic;
using System.IO;

namespace onrkn;

internal class lofzx : xxolr
{
	private int qpvjw;

	private long uagqk;

	private long vcehd;

	private List<int> tygpg;

	private int uqnnt;

	internal override int zqmsy
	{
		get
		{
			if (tygpg.Count != 0 && 0 == 0)
			{
				return tygpg[0];
			}
			return -2;
		}
	}

	public override bool CanRead => true;

	public override bool CanSeek => true;

	public override bool CanWrite => false;

	public override long Length => vcehd;

	public override long Position
	{
		get
		{
			return uagqk;
		}
		set
		{
			if (value < 0 || value > vcehd)
			{
				throw hifyx.nztrs("value", value, "Argument is out of range of valid values.");
			}
			if (vcehd != 0)
			{
				uqnnt = (int)(value / base.cqgnw);
				qpvjw = (int)(value & (base.cqgnw - 1));
				uagqk = value;
				if (uqnnt >= tygpg.Count)
				{
					throw new uwkib("Invalid stream chain definition.");
				}
				int num = tygpg[uqnnt];
				if (num >= 0)
				{
					base.mlerv.Seek(wftjv(num) + qpvjw, SeekOrigin.Begin);
				}
				else if (num != -2)
				{
					throw new uwkib("Invalid stream chain definition.");
				}
			}
		}
	}

	internal lofzx(jxtqv owner, int sectorId, long length, bool isShortStream)
		: base(owner, isShortStream)
	{
		tygpg = new List<int>();
		tygpg.Add(sectorId);
		if (sectorId < 0)
		{
			if (length != 0)
			{
				throw new uwkib("Invalid stream definition (reference beyond the SAT for zero length stream).");
			}
			return;
		}
		for (sectorId = base.lexyq[sectorId]; sectorId != -2; sectorId = base.lexyq[sectorId])
		{
			if (sectorId < 0 || sectorId >= base.lexyq.Count)
			{
				throw new uwkib("Invalid stream definition (reference beyond the SAT).");
			}
			if (tygpg.Count > base.lexyq.Count)
			{
				throw new uwkib("Invalid stream definition (infinite loop detected).");
			}
			tygpg.Add(sectorId);
		}
		tygpg.Add(sectorId);
		if (length < 0)
		{
			vcehd = tygpg.Count * base.cqgnw;
		}
		else
		{
			vcehd = length;
		}
		base.mlerv.Seek(wftjv(tygpg[0]), SeekOrigin.Begin);
	}

	public override void Flush()
	{
	}

	public override int Read(byte[] buffer, int offset, int count)
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
		if (count == 0 || 1 == 0)
		{
			return 0;
		}
		long num = vcehd - uagqk;
		if (num <= 0)
		{
			return 0;
		}
		int count2 = ((num < count) ? Math.Min((int)num, base.cqgnw - qpvjw) : Math.Min(count, base.cqgnw - qpvjw));
		count2 = base.mlerv.Read(buffer, offset, count2);
		uagqk += count2;
		qpvjw += count2;
		if (qpvjw == base.cqgnw)
		{
			qpvjw = 0;
			uqnnt++;
			if (uqnnt == tygpg.Count)
			{
				if (uagqk != vcehd)
				{
					throw new uwkib("Invalid stream chain definition.");
				}
				return count2;
			}
			int num2 = tygpg[uqnnt];
			if (num2 >= 0)
			{
				base.mlerv.Seek(wftjv(num2), SeekOrigin.Begin);
			}
			else if (num2 != -2)
			{
				throw new uwkib("Invalid stream chain definition.");
			}
		}
		return count2;
	}

	public override void SetLength(long value)
	{
		throw new InvalidOperationException("Stream is not writable.");
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new InvalidOperationException("Stream is not writable.");
	}
}
