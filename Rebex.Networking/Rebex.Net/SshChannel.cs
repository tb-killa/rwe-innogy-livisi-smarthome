using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using onrkn;

namespace Rebex.Net;

public class SshChannel : IDisposable
{
	private enum wgcpf
	{
		hxnjf = 0,
		avuxb = 2
	}

	internal const int znyze = 131072;

	private SshSession afihi;

	private uint jqppx;

	private uint lszzf;

	private Encoding jsixd;

	private SshChannelType pgnsv;

	private int ffrtv;

	private readonly int fuitr;

	private uint kukdy;

	private int yevvp;

	private bool yvqvk;

	private bool rgxpe;

	private bool ekvtf;

	private SshChannelState xocaz;

	private SshChannelExitStatus puwpg;

	private lmmys vtbgu;

	private SshException wwnzx;

	private bool calrr;

	private hzdap wwnjk;

	private SshChannelExtendedDataMode vahae;

	private Queue<SshExtendedDataReceivedEventArgs> ekbrs;

	private int spyep;

	private int liawc;

	private object oeiuo = new object();

	private object gzdke = new object();

	private object cjxro = new object();

	private object glgbc = new object();

	private EventHandler<SshExtendedDataReceivedEventArgs> fztab;

	internal Encoding ibedp
	{
		get
		{
			return jsixd;
		}
		set
		{
			jsixd = value;
		}
	}

	internal SshSession xwmfj => afihi;

	internal uint evxyv => jqppx;

	internal uint seezg => lszzf;

	public SshChannelType Type => pgnsv;

	internal int hpjxu => ffrtv;

	internal int zgwen => fuitr;

	public SshChannelState State
	{
		get
		{
			if (rgxpe && 0 == 0)
			{
				return SshChannelState.Closed;
			}
			return xocaz;
		}
	}

	public SshChannelExitStatus ExitStatus => puwpg;

	internal SshException forqe => wwnzx;

	public SshChannelExtendedDataMode ExtendedDataMode
	{
		get
		{
			return vahae;
		}
		set
		{
			vahae = value;
		}
	}

	public int TerminalWidth => spyep;

	public int TerminalHeight => liawc;

	public int Available => wwnjk.jgwnt;

