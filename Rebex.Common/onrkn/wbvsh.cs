using System;
using System.IO;

namespace onrkn;

internal class wbvsh : npohs
{
	private sealed class gipse
	{
		public wbvsh kzuwq;

		public byte[] ccors;

		public int jzpqw;

		public int ovpdw;

		public int uvjbk()
		{
			return kzuwq.ccbri(ccors, jzpqw, ovpdw);
		}
	}

	private sealed class izuhu
	{
		public wbvsh hcixq;

		public long kipuw;

		public SeekOrigin uiqea;

		public long fuhca()
		{
			return hcixq.klvch(kipuw, uiqea);
		}
	}

	private sealed class pppap
	{
		public wbvsh fsodr;

		public long rlscs;

		public void pmeyt()
		{
			fsodr.kdkzt(rlscs);
		}
	}

	private sealed class zhocz
	{
		public wbvsh rhpzd;

		public byte[] fvfen;

		public int xbbdp;

		public int tlxwv;

		public void mdgha()
		{
			rhpzd.jegxr(fvfen, xbbdp, tlxwv);
		}
	}

	private sealed class nzbiu
	{
		public wbvsh lcgjb;

		public byte udlpv;

		public void epvzn()
		{
			lcgjb.fipna(udlpv);
		}
	}

	private readonly Func<Exception, Exception> gojrr;

	private readonly znuay hcdsf;

	public bool prsgx => hcdsf.mpjbd;

	public override bool CanRead
	{
		get
		{
			bool canRead = base.CanRead;
			if (!prsgx || 1 == 0)
			{
				return canRead;
			}
			return false;
		}
	}

	public override bool CanSeek
	{
		get
		{
			bool canSeek = base.CanSeek;
			if (!prsgx || 1 == 0)
			{
				return canSeek;
			}
			return false;
		}
	}

	public override bool CanWrite
	{
		get
		{
			bool canWrite = base.CanWrite;
			if (!prsgx || 1 == 0)
			{
				return canWrite;
			}
			return false;
		}
	}

	public override long Length => base.Length;

	public override long Position
	{
		get
		{
			return base.Position;
		}
		set
		{
			ptxlt();
			base.Position = value;
		}
	}

	public wbvsh(Stream inner, znuay cts, Func<Exception, Exception> createCancelException)
		: this(inner, leaveOpen: false, cts, createCancelException)
	{
	}

	public wbvsh(Stream inner, bool leaveOpen, znuay cts, Func<Exception, Exception> createCancelException)
		: base(inner, leaveOpen)
	{
		if (createCancelException == null || 1 == 0)
		{
			throw new ArgumentNullException("createCancelException");
		}
		gojrr = createCancelException;
		znuay obj = cts;
		if (obj == null || 1 == 0)
		{
			obj = new znuay();
		}
		hcdsf = obj;
	}

	private void ptxlt()
	{
		tibxc();
		if (prsgx && 0 == 0)
		{
			throw gojrr(null);
		}
	}

	public void dkamk()
	{
		tibxc();
		hcdsf.pvutk();
	}

	public override void Flush()
	{
		emrqz(nvnbl);
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		gipse gipse = new gipse();
		gipse.ccors = buffer;
		gipse.jzpqw = offset;
		gipse.ovpdw = count;
		gipse.kzuwq = this;
		return dmwfn(gipse.uvjbk);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		izuhu izuhu = new izuhu();
		izuhu.kipuw = offset;
		izuhu.uiqea = origin;
		izuhu.hcixq = this;
		return dmwfn(izuhu.fuhca);
	}

	public override void SetLength(long value)
	{
		pppap pppap = new pppap();
		pppap.rlscs = value;
		pppap.fsodr = this;
		emrqz(pppap.pmeyt);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		zhocz zhocz = new zhocz();
		zhocz.fvfen = buffer;
		zhocz.xbbdp = offset;
		zhocz.tlxwv = count;
		zhocz.rhpzd = this;
		emrqz(zhocz.mdgha);
	}

	public override int ReadByte()
	{
		return dmwfn(igisl);
	}

	public override void WriteByte(byte value)
	{
		nzbiu nzbiu = new nzbiu();
		nzbiu.udlpv = value;
		nzbiu.lcgjb = this;
		emrqz(nzbiu.epvzn);
	}

	private T dmwfn<T>(Func<T> p0)
	{
		ptxlt();
		try
		{
			return p0();
		}
		catch (Exception arg)
		{
			if (prsgx && 0 == 0)
			{
				throw gojrr(arg);
			}
			throw;
		}
	}

	private void emrqz(Action p0)
	{
		ptxlt();
		try
		{
			p0();
		}
		catch (Exception arg)
		{
			if (prsgx && 0 == 0)
			{
				throw gojrr(arg);
			}
			throw;
		}
	}

	private void kpyrr()
	{
		base.Flush();
	}

	private void nvnbl()
	{
		kpyrr();
	}

	private int ccbri(byte[] p0, int p1, int p2)
	{
		return base.Read(p0, p1, p2);
	}

	private long klvch(long p0, SeekOrigin p1)
	{
		return base.Seek(p0, p1);
	}

	private void kdkzt(long p0)
	{
		base.SetLength(p0);
	}

	private void jegxr(byte[] p0, int p1, int p2)
	{
		base.Write(p0, p1, p2);
	}

	private int wziyq()
	{
		return base.ReadByte();
	}

	private int igisl()
	{
		return wziyq();
	}

	private void fipna(byte p0)
	{
		base.WriteByte(p0);
	}
}
