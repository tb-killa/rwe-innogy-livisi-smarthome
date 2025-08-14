using System;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class SubjectIdentifier : PkcsBase
{
	[NonSerialized]
	private SubjectIdentifierType mwxae;

	[NonSerialized]
	private sxlwf puiuh;

	[NonSerialized]
	private rwolq ggpib;

	[NonSerialized]
	private isamm apedu;

	[NonSerialized]
	private byte[] tchhs;

	[NonSerialized]
	private byte[] ddrfk;

	public SubjectIdentifierType Type => mwxae;

	public DistinguishedName Issuer
	{
		get
		{
			if (puiuh == null || 1 == 0)
			{
				return null;
			}
			return puiuh.mmlwe;
		}
	}

	public byte[] SerialNumber
	{
		get
		{
			if (puiuh == null || 1 == 0)
			{
				return null;
			}
			if (tchhs == null || 1 == 0)
			{
				tchhs = (byte[])puiuh.btmfq.Clone();
			}
			return tchhs;
		}
	}

	public byte[] SubjectKeyIdentifier
	{
		get
		{
			if (ggpib == null || 1 == 0)
			{
				return null;
			}
			if (ddrfk == null || 1 == 0)
			{
				ddrfk = (byte[])ggpib.rtrhq.Clone();
			}
			return ddrfk;
		}
	}

	public AlgorithmIdentifier PublicKeyAlgorithm
	{
		get
		{
			if (apedu == null || 1 == 0)
			{
				return null;
			}
			return apedu.xrkhc;
		}
	}

	public byte[] PublicKey
	{
		get
		{
			if (apedu == null || 1 == 0)
			{
				return null;
			}
			return apedu.boazh;
		}
	}

	internal SubjectIdentifier(Certificate certificate)
		: this(certificate, SubjectIdentifierType.IssuerAndSerialNumber)
	{
	}

	internal SubjectIdentifier(Certificate certificate, SubjectIdentifierType subjectIdentifierType)
	{
		switch (subjectIdentifierType)
		{
		case SubjectIdentifierType.IssuerAndSerialNumber:
			puiuh = new sxlwf(certificate.GetIssuer(), certificate.GetSerialNumber());
			break;
		case SubjectIdentifierType.SubjectKeyIdentifier:
			ggpib = new rwolq(certificate.GetSubjectKeyIdentifier(), clone: false);
			break;
		case SubjectIdentifierType.PublicKey:
			throw new NotSupportedException("Public key subject identifiers are not supported yet.");
		default:
			throw new ArgumentException("Unsupported subject identifier type.", "subjectIdentifierType");
		}
		mwxae = subjectIdentifierType;
	}

	internal SubjectIdentifier(SubjectIdentifierType type)
	{
		mwxae = type;
		switch (mwxae)
		{
		case SubjectIdentifierType.IssuerAndSerialNumber:
			puiuh = new sxlwf();
			break;
		case SubjectIdentifierType.SubjectKeyIdentifier:
			ggpib = new rwolq();
			break;
		case SubjectIdentifierType.PublicKey:
			apedu = new isamm();
			break;
		default:
			throw new ArgumentException("Unsupported subject identifier type.", "type");
		}
	}

	internal SubjectIdentifier(sxlwf issuerAndSerialNumber)
	{
		puiuh = issuerAndSerialNumber;
		mwxae = SubjectIdentifierType.IssuerAndSerialNumber;
	}

	internal SubjectIdentifier(rwolq subjectKeyIdentifier)
	{
		ggpib = subjectKeyIdentifier;
		mwxae = SubjectIdentifierType.SubjectKeyIdentifier;
	}

	internal SubjectIdentifier(isamm originatorPublicKey)
	{
		apedu = originatorPublicKey;
		mwxae = SubjectIdentifierType.PublicKey;
	}

	internal lnabj wdiep()
	{
		return mwxae switch
		{
			SubjectIdentifierType.IssuerAndSerialNumber => puiuh, 
			SubjectIdentifierType.SubjectKeyIdentifier => new rwknq(ggpib, 0, rmkkr.zkxoz), 
			SubjectIdentifierType.PublicKey => new rwknq(apedu, 1, rmkkr.osptv), 
			_ => null, 
		};
	}

	internal pddyr ymxec()
	{
		return mwxae switch
		{
			SubjectIdentifierType.IssuerAndSerialNumber => new pddyr(puiuh), 
			SubjectIdentifierType.SubjectKeyIdentifier => new pddyr(ggpib), 
			_ => null, 
		};
	}

	internal CertificateChain mgsxu(CertificateStore p0, ICertificateFinder p1)
	{
		CertificateChain certificateChain = null;
		if (p1 != null && 0 == 0)
		{
			certificateChain = p1.Find(this, p0);
		}
		if (certificateChain == null || false || certificateChain.Count == 0 || 1 == 0)
		{
			certificateChain = CertificateFinder.pdauk.Find(this, p0);
		}
		if (certificateChain == null || false || certificateChain.Count == 0 || 1 == 0)
		{
			return null;
		}
		return certificateChain;
	}
}
