using System;

namespace onrkn;

internal class yvnjx : EventArgs
{
	private long lhnaf;

	private long pwfjb;

	private nttuj mmcyy;

	public long meutf
	{
		get
		{
			return lhnaf;
		}
		private set
		{
			lhnaf = value;
		}
	}

	public long dvarv
	{
		get
		{
			return pwfjb;
		}
		private set
		{
			pwfjb = value;
		}
	}

	public nttuj btcpi
	{
		get
		{
			return mmcyy;
		}
		private set
		{
			mmcyy = value;
		}
	}

	public yvnjx(long bytesTransferred, long bytesTotal, nttuj state)
	{
		meutf = bytesTransferred;
		dvarv = bytesTotal;
		btcpi = state;
	}
}
