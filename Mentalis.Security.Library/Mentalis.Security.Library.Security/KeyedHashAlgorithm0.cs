using System;
using System.Security.Cryptography;

namespace Mentalis.Security.Library.Security;

public abstract class KeyedHashAlgorithm0 : HashAlgorithm, IDisposable
{
	protected byte[] KeyValue;

	public byte[] Key
	{
		get
		{
			return KeyValue;
		}
		set
		{
			KeyValue = value;
		}
	}

	void IDisposable.Dispose()
	{
		base.Dispose(disposing: true);
	}
}
