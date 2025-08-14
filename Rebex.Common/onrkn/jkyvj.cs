using System;

namespace onrkn;

internal class jkyvj : nebdz, fhryo, ktjcg, IDisposable
{
	private readonly int lbwhc;

	private readonly byte[] wivcc;

	public jkyvj(riucd gcm, int authTagSize)
		: base(gcm)
	{
		ainpx.sraoa(authTagSize, "authTagSize");
		lbwhc = authTagSize / 8;
		wivcc = new byte[lbwhc];
	}

	public int mdexb(byte[] p0, int p1)
	{
		spvwl();
		if (wdccm.gveuj && 0 == 0)
		{
			throw new InvalidOperationException("Authentication tag is not available yet.");
		}
		wivcc.CopyTo(p0, p1);
		return wivcc.Length;
	}

	public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
	{
		spvwl();
		qbjpk(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset, p5: true);
		wdccm.ktplf(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
		xzegs += inputCount;
		wdccm.tlsva(wdccm.hnquu, outputBuffer, outputOffset, inputCount, wdccm.hnquu);
		return inputCount;
	}

	public override int yajzn(byte[] p0, int p1, int p2, byte[] p3, int p4)
	{
		spvwl();
		qbjpk(p0, p1, p2, p3, p4, p5: false);
		wdccm.ktplf(p0, p1, p3, p4, p2);
		xzegs += p2;
		wdccm.xrmgr(wdccm.hnquu, p3, p4, p2, wdccm.hnquu, xzegs);
		qfrah(wdccm.hnquu, wivcc, lbwhc);
		shwke();
		return p2;
	}

	protected void qfrah(byte[] p0, byte[] p1, int p2)
	{
		Buffer.BlockCopy(p0, 0, p1, 0, p2);
	}

	protected override void nbfwm(bool p0)
	{
		if (p0 && 0 == 0 && (!kbegk || 1 == 0))
		{
			Array.Clear(wivcc, 0, wivcc.Length);
		}
		base.nbfwm(p0);
	}
}
