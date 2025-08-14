using System;

namespace onrkn;

internal class fwwdw : Exception
{
	public fwwdw(string message)
		: base(message)
	{
	}

	public static fwwdw exkbn(string p0)
	{
		return new fwwdw("Trial license key for " + p0 + " not set. Please visit https://www.rebex.net/support/trial/ to get a key and start your 30-day trial period.");
	}

	internal static fwwdw qgara(string p0)
	{
		return new fwwdw("Trial version of " + p0 + " has expired. To continue using it, please purchase a license at https://www.rebex.net/shop/.");
	}
}
