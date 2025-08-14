using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Rebex;
using Rebex.Net;
using Rebex.Security.Authentication;

namespace onrkn;

internal class rbuzv : gipfx
{
	private enum wcjco
	{
		pvmrq,
		bsjhx,
		lickp,
		iljgt
	}

	private enum zgyrv
	{
		oupgt,
		mewln,
		yirtx
	}

	private string hxhqd;

	private string jzapt;

	private string gglxz;

	private byte[] rvcfo = new byte[16];

	private wcjco zcyba;

	private IPEndPoint liloz;

	private SspiAuthentication cwzwz;

	public override EndPoint kkmbs(EndPoint p0)
	{
		gipfx.wqwvw(p0, out var p1, out var p2);
		hxhqd = "CONNECT";
		jzapt = p1 + ":" + p2;
		gglxz = hxhqd + " " + jzapt + " HTTP/1.1\r\nHost: " + jzapt + "\r\nConnection: Keep-Alive\r\n";
		if (!string.IsNullOrEmpty(zweft) || 1 == 0)
		{
			gglxz = gglxz + "User-Agent: " + zweft + "\r\n";
		}
		liloz = bgsra(edbsx, fdtan, ProxySocketExceptionStatus.ProxyNameResolutionFailure);
		zgyrv zgyrv = zgyrv.oupgt;
		while (true)
		{
			switch (zgyrv)
			{
			case zgyrv.oupgt:
				zgyrv = ddndg();
				break;
			case zgyrv.mewln:
				zgyrv = fhonr();
				break;
			default:
				return p0;
			}
		}
	}

	private zgyrv ddndg()
	{
		npqbr(liloz);
		string text = gglxz;
		if (wbsqt == ProxyAuthentication.Ntlm)
		{
			if (!SspiAuthentication.IsSupported("NTLM") || 1 == 0)
			{
				throw new PlatformNotSupportedException("NTLM authentication is not supported on this platform.");
			}
			cwzwz = new SspiAuthentication("NTLM", SspiDataRepresentation.Native, edbsx, (SspiRequirements)0, meang, gobbp, tyocn);
			bool complete;
			byte[] nextMessage = cwzwz.GetNextMessage(null, out complete);
			string text2 = Convert.ToBase64String(nextMessage, 0, nextMessage.Length);
			text = text + "Proxy-Authorization: NTLM " + text2 + "\r\n";
			zcyba = wcjco.lickp;
		}
		else if (wbsqt == ProxyAuthentication.Digest)
		{
			if (zcyba == wcjco.pvmrq || 1 == 0)
			{
				zcyba = wcjco.iljgt;
			}
		}
		else
		{
			if (meang.Length > 0 && gobbp.Length > 0)
			{
				byte[] bytes = alsmq.GetBytes(meang + ":" + gobbp);
				text = text + "Proxy-Authorization: Basic " + Convert.ToBase64String(bytes, 0, bytes.Length) + "\r\n";
			}
			zcyba = wcjco.bsjhx;
		}
		text += "\r\n";
		byte[] bytes2 = alsmq.GetBytes(text);
		msrrm(bytes2);
		return zgyrv.mewln;
	}

	private MemoryStream dlqmy()
	{
		int num = 0;
		int num2 = 12;
		if (num2 == 0)
		{
			goto IL_000c;
		}
		goto IL_005a;
		IL_000c:
		int num3 = base.pnwnu.Receive(rvcfo, num, num2, SocketFlags.None);
		if (num3 == 0 || 1 == 0)
		{
			throw new ProxySocketException("Socket has been closed.", ProxySocketExceptionStatus.ConnectionClosed);
		}
		fpizj(LogLevel.Verbose, "Proxy", "Received data:", rvcfo, num, num3);
		num += num3;
		num2 -= num3;
		goto IL_005a;
		IL_005a:
		if (num2 == 0 || 1 == 0)
		{
			string text = alsmq.GetString(rvcfo, 0, 12);
			if (text.Substring(0, 5) != "HTTP/" && 0 == 0)
			{
				throw new ProxySocketException("Invalid HTTP response.", ProxySocketExceptionStatus.ServerProtocolViolation);
			}
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(rvcfo, 0, 12);
			return memoryStream;
		}
		goto IL_000c;
	}

