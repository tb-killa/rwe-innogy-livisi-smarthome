using System.IO;

namespace onrkn;

internal class fxtcx : Stream
{
	private Stream nuhnr;

	private long xamya;

	public override long Length => nuhnr.Length;

	public override long Position
	{
		get
		{
			return xamya;
		}
		set
		{
			nuhnr.Position = value;
			nuhnr.Flush();
			xamya = nuhnr.Position;
		}
	}

	public override bool CanRead => nuhnr.CanRead;

	public override bool CanSeek => nuhnr.CanSeek;

	public override bool CanWrite => nuhnr.CanWrite;

	public fxtcx(Stream wrappedStream)
	{
		nuhnr = wrappedStream;
	}

	public override void SetLength(long value)
	{
		nuhnr.SetLength(value);
		xamya = nuhnr.Position;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		if (origin == SeekOrigin.Current)
		{
			xamya = nuhnr.Seek(xamya + offset, SeekOrigin.Begin);
		}
		else
		{
			xamya = nuhnr.Seek(offset, origin);
		}
		nuhnr.Flush();
		return xamya;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (nuhnr.Position != xamya)
		{
			nuhnr.Position = xamya;
			nuhnr.Flush();
		}
		int result = nuhnr.Read(buffer, offset, count);
		xamya = nuhnr.Position;
		return result;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (nuhnr.Position != xamya)
		{
			nuhnr.Position = xamya;
			nuhnr.Flush();
		}
		nuhnr.Write(buffer, offset, count);
		xamya = nuhnr.Position;
	}

	public override void Flush()
	{
		nuhnr.Flush();
	}
}
