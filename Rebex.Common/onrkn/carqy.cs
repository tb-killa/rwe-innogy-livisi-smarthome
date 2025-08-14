using System;
using System.IO;

namespace onrkn;

internal class carqy : xaxit
{
	public class uxtsv
	{
		private Action qjiut;

		private Func<long, SeekOrigin, long> zctja;

		private Action<long> kpirv;

		private Func<byte[], int, int, int> ncfil;

		private Action<byte[], int, int> gzxbu;

		private Func<bool> enbvb;

		private Func<bool> ddajb;

		private Func<bool> efpcx;

		private Func<long> fkioi;

		private Func<long> avbwk;

		private Action ekuen;

		private Action<long> opymf;

		public Action vytup
		{
			get
			{
				return qjiut;
			}
			set
			{
				qjiut = value;
			}
		}

		public Func<long, SeekOrigin, long> ezxku
		{
			get
			{
				return zctja;
			}
			set
			{
				zctja = value;
			}
		}

		public Action<long> wzvyw
		{
			get
			{
				return kpirv;
			}
			set
			{
				kpirv = value;
			}
		}

		public Func<byte[], int, int, int> aicbz
		{
			get
			{
				return ncfil;
			}
			set
			{
				ncfil = value;
			}
		}

		public Action<byte[], int, int> ldncm
		{
			get
			{
				return gzxbu;
			}
			set
			{
				gzxbu = value;
			}
		}

		public Func<bool> kjxtv
		{
			get
			{
				return enbvb;
			}
			set
			{
				enbvb = value;
			}
		}

		public Func<bool> qvtqm
		{
			get
			{
				return ddajb;
			}
			set
			{
				ddajb = value;
			}
		}

		public Func<bool> yablw
		{
			get
			{
				return efpcx;
			}
			set
			{
				efpcx = value;
			}
		}

		public Func<long> pyrwz
		{
			get
			{
				return fkioi;
			}
			set
			{
				fkioi = value;
			}
		}

		public Func<long> fqtns
		{
			get
			{
				return avbwk;
			}
			set
			{
				avbwk = value;
			}
		}

		public Action lreec
		{
			get
			{
				return ekuen;
			}
			set
			{
				ekuen = value;
			}
		}

		public Action<long> obtea
		{
			get
			{
				return opymf;
			}
			set
			{
				opymf = value;
			}
		}
	}

	private sealed class njdsb
	{
		public Stream jtich;

		public bool oxvpk()
		{
			return jtich.CanRead;
		}

		public bool nsxlj()
		{
			return jtich.CanSeek;
		}

		public bool cefjz()
		{
			return jtich.CanWrite;
		}

		public long uopgv()
		{
			return jtich.Length;
		}

		public long pnjed()
		{
			return jtich.Position;
		}

		public void yeegp(long p0)
		{
			jtich.Position = p0;
		}
	}

	private readonly uxtsv mrrgy;

	private static Func<uxtsv, uxtsv> zvgru;

	public override bool CanRead => mrrgy.kjxtv();

	public override bool CanSeek => mrrgy.yablw();

	public override bool CanWrite => mrrgy.qvtqm();

	public override long Length => mrrgy.pyrwz();

	public override long Position
	{
		get
		{
			return mrrgy.fqtns();
		}
		set
		{
			mrrgy.obtea(value);
		}
	}

	public carqy(uxtsv mapping)
	{
		mrrgy = mapping;
		if (mapping == null || 1 == 0)
		{
			throw new ArgumentNullException("mapping");
		}
	}

	public override void Flush()
	{
		mrrgy.vytup();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return mrrgy.ezxku(offset, origin);
	}

	public override void SetLength(long value)
	{
		mrrgy.wzvyw(value);
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return mrrgy.aicbz(buffer, offset, count);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		mrrgy.ldncm(buffer, offset, count);
	}

	public static carqy rcdwj(Stream p0, Func<uxtsv, uxtsv> p1 = null)
	{
		njdsb njdsb = new njdsb();
		njdsb.jtich = p0;
		if (njdsb.jtich == null || 1 == 0)
		{
			throw new ArgumentNullException("stream");
		}
		Func<uxtsv, uxtsv> func = p1;
		if (func == null || 1 == 0)
		{
			if (zvgru == null || 1 == 0)
			{
				zvgru = jcdne;
			}
			func = zvgru;
		}
		p1 = func;
		uxtsv uxtsv = new uxtsv();
		uxtsv.kjxtv = njdsb.oxvpk;
		uxtsv.yablw = njdsb.nsxlj;
		uxtsv.qvtqm = njdsb.cefjz;
		uxtsv.lreec = njdsb.jtich.Close;
		uxtsv.vytup = njdsb.jtich.Flush;
		uxtsv.pyrwz = njdsb.uopgv;
		uxtsv.fqtns = njdsb.pnjed;
		uxtsv.aicbz = njdsb.jtich.Read;
		uxtsv.ezxku = njdsb.jtich.Seek;
		uxtsv.wzvyw = njdsb.jtich.SetLength;
		uxtsv.obtea = njdsb.yeegp;
		uxtsv.ldncm = njdsb.jtich.Write;
		uxtsv arg = uxtsv;
		arg = p1(arg);
		return new carqy(arg);
	}

	protected override void julnt()
	{
		if (mrrgy.lreec != null)
		{
			mrrgy.lreec();
		}
	}

	private static uxtsv jcdne(uxtsv p0)
	{
		return p0;
	}
}
