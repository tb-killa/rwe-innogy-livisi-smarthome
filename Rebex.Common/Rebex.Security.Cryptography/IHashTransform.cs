using System;

namespace Rebex.Security.Cryptography;

public interface IHashTransform : IDisposable
{
	int HashSize { get; }

	void Process(byte[] buffer, int offset, int count);

	byte[] GetHash();

	void Reset();
}
