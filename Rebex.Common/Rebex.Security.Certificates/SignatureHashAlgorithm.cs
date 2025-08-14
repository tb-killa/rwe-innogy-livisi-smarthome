namespace Rebex.Security.Certificates;

public enum SignatureHashAlgorithm
{
	Unsupported = -1,
	MD5 = 0,
	SHA1 = 1,
	MD5SHA1 = 2,
	SHA256 = 4,
	SHA384 = 5,
	SHA512 = 6,
	MD4 = 7,
	SHA224 = 8
}
