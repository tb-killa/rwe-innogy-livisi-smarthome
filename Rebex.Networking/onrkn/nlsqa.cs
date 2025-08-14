using System;
using System.Net;
using Rebex;

namespace onrkn;

internal class nlsqa
{
	private bool lcrje;

	private IPEndPoint gngjt;

	private smalv sfrug;

	private ecllo gslcz;

	private tuuua tdszm;

	private Action<Exception> tobfk;

	private awngk wlnmf;

	private nebnu cvisi;

	private TimeSpan ikuvh;

	public bool ulwec
	{
		get
		{
			return lcrje;
		}
		set
		{
			lcrje = value;
		}
	}

	public IPEndPoint sdmcm
	{
		get
		{
			return gngjt;
		}
		private set
		{
			gngjt = value;
		}
	}

	public smalv hpkux
	{
		get
		{
			return sfrug;
		}
		set
		{
			sfrug = value;
		}
	}

	public ecllo sgheu
	{
		get
		{
			return gslcz;
		}
		set
		{
			gslcz = value;
		}
	}

	public tuuua zwvmm
	{
		get
		{
			return tdszm;
		}
		set
		{
			tdszm = value;
		}
	}

	public Action<Exception> wqmlk
	{
		get
		{
			return tobfk;
		}
		set
		{
			tobfk = value;
		}
	}

	public ILogWriter uhtdq
	{
		get
		{
			return newbe.xxboi;
		}
		set
		{
			newbe.xxboi = value;
		}
	}

	internal awngk newbe
	{
		get
		{
			return wlnmf;
		}
		private set
		{
			wlnmf = value;
		}
	}

	public nebnu lcqnx
	{
		get
		{
			return cvisi;
		}
		set
		{
			cvisi = value;
		}
	}

	public TimeSpan cghnr
	{
		get
		{
			return ikuvh;
		}
		set
		{
			ikuvh = value;
		}
	}

	public nlsqa(IPEndPoint clientListeningEndPoint)
	{
		if (clientListeningEndPoint == null || 1 == 0)
		{
			throw new ArgumentNullException("clientListeningEndPoint");
		}
		sdmcm = clientListeningEndPoint;
		newbe = new awngk(typeof(trheg), null);
		cghnr = TimeSpan.FromSeconds(60.0);
	}
}
