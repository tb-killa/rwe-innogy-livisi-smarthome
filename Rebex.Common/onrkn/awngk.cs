using System;
using Rebex;

namespace onrkn;

internal class awngk : scfgy, ILogWriter, sjhqe
{
	private class phzry : IDisposable
	{
		private readonly awngk bmpka;

		public phzry(awngk owner)
		{
			if (owner.drvpx && 0 == 0)
			{
				throw new InvalidOperationException("Multiple data hiding is not supported.");
			}
			bmpka = owner;
			bmpka.drvpx = true;
		}

		public void Dispose()
		{
			bmpka.drvpx = false;
		}
	}

	private class jbloy : scfgy, ILogWriter, sjhqe
	{
		private readonly awngk oufcn;

		private readonly Type iarla;

		private readonly int lvsuu;

		private readonly string jxwld;

		private readonly bool xrkrg;

		public LogLevel Level => oufcn.Level;

		private LogLevel xfrgj
		{
			get
			{
				return oufcn.Level;
			}
			set
			{
				((ILogWriter)oufcn).Level = value;
			}
		}

		public jbloy(awngk holder, Type type, int id, string prefix, bool infoToDebug)
		{
			oufcn = holder;
			iarla = type;
			lvsuu = id;
			jxwld = prefix;
			xrkrg = infoToDebug;
		}

		public void rfpvf(LogLevel p0, string p1, string p2)
		{
			if (kqwwm(ref p0, ref p2) && 0 == 0)
			{
				oufcn.vsffe(p0, iarla, lvsuu, p1, p2);
			}
		}

		private void eyjki(LogLevel p0, Type p1, int p2, string p3, string p4)
		{
			rfpvf(p0, p3, p4);
		}

		void ILogWriter.Write(LogLevel p0, Type p1, int p2, string p3, string p4)
		{
			//ILSpy generated this explicit interface implementation from .override directive in eyjki
			this.eyjki(p0, p1, p2, p3, p4);
		}

		public void iyauk(LogLevel p0, string p1, string p2, byte[] p3, int p4, int p5)
		{
			if (kqwwm(ref p0, ref p2) && 0 == 0)
			{
				oufcn.lnhvs(p0, iarla, lvsuu, p1, p2, p3, p4, p5);
			}
		}

		private void aisuz(LogLevel p0, Type p1, int p2, string p3, string p4, byte[] p5, int p6, int p7)
		{
			iyauk(p0, p3, p4, p5, p6, p7);
		}

		void ILogWriter.Write(LogLevel p0, Type p1, int p2, string p3, string p4, byte[] p5, int p6, int p7)
		{
			//ILSpy generated this explicit interface implementation from .override directive in aisuz
			this.aisuz(p0, p1, p2, p3, p4, p5, p6, p7);
		}

		private bool kqwwm(ref LogLevel p0, ref string p1)
		{
			if (p0 == LogLevel.Info && xrkrg && 0 == 0)
			{
				p0 = LogLevel.Debug;
			}
			if (oufcn.Level > p0)
			{
				return false;
			}
			if (jxwld != null && 0 == 0)
			{
				p1 = jxwld + p1;
			}
			return true;
		}
	}

	private readonly Type okktz;

	private readonly int vbdnl;

	private bool drvpx;

	private static readonly awngk sndtt = new awngk();

	private ILogWriter vktfc;

	private bool ionoo;

	public ILogWriter xxboi
	{
		get
		{
			return vktfc;
		}
		set
		{
			vktfc = value;
		}
	}

	public Type dnvdk => okktz;

	public int gcjit => vbdnl;

	public static sjhqe xffts => sndtt;

	public bool kdlxj => drvpx;

	public bool ngqry
	{
		get
		{
			return ionoo;
		}
		set
		{
			ionoo = value;
		}
	}

	public LogLevel Level
	{
		get
		{
			ILogWriter logWriter = xxboi;
			if (logWriter != null && 0 == 0)
			{
				return logWriter.Level;
			}
			return LogLevel.Off;
		}
	}

