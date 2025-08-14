using System.Collections.Generic;
using onrkn;

namespace Rebex.Net;

public class SshForwardingHandle
{
	private Queue<yxshh> ipakq;

	private string ccbcb;

	private int namcn;

	private object mwfkt;

	public string Address
	{
		get
		{
			return ccbcb;
		}
		internal set
		{
			ccbcb = value;
		}
	}

	public int Port
	{
		get
		{
			return namcn;
		}
		internal set
		{
			namcn = value;
		}
	}

	public object Tag
	{
		get
		{
			return mwfkt;
		}
		set
		{
			mwfkt = value;
		}
	}

	internal SshForwardingHandle()
	{
		ipakq = new Queue<yxshh>();
	}

	internal void xsmvd(yxshh p0)
	{
		lock (ipakq)
		{
			ipakq.Enqueue(p0);
		}
	}

	internal yxshh herzj()
	{
		lock (ipakq)
		{
			if (ipakq.Count == 0 || 1 == 0)
			{
				return null;
			}
			return ipakq.Dequeue();
		}
	}
}
