using System;
using System.Net.Sockets;
using Rebex.Net;

namespace onrkn;

internal class rdvtd : mwxgh
{
	private class krsdb : mwxgh
	{
		private const string wfhro = "SocketExtAvailableProvider";

		private readonly ISocketExt nwrqt;

		public string ukxui => "SocketExtAvailableProvider";

		public krsdb(ISocketExt socketExt)
		{
			if (socketExt == null || 1 == 0)
			{
				throw new ArgumentNullException("socketExt");
			}
			nwrqt = socketExt;
		}

		public njvzu<int> knwel()
		{
			return nwrqt.Available.xtyvd();
		}

		public static mwxgh jrkdz(ISocketExt p0)
		{
			return new krsdb(p0);
		}
	}

	private class dndou : mwxgh
	{
		private const string wxghq = "NetSocketAvailableProvider";

		private readonly Socket eegco;

		public string ukxui => "NetSocketAvailableProvider";

		public dndou(Socket _socket)
		{
			if (_socket == null || 1 == 0)
			{
				throw new ArgumentNullException("_socket");
			}
			eegco = _socket;
		}

		public njvzu<int> knwel()
		{
			return eegco.Available.xtyvd();
		}

		public static mwxgh nblwf(Socket p0)
		{
			return new dndou(p0);
		}
	}

	private sealed class dwdiq
	{
		public vrloh zlvoa;

		public apajk<mwxgh> zfpid()
		{
			return zlvoa.wbidm.ttrww(dndou.nblwf);
		}
	}

	private static Func<rdvtd, rdvtd> epxpi;

	private mwxgh uqtmv;

	private mwxgh visoq;

	private string chdqa = "DefaultAvailableProvider: Primary provider- {{0}} : Secondary provider: {{1}}";

	private static Func<rdvtd, rdvtd> fodge;

	private static Func<mwxgh> tzetm;

	private static Func<ISocket, ISocketExt> ojevr;

	private static Func<rdvtd, rdvtd> ovypj;

	public static Func<rdvtd, rdvtd> lwziq
	{
		get
		{
			return epxpi;
		}
		set
		{
			Func<rdvtd, rdvtd> func = value;
			if (func == null || 1 == 0)
			{
				if (fodge == null || 1 == 0)
				{
					fodge = ighni;
				}
				func = fodge;
			}
			epxpi = func;
		}
	}

	public virtual mwxgh ipebe
	{
		get
		{
			return uqtmv;
		}
		set
		{
			uqtmv = value;
		}
	}

	public virtual mwxgh uuqtt
	{
		get
		{
			return visoq;
		}
		set
		{
			visoq = value;
		}
	}

	public string ukxui => brgjd.edcru(chdqa, ipebe, uuqtt);

	public rdvtd()
	{
		uqtmv = hbhvp.onvsi;
		visoq = hbhvp.onvsi;
	}

	public rdvtd(mwxgh initialProvider)
	{
		if (initialProvider == null || 1 == 0)
		{
			throw new ArgumentNullException("initialProvider");
		}
		uqtmv = (visoq = initialProvider);
	}

	public virtual njvzu<int> knwel()
	{
		object onvsi = ipebe;
		if (onvsi == null || 1 == 0)
		{
			onvsi = uuqtt;
			if (onvsi == null || 1 == 0)
			{
				onvsi = hbhvp.onvsi;
			}
		}
		mwxgh mwxgh2 = (mwxgh)onvsi;
		return mwxgh2.knwel();
	}

	public static rdvtd qdrrq(mggni p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		mwxgh initialProvider = gmjha(p0);
		return lwziq(new rdvtd(initialProvider));
	}

	public static rdvtd omrjr(ISocket p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("socket");
		}
		if (p0 is ISocketExt p1 && 0 == 0)
		{
			return lwziq(new rdvtd(krsdb.jrkdz(p1)));
		}
		if (p0 is vrloh p2 && 0 == 0)
		{
			mwxgh initialProvider = lkeoo(p2);
			return lwziq(new rdvtd(initialProvider));
		}
		return lwziq(new rdvtd(hbhvp.onvsi));
	}

	public static rdvtd ldfxl(mwxgh p0)
	{
		return lwziq(new rdvtd(p0));
	}

	private static mwxgh gmjha(mggni p0)
	{
		if (p0 is mwxgh result && 0 == 0)
		{
			return result;
		}
		if (p0 is vrloh p1 && 0 == 0)
		{
			return lkeoo(p1);
		}
		return hbhvp.onvsi;
	}

	private static mwxgh lkeoo(vrloh p0)
	{
		dwdiq dwdiq = new dwdiq();
		dwdiq.zlvoa = p0;
		apajk<ISocket> axhuh = dwdiq.zlvoa.axhuh;
		if (ojevr == null || 1 == 0)
		{
			ojevr = obhjs;
		}
		apajk<mwxgh> apajk2 = axhuh.ttrww(ojevr).ttrww(krsdb.jrkdz).ocgqf(dwdiq.zfpid);
		if (tzetm == null || 1 == 0)
		{
			tzetm = fjigt;
		}
		return apajk2.bbtvl(tzetm);
	}

	private static rdvtd ighni(rdvtd p0)
	{
		return p0;
	}

	private static mwxgh fjigt()
	{
		return hbhvp.onvsi;
	}

	private static ISocketExt obhjs(ISocket p0)
	{
		return p0 as ISocketExt;
	}

	static rdvtd()
	{
		if (ovypj == null || 1 == 0)
		{
			ovypj = njmeg;
		}
		epxpi = ovypj;
	}

	private static rdvtd njmeg(rdvtd p0)
	{
		return p0;
	}
}
