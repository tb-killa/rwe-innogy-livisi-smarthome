using System.Collections.Generic;

namespace onrkn;

internal abstract class xxolr : rnsvi
{
	private jxtqv iufzv;

	private bool uiylb;

	private fxtcx qqbyp;

	internal fxtcx mlerv
	{
		get
		{
			if (qqbyp == null || 1 == 0)
			{
				qqbyp = new fxtcx((uiylb ? true : false) ? iufzv.ulhnp : iufzv.kzier);
			}
			return qqbyp;
		}
	}

	internal abstract int zqmsy { get; }

	internal jxtqv ucrvs => iufzv;

	internal bool wbedc => uiylb;

	internal int cqgnw
	{
		get
		{
			if (!uiylb || 1 == 0)
			{
				return iufzv.xgzqc;
			}
			return iufzv.mhpfn;
		}
	}

	internal List<int> lexyq
	{
		get
		{
			if (!uiylb || 1 == 0)
			{
				return iufzv.vtwew;
			}
			return iufzv.iqsju;
		}
	}

	internal long wftjv(int p0)
	{
		if (!uiylb || 1 == 0)
		{
			return iufzv.bjbna(p0);
		}
		return iufzv.afnxt(p0);
	}

	internal xxolr(jxtqv owner, bool isShortStream)
	{
		iufzv = owner;
		uiylb = isShortStream;
	}

	internal void hejrp(int p0)
	{
		if (uiylb && 0 == 0)
		{
			iufzv.jcuza(p0);
		}
		else
		{
			iufzv.lngir(p0);
		}
	}
}
