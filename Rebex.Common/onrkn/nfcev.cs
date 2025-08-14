using System;

namespace onrkn;

internal class nfcev : Exception
{
	private fvjcl zwdaq;

	private int ottpg;

	public fvjcl uypss
	{
		get
		{
			return zwdaq;
		}
		private set
		{
			zwdaq = value;
		}
	}

	public int itamh
	{
		get
		{
			return ottpg;
		}
		private set
		{
			ottpg = value;
		}
	}

	public nfcev(fvjcl status)
		: this(status, 0, shdfo.dprqm(status), null)
	{
	}

	public nfcev(fvjcl status, Exception inner)
		: this(status, 0, shdfo.dprqm(status), inner)
	{
	}

	public nfcev(fvjcl status, string message)
		: this(status, 0, message, null)
	{
	}

	public nfcev(fvjcl status, int nativeErrorCode, string message, Exception inner)
		: base(message, inner)
	{
		uypss = status;
		itamh = nativeErrorCode;
	}
}
