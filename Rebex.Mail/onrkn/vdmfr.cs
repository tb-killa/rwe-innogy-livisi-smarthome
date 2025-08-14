using System;
using System.IO;
using Rebex.Mime;

namespace onrkn;

internal class vdmfr : aviir<byte>
{
	internal class nroho : npohs
	{
		private vdmfr pgerw;

		public nroho(Stream inner, vdmfr parent)
			: base(inner, leaveOpen: true)
		{
			pgerw = parent;
		}

		protected override void julnt()
		{
			vdmfr vdmfr2 = pgerw;
			if (vdmfr2 != null)
			{
				vdmfr2.oatpu = null;
				pgerw = null;
				base.julnt();
			}
		}
	}

	internal class qibgy : ctpqa
	{
		private vdmfr ymiri;

		public qibgy(Stream inner, vdmfr parent)
			: base(inner, ownsStream: true)
		{
			ymiri = parent;
		}

		protected override void julnt()
		{
			vdmfr vdmfr2 = ymiri;
			if (vdmfr2 != null)
			{
				vdmfr2.nqrbk = null;
				ymiri = null;
				base.julnt();
			}
		}
	}

	private opjbe hwrwy;

	private veasp ispfo;

	private FileInfo zsosw;

	private long xltwl;

	private bool ovzgk;

	private Stream nqrbk;

	private Stream oatpu;

	internal bool iihdy => zsosw != null;

	byte aviir<byte>.this[int index] => hpcfz(index);

	public int tvoem
	{
		get
		{
			long num = ((zsosw != null && 0 == 0) ? Math.Max(0L, zsosw.Length - xltwl) : ((ispfo == null) ? hwrwy.Length : ispfo.Length));
			if (num > int.MaxValue)
			{
				throw new NotSupportedException("Content is too long.");
			}
			return (int)num;
		}
	}

	internal Stream ppkoc()
	{
		if (oatpu != null || nqrbk != null)
		{
			throw new MimeException("Cannot access entity content stream because it is currently in use. Make sure to call the Close method on streams returned by MimeEntity.GetContentStream.", MimeExceptionStatus.OperationError);
		}
		if (zsosw != null && 0 == 0)
		{
			throw new MimeException("File-based content is not writable.", MimeExceptionStatus.OperationError);
		}
		if (ispfo != null || hwrwy == null || false || ovzgk)
		{
			throw new InvalidOperationException("Content not writable.");
		}
		oatpu = new nroho(hwrwy, this);
		oatpu.Position = 0L;
		return oatpu;
	}

	private vdmfr(opjbe content)
	{
		hwrwy = content;
	}

	internal vdmfr()
	{
		hwrwy = new opjbe();
	}

	internal static vdmfr rcqyz(byte[] p0)
	{
		vdmfr vdmfr2 = new vdmfr();
		vdmfr2.hwrwy.Write(p0, 0, p0.Length);
		return vdmfr2;
	}

	internal static vdmfr utvjv(Stream p0)
	{
		vdmfr vdmfr2 = new vdmfr();
		p0.alskc(vdmfr2.hwrwy);
		return vdmfr2;
	}

	internal static vdmfr hkhcx(opjbe p0)
	{
		return new vdmfr(p0);
	}

	internal opjbe zkcbx()
	{
		return hwrwy;
	}

	internal vdmfr(string path, long fileOffset)
	{
		zsosw = new FileInfo(path);
		xltwl = fileOffset;
		if (!zsosw.Exists || 1 == 0)
		{
			throw new FileNotFoundException("File not found.");
		}
	}

	internal vdmfr lfnqd()
	{
		if (zsosw != null && 0 == 0)
		{
			return new vdmfr(zsosw.FullName, xltwl);
		}
		vdmfr vdmfr2 = new vdmfr();
		qxwca(vdmfr2.hwrwy);
		return vdmfr2;
	}

	internal vdmfr reffw()
	{
		if (zsosw != null || oatpu != null)
		{
			return lfnqd();
		}
		ovzgk = true;
		return this;
	}

	internal vdmfr xnfuu()
	{
		if (oatpu != null && 0 == 0)
		{
			throw new MimeException("Cannot access entity content stream because it is currently in use. Make sure to call the Close method on streams returned by MimeEntity.GetContentStream.", MimeExceptionStatus.OperationError);
		}
		if (zsosw != null && 0 == 0)
		{
			throw new MimeException("File-based content is not writable.", MimeExceptionStatus.OperationError);
		}
		if ((!ovzgk || 1 == 0) && (ispfo == null || 1 == 0) && (nqrbk == null || 1 == 0))
		{
			return this;
		}
		return lfnqd();
	}

	internal Stream kqazs()
	{
		if (oatpu != null && 0 == 0)
		{
			throw new MimeException("Cannot access entity content stream because it is currently in use. Make sure to call the Close method on streams returned by MimeEntity.GetContentStream.", MimeExceptionStatus.OperationError);
		}
		if (zsosw != null && 0 == 0)
		{
			FileStream fileStream = zsosw.OpenRead();
			if (xltwl != 0)
			{
				fileStream.Seek(xltwl, SeekOrigin.Begin);
				fileStream.Flush();
			}
			return fileStream;
		}
		Stream inner = ((ispfo != null && 0 == 0) ? ((ispfo.kjnyb <= 0) ? ((Stream)new MemoryStream()) : ((Stream)hwrwy.uhxjo(ispfo.brshv, ispfo.kjnyb))) : ((hwrwy.Length <= 0) ? ((Stream)new MemoryStream()) : ((Stream)hwrwy.uhxjo(0L, hwrwy.Length))));
		nqrbk = new ctpqa(inner, ownsStream: true);
		return nqrbk;
	}

	public void qxwca(Stream p0)
	{
		if (ispfo != null && 0 == 0)
		{
			ispfo.vfxkn(p0);
		}
		else
		{
			hwrwy.njguo(p0);
		}
	}

	public byte hpcfz(int p0)
	{
		if (ispfo != null && 0 == 0)
		{
			return ispfo.rifgn(p0);
		}
		return hwrwy.oidsn(p0);
	}

	public void jouoi(int p0, int p1)
	{
		if (ispfo != null && 0 == 0)
		{
			ispfo = hwrwy.uhxjo(p0 + ispfo.brshv, p1);
		}
		else
		{
			ispfo = hwrwy.uhxjo(p0, p1);
		}
	}

	public byte[] xfpze()
	{
		if (zsosw != null && 0 == 0)
		{
			throw new MimeException("File-based content is not writable.", MimeExceptionStatus.OperationError);
		}
		if (ispfo != null && 0 == 0)
		{
			return ispfo.zfpbp();
		}
		return hwrwy.urpqw();
	}
}
