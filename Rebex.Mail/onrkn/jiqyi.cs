using System;
using System.IO;

namespace onrkn;

internal class jiqyi : Stream
{
	private readonly byte[] wrfqo;

	private readonly Stream dtczd;

	private readonly long ccrvz;

	private long kjchs;

	public override bool CanRead => true;

	public override bool CanSeek => false;

	public override bool CanWrite => false;

	public override long Length => dtczd.Length - ccrvz + wrfqo.Length;

	public override long Position
	{
		get
		{
			return kjchs;
		}
		set
		{
			throw new InvalidOperationException("Stream is not seekable.");
		}
	}

	public jiqyi(byte[] preamble, Stream inner, long streamPosition)
	{
		if (inner == null || 1 == 0)
		{
			throw new ArgumentNullException("inner", "Stream cannot be null.");
		}
		if (preamble == null || 1 == 0)
		{
			throw new ArgumentNullException("preamble", "Buffer cannot be null.");
		}
		if (!inner.CanRead || 1 == 0)
		{
			throw new ArgumentException("Input stream has to be readable.", "inner");
		}
		dtczd = inner;
		wrfqo = preamble;
		ccrvz = streamPosition;
	}

	public override void Flush()
	{
		dtczd.Flush();
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
			throw new ArgumentException("Count is out of range of valid values.", "count");
		}
		if (count == 0 || 1 == 0)
		{
			return 0;
		}
		int num = 0;
		if (kjchs < wrfqo.Length)
		{
			int num2 = Math.Min(count, wrfqo.Length - (int)kjchs);
			Array.Copy(wrfqo, (int)kjchs, buffer, offset, num2);
			kjchs += num2;
			offset += num2;
			count -= num2;
			num += num2;
		}
		while (count > 0)
		{
			int num3 = dtczd.Read(buffer, offset, count);
			if (num3 == 0 || 1 == 0)
			{
				return num;
			}
			kjchs += num3;
			offset += num3;
			count -= num3;
			num += num3;
		}
		return num;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new InvalidOperationException("Stream is not seekable.");
	}

	public override void SetLength(long value)
	{
		throw new InvalidOperationException("Stream is not writable.");
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new InvalidOperationException("Stream is not writable.");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && 0 == 0)
		{
			dtczd.Close();
		}
		base.Dispose(disposing);
	}
}
