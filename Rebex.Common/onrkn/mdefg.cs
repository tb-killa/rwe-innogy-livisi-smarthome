using System;
using System.IO;
using Rebex;

namespace onrkn;

internal class mdefg : xaxit
{
	private const int ujlus = 76;

	public static readonly byte[] rrcda = new byte[2] { 13, 10 };

	private readonly Stream cccvf;

	private readonly byte[] hkigb;

	private int znxrr;

	public override bool CanRead => false;

	public override bool CanSeek => false;

	public override bool CanWrite => cccvf.CanWrite;

	public override long Length => cccvf.Length;

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

	public mdefg(Stream inner)
	{
		if (inner == null || 1 == 0)
		{
			throw new ArgumentNullException("inner");
		}
		cccvf = inner;
		hkigb = new byte[57];
	}

	protected override void julnt()
	{
		if (znxrr > 0)
		{
			xyqrz();
		}
		cccvf.Close();
	}

	public override void Flush()
	{
		cccvf.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long v)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (znxrr > 0)
		{
			int num = hkigb.Length - znxrr;
			if (count < num)
			{
				num = count;
			}
			Array.Copy(buffer, offset, hkigb, znxrr, num);
			znxrr += num;
			offset += num;
			count -= num;
			if (znxrr != hkigb.Length)
			{
				return;
			}
			xyqrz();
		}
		while (count >= hkigb.Length)
		{
			wtmxe(buffer, offset, hkigb.Length);
			offset += hkigb.Length;
			count -= hkigb.Length;
		}
		if (count > 0)
		{
			Array.Copy(buffer, offset, hkigb, 0, count);
			znxrr = count;
		}
	}

	private void xyqrz()
	{
		int p = znxrr;
		znxrr = 0;
		wtmxe(hkigb, 0, p);
	}

	private void wtmxe(byte[] p0, int p1, int p2)
	{
		string s = Convert.ToBase64String(p0, p1, p2);
		byte[] bytes = EncodingTools.ASCII.GetBytes(s);
		cccvf.Write(bytes, 0, bytes.Length);
		cccvf.Write(rrcda, 0, rrcda.Length);
	}
}
