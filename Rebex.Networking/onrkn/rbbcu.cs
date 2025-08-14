using System;
using Rebex.Net;

namespace onrkn;

internal class rbbcu : udlmn
{
	private readonly ISocketFactory oqjac;

	private phvuu uqahs;

	private TlsCipher qscpt;

	private TlsParameters zoouq;

	public TlsParameters floak
	{
		get
		{
			return zoouq;
		}
		set
		{
			zoouq = value;
		}
	}

	public TlsSocket kwidd => uqahs as TlsSocket;

	public TlsCipher wsksi => qscpt;

	private static Uri mmjqh(string p0, int p1, bool p2)
	{
		if (string.IsNullOrEmpty(p0) && 0 == 0)
		{
			throw new ArgumentException("Hostname cannot be empty.", "host");
		}
		if (p1 <= 0 || p1 > 65535)
		{
			throw hifyx.nztrs("port", p1, "Port is out of range of valid values.");
		}
		try
		{
			return new Uri(brgjd.edcru("http{0}://{1}:{2}/", (p2 ? true : false) ? "s" : "", p0, p1));
		}
		catch (UriFormatException)
		{
			throw new ArgumentException("Hostname is invalid.", "host");
		}
	}

	public rbbcu(string host, int port, bool secure, ISocketFactory socketFactory)
		: base(mmjqh(host, port, secure))
	{
		if (socketFactory == null || 1 == 0)
		{
			throw new ArgumentNullException("socketFactory");
		}
		oqjac = todgf.njpxf(socketFactory, base.elpzg);
	}

	protected virtual TlsSocket dogje(ISocket p0)
	{
		TlsSocket tlsSocket = TlsSocket.xxegh(p0);
		tlsSocket.Timeout = base.pjvho;
		tlsSocket.LogWriter = base.elpzg;
		tlsSocket.Parameters = floak;
		tlsSocket.Negotiate();
		return tlsSocket;
	}

	protected override phvuu dkczn(string p0, int p1, bool p2)
	{
		ISocket socket = oqjac.CreateSocket();
		socket.Timeout = base.pjvho;
		socket.Connect(p0, p1);
		if (p2 && 0 == 0)
		{
			TlsSocket tlsSocket = dogje(socket);
			socket = tlsSocket;
			uqahs = tlsSocket;
			qscpt = tlsSocket.Cipher;
		}
		else
		{
			uqahs = null;
			qscpt = null;
		}
		phvuu phvuu2 = socket as phvuu;
		if (phvuu2 == null || 1 == 0)
		{
			phvuu2 = (uqahs = new raqrd(socket));
		}
		return phvuu2;
	}

	private static bool fjaln(TlsException p0)
	{
		zppmb rkbch = p0.rkbch;
		if (rkbch != null && 0 == 0)
		{
			switch ((mjddr)rkbch.jmwmm)
			{
			case mjddr.fvtwt:
			case mjddr.zfebd:
			case mjddr.wskoy:
			case mjddr.cyvqp:
			case mjddr.vyvjd:
			case mjddr.kxgat:
				return true;
			}
		}
		return false;
	}

	public override bool utzrt(Exception p0)
	{
		if (p0 is TlsException p1 && 0 == 0)
		{
			return !fjaln(p1);
		}
		return base.utzrt(p0);
	}

	private static ezmya iqwwx(ProxySocketExceptionStatus p0)
	{
		switch (p0)
		{
		case ProxySocketExceptionStatus.ConnectFailure:
			return ezmya.mgopu;
		case ProxySocketExceptionStatus.ConnectionClosed:
			return ezmya.bbuiy;
		case ProxySocketExceptionStatus.NameResolutionFailure:
			return ezmya.tkmqc;
		case ProxySocketExceptionStatus.ProxyNameResolutionFailure:
			return ezmya.kmpus;
		case ProxySocketExceptionStatus.ReceiveFailure:
			return ezmya.yhvcm;
		case ProxySocketExceptionStatus.ServerProtocolViolation:
			return ezmya.zmpaw;
		case ProxySocketExceptionStatus.SendRetryTimeout:
		case ProxySocketExceptionStatus.Timeout:
			return ezmya.qrtnk;
		default:
			return ezmya.mmsdg;
		}
	}

	protected override ujepc qqrys(string p0, ezmya p1, Exception p2)
	{
		if (p2 is ProxySocketException ex && 0 == 0)
		{
			p1 = iqwwx(ex.Status);
			return new ujepc(p2.Message, p1, p2);
		}
		if (p2 is TlsException p3 && 0 == 0)
		{
			p1 = ((fjaln(p3) ? true : false) ? ezmya.zoxkg : ezmya.orpxf);
			return new ujepc(p2.Message, p1, p2);
		}
		return base.qqrys(p0, p1, p2);
	}

	public mggni tiiuh()
	{
		nymgm nymgm2 = lbevt();
		ArraySegment<byte> p;
		phvuu phvuu2 = nymgm2.qgyvx(out p);
		ISocket socket = phvuu2 as ISocket;
		if (socket == null || 1 == 0)
		{
			if (!(phvuu2 is raqrd raqrd2))
			{
				throw new NotSupportedException("This socket reader cannot be converted to a channel.");
			}
			socket = raqrd2.muiaz();
		}
		mggni inner = new qkfas(socket);
		return new bbvor(inner, p);
	}
}
