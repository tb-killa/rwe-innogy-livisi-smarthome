namespace Rebex.Net;

public enum SmtpAuthentication
{
	Auto = 0,
	Plain = 1,
	DigestMD5 = 2,
	CramMD5 = 3,
	Login = 4,
	Ntlm = 7,
	GssApi = 9,
	OAuth20 = 10
}
