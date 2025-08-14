using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Rebex;

namespace onrkn;

internal class thths
{
	private readonly string ykusb;

	private readonly pjyrs xtdth;

	private readonly long? noiqg;

	private readonly hspjp tnsni;

	private readonly wbvsh quqrl;

	private static readonly Regex opipd = new Regex("^HTTP/(1\\.[01]) (.*)", RegexOptions.Compiled);

	private fklrq rcspv;

	private bool iwqvd;

	private Version etpyg;

	private Action<nymgm> zfdir;

	private Func<ujepc, Exception> xudbd;

	public string eelqe => ykusb;

	public pyuak yddmu
	{
		get
		{
			pyuak pyuak2 = (pyuak)(ykusb[0] - 48);
			if (pyuak2 < pyuak.ahxes || pyuak2 > pyuak.jojyr)
			{
				pyuak2 = pyuak.ramjd;
			}
			return pyuak2;
		}
	}

	public HttpStatusCode xgkmt
	{
		get
		{
			if (ykusb.Length < 3)
			{
				return (HttpStatusCode)0;
			}
			if (!dahxy.crqjb(ykusb.Substring(0, 3), out var p) || 1 == 0)
			{
				return (HttpStatusCode)0;
			}
			return (HttpStatusCode)p;
		}
	}

	public string vbeuo
	{
		get
		{
			if (ykusb.Length < 5)
			{
				return "";
			}
			return ykusb.Substring(4);
		}
	}

	public fklrq irbum
	{
		get
		{
			return rcspv;
		}
		private set
		{
			rcspv = value;
		}
	}

	public pjyrs virwn => xtdth;

	public long belbk => noiqg ?? (-1);

	public string zgbyk
	{
		get
		{
			object obj = xtdth["Content-Type"];
			if (obj == null || 1 == 0)
			{
				obj = "text/html";
			}
			return (string)obj;
		}
	}

	internal bool qvnsr
	{
		get
		{
			return iwqvd;
		}
		private set
		{
			iwqvd = value;
		}
	}

	public Version fiqxe
	{
		get
		{
			return etpyg;
		}
		private set
		{
			etpyg = value;
		}
	}

	public Action<nymgm> tbxqq
	{
		get
		{
			return zfdir;
		}
		set
		{
			zfdir = value;
		}
	}

	public Func<ujepc, Exception> ruquh
	{
		get
		{
			return xudbd;
		}
		set
		{
			xudbd = value;
		}
	}

	internal static thths nfbia(nymgm p0, udlmn p1)
	{
		string text = p0.ReadLine();
		if (text == null || 1 == 0)
		{
			return null;
		}
		return new thths(p0, p1, text);
	}

