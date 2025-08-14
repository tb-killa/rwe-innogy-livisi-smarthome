using System;
using System.Collections.Generic;
using System.IO;

namespace onrkn;

internal class eorvm : Stream
{
	private const int pkrkc = 65536;

	private byte[] febhk;

	private int lgvhx;

	private byte[] sazst;

	private int yxzxd;

	private Queue<byte[]> wumla;

	private long gajsg;

	public override bool CanRead => true;

	public override bool CanSeek => false;

	public override bool CanWrite => true;

	public override long Length => gajsg;

	public override long Position
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public eorvm()
	{
		febhk = null;
		lgvhx = (yxzxd = 65536);
		wumla = new Queue<byte[]>();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long v)
	{
		throw new NotSupportedException();
	}

	public override void Flush()
	{
	}

	public override void Close()
	{
	}

	public override void WriteByte(byte value)
	{
		if (yxzxd == 65536)
		{
			sazst = new byte[65536];
			yxzxd = 0;
			wumla.Enqueue(sazst);
		}
		sazst[yxzxd] = value;
		yxzxd++;
		gajsg++;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		while (count > 0)
		{
			int num = Math.Min(count, 65536 - yxzxd);
			if (num == 0 || 1 == 0)
			{
				sazst = new byte[65536];
				yxzxd = 0;
				wumla.Enqueue(sazst);
				num = Math.Min(count, 65536);
			}
			Array.Copy(buffer, offset, sazst, yxzxd, num);
			offset += num;
			count -= num;
			yxzxd += num;
			gajsg += num;
		}
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		int num = 0;
		while (true)
		{
			count = (int)Math.Min(count, gajsg);
			if (count == 0)
			{
				break;
			}
			if (lgvhx == 65536)
			{
				febhk = wumla.Dequeue();
				lgvhx = 0;
			}
			int num2 = Math.Min(count, 65536 - lgvhx);
			Array.Copy(febhk, lgvhx, buffer, offset, num2);
			lgvhx += num2;
			offset += num2;
			count -= num2;
			gajsg -= num2;
			num += num2;
		}
		return num;
	}

	public byte[] ozoyw()
	{
		int num = 0;
		int num2 = (int)gajsg;
		byte[] array = new byte[num2];
		while (num2 > 0)
		{
			int num3 = Read(array, num, num2);
			if (num3 == 0 || 1 == 0)
			{
				throw new InvalidOperationException("Corrupted queue stream.");
			}
			num += num3;
			num2 -= num3;
		}
		return array;
	}
}
