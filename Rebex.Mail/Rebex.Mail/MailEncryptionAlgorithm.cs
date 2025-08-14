namespace Rebex.Mail;

public enum MailEncryptionAlgorithm
{
	None = -2,
	Unsupported = -1,
	TripleDES = 0,
	DES = 1,
	RC2 = 2,
	AES128 = 4,
	AES192 = 5,
	AES256 = 6
}