	private thths(nymgm reader, udlmn client, string protocolHeader)
	{
		Func<Exception, Exception> func = null;
		base._002Ector();
		Match match = opipd.Match(protocolHeader);
		if (!match.Success || 1 == 0)
		{
			client.iptfx(LogLevel.Error, "HTTP", "Received invalid response: {0}.", protocolHeader);
			throw new ujepc("Invalid protocol.", ezmya.zmpaw);
		}
		fiqxe = new Version(match.Groups[1].Value);
		ykusb = match.Groups[2].Value;
		client.iptfx(LogLevel.Info, "HTTP", "Received response: {0}.", ykusb);
		if (ykusb.Length < 3)
		{
			throw new ujepc("Invalid response status code.", ezmya.zmpaw);
		}
		int num = 0;
		xtdth = new pjyrs();
		while (true)
		{
			string text = reader.ReadLine();
			if (string.IsNullOrEmpty(text) ? true : false)
			{
				break;
			}
			int num2 = text.IndexOf(':');
			if (num2 > 0)
			{
				string text2 = text.Substring(0, num2).Trim();
				string p = text.Substring(num2 + 1).Trim();
				if (text2.Length > 0)
				{
					xtdth.imhki(text2, p);
				}
			}
			num++;
		}
		client.iptfx(LogLevel.Debug, "HTTP", "Received {0} headers.", num);
		if (ykusb.StartsWith("100 ") || ykusb.StartsWith("101 "))
		{
			return;
		}
		if (ykusb.StartsWith("401 ") && 0 == 0)
		{
			fklrq fklrq2 = fklrq.uzjbr;
			IEnumerator<string> enumerator = xtdth.llppa("WWW-Authenticate").GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					string current = enumerator.Current;
					if (current.StartsWith("Basic", StringComparison.OrdinalIgnoreCase) && 0 == 0)
					{
						fklrq2 |= fklrq.okxlf;
					}
					else if (current.StartsWith("Digest", StringComparison.OrdinalIgnoreCase) && 0 == 0)
					{
						fklrq2 |= fklrq.rafbm;
					}
					else if (current.StartsWith("NTLM", StringComparison.OrdinalIgnoreCase) && 0 == 0)
					{
						fklrq2 |= fklrq.cepgj;
					}
					else if (!dahxy.hdfhq || 1 == 0)
					{
						if (current.StartsWith("Negotiate", StringComparison.OrdinalIgnoreCase) && 0 == 0)
						{
							fklrq2 |= fklrq.sjjmf;
						}
						else if (current.StartsWith("Kerberos", StringComparison.OrdinalIgnoreCase) && 0 == 0)
						{
							fklrq2 |= fklrq.cznae;
						}
					}
				}
			}
			finally
			{
				if (enumerator != null && 0 == 0)
				{
					enumerator.Dispose();
				}
			}
			irbum = fklrq2;
		}
		string text3 = xtdth["Content-Length"];
		long? num3 = null;
		if (text3 != null && 0 == 0)
		{
			num3 = long.Parse(text3, NumberStyles.None, CultureInfo.InvariantCulture);
			client.iptfx(LogLevel.Debug, "HTTP", "Response Content-Length: {0} bytes.", text3);
		}
		else
		{
			client.iptfx(LogLevel.Debug, "HTTP", "Response Content-Length not specified.");
		}
		string text4 = xtdth["Connection"];
		bool flag;
		if (text4 == null || 1 == 0)
		{
			flag = fiqxe >= pjyrs.dkkoe;
			client.iptfx(LogLevel.Debug, "HTTP", "Response Connection not specified; using '{0}'.", (flag ? true : false) ? "keep-alive" : "close");
		}
		else
		{
			flag = string.Equals(text4, "keep-alive", StringComparison.OrdinalIgnoreCase);
			client.iptfx(LogLevel.Debug, "HTTP", "Response Connection: {0}.", text4);
		}
		string text5 = xtdth["Content-Encoding"];
		if (text5 == null || 1 == 0)
		{
			client.iptfx(LogLevel.Debug, "HTTP", "Response Content-Encoding not specified.");
		}
		else
		{
			client.iptfx(LogLevel.Debug, "HTTP", "Response Content-Encoding: {0}.", text5);
		}
		if (client.tdnkk == "HEAD" || (ykusb.StartsWith("1") ? true : false) || (ykusb.StartsWith("204") ? true : false) || ykusb.StartsWith("304"))
		{
			tnsni = new hspjp(reader, chunked: false, "", 0L, flag, client, this);
		}
		else
		{
			bool flag2 = false;
			long? length = num3;
			string text6 = xtdth["Transfer-Encoding"];
			if (text6 != null && 0 == 0)
			{
				client.iptfx(LogLevel.Debug, "HTTP", "Response Transfer-Encoding: {0}.", text6);
				if (string.Equals(text6, "chunked", StringComparison.OrdinalIgnoreCase) && 0 == 0)
				{
					flag2 = true;
				}
				if (text6.Length > 0 && length.HasValue && 0 == 0)
				{
					length = null;
					client.iptfx(LogLevel.Debug, "HTTP", "Ignoring Content-Length because Transfer-Encoding was specified.");
				}
			}
			else
			{
				client.iptfx(LogLevel.Debug, "HTTP", "Response Transfer-Encoding not specified.");
			}
			if ((!length.HasValue || 1 == 0) && (!flag2 || 1 == 0) && flag && 0 == 0)
			{
				throw new ujepc("Insufficient response.", ezmya.zmpaw);
			}
			tnsni = new hspjp(reader, flag2, text5, length, flag, client, this);
		}
		noiqg = ((tnsni.rkwcn == DecompressionMethods.None) ? num3 : ((long?)null));
		qvnsr = flag;
		hspjp inner = tnsni;
		znuay pqqdp = client.pqqdp;
		if (func == null || 1 == 0)
		{
			func = wyjue;
		}
		quqrl = new wbvsh(inner, pqqdp, func);
	}

	internal void fract()
	{
		if (tnsni != null && 0 == 0)
		{
			tnsni.ncycz();
		}
	}

	public Stream vhkfm()
	{
		Stream result = ((tnsni != null && 0 == 0 && (tnsni.CanRead ? true : false)) ? quqrl : Stream.Null);
		if (quqrl != null && 0 == 0 && quqrl.prsgx && 0 == 0)
		{
			throw new ujepc("Operation was canceled.", ezmya.ydksh, this);
		}
		return result;
	}

	public void jpxci()
	{
		if (tnsni != null && 0 == 0)
		{
			tnsni.Close();
		}
	}

	internal void cqvrm(nymgm p0)
	{
		Action<nymgm> action = tbxqq;
		if (action != null && 0 == 0)
		{
			action(p0);
		}
	}

	internal Exception phrhc(ujepc p0)
	{
		Func<ujepc, Exception> func = ruquh;
		if (func != null && 0 == 0)
		{
			return func(p0);
		}
		return p0;
	}

	private Exception wyjue(Exception p0)
	{
		return phrhc(new ujepc("Operation was canceled.", ezmya.ydksh, this, p0));
	}
}
