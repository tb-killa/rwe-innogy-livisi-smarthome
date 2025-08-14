using System;
using System.Collections.Generic;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class CertificateInfo
{
	private byte[] puptr;

	private DistinguishedName xqtzp;

	private KeyUses moedc = KeyUses.DigitalSignature | KeyUses.KeyEncipherment;

	private string[] horbi = new string[0];

	private DateTime fwcmx;

	private DateTime ppekm;

	private CertificateExtensionCollection hegmv = new CertificateExtensionCollection();

	private CrlDistributionPointCollection eyech = new CrlDistributionPointCollection();

	private vcnjn bungb = new vcnjn();

	private string yunet;

	private string[] ftaad;

	private KeyValuePair<ObjectIdentifier, byte[]>[] duzin;

	private HashingAlgorithmId jpagp;

	public DistinguishedName Subject
	{
		get
		{
			return xqtzp;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			xqtzp = value;
		}
	}

	public KeyUses Usage
	{
		get
		{
			return moedc;
		}
		set
		{
			moedc = value;
		}
	}

	public DateTime EffectiveDate
	{
		get
		{
			return fwcmx;
		}
		set
		{
			if (value.ToUniversalTime().Year < 1970)
			{
				throw new ArgumentException("Invalid date.", "value");
			}
			fwcmx = value;
		}
	}

	public DateTime ExpirationDate
	{
		get
		{
			return ppekm;
		}
		set
		{
			if (value.ToUniversalTime().Year < 1970)
			{
				throw new ArgumentException("Invalid date.", "value");
			}
			ppekm = value;
		}
	}

	public CertificateExtensionCollection Extensions => hegmv;

	public CrlDistributionPointCollection CrlDistributionPoints => eyech;

	internal vcnjn hqdrd => bungb;

	public string MailAddress
	{
		get
		{
			return yunet;
		}
		set
		{
			int num;
			if (value != null && 0 == 0)
			{
				num = 0;
				if (num != 0)
				{
					goto IL_0010;
				}
				goto IL_004f;
			}
			goto IL_0060;
			IL_0060:
			yunet = value;
			return;
			IL_004f:
			if (num >= value.Length)
			{
				goto IL_0060;
			}
			goto IL_0010;
			IL_0010:
			char c = value[num];
			if (c < ' ' || c >= '\u0080')
			{
				throw new ArgumentException(brgjd.edcru("Invalid character at position {0}.", num), "value");
			}
			num++;
			goto IL_004f;
		}
	}

	public HashingAlgorithmId SignatureHashAlgorithm
	{
		get
		{
			return jpagp;
		}
		set
		{
			jpagp = value;
		}
	}

	public CertificateInfo()
	{
		DateTime now = DateTime.Now;
		fwcmx = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
		ppekm = fwcmx.AddYears(1);
	}

	public byte[] GetSerialNumber()
	{
		if (puptr == null || 1 == 0)
		{
			return null;
		}
		return (byte[])puptr.Clone();
	}

	public void SetSerialNumber(byte[] serialNumber)
	{
		if (serialNumber == null || 1 == 0)
		{
			throw new ArgumentNullException("serialNumber");
		}
		if (serialNumber.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Empty serial number.", "serialNumber");
		}
		puptr = (byte[])serialNumber.Clone();
	}

	public void SetSerialNumber(int serialNumber)
	{
		SetSerialNumber(new zjcch(serialNumber).rtrhq);
	}

	public string[] GetExtendedUsage()
	{
		return (string[])horbi.Clone();
	}

	public void SetExtendedUsage(params string[] extendedUsage)
	{
		if (extendedUsage == null || 1 == 0)
		{
			horbi = new string[0];
		}
		else
		{
			horbi = (string[])extendedUsage.Clone();
		}
	}

	public void SetAlternativeHostnames(params string[] hostnames)
	{
		if (hostnames != null && 0 == 0 && (hostnames.Length == 0 || 1 == 0))
		{
			hostnames = null;
		}
		ftaad = hostnames;
	}

	public string[] GetAlternativeHostnames()
	{
		return ftaad;
	}

	public void SetOtherNames(params KeyValuePair<ObjectIdentifier, byte[]>[] names)
	{
		if (names != null && 0 == 0 && (names.Length == 0 || 1 == 0))
		{
			names = null;
		}
		duzin = names;
	}

	public KeyValuePair<ObjectIdentifier, byte[]>[] GetOtherNames()
	{
		return duzin;
	}

	internal SignatureHashAlgorithm? nulsj()
	{
		HashingAlgorithmId signatureHashAlgorithm = SignatureHashAlgorithm;
		if (signatureHashAlgorithm == (HashingAlgorithmId)0 || 1 == 0)
		{
			return null;
		}
		SignatureHashAlgorithm signatureHashAlgorithm2 = bpkgq.vfyof(signatureHashAlgorithm);
		if (signatureHashAlgorithm2 == Rebex.Security.Certificates.SignatureHashAlgorithm.Unsupported)
		{
			throw new InvalidOperationException("Specified CertificateInfo.SignatureHashAlgorithm is not supported.");
		}
		return signatureHashAlgorithm2;
	}
}
