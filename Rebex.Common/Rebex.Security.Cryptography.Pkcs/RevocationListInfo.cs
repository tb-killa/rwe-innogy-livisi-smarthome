using System;

namespace Rebex.Security.Cryptography.Pkcs;

public class RevocationListInfo
{
	private DateTime aphgj;

	private DateTime behxt;

	public DateTime ThisUpdate
	{
		get
		{
			return aphgj;
		}
		set
		{
			if (value.ToUniversalTime().Year < 1970)
			{
				throw new ArgumentException("Invalid date.", "value");
			}
			aphgj = value;
		}
	}

	public DateTime NextUpdate
	{
		get
		{
			return behxt;
		}
		set
		{
			if (value.ToUniversalTime().Year < 1970)
			{
				throw new ArgumentException("Invalid date.", "value");
			}
			behxt = value;
		}
	}

	public RevocationListInfo()
	{
		DateTime now = DateTime.Now;
		aphgj = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
		behxt = aphgj.AddDays(7.0);
	}
}
