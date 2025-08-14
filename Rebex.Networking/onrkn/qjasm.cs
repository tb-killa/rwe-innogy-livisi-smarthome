using System;

namespace onrkn;

internal class qjasm
{
	public const long chzus = 1000L;

	public const double ypfjx = 0.001;

	public static qjasm qvmss = new qjasm(-1);

	private TimeSpan qeimd;

	private TimeSpan uzjrb;

	private TimeSpan vwufr;

	public virtual TimeSpan fapwd
	{
		get
		{
			return qeimd;
		}
		private set
		{
			qeimd = value;
		}
	}

	public virtual TimeSpan cckbt
	{
		get
		{
			return uzjrb;
		}
		private set
		{
			uzjrb = value;
		}
	}

	public virtual TimeSpan ukbxj
	{
		get
		{
			return vwufr;
		}
		private set
		{
			vwufr = value;
		}
	}

	public virtual int aeqtf => ltjke(fapwd);

	public virtual int abtaf => ltjke(cckbt);

	public virtual int yiuzh => ltjke(ukbxj);

	public qjasm(TimeSpan receiveTimeout, TimeSpan sendTimeout, TimeSpan cleanUpTimeout)
	{
		fapwd = receiveTimeout;
		cckbt = sendTimeout;
		ukbxj = cleanUpTimeout;
	}

	public qjasm(TimeSpan receiveTimeout, TimeSpan sendTimeout)
		: this(receiveTimeout, sendTimeout, npvwh())
	{
		fapwd = receiveTimeout;
		cckbt = sendTimeout;
	}

	public qjasm(int receiveTimeoutInMs, int sendTimeoutInMs, int cleanUpTimeoutInMs)
		: this(TimeSpan.FromMilliseconds(receiveTimeoutInMs), TimeSpan.FromMilliseconds(sendTimeoutInMs), TimeSpan.FromMilliseconds(cleanUpTimeoutInMs))
	{
	}

	public qjasm(int receiveTimeoutInMs, int sendTimeoutInMs)
		: this(receiveTimeoutInMs, sendTimeoutInMs, -1)
	{
	}

	public qjasm(TimeSpan timeout)
		: this(timeout, timeout, timeout)
	{
	}

	public qjasm(int timeoutInMs)
		: this(timeoutInMs, timeoutInMs, timeoutInMs)
	{
	}

	private static TimeSpan npvwh()
	{
		return TimeSpan.FromMilliseconds(-1.0);
	}

	private int ltjke(TimeSpan p0)
	{
		if (!(p0.TotalMilliseconds <= 0.0) && !(p0.TotalMilliseconds > 2147483647.0))
		{
			return (int)p0.TotalMilliseconds;
		}
		return -1;
	}

	public qjasm mkkja(int? p0 = null, int? p1 = null, int? p2 = null)
	{
		int? num = p0;
		int receiveTimeoutInMs = ((num.HasValue ? true : false) ? num.GetValueOrDefault() : aeqtf);
		int? num2 = p1;
		int sendTimeoutInMs = ((num2.HasValue ? true : false) ? num2.GetValueOrDefault() : abtaf);
		int? num3 = p2;
		return new qjasm(receiveTimeoutInMs, sendTimeoutInMs, (num3.HasValue ? true : false) ? num3.GetValueOrDefault() : yiuzh);
	}
}
