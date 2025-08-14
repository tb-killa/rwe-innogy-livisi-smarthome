using System.Threading;

namespace onrkn;

internal class bvilq
{
	private sealed class qbzqu
	{
		public WaitCallback hrjcb;

		public object eulrr;

		public void nzhop()
		{
			hrjcb(eulrr);
		}
	}

	private sealed class ifxek
	{
		public WaitCallback jmwuy;

		public void oxcwt()
		{
			jmwuy(null);
		}
	}

	private static readonly rwfyw<cjwpp> bfdjn = new rwfyw<cjwpp>();

	public static bool cphqj(WaitCallback p0, object p1)
	{
		qbzqu qbzqu = new qbzqu();
		qbzqu.hrjcb = p0;
		qbzqu.eulrr = p1;
		bfdjn.avlfd.atxrq(qbzqu.nzhop);
		return true;
	}

	public static bool eiuho(WaitCallback p0)
	{
		ifxek ifxek = new ifxek();
		ifxek.jmwuy = p0;
		bfdjn.avlfd.atxrq(ifxek.oxcwt);
		return true;
	}
}
