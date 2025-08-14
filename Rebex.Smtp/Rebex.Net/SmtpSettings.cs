namespace Rebex.Net;

public class SmtpSettings : SslSettings
{
	private SmtpOptions ozbzh;

	internal int rvdnp;

	private bool daiuq;

	private bool cxthy;

	public bool AllowNullSender
	{
		get
		{
			return juorf(SmtpOptions.AllowNullSender);
		}
		set
		{
			wgznp(SmtpOptions.AllowNullSender, value);
		}
	}

	public bool SendWithNoBuffer
	{
		get
		{
			return juorf(SmtpOptions.SendWithNoBuffer);
		}
		set
		{
			wgznp(SmtpOptions.SendWithNoBuffer, value);
		}
	}

	public bool ReportTransferredData
	{
		get
		{
			return daiuq;
		}
		set
		{
			daiuq = value;
		}
	}

	public bool SkipContentTransferEncodingCheck
	{
		get
		{
			return cxthy;
		}
		set
		{
			cxthy = value;
		}
	}

	public SmtpSettings()
	{
		base.SslAllowedVersions = TlsVersion.Any;
	}

	internal SmtpOptions mvylp()
	{
		return ozbzh;
	}

	internal void civyv(SmtpOptions p0)
	{
		ozbzh = p0;
	}

	private bool juorf(SmtpOptions p0)
	{
		return (ozbzh & p0) == p0;
	}

	private void wgznp(SmtpOptions p0, bool p1)
	{
		if (p1 && 0 == 0)
		{
			ozbzh |= p0;
		}
		else
		{
			ozbzh &= ~p0;
		}
	}

	public SmtpSettings Clone()
	{
		SmtpSettings smtpSettings = (SmtpSettings)MemberwiseClone();
		smtpSettings.rvdnp = 0;
		return smtpSettings;
	}
}
