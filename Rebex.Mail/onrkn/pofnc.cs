using System;
using System.Collections;
using System.IO;
using System.Text;
using Rebex;
using Rebex.Mime;

namespace onrkn;

internal class pofnc : Stream
{
	private int facra;

	private readonly ArrayList qxywx;

	private readonly eorvm bhwim;

	private readonly Encoding ylrmp;

	private object eupga;

	private MimeEntity crzln;

	private static readonly byte[] wndaq = new byte[2] { 13, 10 };

	public override bool CanRead => true;

	public override bool CanSeek => false;

	public override bool CanWrite => false;

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

	public pofnc(MimeEntity entity, bool top, bool signed, Encoding defaultEncoding)
	{
		entity.kjyki();
		qxywx = new ArrayList();
		entity.vsqfc(qxywx, top, signed);
		bhwim = new eorvm();
		ylrmp = defaultEncoding;
	}

	private bool fwecr()
	{
		if (eupga != null && 0 == 0)
		{
			eupga = crzln.deomd(eupga);
			if (eupga == null || 1 == 0)
			{
				crzln = null;
				bhwim.Write(wndaq, 0, wndaq.Length);
			}
			return true;
		}
		if (facra >= qxywx.Count)
		{
			return false;
		}
		object obj = qxywx[facra];
		facra++;
		if (obj == null || 1 == 0)
		{
			bhwim.Write(wndaq, 0, wndaq.Length);
			return true;
		}
		if (obj is string s && 0 == 0)
		{
			byte[] bytes = EncodingTools.Default.GetBytes(s);
			bhwim.Write(bytes, 0, bytes.Length);
			bhwim.Write(wndaq, 0, wndaq.Length);
			return true;
		}
		if (obj is MimeHeader mimeHeader && 0 == 0)
		{
			zncis writer = new zncis(bhwim, ylrmp);
			mimeHeader.Encode(writer);
			return true;
		}
		if (obj is MimeEntity mimeEntity && 0 == 0)
		{
			eupga = mimeEntity.jfupv(bhwim);
			if (eupga != null && 0 == 0)
			{
				crzln = mimeEntity;
			}
			else
			{
				bhwim.Write(wndaq, 0, wndaq.Length);
			}
			return true;
		}
		if (obj is nxtme<byte> && 0 == 0)
		{
			nxtme<byte> nxtme2 = (nxtme<byte>)obj;
			bhwim.Write(nxtme2.lthjd, nxtme2.frlfs, nxtme2.tvoem);
			return true;
		}
		if (obj is byte[] array && 0 == 0)
		{
			bhwim.Write(array, 0, array.Length);
			return true;
		}
		throw new MimeException("Unknown message item encountered.", MimeExceptionStatus.UnspecifiedError);
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_003d;
		IL_0006:
		int num2 = bhwim.Read(buffer, offset, count);
		num += num2;
		offset += num2;
		count -= num2;
		if (count > 0 && (!fwecr() || 1 == 0))
		{
			return num;
		}
		goto IL_003d;
		IL_003d:
		if (num < count)
		{
			goto IL_0006;
		}
		return num;
	}

	public override void Flush()
	{
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}
}
