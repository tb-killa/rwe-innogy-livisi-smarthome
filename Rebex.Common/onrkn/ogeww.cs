using System;
using System.Collections.Generic;

namespace onrkn;

internal sealed class ogeww : IDisposable
{
	private readonly int sexbg;

	private readonly List<jicrh<object>> drzrq;

	private readonly Action<object> qqach;

	private readonly Action<exkzi, object> qjxks;

	private int unrmv;

	private bool geida;

	private static Action<exkzi, object> qyegk;

	public int xmaza
	{
		get
		{
			zsugx();
			return unrmv;
		}
	}

	public int unyxp
	{
		get
		{
			zsugx();
			return sexbg;
		}
	}

	public int midzp
	{
		get
		{
			zsugx();
			lock (drzrq)
			{
				return drzrq.Count;
			}
		}
	}

	public ogeww(int initialCount, int maxCount)
	{
		if (qyegk == null || 1 == 0)
		{
			qyegk = egbeb;
		}
		qjxks = qyegk;
		base._002Ector();
		if (maxCount <= 0)
		{
			throw new ArgumentOutOfRangeException("maxCount");
		}
		if (initialCount > maxCount || initialCount < 0)
		{
			throw new ArgumentOutOfRangeException("initialCount");
		}
		unrmv = initialCount;
		sexbg = maxCount;
		drzrq = new List<jicrh<object>>();
		qqach = fuycd;
		geida = false;
	}

	public void Dispose()
	{
		if (geida)
		{
			return;
		}
		lock (drzrq)
		{
			while (drzrq.Count > 0)
			{
				jicrh<object> jicrh2 = drzrq[0];
				drzrq.RemoveAt(0);
				jicrh2.ajblh();
			}
		}
		geida = true;
	}

	public exkzi iikte()
	{
		return qlvza(ddmlv.prdik);
	}

	public exkzi qlvza(ddmlv p0)
	{
		zsugx();
		p0.uxxyi();
		lock (drzrq)
		{
			if (unrmv > 0)
			{
				unrmv--;
				return rxpjc.iccat;
			}
			jicrh<object> jicrh2 = new jicrh<object>();
			tfsbt tfsbt2 = p0.kjwdi(qqach, jicrh2);
			drzrq.Add(jicrh2);
			njvzu<object> wmfea = jicrh2.wmfea;
			wmfea.wdogv(qjxks, tfsbt2);
			return wmfea;
		}
	}

	public void zvrwx()
	{
		zsugx();
		lock (drzrq)
		{
			if (unrmv == sexbg)
			{
				throw new hsmgx();
			}
			if (drzrq.Count <= 0)
			{
				unrmv++;
				return;
			}
			jicrh<object> jicrh2 = drzrq[0];
			drzrq.RemoveAt(0);
			jicrh2.spaxx(null);
		}
	}

	public override string ToString()
	{
		zsugx();
		return brgjd.edcru("CurrentCount: {0}, MaximumCount: {1}, WaitingCount: {2}", xmaza, unyxp, midzp);
	}

	private void fuycd(object p0)
	{
		jicrh<object> jicrh2 = (jicrh<object>)p0;
		try
		{
			lock (drzrq)
			{
				drzrq.Remove(jicrh2);
			}
		}
		finally
		{
			jicrh2.ajblh();
		}
	}

	private void zsugx()
	{
		if (geida && 0 == 0)
		{
			throw new ObjectDisposedException("AsyncSemaphore");
		}
	}

	private static void egbeb(exkzi p0, object p1)
	{
		((tfsbt)p1).Dispose();
	}
}
