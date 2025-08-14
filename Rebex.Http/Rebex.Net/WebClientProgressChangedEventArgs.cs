using onrkn;

namespace Rebex.Net;

public class WebClientProgressChangedEventArgs : ProgressChangedEventArgs
{
	private long szpng;

	private long? osrkt;

	public long BytesTransferred
	{
		get
		{
			return szpng;
		}
		private set
		{
			szpng = value;
		}
	}

	public long? BytesTotal
	{
		get
		{
			return osrkt;
		}
		private set
		{
			osrkt = value;
		}
	}

	internal WebClientProgressChangedEventArgs(yvnjx args)
		: this(args.meutf, args.dvarv)
	{
	}

	internal WebClientProgressChangedEventArgs(long bytesTransferred, long bytesTotal)
		: base(jglkz(bytesTransferred, bytesTotal), null)
	{
		BytesTransferred = bytesTransferred;
		BytesTotal = ((bytesTotal >= 0) ? new long?(bytesTotal) : ((long?)null));
	}

	private static int jglkz(long p0, long p1)
	{
		if (p1 < 0)
		{
			return 0;
		}
		double num = p1;
		double num2 = p0;
		return (int)(num2 * 100.0 / num);
	}
}