	private LogLevel odxkk
	{
		get
		{
			return Level;
		}
		set
		{
			throw new InvalidOperationException("Unable to change log level.");
		}
	}

	private awngk()
	{
	}

	public awngk(Type objectType, int? objectId, ILogWriter logger = null)
	{
		xxboi = logger;
		okktz = objectType;
		vbdnl = objectId ?? 0;
	}

	public void rfpvf(LogLevel p0, string p1, string p2)
	{
		vsffe(p0, null, null, p1, p2);
	}

	private void zywei(LogLevel p0, Type p1, int p2, string p3, string p4)
	{
		vsffe(p0, null, null, p3, p4);
	}

	void ILogWriter.Write(LogLevel p0, Type p1, int p2, string p3, string p4)
	{
		//ILSpy generated this explicit interface implementation from .override directive in zywei
		this.zywei(p0, p1, p2, p3, p4);
	}

	public void iyauk(LogLevel p0, string p1, string p2, byte[] p3, int p4, int p5)
	{
		lnhvs(p0, null, null, p1, p2, p3, p4, p5);
	}

	private void cokap(LogLevel p0, Type p1, int p2, string p3, string p4, byte[] p5, int p6, int p7)
	{
		lnhvs(p0, null, null, p3, p4, p5, p6, p7);
	}

	void ILogWriter.Write(LogLevel p0, Type p1, int p2, string p3, string p4, byte[] p5, int p6, int p7)
	{
		//ILSpy generated this explicit interface implementation from .override directive in cokap
		this.cokap(p0, p1, p2, p3, p4, p5, p6, p7);
	}

	private void vsffe(LogLevel p0, Type p1, int? p2, string p3, string p4)
	{
		ILogWriter logWriter = xxboi;
		if (logWriter != null && 0 == 0 && logWriter.Level <= p0)
		{
			Type type = p1;
			if ((object)type == null || 1 == 0)
			{
				type = okktz;
			}
			logWriter.Write(p0, type, p2.GetValueOrDefault(vbdnl), p3, p4);
		}
	}

	private void lnhvs(LogLevel p0, Type p1, int? p2, string p3, string p4, byte[] p5, int p6, int p7)
	{
		ILogWriter logWriter = xxboi;
		if (logWriter == null || false || logWriter.Level > p0)
		{
			return;
		}
		if (drvpx && 0 == 0 && p0 <= LogLevel.Verbose)
		{
			p4 = brgjd.edcru("{0} ({1} hidden bytes).", p4.TrimEnd(), p7);
			Type type = p1;
			if ((object)type == null || 1 == 0)
			{
				type = okktz;
			}
			logWriter.Write(p0, type, p2.GetValueOrDefault(vbdnl), p3, p4);
		}
		else
		{
			Type type2 = p1;
			if ((object)type2 == null || 1 == 0)
			{
				type2 = okktz;
			}
			logWriter.Write(p0, type2, p2.GetValueOrDefault(vbdnl), p3, p4, p5, p6, p7);
		}
	}

	public IDisposable cllqt(bool p0)
	{
		if (!p0 || false || ngqry)
		{
			return default(fbqke);
		}
		return new phzry(this);
	}

	public ILogWriter lhftu()
	{
		return new jbloy(this, okktz, vbdnl, null, infoToDebug: false);
	}

	public ILogWriter dtevb(Type p0, int? p1)
	{
		Type type = p0;
		if ((object)type == null || 1 == 0)
		{
			type = okktz;
		}
		return new jbloy(this, type, p1.GetValueOrDefault(vbdnl), null, infoToDebug: false);
	}

	public ILogWriter qtbyf(string p0)
	{
		return new jbloy(this, okktz, vbdnl, p0, infoToDebug: false);
	}

	public ILogWriter urxvu()
	{
		return new jbloy(this, okktz, vbdnl, null, infoToDebug: true);
	}

	public scfgy ymjug(Type p0, int? p1)
	{
		Type type = p0;
		if ((object)type == null || 1 == 0)
		{
			type = okktz;
		}
		return new jbloy(this, type, p1.GetValueOrDefault(vbdnl), null, infoToDebug: false);
	}
}
