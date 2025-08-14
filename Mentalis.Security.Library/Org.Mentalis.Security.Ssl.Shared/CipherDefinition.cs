using System;

namespace Org.Mentalis.Security.Ssl.Shared;

internal struct CipherDefinition
{
	public Type BulkCipherAlgorithm;

	public int BulkExpandedSize;

	public int BulkIVSize;

	public int BulkKeySize;

	public bool Exportable;

	public Type HashAlgorithm;

	public HashType HashAlgorithmType;

	public int HashSize;

	public SslAlgorithms Scheme;

	public CipherDefinition(SslAlgorithms scheme, Type bulk, int keysize, int ivsize, int expsize, Type hash, HashType hashType, int hashsize, bool exportable)
	{
		Scheme = scheme;
		BulkCipherAlgorithm = bulk;
		BulkKeySize = keysize;
		BulkIVSize = ivsize;
		BulkExpandedSize = expsize;
		HashAlgorithm = hash;
		HashAlgorithmType = hashType;
		HashSize = hashsize;
		Exportable = exportable;
	}
}
