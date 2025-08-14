using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Rebex;

namespace onrkn;

internal class trheg : IDisposable, bbwjd<Socket>
{
	private sealed class snfvo
	{
		public kylha ujiwk;

		public trheg bagji;

		public void tnfvl(exkzi p0)
		{
			if (p0.ijeei && 0 == 0)
			{
				Exception mnscz = p0.mnscz;
				Exception ex = ((mnscz is nagsk) ? mnscz.InnerException : mnscz);
				ex = ((ex is nagsk) ? ex.InnerException : ex);
				if (ex is pqotq && 0 == 0)
				{
					bagji.szxki.byfnx(LogLevel.Error, "SocksServer", "SOCKS5 session #{0} received invalid response: {1}", ujiwk.mkntd, ex.Message);
				}
				else
				{
					bool flag = false;
					tuuua zwvmm = bagji.kobou.zwvmm;
					if (zwvmm != null && 0 == 0)
					{
						flag = zwvmm(bagji.szxki, ujiwk.mkntd, ex);
					}
					if (!flag || 1 == 0)
					{
						bagji.szxki.byfnx(LogLevel.Error, "SocksServer", "SOCKS5 session #{0} failed: {1}", ujiwk.mkntd, mnscz);
					}
				}
				if (!(ex is ObjectDisposedException) || 1 == 0)
				{
					bagji.nbyiv(ex);
				}
			}
			lock (bagji.zmzaz)
			{
				ujiwk.Dispose();
				bagji.kxctd.Remove(ujiwk);
			}
		}
	}

	private static int wtxwi;

	private pffmm yguav;

	private readonly sjhqe szxki;

	private readonly List<kylha> kxctd = new List<kylha>();

	private readonly object zmzaz = new object();

	private int ezthm;

	private nlsqa onpoo;

	internal int aoral
	{
		get
		{
			return ezthm;
		}
		private set
		{
			ezthm = value;
		}
	}

	public nlsqa kobou
	{
		get
		{
			return onpoo;
		}
		private set
		{
			onpoo = value;
		}
	}

	public trheg(IPEndPoint clientListeningEndPoint)
		: this(new nlsqa(clientListeningEndPoint))
	{
	}

	public trheg(nlsqa options)
	{
		if (options == null || 1 == 0)
		{
			throw new ArgumentNullException("options");
		}
		kobou = options;
		aoral = Interlocked.Increment(ref wtxwi);
		szxki = options.newbe.ymjug(typeof(trheg), aoral);
		yguav = new pffmm(options.sdmcm);
		yguav.sypnm(this);
		szxki.byfnx(LogLevel.Debug, "SocksServer", "SOCKS5 server #{0} initialized.", aoral);
	}

	public void Dispose()
	{
		xnpvp();
		pffmm pffmm2 = Interlocked.Exchange(ref yguav, null);
		if (pffmm2 != null && 0 == 0)
		{
			pffmm2.Dispose();
		}
	}

	public void wjhjx()
	{
		yguav.hqejt();
		szxki.byfnx(LogLevel.Debug, "SocksServer", "SOCKS5 server #{0} listening at: {1}", aoral, kobou.sdmcm);
	}

	public void xnpvp()
	{
		yguav.jikiw();
		szxki.byfnx(LogLevel.Debug, "SocksServer", "SOCKS5 server #{0} stopped.", aoral);
		lock (zmzaz)
		{
			kylha[] array = kxctd.ToArray();
			int num = 0;
			if (num != 0)
			{
				goto IL_005f;
			}
			goto IL_0070;
			IL_005f:
			kylha kylha2 = array[num];
			kylha2.hqfdd();
			num++;
			goto IL_0070;
			IL_0070:
			if (num >= array.Length)
			{
				kxctd.Clear();
				return;
			}
			goto IL_005f;
		}
	}

	private void tutaw(Socket p0)
	{
		snfvo snfvo = new snfvo();
		snfvo.bagji = this;
		szxki.byfnx(LogLevel.Debug, "SocksServer", "SOCKS5 server #{0} accepted SOCKS5 session from: {1}", aoral, p0.RemoteEndPoint);
		snfvo.ujiwk = new kylha(p0, this);
		lock (zmzaz)
		{
			kxctd.Add(snfvo.ujiwk);
		}
		snfvo.ujiwk.gnenj().wszna(snfvo.tnfvl);
	}

	void bbwjd<Socket>.cvhgi(Socket p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in tutaw
		this.tutaw(p0);
	}

	private void nbyiv(Exception p0)
	{
		Action<Exception> wqmlk = kobou.wqmlk;
		if (wqmlk != null && 0 == 0)
		{
			wqmlk(p0);
		}
	}

	private void wothm(Exception p0)
	{
		szxki.byfnx(LogLevel.Error, "SocksServer", "SOCKS5 server #{0} error: {1}", aoral, p0);
		nbyiv(p0);
	}

	void bbwjd<Socket>.zvcvv(Exception p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in wothm
		this.wothm(p0);
	}

	private void fvpbi()
	{
	}

	void bbwjd<Socket>.suvgv()
	{
		//ILSpy generated this explicit interface implementation from .override directive in fvpbi
		this.fvpbi();
	}
}
