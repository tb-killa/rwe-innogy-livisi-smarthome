using System;
using System.Security.Cryptography;

namespace Rebex.Security.Cryptography;

public abstract class KeyedHashAlgorithm : HashAlgorithm
{
	protected byte[] KeyValue;

	public virtual byte[] Key
	{
		get
		{
			return (byte[])KeyValue.Clone();
		}
		set
		{
			if (State != 0 && 0 == 0)
			{
				throw new CryptographicException("Cannot change key when hash transform is in progress.");
			}
			KeyValue = (byte[])value.Clone();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (KeyValue != null && 0 == 0)
		{
			Array.Clear(KeyValue, 0, KeyValue.Length);
			KeyValue = null;
		}
		base.Dispose(disposing);
	}
}
