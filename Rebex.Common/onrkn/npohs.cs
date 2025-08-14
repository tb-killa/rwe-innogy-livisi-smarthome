using System;
using System.IO;

namespace onrkn;

internal class npohs : xaxit
{
	private readonly bool wfawl;

	private Stream zcghe;

	protected Stream klhjv => uvesr();

	internal bool vqudn => zcghe == null;

	public override bool CanRead => klhjv.CanRead;

	public override bool CanSeek => klhjv.CanSeek;

	public override bool CanWrite => klhjv.CanWrite;

	public override long Length => klhjv.Length;

	public override long Position
	{
		get
		{
			return klhjv.Position;
		}
		set
		{
			klhjv.Position = value;
			klhjv.Flush();
		}
	}

	internal void tibxc()
	{
		uvesr();
	}

	private Stream uvesr()
	{
		Stream stream = zcghe;
		if (stream == null || 1 == 0)
		{
			throw new ObjectDisposedException(null, "Cannot access a closed stream.");
		}
		return stream;
	}

	public npohs(Stream inner)
		: this(inner, leaveOpen: false)
	{
	}

	public npohs(Stream inner, bool leaveOpen)
	{
		if (inner == null || 1 == 0)
		{
			throw new ArgumentNullException("inner");
		}
		zcghe = inner;
		wfawl = leaveOpen;
	}

	public override void Flush()
	{
		klhjv.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return klhjv.Read(buffer, offset, count);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		long result = klhjv.Seek(offset, origin);
		klhjv.Flush();
		return result;
	}

	public override void SetLength(long value)
	{
		klhjv.SetLength(value);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		klhjv.Write(buffer, offset, count);
	}

	public override int ReadByte()
	{
		return klhjv.ReadByte();
	}

	public override void WriteByte(byte value)
	{
		klhjv.WriteByte(value);
	}

	protected override void julnt()
	{
		Stream stream = zcghe;
		if (stream != null && 0 == 0)
		{
			zcghe = null;
			if (!wfawl || 1 == 0)
			{
				stream.Close();
			}
		}
	}
}
