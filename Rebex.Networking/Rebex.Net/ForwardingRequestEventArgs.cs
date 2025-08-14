using System;
using onrkn;

namespace Rebex.Net;

public class ForwardingRequestEventArgs : EventArgs
{
	private SshSession odtid;

	private yxshh sjxnn;

	private SshForwardingHandle uasiv;

	private bool? corfi;

	public SshForwardingHandle Handle
	{
		get
		{
			return uasiv;
		}
		private set
		{
			uasiv = value;
		}
	}

	internal bool? roesf
	{
		get
		{
			return corfi;
		}
		private set
		{
			corfi = value;
		}
	}

	public SshChannel Accept()
	{
		if (roesf.HasValue && 0 == 0)
		{
			throw new InvalidOperationException("Already accepted or rejected.");
		}
		roesf = true;
		return odtid.umiyr(sjxnn);
	}

	public void Reject()
	{
		if (roesf.HasValue && 0 == 0)
		{
			throw new InvalidOperationException("Already accepted or rejected.");
		}
		roesf = false;
		odtid.vjabb(sjxnn);
	}

	internal ForwardingRequestEventArgs(SshSession session, SshForwardingHandle handle, yxshh info)
	{
		odtid = session;
		Handle = handle;
		sjxnn = info;
	}
}
