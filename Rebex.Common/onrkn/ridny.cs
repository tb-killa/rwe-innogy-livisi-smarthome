using System.Threading;

namespace onrkn;

internal sealed class ridny
{
	private const int plyhi = 1;

	private const int ldnjy = 0;

	private const int udgut = -1;

	private int sqiqw;

	public bool scnlq
	{
		get
		{
			int num = Interlocked.CompareExchange(ref sqiqw, -1, -1);
			return num == 1;
		}
	}

	public bool lsqby => !scnlq;

	public ridny()
	{
		sqiqw = 0;
	}

	public bool qxocb()
	{
		int num = Interlocked.CompareExchange(ref sqiqw, 1, 0);
		return num == 0;
	}

	public bool fafuc()
	{
		int num = Interlocked.CompareExchange(ref sqiqw, 0, 1);
		return num == 1;
	}
}
