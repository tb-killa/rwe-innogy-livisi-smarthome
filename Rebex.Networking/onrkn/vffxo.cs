using System;

namespace onrkn;

internal class vffxo : qjasm
{
	private qjasm snwyb;

	public override TimeSpan fapwd
	{
		get
		{
			if (snwyb != null && 0 == 0)
			{
				return snwyb.fapwd;
			}
			return base.fapwd;
		}
	}

	public override TimeSpan cckbt
	{
		get
		{
			if (snwyb != null && 0 == 0)
			{
				return snwyb.cckbt;
			}
			return base.cckbt;
		}
	}

	public override TimeSpan ukbxj
	{
		get
		{
			if (snwyb != null && 0 == 0)
			{
				return snwyb.ukbxj;
			}
			return base.ukbxj;
		}
	}

	public override int aeqtf
	{
		get
		{
			if (snwyb != null && 0 == 0)
			{
				return snwyb.aeqtf;
			}
			return base.aeqtf;
		}
	}

	public int eqjbt
	{
		get
		{
			if (snwyb != null && 0 == 0)
			{
				return qmuwb(snwyb.aeqtf);
			}
			return qmuwb(base.aeqtf);
		}
	}

	public override int abtaf
	{
		get
		{
			if (snwyb != null && 0 == 0)
			{
				return snwyb.abtaf;
			}
			return base.aeqtf;
		}
	}

	public int trklw
	{
		get
		{
			if (snwyb != null && 0 == 0)
			{
				return qmuwb(snwyb.abtaf);
			}
			return qmuwb(base.abtaf);
		}
	}

	public override int yiuzh
	{
		get
		{
			if (snwyb != null && 0 == 0)
			{
				return snwyb.yiuzh;
			}
			return base.yiuzh;
		}
	}

	public vffxo(TimeSpan receiveTimeout, TimeSpan sendTimeout, TimeSpan cleanUpTimeout)
		: base(receiveTimeout, sendTimeout, cleanUpTimeout)
	{
	}

	public vffxo(TimeSpan receiveTimeout, TimeSpan sendTimeout)
		: base(receiveTimeout, sendTimeout)
	{
	}

	public vffxo(int receiveTimeoutInMs, int sendTimeoutInMs, int cleanUpTimeoutInMs)
		: base(receiveTimeoutInMs, sendTimeoutInMs, cleanUpTimeoutInMs)
	{
	}

	public vffxo(int receiveTimeoutInMs, int sendTimeoutInMs)
		: base(receiveTimeoutInMs, sendTimeoutInMs)
	{
	}

	public vffxo(TimeSpan timeout)
		: base(timeout)
	{
	}

	public vffxo(int timeoutInMs)
		: base(timeoutInMs)
	{
	}

	public void ltwwl(int p0)
	{
		int value = egkmw(p0);
		snwyb = ((snwyb == null) ? mkkja(value, value) : snwyb.mkkja(value, value));
	}

	public void qnlvh(int p0)
	{
		int value = egkmw(p0);
		snwyb = ((snwyb == null) ? mkkja(value) : snwyb.mkkja(value));
	}

	public void skzcl(int p0)
	{
		int value = egkmw(p0);
		snwyb = ((snwyb == null) ? mkkja(null, value) : snwyb.mkkja(null, value));
	}

	private static int egkmw(int p0)
	{
		if (p0 < -1)
		{
			throw hifyx.nztrs("value", p0, "Timeout is out of range of valid values.");
		}
		if (p0 <= 0)
		{
			return -1;
		}
		if (p0 >= 1000)
		{
			return p0;
		}
		return 1000;
	}

	public static int qmuwb(int p0)
	{
		if (p0 > 0)
		{
			return p0;
		}
		return -1;
	}
}
