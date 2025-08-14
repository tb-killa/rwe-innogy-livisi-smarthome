namespace SipcosCommandHandler;

public enum NetworkAcceptKey : byte
{
	PROTECTED_SECURITY_RSA = 1,
	PROTECTED_SECURITY_SYM_KEY_EXCHANGE = 2,
	SECURITY_DEFAULT_KEY = 4,
	MAC_SECURITY_DISABLED = 8,
	AD_HOC = 0x10
}
