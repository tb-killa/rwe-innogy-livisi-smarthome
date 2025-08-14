using System;
using System.IO;
using System.Threading;

namespace onrkn;

internal class peuid : npohs
{
	private sealed class acpkv
	{
		public long xlirr;

		public TimeSpan vezql;

		public bool vdzqx(long p0, TimeSpan p1)
		{
			if (p0 >= xlirr)
			{
				return p1 >= vezql;
			}
			return false;
		}
	}

	private EventHandler<yvnjx> rbzjc;

	private EventHandler<yvnjx> anhzn;

	private readonly Func<long, TimeSpan, bool> guaze;

	private readonly long aatru;

	private readonly long ctnjg;

	private long xtrlb;

	private long vpbjg;

	private DateTime xubrm;

	private DateTime xxvng;

	private long yycvq;

	private long hlegu;

	private nttuj rghcj;

	private nttuj xoigr;

	private static Func<long, TimeSpan, bool> ztqya;

	public override bool CanSeek => false;

	public override long Position
	{
		get
		{
			return base.Position;
		}
		set
		{
			throw new NotSupportedException("Setting 'Position' property is not supported on ProgressReportingStream.");
		}
	}

	public event EventHandler<yvnjx> nunrb
	{
		add
		{
			EventHandler<yvnjx> eventHandler = rbzjc;
			EventHandler<yvnjx> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<yvnjx> value2 = (EventHandler<yvnjx>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref rbzjc, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<yvnjx> eventHandler = rbzjc;
			EventHandler<yvnjx> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<yvnjx> value2 = (EventHandler<yvnjx>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref rbzjc, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler<yvnjx> gofvo
	{
		add
		{
			EventHandler<yvnjx> eventHandler = anhzn;
			EventHandler<yvnjx> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<yvnjx> value2 = (EventHandler<yvnjx>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref anhzn, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<yvnjx> eventHandler = anhzn;
			EventHandler<yvnjx> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<yvnjx> value2 = (EventHandler<yvnjx>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref anhzn, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public peuid(Stream inner, long minByteSpan, TimeSpan minTimeSpan, long bytesToRead, long bytesToWrite)
	{
		Func<long, TimeSpan, bool> func = null;
		acpkv acpkv = new acpkv
		{
			xlirr = minByteSpan,
			vezql = minTimeSpan
		};
		if (func == null || 1 == 0)
		{
			func = acpkv.vdzqx;
		}
		this._002Ector(inner, func, bytesToRead, bytesToWrite);
	}

	public peuid(Stream inner, Func<long, TimeSpan, bool> reportFilter, long bytesToRead, long bytesToWrite)
	{
		aatru = -1L;
		ctnjg = -1L;
		base._002Ector(inner);
		Func<long, TimeSpan, bool> func = reportFilter;
		if (func == null || 1 == 0)
		{
			if (ztqya == null || 1 == 0)
			{
				ztqya = cuebn;
			}
			func = ztqya;
		}
		guaze = func;
		aatru = bytesToRead;
		ctnjg = bytesToWrite;
		xubrm = DateTime.UtcNow;
		xxvng = DateTime.UtcNow;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		urlxw();
		int num = base.Read(buffer, offset, count);
		yycvq += num;
		urlxw();
		return num;
	}

	public override int ReadByte()
	{
		urlxw();
		int num = base.ReadByte();
		if (num >= 0)
		{
			yycvq++;
		}
		urlxw();
		return num;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		srrge();
		base.Write(buffer, offset, count);
		hlegu += count;
		srrge();
	}

	public override void WriteByte(byte value)
	{
		srrge();
		base.WriteByte(value);
		hlegu++;
		srrge();
	}

	private void urlxw()
	{
		aheqd(zutis, ref rghcj, aatru, ref xubrm, ref xtrlb, ref yycvq);
	}

	private void srrge()
	{
		aheqd(nwpmi, ref xoigr, ctnjg, ref xxvng, ref vpbjg, ref hlegu);
	}

	private void aheqd(Action<yvnjx> p0, ref nttuj p1, long p2, ref DateTime p3, ref long p4, ref long p5)
	{
		DateTime utcNow = DateTime.UtcNow;
		TimeSpan arg = utcNow - p3;
		if (p1 != nttuj.glhuq || (guaze(p5, arg) ? true : false))
		{
			p3 = utcNow;
			p4 += p5;
			p5 = 0L;
			if (p1 == nttuj.rjubn)
			{
				p2 = p4;
			}
			p0(new yvnjx(p4, p2, p1));
			if (p1 == nttuj.fmumz || 1 == 0)
			{
				p1 = nttuj.glhuq;
			}
		}
	}

	protected virtual void zutis(yvnjx p0)
	{
		if (rbzjc != null && 0 == 0)
		{
			rbzjc(this, p0);
		}
	}

	protected virtual void nwpmi(yvnjx p0)
	{
		if (anhzn != null && 0 == 0)
		{
			anhzn(this, p0);
		}
	}

	protected override void julnt()
	{
		base.julnt();
		xoigr = nttuj.rjubn;
		srrge();
		rghcj = nttuj.rjubn;
		urlxw();
	}

	public sealed override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		throw new NotSupportedException("Method 'BeginRead' is not supported on ProgressReportingStream.");
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		throw new NotSupportedException("Method 'BeginWrite' is not supported on ProgressReportingStream.");
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		throw new NotSupportedException("Method 'EndRead' is not supported on ProgressReportingStream.");
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		throw new NotSupportedException("Method 'EndWrite' is not supported on ProgressReportingStream.");
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException("Method 'Seek' is not supported on ProgressReportingStream.");
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException("Method 'SetLength' is not supported on ProgressReportingStream.");
	}

	private static bool cuebn(long p0, TimeSpan p1)
	{
		return p0 > 0;
	}
}
