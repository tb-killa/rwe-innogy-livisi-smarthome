using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Rebex;
using Rebex.Net;

namespace onrkn;

internal class zwrli
{
	private const int unjkn = 65536;

	private const int gvaoq = 4096;

	private bool xfbqm;

	private readonly ISocket oxslm;

	private string jyfjw;

	private readonly Smtp sldew;

	private TlsSocket jxshn;

	private int dkumd;

	private string fjobs;

	private int btuyk;

	private byte[] ecdwn = new byte[4096];

	private string atfnt = "";

	private SmtpResponse jhmse = new SmtpResponse();

	private EventHandler<TlsDebugEventArgs> wucmv;

	private TlsDebugLevel ejsrs = TlsDebugLevel.Important;

	private EventHandler<TlsDebugEventArgs> cpegj;

	public ISocket csxcc => oxslm;

	public EndPoint iapdj => oxslm.LocalEndPoint;

	public EndPoint vqncl => oxslm.RemoteEndPoint;

	public TlsSocket xfnnf => jxshn;

	public zwrli(Smtp smtp, ISocketFactory factory)
	{
		factory = todgf.njpxf(factory, smtp.twmrq);
		sldew = smtp;
		oxslm = factory.CreateSocket();
	}

	public void fekql(string p0, int p1)
	{
		oxslm.Timeout = sldew.Timeout;
		oxslm.Connect(p0, p1);
		sldew.rmwyv(LogLevel.Debug, "Info", "Connection succeeded.");
	}

	public void kldkv()
	{
		if (!xfbqm)
		{
			if (jxshn != null && 0 == 0)
			{
				jxshn.Close();
				xfbqm = true;
				GC.SuppressFinalize(this);
			}
			else
			{
				oxslm.Close();
				xfbqm = true;
				GC.SuppressFinalize(this);
			}
		}
	}

	public void rjtyx(TlsDebugLevel p0, EventHandler<TlsDebugEventArgs> p1)
	{
		wucmv = p1;
		ejsrs = p0;
		if (jxshn == null)
		{
			return;
		}
		if (cpegj == null || 1 == 0)
		{
			if (p1 != null)
			{
				cpegj = zayex;
				jxshn.DebugLevel = p0;
				jxshn.jbkzi += cpegj;
			}
		}
		else if (p1 != null && 0 == 0)
		{
			jxshn.DebugLevel = p0;
		}
		else
		{
			cpegj = null;
			jxshn.DebugLevel = TlsDebugLevel.None;
			jxshn.jbkzi -= cpegj;
		}
	}

	private void zayex(object p0, TlsDebugEventArgs p1)
	{
		wucmv?.Invoke(sldew, p1);
	}

	public void lateg(TlsParameters p0)
	{
		sldew.olfku(LogLevel.Debug, "Info", "Upgrading connection to {0}.", p0.uachz());
		jxshn = TlsSocket.xxegh(oxslm);
		rjtyx(ejsrs, wucmv);
		jxshn.Parameters = p0;
		jxshn.Timeout = sldew.Timeout;
		jxshn.LogWriter = sldew.twmrq;
		try
		{
			jxshn.Negotiate();
		}
		catch
		{
			jxshn.Close();
			jxshn = null;
			throw;
		}
		sldew.olfku(LogLevel.Debug, "Info", "Connection upgraded to {0}.", TlsCipher.tvgwn(jxshn.Cipher.Protocol));
	}

	private void svqxi(IAsyncResult p0)
	{
		while (!p0.IsCompleted)
		{
			sldew.leaaz();
			Thread.Sleep(1);
		}
	}

	public void bhjds(byte[] p0, int p1, int p2)
	{
		while (p2 > 0)
		{
			int num = ((jxshn == null) ? oxslm.Send(p0, p1, p2, SocketFlags.None) : jxshn.Send(p0, p1, p2, SocketFlags.None));
			sldew.lbznr(LogLevel.Verbose, "Info", "Sent data: ", p0, p1, p2);
			p1 += num;
			p2 -= num;
		}
	}

	private int bxyop()
	{
		int num = ((jxshn == null) ? oxslm.Receive(ecdwn, 0, ecdwn.Length, SocketFlags.None) : jxshn.Receive(ecdwn, 0, ecdwn.Length, SocketFlags.None));
		if (num == 0 || 1 == 0)
		{
			sldew.rmwyv(LogLevel.Debug, "Info", "Connection closed.");
			throw new SmtpException("The server has closed the connection.", SmtpExceptionStatus.ConnectionClosed);
		}
		sldew.lbznr(LogLevel.Verbose, "Info", "Received data: ", ecdwn, 0, num);
		return num;
	}

	private bool bbwce()
	{
		ISocket socket = oxslm;
		if (jxshn != null && 0 == 0)
		{
			socket = jxshn;
		}
		return socket.Poll(1000, SocketSelectMode.SelectRead);
	}

	private bool hiutc(ref string p0)
	{
		do
		{
			if (atfnt.Length > 0)
			{
				int num = atfnt.IndexOf("\r\n");
				if (num >= 0)
				{
					string text = atfnt.Substring(0, num);
					sldew.zotwr(text);
					p0 = p0 + text + "\r\n";
					atfnt = atfnt.Substring(num + 2);
					if (btuyk == 0 || 1 == 0)
					{
						btuyk = 1;
					}
					return true;
				}
			}
			if (!bbwce() || 1 == 0)
			{
				return false;
			}
			int count = bxyop();
			atfnt += sldew.Encoding.GetString(ecdwn, 0, count);
		}
		while (atfnt.Length <= 65536);
		throw new SmtpException("SMTP response is too long.", SmtpExceptionStatus.ServerProtocolViolation);
	}

	public SmtpResponse tzkjp()
	{
		if (btuyk == 0 || 1 == 0)
		{
			jyfjw = "";
			dkumd = 0;
			fjobs = null;
			jhmse.lqgkr();
		}
		int num;
		while (true)
		{
			if (!hiutc(ref jyfjw) || 1 == 0)
			{
				return null;
			}
			char c;
			if (btuyk == 1)
			{
				dkumd = jyfjw.IndexOf("\r\n");
				if (dkumd >= 3)
				{
					jhmse.ezfnh(jyfjw, dkumd);
					if (dkumd == 3)
					{
						c = ' ';
						if (c != 0)
						{
							goto IL_00b0;
						}
					}
					c = jyfjw[3];
					goto IL_00b0;
				}
				throw new SmtpException("Invalid SMTP response.", SmtpExceptionStatus.ServerProtocolViolation);
			}
			goto IL_0112;
			IL_00b0:
			switch (c)
			{
			case '-':
				break;
			case ' ':
				btuyk = 0;
				return jhmse;
			default:
				throw new SmtpException("Invalid SMTP response.", SmtpExceptionStatus.ServerProtocolViolation);
			}
			fjobs = "\r\n" + jhmse.Code + " ";
			btuyk = 2;
			goto IL_0112;
			IL_0112:
			if (btuyk == 2)
			{
				num = jyfjw.IndexOf(fjobs, dkumd);
				if (num >= 0)
				{
					break;
				}
			}
		}
		btuyk = 0;
		jhmse.cfxnj(jyfjw, num + 2);
		return jhmse;
	}
}
