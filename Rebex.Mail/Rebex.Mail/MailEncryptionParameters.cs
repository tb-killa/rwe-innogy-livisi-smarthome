using System;
using System.ComponentModel;
using Rebex.Security.Cryptography;
using onrkn;

namespace Rebex.Mail;

public class MailEncryptionParameters
{
	private readonly EncryptionParameters sncfq;

	private MailEncryptionAlgorithm dvmxl;

	private bool zpfeg;

	internal EncryptionParameters akbbi => sncfq;

	public MailEncryptionAlgorithm EncryptionAlgorithm
	{
		get
		{
			return dvmxl;
		}
		set
		{
			dvmxl = value;
		}
	}

	public MailEncryptionPaddingScheme PaddingScheme
	{
		get
		{
			return (MailEncryptionPaddingScheme)sncfq.PaddingScheme;
		}
		set
		{
			sncfq.PaddingScheme = (EncryptionPaddingScheme)value;
		}
	}

	public MailHashingAlgorithm HashAlgorithm
	{
		get
		{
			return (MailHashingAlgorithm)sncfq.HashAlgorithm;
		}
		set
		{
			sncfq.HashAlgorithm = (HashingAlgorithmId)value;
		}
	}

	public byte[] Label
	{
		get
		{
			return sncfq.Label;
		}
		set
		{
			sncfq.Label = value;
		}
	}

	internal MailHashingAlgorithm twose
	{
		get
		{
			return (MailHashingAlgorithm)sncfq.mciwu;
		}
		set
		{
			sncfq.mciwu = (HashingAlgorithmId)value;
		}
	}

	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("MailEncryptionParameters.Silent has been deprecated and has no effect.", false)]
	public bool Silent
	{
		get
		{
			return zpfeg;
		}
		set
		{
			zpfeg = value;
		}
	}

	internal MailEncryptionParameters(EncryptionParameters parameters, MailEncryptionAlgorithm encryption)
	{
		sncfq = parameters;
		EncryptionAlgorithm = encryption;
	}

	public MailEncryptionParameters()
	{
		sncfq = new EncryptionParameters();
		sncfq.mctrn = HashingAlgorithmId.SHA256;
		EncryptionAlgorithm = MailEncryptionAlgorithm.AES256;
		PaddingScheme = MailEncryptionPaddingScheme.Pkcs1;
		HashAlgorithm = MailHashingAlgorithm.Default;
		twose = MailHashingAlgorithm.SHA256;
	}

	public MailEncryptionParameters(MailEncryptionAlgorithm encryptionAlgorithm)
		: this()
	{
		petat(encryptionAlgorithm, "encryptionAlgorithm");
		EncryptionAlgorithm = encryptionAlgorithm;
	}

	internal static string petat(MailEncryptionAlgorithm p0, string p1)
	{
		return p0 switch
		{
			MailEncryptionAlgorithm.TripleDES => bpkgq.oiant(SymmetricKeyAlgorithmId.TripleDES, 0), 
			MailEncryptionAlgorithm.DES => bpkgq.oiant(SymmetricKeyAlgorithmId.DES, 0), 
			MailEncryptionAlgorithm.RC2 => bpkgq.oiant(SymmetricKeyAlgorithmId.ArcTwo, 0), 
			MailEncryptionAlgorithm.AES128 => bpkgq.oiant(SymmetricKeyAlgorithmId.AES, 128), 
			MailEncryptionAlgorithm.AES192 => bpkgq.oiant(SymmetricKeyAlgorithmId.AES, 192), 
			MailEncryptionAlgorithm.AES256 => bpkgq.oiant(SymmetricKeyAlgorithmId.AES, 256), 
			_ => throw new ArgumentException("Invalid encryption algorithm.", p1), 
		};
	}
}
