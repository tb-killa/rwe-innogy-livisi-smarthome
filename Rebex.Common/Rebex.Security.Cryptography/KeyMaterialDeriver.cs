using System;

namespace Rebex.Security.Cryptography;

public abstract class KeyMaterialDeriver : IDisposable
{
	public abstract byte[] DeriveKeyMaterial(KeyDerivationParameters parameters);

	internal KeyMaterialDeriver()
	{
	}

	internal virtual void vdnfv(bool p0)
	{
	}

	~KeyMaterialDeriver()
	{
		vdnfv(p0: false);
	}

	public void Dispose()
	{
		vdnfv(p0: true);
		GC.SuppressFinalize(this);
	}
}
