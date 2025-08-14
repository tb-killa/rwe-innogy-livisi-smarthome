using System;
using System.Security.Cryptography;

namespace onrkn;

internal class chhkf : nebdz, gajry, ktjcg, IDisposable
{
	private readonly int ycemt;

	private readonly byte[] gluvv;

	public chhkf(riucd gcm, int authTagSize)
		: base(gcm)
	{
		ainpx.sraoa(authTagSize, "authTagSize");
		ycemt = authTagSize / 8;
		gluvv = new byte[ycemt];
	}

	protected void axgfh()
	{
		if (gluvv == null || 1 == 0)
		{
			throw new InvalidOperationException("Authentication tag is not specified.");
		}
	}

	public void tglzc(byte[] p0, int p1, int p2)
	{
		spvwl();
		if (p2 != ycemt)
		{
			throw new InvalidOperationException("Invalid authentication tag size.");
		}
		if (wdccm.gveuj && 0 == 0)
		{
			throw new InvalidOperationException("Cannot set authentication tag in this state.");
		}
		Array.Copy(p0, p1, gluvv, 0, p2);
	}

	public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
	{
		spvwl();
		axgfh();
		qbjpk(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset, p5: true);
		xzegs += inputCount;
		wdccm.tlsva(wdccm.hnquu, inputBuffer, inputOffset, inputCount, wdccm.hnquu);
		wdccm.ktplf(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
		return inputCount;
	}

	public override int yajzn(byte[] p0, int p1, int p2, byte[] p3, int p4)
	{
		spvwl();
		axgfh();
		qbjpk(p0, p1, p2, p3, p4, p5: false);
		xzegs += p2;
		wdccm.xrmgr(wdccm.hnquu, p0, p1, p2, wdccm.hnquu, xzegs);
		bool flag = jlfbq.ccahg(gluvv, 0, wdccm.hnquu, 0, gluvv.Length);
		wdccm.ktplf(p0, p1, p3, p4, p2);
		shwke();
		if (!flag || 1 == 0)
		{
			throw new CryptographicException("Authentication tag check failed.");
		}
		return p2;
	}
}
