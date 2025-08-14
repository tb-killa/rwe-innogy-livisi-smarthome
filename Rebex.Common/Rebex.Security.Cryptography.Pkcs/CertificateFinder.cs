using System.Security.Cryptography;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public abstract class CertificateFinder
{
	private class uqeaf : ICertificateFinder
	{
		private readonly CertificateChain[] dwrof;

		public uqeaf(CertificateChain[] chains)
		{
			dwrof = chains;
		}

		public CertificateChain Find(SubjectIdentifier subjectIdentifier, CertificateStore additional)
		{
			int num;
			if (dwrof != null && 0 == 0)
			{
				num = 0;
				if (num != 0)
				{
					goto IL_001e;
				}
				goto IL_0191;
			}
			goto IL_01ad;
			IL_0191:
			if (num >= dwrof.Length)
			{
				goto IL_01ad;
			}
			goto IL_001e;
			IL_01ad:
			return Default.Find(subjectIdentifier, additional);
			IL_001e:
			int num2 = 0;
			if (num2 != 0)
			{
				goto IL_0026;
			}
			goto IL_017a;
			IL_0026:
			switch (subjectIdentifier.Type)
			{
			case SubjectIdentifierType.IssuerAndSerialNumber:
				if (subjectIdentifier.Issuer.ToString() == dwrof[num][num2].GetIssuer().ToString() && 0 == 0 && jlfbq.oreja(subjectIdentifier.SerialNumber, dwrof[num][num2].GetSerialNumber()) && 0 == 0)
				{
					return dwrof[num];
				}
				break;
			case SubjectIdentifierType.SubjectKeyIdentifier:
			{
				byte[] subjectKeyIdentifier = dwrof[num][num2].GetSubjectKeyIdentifier();
				byte[] subjectKeyIdentifier2 = subjectIdentifier.SubjectKeyIdentifier;
				if (jlfbq.oreja(subjectKeyIdentifier2, subjectKeyIdentifier) && 0 == 0)
				{
					return dwrof[num];
				}
				if (subjectKeyIdentifier == null || 1 == 0)
				{
					HashingAlgorithm hashingAlgorithm = new HashingAlgorithm(HashingAlgorithmId.SHA1);
					subjectKeyIdentifier = hashingAlgorithm.ComputeHash(dwrof[num][num2].GetPublicKey());
					if (jlfbq.oreja(subjectKeyIdentifier2, subjectKeyIdentifier) && 0 == 0)
					{
						return dwrof[num];
					}
					RSAParameters rSAParameters = dwrof[num][num2].GetRSAParameters(exportPrivateKeys: false, silent: true);
					PublicKeyInfo publicKeyInfo = new PublicKeyInfo(rSAParameters);
					subjectKeyIdentifier = hashingAlgorithm.ComputeHash(publicKeyInfo.Encode());
					if (jlfbq.oreja(subjectKeyIdentifier2, subjectKeyIdentifier) && 0 == 0)
					{
						return dwrof[num];
					}
				}
				break;
			}
			}
			num2++;
			goto IL_017a;
			IL_017a:
			if (num2 < dwrof[num].Count)
			{
				goto IL_0026;
			}
			num++;
			goto IL_0191;
		}
	}

	private class piazq : ICertificateFinder
	{
		private readonly bool uowzf;

		public piazq(bool additionalOnly)
		{
			uowzf = additionalOnly;
		}

		public CertificateChain Find(SubjectIdentifier subjectIdentifier, CertificateStore additional)
		{
			if (additional != null && 0 == 0)
			{
				CertificateChain certificateChain = onrfz(additional, subjectIdentifier, additional);
				if (certificateChain != null && 0 == 0)
				{
					return certificateChain;
				}
			}
			if (uowzf && 0 == 0)
			{
				return null;
			}
			CertificateStoreName[] array = new CertificateStoreName[3]
			{
				CertificateStoreName.My,
				CertificateStoreName.TrustedPeople,
				CertificateStoreName.AddressBook
			};
			int num = 0;
			if (num != 0)
			{
				goto IL_0055;
			}
			goto IL_0093;
			IL_0093:
			if (num < array.Length)
			{
				goto IL_0055;
			}
			return null;
			IL_0055:
			if (CertificateStore.Exists(array[num]) && 0 == 0)
			{
				CertificateStore certificateStore = new CertificateStore(array[num]);
				CertificateChain certificateChain2 = onrfz(certificateStore, subjectIdentifier, additional);
				certificateStore.Dispose();
				if (certificateChain2 != null && 0 == 0)
				{
					return certificateChain2;
				}
			}
			num++;
			goto IL_0093;
		}

		private static CertificateChain onrfz(CertificateStore p0, SubjectIdentifier p1, CertificateStore p2)
		{
			DistinguishedName issuer = p1.Issuer;
			byte[] serialNumber = p1.SerialNumber;
			byte[] subjectKeyIdentifier = p1.SubjectKeyIdentifier;
			Certificate[] array = ((p1.Type != SubjectIdentifierType.IssuerAndSerialNumber) ? p0.FindCertificates(CertificateFindType.SubjectKeyIdentifier, subjectKeyIdentifier, CertificateFindOptions.None) : p0.FindCertificates(issuer, serialNumber, CertificateFindOptions.None));
			if (array.Length == 0 || 1 == 0)
			{
				return null;
			}
			return CertificateEngine.kemvq().wwils(array[0], p2);
		}
	}

	public static readonly ICertificateFinder Default = new piazq(additionalOnly: false);

	internal static readonly ICertificateFinder pdauk = new piazq(additionalOnly: true);

	private CertificateFinder()
	{
	}

	public static ICertificateFinder CreateFinder(params CertificateChain[] certificates)
	{
		return new uqeaf(certificates);
	}
}
