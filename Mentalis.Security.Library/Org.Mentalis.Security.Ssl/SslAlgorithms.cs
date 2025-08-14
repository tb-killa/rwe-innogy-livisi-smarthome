using System;

namespace Org.Mentalis.Security.Ssl;

[Flags]
public enum SslAlgorithms
{
	NONE = 0,
	RSA_RC4_40_MD5 = 1,
	RSA_RC4_128_MD5 = 2,
	RSA_RC4_128_SHA = 4,
	RSA_RC2_40_MD5 = 8,
	RSA_DES_56_SHA = 0x10,
	RSA_3DES_168_SHA = 0x20,
	RSA_DES_40_SHA = 0x40,
	RSA_AES_128_SHA = 0x80,
	RSA_AES_256_SHA = 0x100,
	SECURE_CIPHERS = 0x1001A6,
	NULL_COMPRESSION = 0x100000,
	ALL = int.MaxValue
}
