using System.Net;

namespace onrkn;

internal class rlrlj
{
	private mggni nfnvy;

	private EndPoint okosb;

	private EndPoint xuslg;

	public mggni cvyrt
	{
		get
		{
			return nfnvy;
		}
		set
		{
			nfnvy = value;
		}
	}

	public EndPoint jgwdr
	{
		get
		{
			return okosb;
		}
		set
		{
			okosb = value;
		}
	}

	public EndPoint grnci
	{
		get
		{
			return xuslg;
		}
		set
		{
			xuslg = value;
		}
	}

	public rlrlj(mggni channel, EndPoint localEndPoint, EndPoint remoteEndPoint)
	{
		cvyrt = channel;
		jgwdr = localEndPoint;
		grnci = remoteEndPoint;
	}
}