	public event EventHandler<SshExtendedDataReceivedEventArgs> ExtendedDataReceived
	{
		add
		{
			EventHandler<SshExtendedDataReceivedEventArgs> eventHandler = fztab;
			EventHandler<SshExtendedDataReceivedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SshExtendedDataReceivedEventArgs> value2 = (EventHandler<SshExtendedDataReceivedEventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref fztab, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<SshExtendedDataReceivedEventArgs> eventHandler = fztab;
			EventHandler<SshExtendedDataReceivedEventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<SshExtendedDataReceivedEventArgs> value2 = (EventHandler<SshExtendedDataReceivedEventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref fztab, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	internal SshChannel(SshSession session, uint localId, SshChannelType type, int localWindowSize, int maxPacketSize, Encoding encoding)
	{
		afihi = session;
		jqppx = localId;
		pgnsv = type;
		ffrtv = localWindowSize;
		fuitr = maxPacketSize;
		wwnjk = new hzdap(localWindowSize);
		jsixd = encoding;
		yvqvk = (session.Options & SshOptions.DoNotSplitChannelPackets) == 0;
		ekbrs = new Queue<SshExtendedDataReceivedEventArgs>();
	}

	internal void flyta(byte[] p0, int p1, int p2)
	{
		switch (xocaz)
		{
		case SshChannelState.None:
			xcjzl(p0, p1, p2);
			return;
		default:
		{
			byte b = p0[p1];
			if (b != 95 && b != 98)
			{
				throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", p0[p1]));
			}
			break;
		}
		case SshChannelState.Connected:
			break;
		}
		switch (p0[p1])
		{
		case 99:
			vtbgu = lmmys.hxrgw;
			break;
		case 100:
			vtbgu = lmmys.alvtw;
			break;
		case 94:
		case 95:
		{
			bool flag = p0[p1] == 95;
			int num3 = 5;
			int code;
			if (flag && 0 == 0)
			{
				code = jlfbq.yyxrz(p0, p1 + num3);
				num3 += 4;
			}
			else
			{
				code = 0;
			}
			int num4 = p2 - num3 - 4;
			p2 = jlfbq.yyxrz(p0, p1 + num3);
			if (p2 > num4)
			{
				afihi.cnfnb(LogLevel.Error, "Received SSH_MSG_CHANNEL_DATA with a payload of {0} bytes claiming to contain {1} bytes.", num4, p2);
				throw new SshException(tcpjq.svqut, "Received invalid data packet.");
			}
			lock (cjxro)
			{
				if (p2 > ffrtv)
				{
					string text = "SSH protocol violation by the remote side.";
					afihi.iejdf(LogLevel.Error, text);
					throw new SshException(tcpjq.svqut, text);
				}
				ffrtv -= p2;
				if (rgxpe && 0 == 0)
				{
					break;
				}
				if (!flag || false || vahae == SshChannelExtendedDataMode.TreatAsNormalData)
				{
					wwnjk.nujhp(p0, p1 + num3 + 4, p2);
				}
			}
			if (((flag ? true : false) || vahae == (SshChannelExtendedDataMode)42) && fztab != null && 0 == 0)
			{
				mrzwa(new SshExtendedDataReceivedEventArgs(code, p0, p1 + num3 + 4, p2));
			}
			break;
		}
		case 93:
		{
			pfcna pfcna = new pfcna(p0, p1, p2, jsixd);
			uint num2;
			lock (glgbc)
			{
				uint num = kukdy;
				kukdy += pfcna.wdldx;
				if (kukdy < num)
				{
					throw new SshException(tcpjq.svqut, "Window overflow.");
				}
				num2 = kukdy;
			}
			afihi.cnfnb(LogLevel.Debug, "Adjusted remote receive window size: {0} -> {1}.", num2 - pfcna.wdldx, num2);
			break;
		}
		case 97:
			lock (oeiuo)
			{
				if (!rgxpe || 1 == 0)
				{
					rgxpe = true;
					afihi.adxeo(new sybiu(xbrcx.evspj, lszzf));
				}
				xocaz = SshChannelState.Closed;
				afihi.ubjbk(this);
			}
			if (fztab != null && 0 == 0 && vahae == (SshChannelExtendedDataMode)42)
			{
				mrzwa(new SshExtendedDataReceivedEventArgs(0, new byte[0], 0, 0));
			}
			break;
		case 98:
			yjzda(p0, p1, p2);
			break;
		case 96:
			lock (oeiuo)
			{
				if (!rgxpe || 1 == 0)
				{
					rgxpe = true;
					zwpbk();
					afihi.adxeo(new sybiu(xbrcx.evspj, lszzf));
				}
			}
			if (fztab != null && 0 == 0 && vahae == (SshChannelExtendedDataMode)42)
			{
				mrzwa(new SshExtendedDataReceivedEventArgs(0, new byte[0], 0, 0));
			}
			break;
		default:
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unsupported packet {0}.", p0[p1]));
		}
	}

	private void fvsyq()
	{
		EventHandler<SshExtendedDataReceivedEventArgs> eventHandler = fztab;
		if (eventHandler == null)
		{
			return;
		}
		while (true)
		{
			SshExtendedDataReceivedEventArgs e;
			lock (ekbrs)
			{
				if (ekbrs.Count == 0 || 1 == 0)
				{
					break;
				}
				e = ekbrs.Dequeue();
			}
			try
			{
				eventHandler(this, e);
			}
			catch (Exception ex)
			{
				afihi.cnfnb(LogLevel.Error, "Error in ExtendedDataReceived error handler: {0}", ex);
			}
		}
	}

	private void mrzwa(SshExtendedDataReceivedEventArgs p0)
	{
		WaitCallback waitCallback = null;
		if (vahae == (SshChannelExtendedDataMode)2)
		{
			lock (ekbrs)
			{
				ekbrs.Enqueue(p0);
			}
			fvsyq();
			return;
		}
		lock (ekbrs)
		{
			ekbrs.Enqueue(p0);
			if (ekbrs.Count > 1)
			{
				return;
			}
		}
		if (waitCallback == null || 1 == 0)
		{
			waitCallback = jklfq;
		}
		if (bvilq.eiuho(waitCallback) ? true : false)
		{
			return;
		}
		throw new InvalidOperationException("Unable to queue user work item.");
	}

	private void yjzda(byte[] p0, int p1, int p2)
	{
		nvbfm nvbfm = new nvbfm(p0, p1, p2, jsixd);
		string iivww;
		if ((iivww = nvbfm.iivww) != null && 0 == 0 && (iivww == "exit-status" || iivww == "exit-signal"))
		{
			if (puwpg == null || 1 == 0)
			{
				puwpg = new SshChannelExitStatus();
			}
			nvbfm.ohtwy(puwpg);
		}
		else if ((!rgxpe || 1 == 0) && nvbfm.fsffd && 0 == 0)
		{
			pgegi(new sybiu(xbrcx.vqsre, lszzf));
		}
	}

	private static string ipivw(uint p0)
	{
		return p0 switch
		{
			1u => "administratively prohibited", 
			2u => "connect failed", 
			3u => "unknown channel type", 
			4u => "resource shortage", 
			_ => "unknown reason", 
		};
	}

	internal yvedn oassp(yxshh p0)
	{
		lszzf = p0.iixpk;
		kukdy = p0.fgjcv;
		yevvp = (int)Math.Min((uint)fuitr, p0.efdcm);
		xocaz = SshChannelState.Connected;
		return new yvedn(lszzf, jqppx, (uint)ffrtv, (uint)yevvp);
	}

	internal void zwpbk()
	{
		lock (oeiuo)
		{
			if ((!ekvtf || 1 == 0) && !rgxpe)
			{
				ekvtf = true;
				afihi.adxeo(new sybiu(xbrcx.nvcjs, lszzf));
			}
		}
	}

	private void xcjzl(byte[] p0, int p1, int p2)
	{
		switch (p0[p1])
		{
		case 92:
		{
			jgcsx jgcsx = new jgcsx(p0, p1, p2, jsixd);
			afihi.ubjbk(this);
			xocaz = SshChannelState.Closed;
			string text = ipivw(jgcsx.qbavz);
			string text2 = jgcsx.aspgd;
			if (text2.Length > 0)
			{
				text2 = text2[0].ToString().ToUpper(CultureInfo.InvariantCulture).ToString() + text2.Substring(1);
				text2 = text2.TrimEnd('.');
			}
			wwnzx = new SshException(SshExceptionStatus.OperationFailure, brgjd.edcru("Cannot open channel; {0}. {1}.", text, text2));
			break;
		}
		case 91:
		{
			yvedn yvedn = new yvedn(p0, p1, p2, jsixd);
			lszzf = yvedn.xiviv;
			kukdy = yvedn.bsbbm;
			yevvp = Math.Min(fuitr, yvedn.upnin);
			xocaz = SshChannelState.Connected;
			break;
		}
		default:
			afihi.ubjbk(this);
			xocaz = SshChannelState.Closed;
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unsupported packet {0}.", p0[p1]));
		}
	}

	private bool hcuxv(out int p0, object p1)
	{
		if ((rgxpe ? true : false) || xocaz == SshChannelState.Closed)
		{
			p0 = -1;
		}
		else
		{
			p0 = wwnjk.jgwnt;
		}
		return p0 != 0;
	}

	internal int pbbvq(int p0)
	{
		try
		{
			return afihi.ryvxm<int, object>(hcuxv, p0, SshSession.gvyyw.ynwhp | SshSession.gvyyw.zcykt, null, 0, -1);
		}
		finally
		{
			if (afihi.State == SshState.Closed)
			{
				xocaz = SshChannelState.Closed;
			}
		}
	}

	private T kjbel<T, S>(SshSession.nivbm<T, S> p0, S p1)
	{
		try
		{
			return afihi.lrcya(p0, p1);
		}
		finally
		{
			if (afihi.State == SshState.Closed)
			{
				xocaz = SshChannelState.Closed;
			}
		}
	}

	private void pgegi(mkuxt p0)
	{
		lock (oeiuo)
		{
			if (rgxpe && 0 == 0)
			{
				throw new SshException(SshExceptionStatus.OperationFailure, "The channel has been closed.");
			}
			afihi.adxeo(p0);
		}
	}

	private bool sonej(out lmmys p0, object p1)
	{
		p0 = vtbgu;
		if ((p0 == lmmys.inlxr || 1 == 0) && xocaz != SshChannelState.Closed)
		{
			return rgxpe;
		}
		return true;
	}

	private void fuxcc(nvbfm p0)
	{
		try
		{
			vtbgu = lmmys.inlxr;
			pgegi(p0);
			switch (kjbel<lmmys, object>(sonej, null))
			{
			case lmmys.inlxr:
				throw new SshException(SshExceptionStatus.Timeout, "The operation was not completed within the specified time limit.");
			case lmmys.hxrgw:
				return;
			}
			throw new SshException(SshExceptionStatus.OperationFailure, brgjd.edcru("The '{0}' request has failed.", p0.iivww));
		}
		catch (Exception p1)
		{
			afihi.oftwj(p1);
			throw;
		}
	}

	public void RequestPseudoTerminal(string terminal, int width, int height)
	{
		dqfqx(terminal, width, height, null);
	}

	internal void dqfqx(string p0, int p1, int p2, bool? p3)
	{
		afihi.cnfnb(LogLevel.Debug, "Requesting pseudoterminal '{0}' ({1}x{2}).", p0, p1, p2);
		string text = string.Empty;
		if (p3.HasValue && 0 == 0)
		{
			text = text + '5' + ((p3.Value ? true : false) ? "\0\0\0\u0001" : "\0\0\0\0");
		}
		if (text.Length > 0)
		{
			text += '\0';
		}
		fuxcc(new nvbfm(lszzf, "pty-req", true, EncodingTools.ASCII, p0, (uint)p1, (uint)p2, (uint)(p1 * 8), (uint)(p2 * 16), text));
		spyep = p1;
		liawc = p2;
	}

	public void PassEnvironmentVariable(string name, string value)
	{
		afihi.cnfnb(LogLevel.Debug, "Setting environment variable '{0}'.", name);
		fuxcc(new nvbfm(lszzf, "env", true, EncodingTools.ASCII, name, value));
	}

	public void RequestPseudoTerminal()
	{
		RequestPseudoTerminal("vt100", 80, 25);
	}

	public void SetTerminalSize(int width, int height)
	{
		try
		{
			afihi.cnfnb(LogLevel.Debug, "Setting terminal size {0}x{1}.", width, height);
			pgegi(new nvbfm(lszzf, "window-change", false, EncodingTools.ASCII, (uint)width, (uint)height, (uint)(width * 8), (uint)(height * 16)));
			spyep = width;
			liawc = height;
		}
		catch (Exception p)
		{
			afihi.oftwj(p);
			throw;
		}
	}

	public void SendEof()
	{
		try
		{
			lock (oeiuo)
			{
				if ((!ekvtf || 1 == 0) && !rgxpe)
				{
					afihi.iejdf(LogLevel.Debug, "Sending EOF.");
					zwpbk();
				}
			}
		}
		catch (Exception p)
		{
			afihi.oftwj(p);
			throw;
		}
	}

	public void SendBreak(int breakLength)
	{
		try
		{
			afihi.cnfnb(LogLevel.Debug, "Requesting break ({0}ms).", breakLength);
			pgegi(new nvbfm(lszzf, "break", false, EncodingTools.ASCII, (uint)breakLength));
		}
		catch (Exception p)
		{
			afihi.oftwj(p);
			throw;
		}
	}

	public void RequestShell()
	{
		afihi.iejdf(LogLevel.Debug, "Requesting shell.");
		fuxcc(new nvbfm(lszzf, "shell", true, EncodingTools.ASCII));
	}

	public void RequestExec(string command)
	{
		bygni(command, afihi.Encoding);
	}

	internal void bygni(string p0, Encoding p1)
	{
		afihi.cnfnb(LogLevel.Debug, "Executing command '{0}'.", p0);
		fuxcc(new nvbfm(lszzf, "exec", true, p1, p0));
	}

	public void RequestSubsystem(string subsystem)
	{
		afihi.cnfnb(LogLevel.Debug, "Requesting subsystem '{0}'.", subsystem);
		fuxcc(new nvbfm(lszzf, "subsystem", true, EncodingTools.ASCII, subsystem));
	}

	public void Shutdown()
	{
		try
		{
			bool flag;
			bool flag2;
			lock (oeiuo)
			{
				if (xocaz == SshChannelState.Closed)
				{
					return;
				}
				flag = ekvtf;
				flag2 = rgxpe;
				calrr = true;
				ekvtf = true;
				if (afihi.lfvnz() && 0 == 0)
				{
					int num = 16777216;
					lock (cjxro)
					{
						ffrtv += num;
					}
					pgegi(new pfcna(lszzf, (uint)num));
				}
				rgxpe = true;
			}
			if (!flag2 || 1 == 0)
			{
				if (!flag || 1 == 0)
				{
					afihi.adxeo(new sybiu(xbrcx.nvcjs, lszzf));
				}
				afihi.adxeo(new sybiu(xbrcx.evspj, lszzf));
			}
		}
		catch (Exception p)
		{
			afihi.woqmx(LogLevel.Info, p);
		}
	}

	private bool frvns(out object p0, object p1)
	{
		p0 = null;
		return xocaz == SshChannelState.Closed;
	}

	public void Close()
	{
		Shutdown();
		afihi.ryvxm<object, object>(frvns, Math.Max(afihi.Timeout, 10000), SshSession.gvyyw.ynwhp, null, null, null);
	}

	public ISocket ToSocket()
	{
		return new qseon(this);
	}

	public int GetAvailable()
	{
		int num = oalzj(0, SocketSelectMode.SelectRead);
		if (num >= 0)
		{
			return num;
		}
		return 0;
	}

	private int oalzj(int p0, SocketSelectMode p1)
	{
		try
		{
			int jgwnt = wwnjk.jgwnt;
			if (jgwnt > 0)
			{
				return jgwnt;
			}
			return pbbvq(p0 / 1000);
		}
		catch (Exception p2)
		{
			afihi.oftwj(p2);
			throw;
		}
	}

	public bool Poll(int microSeconds, SocketSelectMode mode)
	{
		int num = oalzj(microSeconds, mode);
		return num != 0;
	}

	private bool doajm()
	{
		int num = wwnjk.llzpe / 4;
		if (ffrtv < num)
		{
			return wwnjk.jgwnt < num;
		}
		return false;
	}

	private void ognwh()
	{
		int num = 0;
		int num2 = 0;
		lock (cjxro)
		{
			if (doajm() && 0 == 0)
			{
				num2 = wwnjk.tvcie - ffrtv;
				ffrtv += num2;
				num = ffrtv;
			}
		}
		if (num2 <= 0)
		{
			return;
		}
		afihi.cnfnb(LogLevel.Debug, "Adjusted local receive window size: {0} -> {1}.", num - num2, num);
		lock (oeiuo)
		{
			if (!rgxpe)
			{
				pgegi(new pfcna(lszzf, (uint)num2));
			}
		}
	}

	private bool aiaeu(out object p0, object p1)
	{
		p0 = null;
		if ((rgxpe ? true : false) || xocaz == SshChannelState.Closed)
		{
			return true;
		}
		lock (cjxro)
		{
			if (doajm() && 0 == 0)
			{
				return true;
			}
		}
		if (wwnjk.jgwnt == 0 || 1 == 0)
		{
			return false;
		}
		return true;
	}

	public int Receive(byte[] buffer, int offset, int count)
	{
		try
		{
			return fvsbx(buffer, offset, count);
		}
		catch (Exception p)
		{
			afihi.oftwj(p);
			throw;
		}
	}

	internal int fvsbx(byte[] p0, int p1, int p2)
	{
		switch (xocaz)
		{
		default:
			throw new SshException(SshExceptionStatus.OperationFailure, "The channel is not connected.");
		case SshChannelState.Connected:
		case SshChannelState.Closed:
			if (p2 == 0 || 1 == 0)
			{
				return 0;
			}
			if (calrr && 0 == 0)
			{
				return 0;
			}
			lock (gzdke)
			{
				int num = wwnjk.kknzz(p0, p1, p2);
				p1 += num;
				p2 -= num;
				if (num == 0 || 1 == 0)
				{
					kjbel<object, object>(aiaeu, null);
					int num2 = wwnjk.kknzz(p0, p1, p2);
					if ((num2 == 0 || 1 == 0) && xocaz == SshChannelState.Closed)
					{
						calrr = true;
					}
					num += num2;
					p1 += num2;
					p2 -= num2;
				}
				ognwh();
				return num;
			}
		}
	}

	private bool kcjnr(out object p0, int p1)
	{
		p0 = null;
		if ((rgxpe ? true : false) || xocaz == SshChannelState.Closed)
		{
			return true;
		}
		lock (glgbc)
		{
			if (kukdy < p1)
			{
				return false;
			}
		}
		return true;
	}

	public int Send(byte[] buffer, int offset, int count)
	{
		try
		{
			return sdksb(buffer, offset, count);
		}
		catch (Exception p)
		{
			afihi.oftwj(p);
			throw;
		}
	}

	internal int sdksb(byte[] p0, int p1, int p2)
	{
		switch (xocaz)
		{
		case SshChannelState.Closed:
			throw new SshException(SshExceptionStatus.OperationFailure, "The channel has been closed.");
		default:
			throw new SshException(SshExceptionStatus.OperationFailure, "The channel is not connected.");
		case SshChannelState.Connected:
		{
			if (p2 == 0 || 1 == 0)
			{
				if (!afihi.IsConnected || 1 == 0)
				{
					throw new SshException(SshExceptionStatus.OperationFailure, "The channel has been closed.");
				}
				return 0;
			}
			int result = p2;
			while (p2 > 0)
			{
				uint num;
				lock (glgbc)
				{
					num = kukdy;
				}
				if (num == 0 || 1 == 0)
				{
					afihi.iejdf(LogLevel.Debug, "Remote receive window is full, waiting for adjustment.");
					kjbel<object, int>(kcjnr, 1);
					lock (glgbc)
					{
						num = kukdy;
					}
				}
				int val = Math.Min(p2, yevvp);
				val = (int)Math.Min((uint)val, num);
				try
				{
					lock (oeiuo)
					{
						if ((rgxpe ? true : false) || ekvtf)
						{
							throw new SshException(SshExceptionStatus.OperationFailure, "The channel has been closed.");
						}
						afihi.adxeo(new xwejf(lszzf, p0, p1, val));
					}
				}
				catch (SshException)
				{
					if (afihi.State == SshState.Closed)
					{
						xocaz = SshChannelState.Closed;
					}
					throw;
				}
				lock (glgbc)
				{
					kukdy -= (uint)val;
				}
				p1 += val;
				p2 -= val;
			}
			return result;
		}
		}
	}

	public override string ToString()
	{
		return jqppx.ToString();
	}

	public void Dispose()
	{
		Shutdown();
		GC.SuppressFinalize(this);
	}

	private void jklfq(object p0)
	{
		fvsyq();
	}
}
