using System.Security.Cryptography;
using Rebex.Security.Certificates;

namespace Rebex.Security.Cryptography.Pkcs;

public abstract class RecipientInfo
{
	private EnvelopedData rhmdt;

	public abstract SubjectIdentifier RecipientIdentifier { get; }

	public abstract AlgorithmIdentifier KeyEncryptionAlgorithm { get; }

	public abstract byte[] EncryptedKey { get; }

	public abstract Certificate Certificate { get; }

	public abstract CertificateChain CertificateChain { get; }

	internal abstract bool vqzfk { get; }

	internal abstract int bilco { get; }

	internal EnvelopedData boltq => rhmdt;

	public virtual EncryptionParameters GetEncryptionParameters()
	{
		return null;
	}

	internal abstract void oyacr(CertificateStore p0, ICertificateFinder p1);

	internal abstract byte[] ghozk(bool p0);

	internal abstract bool zqjdy(bool p0);

	internal abstract bool oaeit();

	internal virtual void bvglb(EnvelopedData p0)
	{
		if (p0 != null && 0 == 0)
		{
			if (rhmdt != null && 0 == 0)
			{
				throw new CryptographicException("The recipient was already associated with another message.");
			}
			rhmdt = p0;
		}
	}

	internal abstract RecipientInfo krqfr();
}
