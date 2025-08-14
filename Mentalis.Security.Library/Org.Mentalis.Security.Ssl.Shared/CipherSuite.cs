using System.Security.Cryptography;
using Mentalis.Security.Library.Security;

namespace Org.Mentalis.Security.Ssl.Shared;

internal class CipherSuite
{
	public ICryptoTransform Decryptor;

	public ICryptoTransform Encryptor;

	public KeyedHashAlgorithm0 LocalHasher;

	public KeyedHashAlgorithm0 RemoteHasher;
}
