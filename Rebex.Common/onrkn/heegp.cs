using System;
using System.Security.Cryptography;

namespace onrkn;

[rbjhl("windows")]
internal class heegp : IDisposable
{
	private readonly int weaqg;

	private readonly string ntsms;

	private readonly string dhudh;

	private readonly int bpngb;

	private readonly CspProviderFlags wnoml;

	private bool qtgmw;

	public heegp(CspParameters parameters)
	{
		weaqg = parameters.ProviderType;
		ntsms = parameters.ProviderName;
		dhudh = parameters.KeyContainerName;
		bpngb = parameters.KeyNumber;
		wnoml = parameters.Flags;
		qtgmw = true;
	}

	public void rikva(bool p0)
	{
		qtgmw = p0;
	}

	~heegp()
	{
		kaylp(p0: false);
	}

	public void Dispose()
	{
		kaylp(p0: true);
		GC.SuppressFinalize(this);
	}

	private void kaylp(bool p0)
	{
		if (qtgmw && 0 == 0)
		{
			CspParameters cspParameters = new CspParameters();
			cspParameters.ProviderType = weaqg;
			cspParameters.ProviderName = ntsms;
			cspParameters.KeyContainerName = dhudh;
			cspParameters.KeyNumber = bpngb;
			cspParameters.Flags = wnoml;
			hflqg.pkpvn(cspParameters, p1: false);
		}
	}
}
