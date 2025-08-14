using Rebex.Security.Cryptography;

namespace onrkn;

internal abstract class mfhby : agxpx
{
	private int jypua;

	private readonly byte[] dihmb = new byte[4];

	private readonly IHashTransform lsaxe;

	public override int syelh => jypua;

	public mfhby(agxpx previousState, IHashTransform mac)
	{
		jypua = previousState.syelh;
		lsaxe = mac;
	}

	protected void gedjb()
	{
		jypua++;
	}

	protected byte[] aanup(byte[] p0, int p1, int p2)
	{
		jypua++;
		jlfbq.lyicr(dihmb, 0, jypua);
		lsaxe.Reset();
		lsaxe.Process(dihmb, 0, 4);
		lsaxe.Process(p0, p1, p2);
		return lsaxe.GetHash();
	}

	public override void bwbpr()
	{
		if (lsaxe != null && 0 == 0)
		{
			lsaxe.Dispose();
		}
		base.bwbpr();
	}
}
