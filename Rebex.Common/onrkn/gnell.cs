using System;
using System.IO;

namespace onrkn;

internal class gnell : FileStream
{
	public override long Position
	{
		get
		{
			return base.Position;
		}
		set
		{
			base.Position = value;
			Flush();
		}
	}

	public gnell(string fileName, FileMode mode, FileAccess access, FileShare share)
		: base(fileName, mode, access, share)
	{
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		if (origin == SeekOrigin.End)
		{
			offset = base.Length + offset;
			origin = SeekOrigin.Begin;
		}
		offset = base.Seek(offset, origin);
		Flush();
		return offset;
	}

	public override void SetLength(long value)
	{
		if (value <= Length)
		{
			base.SetLength(value);
			return;
		}
		long position = Position;
		long num = Seek(0L, SeekOrigin.End);
		base.SetLength(value);
		Seek(num, SeekOrigin.Begin);
		long num2 = value - num;
		byte[] array = new byte[4096];
		while (num2 > 0)
		{
			int num3 = (int)Math.Min(array.Length, num2);
			Write(array, 0, num3);
			num2 -= num3;
		}
		Seek(position, SeekOrigin.Begin);
	}
}