	private zgyrv fhonr()
	{
		MemoryStream memoryStream = dlqmy();
		byte[] buffer;
		int count;
		do
		{
			count = base.pnwnu.Receive(rvcfo, 0, 1, SocketFlags.None);
			memoryStream.Write(rvcfo, 0, count);
			buffer = memoryStream.GetBuffer();
			count = (int)memoryStream.Length;
			if (count == 0 || 1 == 0)
			{
				fpizj(LogLevel.Verbose, "Proxy", "Receiving data:", buffer, 12, count - 12);
				hvcsu(LogLevel.Debug, "Proxy", "Connection closed.");
				throw new ProxySocketException("Invalid HTTP response.", ProxySocketExceptionStatus.ConnectionClosed);
			}
		}
		while (buffer[count - 4] != 13 || buffer[count - 3] != 10 || buffer[count - 2] != 13 || buffer[count - 1] != 10);
		fpizj(LogLevel.Verbose, "Proxy", "Received data:", buffer, 12, count - 12);
		string text = alsmq.GetString(buffer, 0, count);
		int num = text.IndexOf("\r\n", StringComparison.OrdinalIgnoreCase);
		if (num < 13)
		{
			throw new ProxySocketException("Invalid HTTP response.", ProxySocketExceptionStatus.ServerProtocolViolation);
		}
		if (text[9] == '4' || text[9] == '5')
		{
			string text2 = vjeqp(text, "Content-Length", p2: false);
			if (text2 != null && 0 == 0)
			{
				int p = -1;
				brgjd.bnrqx(text2, out p);
				if (p < 0 || p > 65536)
				{
					throw new ProxySocketException("Invalid HTTP response.", ProxySocketExceptionStatus.ProtocolError);
				}
				byte[] array = new byte[p];
				while (p > 0)
				{
					int num2 = base.pnwnu.Receive(array, 0, p, SocketFlags.None);
					if (num2 == 0 || 1 == 0)
					{
						throw new ProxySocketException("Invalid HTTP response.", ProxySocketExceptionStatus.ConnectionClosed);
					}
					fpizj(LogLevel.Verbose, "Proxy", "Receiving data:", array, 0, num2);
					p -= num2;
				}
			}
		}
		string a = vjeqp(text, "Connection", p2: false);
		bool flag = string.Equals(a, "close", StringComparison.OrdinalIgnoreCase);
		switch (zcyba)
		{
		case wcjco.lickp:
			if (text[9] != '2')
			{
				string text3 = text.Substring(9, 3);
				if (text3 != "407" && 0 == 0)
				{
					throw lfutl(text, num);
				}
				string text4 = "Proxy-Authenticate: ";
				int num3 = text.IndexOf(text4, StringComparison.OrdinalIgnoreCase);
				if (num3 < 0)
				{
					throw new ProxySocketException("Invalid HTTP response.", ProxySocketExceptionStatus.ProtocolError);
				}
				int num2 = text.IndexOf("NTLM", num3, StringComparison.OrdinalIgnoreCase);
				if (num2 < 0 || num2 > num3 + text4.Length + 10)
				{
					throw new ProxySocketException("Invalid HTTP response.", ProxySocketExceptionStatus.ProtocolError);
				}
				int num4 = text.IndexOf("\r\n", num2 + 5, StringComparison.OrdinalIgnoreCase);
				if (num4 < 0)
				{
					throw new ProxySocketException("Invalid HTTP response.", ProxySocketExceptionStatus.ProtocolError);
				}
				string s = text.Substring(num2 + 4, num4 - (num2 + 4));
				byte[] challenge = Convert.FromBase64String(s);
				bool complete;
				byte[] nextMessage = cwzwz.GetNextMessage(challenge, out complete);
				string text5 = Convert.ToBase64String(nextMessage, 0, nextMessage.Length);
				string text6 = gglxz;
				text6 = text6 + "Proxy-Authorization: NTLM " + text5 + "\r\n";
				text6 += "\r\n";
				zcyba = wcjco.bsjhx;
				byte[] bytes = alsmq.GetBytes(text6);
				msrrm(bytes);
				return zgyrv.mewln;
			}
			break;
		case wcjco.iljgt:
			if (text[9] != '2')
			{
				string s = vjeqp(text, "Proxy-Authenticate", p2: true);
				if (!s.StartsWith("Digest ", StringComparison.OrdinalIgnoreCase) || 1 == 0)
				{
					throw new ProxySocketException("Invalid HTTP response.", ProxySocketExceptionStatus.ProtocolError);
				}
				s = s.Substring(7);
				diwlk diwlk2 = new diwlk(s, "http", base64: false);
				string text7 = diwlk2.vhedm(meang, gobbp, hxhqd, jzapt, "uri", p5: true);
				gglxz = gglxz + "Proxy-Authorization: Digest " + text7 + "\r\n";
				zcyba = wcjco.bsjhx;
				if (flag && 0 == 0)
				{
					hvcsu(LogLevel.Debug, "Proxy", "Reconnecting to {0}:{1}.", liloz.Address, liloz.Port);
					return zgyrv.oupgt;
				}
				byte[] bytes = alsmq.GetBytes(gglxz);
				msrrm(bytes);
				return zgyrv.mewln;
			}
			break;
		default:
			if (text[9] != '2')
			{
				throw lfutl(text, num);
			}
			break;
		}
		memoryStream.Close();
		hvcsu(LogLevel.Debug, "Proxy", "Connection initialized successfully.");
		return zgyrv.yirtx;
	}

	private static string vjeqp(string p0, string p1, bool p2)
	{
		p1 += ": ";
		int num = p0.IndexOf(p1, StringComparison.OrdinalIgnoreCase);
		if (num < 0)
		{
			if (p2 && 0 == 0)
			{
				throw new ProxySocketException("Invalid HTTP response.", ProxySocketExceptionStatus.ProtocolError);
			}
			return null;
		}
		num += p1.Length;
		int num2 = p0.IndexOf("\r\n", num, StringComparison.OrdinalIgnoreCase);
		if (num2 < num)
		{
			throw new ProxySocketException("Invalid HTTP response.", ProxySocketExceptionStatus.ProtocolError);
		}
		return p0.Substring(num, num2 - num);
	}

	private static Exception lfutl(string p0, int p1)
	{
		string text = p0.Substring(9, 3);
		int errorCode;
		try
		{
			errorCode = int.Parse(text, CultureInfo.InvariantCulture);
		}
		catch
		{
			errorCode = 0;
		}
		return new ProxySocketException(brgjd.edcru("Error {1} returned by a HTTP proxy ({0}).", p0.Substring(13, p1 - 13).Trim(), text), ProxySocketExceptionStatus.ProtocolError, errorCode);
	}

	public override EndPoint zebbd(ProxySocket p0)
	{
		throw new NotSupportedException("This method is not supported by a HTTP proxy.");
	}

	public override EndPoint ofjnw()
	{
		throw new NotSupportedException("This method is not supported by a HTTP proxy.");
	}
}
