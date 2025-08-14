using System;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using onrkn;

namespace Rebex.Mail;

public class MailSignatureParameters
{
	private readonly SignatureParameters eqiqt;

	private MailSignatureStyle jdgmc;

	internal SignatureParameters ftkjs => eqiqt;

	public MailSignatureStyle Style
	{
		get
		{
			return jdgmc;
		}
		set
		{
			jdgmc = value;
		}
	}

	public MailHashingAlgorithm HashAlgorithm
	{
		get
		{
			return (MailHashingAlgorithm)eqiqt.HashAlgorithm;
		}
		set
		{
			eqiqt.HashAlgorithm = (HashingAlgorithmId)value;
		}
	}

	public MailSignaturePaddingScheme PaddingScheme
	{
		get
		{
			return (MailSignaturePaddingScheme)eqiqt.PaddingScheme;
		}
		set
		{
			eqiqt.PaddingScheme = (SignaturePaddingScheme)value;
		}
	}

	public int? SaltLength
	{
		get
		{
			return eqiqt.SaltLength;
		}
		set
		{
			eqiqt.SaltLength = value;
		}
	}

	public bool Silent
	{
		get
		{
			return eqiqt.Silent;
		}
		set
		{
			eqiqt.Silent = value;
		}
	}

	internal MailHashingAlgorithm dsahi
	{
		get
		{
			return (MailHashingAlgorithm)eqiqt.sqxle;
		}
		set
		{
			eqiqt.sqxle = (HashingAlgorithmId)value;
		}
	}

	internal MailSignatureParameters(SignatureParameters parameters, MailSignatureStyle style)
	{
		eqiqt = parameters;
		Style = style;
	}

	public MailSignatureParameters()
	{
		eqiqt = new SignatureParameters();
		eqiqt.Format = SignatureFormat.Pkcs;
		eqiqt.aogyv = HashingAlgorithmId.SHA256;
		Style = MailSignatureStyle.Detached;
		HashAlgorithm = MailHashingAlgorithm.Default;
		PaddingScheme = MailSignaturePaddingScheme.Default;
		dsahi = MailHashingAlgorithm.SHA256;
	}

	public MailSignatureParameters(MailHashingAlgorithm hashAlgorithm)
		: this()
	{
		HashAlgorithm = hashAlgorithm;
	}

	public MailSignatureParameters(MailHashingAlgorithm hashAlgorithm, MailSignatureStyle style)
		: this(hashAlgorithm)
	{
		qfrth(style, "style");
		Style = style;
	}

	internal MailSignatureParameters(SignatureHashAlgorithm algorithm)
		: this()
	{
		eqiqt.HashAlgorithm = bpkgq.wrqur(algorithm);
	}

	internal MailSignatureParameters(SignatureHashAlgorithm algorithm, MailSignatureStyle style)
		: this(algorithm)
	{
		qfrth(style, "style");
		Style = style;
	}

	internal static void qfrth(MailSignatureStyle p0, string p1)
	{
		switch (p0)
		{
		case MailSignatureStyle.Detached:
		case MailSignatureStyle.Enveloped:
			return;
		}
		throw new ArgumentException("Invalid signature style.", p1);
	}
}
