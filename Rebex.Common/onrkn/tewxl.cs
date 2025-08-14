using System;
using System.IO;
using Rebex;

namespace onrkn;

internal class tewxl : xaxit
{
	private const int mnkwc = -1;

	private Stream wedqa;

	private byte[] gdjqo;

	private int lvcfk;

	private byte[] isozs;

	private int jxbtl;

	public override bool CanRead => false;

	public override bool CanSeek => false;

	public override bool CanWrite => true;

	public override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}

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

	public tewxl(Stream inner, string typeName)
	{
		if (inner == null || 1 == 0)
		{
			throw new ArgumentNullException("inner");
		}
		wedqa = inner;
		gdjqo = EncodingTools.dmppd.GetBytes("-----BEGIN " + typeName + "-----");
		lvcfk = -1;
		isozs = EncodingTools.dmppd.GetBytes("-----END " + typeName + "-----");
		jxbtl = -1;
	}

	public override void Flush()
	{
		wedqa.Flush();
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
		if (jxbtl == isozs.Length - 1)
		{
			return;
		}
		int i = offset;
		int num = offset + count;
		if (lvcfk < gdjqo.Length - 1)
		{
			for (; i < num; i++)
			{
				if (lvcfk == -1 && buffer[i] != gdjqo[0])
				{
					continue;
				}
				if (lvcfk <= gdjqo.Length - 2)
				{
					if (buffer[i] == gdjqo[lvcfk + 1])
					{
						lvcfk++;
					}
					else
					{
						lvcfk = -1;
					}
				}
				if (lvcfk == gdjqo.Length - 1)
				{
					i++;
					break;
				}
			}
		}
		count -= i - offset;
		offset = i;
		if (jxbtl < isozs.Length - 1)
		{
			for (; i < num; i++)
			{
				if (jxbtl == -1 && buffer[i] != isozs[0])
				{
					continue;
				}
				if (jxbtl <= isozs.Length - 2)
				{
					if (buffer[i] == isozs[jxbtl + 1])
					{
						jxbtl++;
					}
					else
					{
						jxbtl = -1;
					}
				}
				if (jxbtl == isozs.Length - 1)
				{
					count = i - offset - isozs.Length;
					break;
				}
			}
		}
		if (offset < buffer.Length && count > 0)
		{
			wedqa.Write(buffer, offset, count);
		}
	}

	protected override void julnt()
	{
		Flush();
		wedqa.Close();
		base.julnt();
	}
}
