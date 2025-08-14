using System;
using System.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal abstract class jayrg : IDisposable
{
	private bool bezzv;

	public abstract PrivateKeyInfo jbbgs(bool p0);

	public abstract CspParameters iqqfj();

	public abstract bool vmedb(mrxvh p0);

	public abstract byte[] rypyi(byte[] p0, mrxvh p1);

	public abstract bool knvjq(jyamo p0);

	public abstract byte[] lhhds(byte[] p0, jyamo p1);

	protected void cehla()
	{
		if (bezzv && 0 == 0)
		{
			throw new CryptographicException("Key has been disposed.");
		}
	}

	protected abstract void temua(bool p0);

	~jayrg()
	{
		ypnzt(p0: false);
	}

	public void Dispose()
	{
		ypnzt(p0: true);
		GC.SuppressFinalize(this);
	}

	private void ypnzt(bool p0)
	{
		if (!bezzv || 1 == 0)
		{
			temua(p0);
			bezzv = true;
		}
	}
}
