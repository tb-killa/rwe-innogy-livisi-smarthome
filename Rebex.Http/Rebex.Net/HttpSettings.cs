using Rebex.Net.ConnectionManagement;
using onrkn;

namespace Rebex.Net;

public class HttpSettings : SslSettings
{
	private int zppjg;

	internal int elmdq = 30000;

	internal int hvyjf = 300000;

	private AutoConnectType ttiys;

	private bool hlfsl;

	private bool zqbip;

	private bool kvuvo;

	public AutoConnectType AutoConnectToInternet
	{
		get
		{
			return ttiys;
		}
		set
		{
			ttiys = value;
		}
	}

	public bool HttpSessionCacheEnabled
	{
		get
		{
			return hlfsl;
		}
		set
		{
			hlfsl = value;
		}
	}

	public int HttpSessionCacheTimeout
	{
		get
		{
			return zppjg;
		}
		set
		{
			if (value < 5000)
			{
				throw hifyx.nztrs("value", value, "Minimal value is 5000.");
			}
			zppjg = value;
		}
	}

	public bool SslSessionCacheEnabled
	{
		get
		{
			return zqbip;
		}
		set
		{
			zqbip = value;
		}
	}

	public bool AllowRedirectDowngrade
	{
		get
		{
			return kvuvo;
		}
		set
		{
			kvuvo = value;
		}
	}

	public HttpSettings()
	{
		HttpSessionCacheEnabled = true;
		SslSessionCacheEnabled = true;
		HttpSessionCacheTimeout = 50000;
		AutoConnectToInternet = AutoConnectType.Disabled;
		AllowRedirectDowngrade = true;
	}
}
