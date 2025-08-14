using System.Text;
using Rebex.Net;

namespace onrkn;

internal class ckzrf : mkuxt
{
	private byte[] swzfa;

	private string[] dvvxu;

	private string[] fsrsi;

	private bool rlstk;

	private string[] twikk;

	private string[] zsvnc;

	private string[] xxbop;

	private string[] wlquv;

	private string[] ylnoo;

	private string[] oewbi;

	private string[] qigog;

	private string[] tshkp;

	internal string[] fjrpn
	{
		get
		{
			return twikk;
		}
		private set
		{
			twikk = value;
		}
	}

	internal string[] kwdyx
	{
		get
		{
			return zsvnc;
		}
		private set
		{
			zsvnc = value;
		}
	}

	internal string[] wdccr
	{
		get
		{
			return xxbop;
		}
		private set
		{
			xxbop = value;
		}
	}

	internal string[] yunxz
	{
		get
		{
			return wlquv;
		}
		private set
		{
			wlquv = value;
		}
	}

	internal string[] hfzts
	{
		get
		{
			return ylnoo;
		}
		private set
		{
			ylnoo = value;
		}
	}

	internal string[] ipvna
	{
		get
		{
			return oewbi;
		}
		private set
		{
			oewbi = value;
		}
	}

	internal string[] cwmdd
	{
		get
		{
			return qigog;
		}
		private set
		{
			qigog = value;
		}
	}

	internal string[] jsjqy
	{
		get
		{
			return tshkp;
		}
		private set
		{
			tshkp = value;
		}
	}

	public bool xcdcy => rlstk;

	public bool oqcyi(ckzrf p0)
	{
		if (fjrpn.Length == 0 || false || p0.fjrpn.Length == 0 || 1 == 0)
		{
			return false;
		}
		if (kwdyx.Length == 0 || false || p0.kwdyx.Length == 0 || 1 == 0)
		{
			return false;
		}
		if (fjrpn[0] != p0.fjrpn[0] && 0 == 0)
		{
			return false;
		}
		if (kwdyx[0] != p0.kwdyx[0] && 0 == 0)
		{
			return false;
		}
		return true;
	}

	private static string bevxn(string[] p0, string[] p1)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0032;
		IL_0006:
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_000b;
		}
		goto IL_0028;
		IL_000b:
		if (p0[num] == p1[num2] && 0 == 0)
		{
			return p1[num2];
		}
		num2++;
		goto IL_0028;
		IL_0028:
		if (num2 < p1.Length)
		{
			goto IL_000b;
		}
		num++;
		goto IL_0032;
		IL_0032:
		if (num < p0.Length)
		{
			goto IL_0006;
		}
		return null;
	}

	public kumym dvufx(ckzrf p0, out SshKeyExchangeAlgorithm p1, out string p2, out SshHostKeyAlgorithm p3, out string p4)
	{
		p2 = bevxn(p0.fjrpn, fjrpn);
		p1 = SshCipher.xqnae(p2, out var p5, out var p6, out var p7, out var p8);
		p4 = bevxn(p0.kwdyx, kwdyx);
		p3 = SshCipher.uxprc(p4);
		if (p1 == SshKeyExchangeAlgorithm.None || false || p3 == SshHostKeyAlgorithm.None || 1 == 0)
		{
			return null;
		}
		return p6 switch
		{
			0 => new eduou(p4, p5), 
			-1 => new qcmxm(p4, p8, p5, p7), 
			_ => new xzewx(p6, p4, p5), 
		};
	}

	public ovpxz ixswi(ckzrf p0)
	{
		string alg = bevxn(p0.wdccr, wdccr);
		ovpxz ovpxz2 = new ovpxz(alg, encryptor: true);
		if (ovpxz2.hzzec == null || 1 == 0)
		{
			return null;
		}
		return ovpxz2;
	}

	public ovpxz rtpcs(ckzrf p0)
	{
		string alg = bevxn(p0.yunxz, yunxz);
		ovpxz ovpxz2 = new ovpxz(alg, encryptor: false);
		if (ovpxz2.hzzec == null || 1 == 0)
		{
			return null;
		}
		return ovpxz2;
	}

	public eswpb hijph(ckzrf p0)
	{
		string p1 = bevxn(p0.hfzts, hfzts);
		return eswpb.apzyo(p1);
	}

	public eswpb iwqiu(ckzrf p0)
	{
		string p1 = bevxn(p0.ipvna, ipvna);
		return eswpb.apzyo(p1);
	}

	public string onfqr(ckzrf p0)
	{
		string p1 = bevxn(p0.cwmdd, cwmdd);
		return xdkpm(p1);
	}

	public string zkxxn(ckzrf p0)
	{
		string p1 = bevxn(p0.jsjqy, jsjqy);
		return xdkpm(p1);
	}

	private static string xdkpm(string p0)
	{
		string text;
		if ((text = p0) != null && 0 == 0 && (text == "zlib" || text == "zlib@openssh.com"))
		{
			return p0;
		}
		return null;
	}

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 20);
		mkuxt.lcbhj(p0, swzfa, p2: true);
		mkuxt.ijaon(p0, fjrpn);
		mkuxt.ijaon(p0, kwdyx);
		mkuxt.ijaon(p0, wdccr);
		mkuxt.ijaon(p0, yunxz);
		mkuxt.ijaon(p0, hfzts);
		mkuxt.ijaon(p0, ipvna);
		mkuxt.ijaon(p0, cwmdd);
		mkuxt.ijaon(p0, jsjqy);
		mkuxt.ijaon(p0, dvvxu);
		mkuxt.ijaon(p0, fsrsi);
		mkuxt.duaqa(p0, rlstk);
		mkuxt.ebmel(p0, 0u);
	}

	public ckzrf(SshParameters parameters, string welcome)
	{
		swzfa = new byte[16];
		jtxhe.ubsib(swzfa, 0, swzfa.Length);
		kwdyx = parameters.chqyr(SshHostKeyAlgorithm.Any, SshHostKeyAlgorithm.Any);
		fjrpn = parameters.khess();
		wdccr = (yunxz = parameters.yhshu(welcome));
		hfzts = (ipvna = parameters.eolzs(welcome));
		cwmdd = (jsjqy = parameters.xczlj());
		dvvxu = (fsrsi = new string[0]);
	}

	public ckzrf(byte[] buffer, int offset, int count, Encoding encoding)
	{
		zyppx zyppx2 = new zyppx(buffer, offset, count, encoding);
		byte b = zyppx2.sfolp();
		if (b != 20)
		{
			throw new SshException(tcpjq.svqut, brgjd.edcru("Unexpected packet {0}.", b));
		}
		swzfa = zyppx2.jvszb(16);
		fjrpn = zyppx2.dxxld();
		kwdyx = zyppx2.dxxld();
		wdccr = zyppx2.dxxld();
		yunxz = zyppx2.dxxld();
		hfzts = zyppx2.dxxld();
		ipvna = zyppx2.dxxld();
		cwmdd = zyppx2.dxxld();
		jsjqy = zyppx2.dxxld();
		dvvxu = zyppx2.dxxld();
		fsrsi = zyppx2.dxxld();
		rlstk = zyppx2.qxurr();
		zyppx2.fiswn();
	}
}
