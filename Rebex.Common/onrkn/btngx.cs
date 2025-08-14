using Rebex.Security.Certificates;

namespace onrkn;

internal class btngx
{
	private ukjdk qrysd;

	private rwolq xbcvz;

	internal btngx()
	{
	}

	public btngx(DistinguishedName name)
	{
		qrysd = new ukjdk(name);
	}

	public btngx(byte[] keyHashSha1)
	{
		xbcvz = new rwolq(keyHashSha1);
	}

	public static lnabj ttzrc(rmkkr p0, bool p1, int p2)
	{
		return p2 switch
		{
			65537 => new ukjdk(), 
			65538 => new rwolq(), 
			_ => null, 
		};
	}
}
