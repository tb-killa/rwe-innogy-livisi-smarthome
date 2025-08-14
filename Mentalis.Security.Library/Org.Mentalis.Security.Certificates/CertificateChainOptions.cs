namespace Org.Mentalis.Security.Certificates;

public enum CertificateChainOptions
{
	Default = 0,
	RevocationCheckEndCert = 268435456,
	RevocationCheckChain = 536870912,
	RevocationCheckChainExcludeRoot = 1073741824,
	RevocationCacheEndCert = 1,
	RevocationCheckCacheOnly = int.MinValue,
	CacheOnlyUrlRetrieval = 4,
	DisablePass1QualityFiltering = 64,
	ReturnLowerQualityContexts = 128,
	DisableAuthRootAutoUpdate = 256
}
