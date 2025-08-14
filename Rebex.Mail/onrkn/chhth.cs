using System;

namespace onrkn;

internal class chhth : rnsvi
{
	private long crurc;

	public override bool CanRead => true;

	public override bool CanSeek => true;

	public override bool CanWrite => false;

	public override long Length => 0L;

	public override long Position
	{
		get
		{
			return crurc;
		}
		set
		{
			if (value < 0)
			{
				throw hifyx.nztrs("value", value, "Argument is out of range of valid values.");
			}
			crurc = value;
		}
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return 0;
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
