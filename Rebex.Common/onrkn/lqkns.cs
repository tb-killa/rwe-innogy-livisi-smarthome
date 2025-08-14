using System;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class lqkns : DeriveBytes
{
	private readonly rkpix sqnen;

	private readonly byte[] qdnij;

	private readonly byte[] ksgzr;

	private readonly byte[] lrjdm;

	private readonly byte[] isukz;

	public lqkns(rkpix argon2Configuration, byte[] message, byte[] key, byte[] salt, byte[] additionalData)
	{
		if (argon2Configuration == null || 1 == 0)
		{
			throw new ArgumentNullException("argon2Configuration");
		}
		sqnen = argon2Configuration;
		qdnij = message;
		ksgzr = key;
		lrjdm = salt;
		isukz = additionalData;
	}

	public lqkns(rkpix argon2Configuration, byte[] message, byte[] key, byte[] salt)
		: this(argon2Configuration, message, key, salt, null)
	{
	}

	public lqkns(rkpix argon2Configuration, byte[] message, byte[] salt)
		: this(argon2Configuration, message, null, salt, null)
	{
	}

	public lqkns(rkpix argon2Configuration, byte[] message)
		: this(argon2Configuration, message, null, null, null)
	{
	}

	public override byte[] GetBytes(int cb)
	{
		byte[] array = new byte[cb];
		gpkne.zxiwe(qdnij, ksgzr, lrjdm, isukz, sqnen, array);
		return array;
	}

	public override void Reset()
	{
	}
}
