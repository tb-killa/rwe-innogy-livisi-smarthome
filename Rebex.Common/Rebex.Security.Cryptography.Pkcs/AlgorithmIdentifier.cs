using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class AlgorithmIdentifier : lnabj
{
	private wyjqw yfhkt;

	private nnzwd zpgoz;

	private byte[] djdxo;

	public ObjectIdentifier Oid => yfhkt.scakm;

	public byte[] Parameters
	{
		get
		{
			if (djdxo == null || 1 == 0)
			{
				if (zpgoz == null || 1 == 0)
				{
					djdxo = new byte[0];
				}
				else
				{
					djdxo = zpgoz.lktyp;
				}
			}
			return djdxo;
		}
	}

	private AlgorithmIdentifier(AlgorithmIdentifier alg)
	{
		yfhkt = alg.yfhkt;
		if (alg.zpgoz != null && 0 == 0)
		{
			zpgoz = new nnzwd((byte[])alg.zpgoz.lktyp.Clone());
		}
	}

	internal AlgorithmIdentifier evxkk()
	{
		return new AlgorithmIdentifier(this);
	}

	public AlgorithmIdentifier(ObjectIdentifier oid)
		: this(oid, null)
	{
	}

	public AlgorithmIdentifier(ObjectIdentifier oid, byte[] parameters)
	{
		if (oid == null || 1 == 0)
		{
			throw new ArgumentNullException("oid");
		}
		yfhkt = new wyjqw(oid);
		if (parameters != null && 0 == 0)
		{
			zpgoz = new nnzwd();
			try
			{
				hfnnn.qnzgo(zpgoz, parameters);
			}
			catch (Exception innerException)
			{
				throw new ArgumentException("Parameters are unparsable.", innerException);
			}
		}
	}

	internal AlgorithmIdentifier()
	{
		yfhkt = new wyjqw();
	}

	internal static AlgorithmIdentifier xhnfa(ObjectIdentifier p0, ObjectIdentifier p1)
	{
		string value;
		if ((value = p0.Value) != null && 0 == 0)
		{
			byte[] parameters;
			if (!(value == "1.2.840.10045.2.1") || 1 == 0)
			{
				if ((!(value == "1.3.101.112") || 1 == 0) && !(value == "1.3.101.110"))
				{
					goto IL_00a0;
				}
				if (p1 != null && 0 == 0 && p1.Value != p0.Value && 0 == 0)
				{
					throw new CryptographicException("Unexpected curve.");
				}
				parameters = null;
			}
			else
			{
				parameters = p1.ToArray(useDer: true);
			}
			return new AlgorithmIdentifier(p0, parameters);
		}
		goto IL_00a0;
		IL_00a0:
		throw new CryptographicException("Unexpected key algorithm.");
	}

	internal static AlgorithmIdentifier lehcn(AlgorithmIdentifier p0, SignatureHashAlgorithm? p1)
	{
		AsymmetricKeyAlgorithmId asymmetricKeyAlgorithmId = p0.edeag();
		switch (asymmetricKeyAlgorithmId)
		{
		case AsymmetricKeyAlgorithmId.RSA:
			return zcpsk(p1);
		case AsymmetricKeyAlgorithmId.DSA:
			return xhphy(p1);
		case AsymmetricKeyAlgorithmId.ECDsa:
		{
			string p2 = bpkgq.cafoz(p0);
			return ibxih(p2, p1);
		}
		default:
			throw new CryptographicException("Unsupported key algorithm '" + bpkgq.bjusl(asymmetricKeyAlgorithmId) + "'.");
		}
	}

	internal static AlgorithmIdentifier zcpsk(SignatureHashAlgorithm? p0)
	{
		SignatureHashAlgorithm valueOrDefault = p0.GetValueOrDefault();
		string text = ((!p0.HasValue) ? "1.2.840.113549.1.1.11" : (valueOrDefault switch
		{
			SignatureHashAlgorithm.MD5 => "1.2.840.113549.1.1.4", 
			SignatureHashAlgorithm.SHA1 => "1.2.840.113549.1.1.5", 
			SignatureHashAlgorithm.SHA224 => "1.2.840.113549.1.1.14", 
			SignatureHashAlgorithm.SHA256 => "1.2.840.113549.1.1.11", 
			SignatureHashAlgorithm.SHA384 => "1.2.840.113549.1.1.12", 
			SignatureHashAlgorithm.SHA512 => "1.2.840.113549.1.1.13", 
			_ => throw hifyx.nztrs("signatureHashAlgorithm", p0, "Hash algorithm cannot be used with the specified key algorithm."), 
		}));
		return new AlgorithmIdentifier(text, new mdvaz().ionjf());
	}

	internal static AlgorithmIdentifier xhphy(SignatureHashAlgorithm? p0)
	{
		SignatureHashAlgorithm valueOrDefault = p0.GetValueOrDefault();
		if (p0.HasValue && 0 == 0 && valueOrDefault != SignatureHashAlgorithm.SHA1)
		{
			throw hifyx.nztrs("signatureHashAlgorithm", p0, "Hash algorithm cannot be used with the specified key algorithm.");
		}
		return new AlgorithmIdentifier("1.2.840.10040.4.3");
	}

	internal static AlgorithmIdentifier glnvf(string p0, string p1, SignatureHashAlgorithm? p2)
	{
		string text;
		if ((text = p0) != null && 0 == 0)
		{
			if (text == "1.2.840.10045.2.1")
			{
				return ibxih(p1, p2);
			}
			if (text == "1.3.101.112")
			{
				if (p2.HasValue && 0 == 0 && p2 != SignatureHashAlgorithm.SHA512 && 0 == 0)
				{
					throw hifyx.nztrs("signatureHashAlgorithm", p2, "Hash algorithm cannot be used with the specified key algorithm.");
				}
				return new AlgorithmIdentifier("1.3.101.112", null);
			}
		}
		throw new CryptographicException("Unsupported key algorithm '" + bpkgq.zyzdj(p0) + "'.");
	}

	internal static AlgorithmIdentifier ibxih(string p0, SignatureHashAlgorithm? p1)
	{
		string text = ((!p1.HasValue) ? bpkgq.gjkao(p0) : p1.Value) switch
		{
			SignatureHashAlgorithm.SHA1 => "1.2.840.10045.4.1", 
			SignatureHashAlgorithm.SHA224 => "1.2.840.10045.4.3.1", 
			SignatureHashAlgorithm.SHA256 => "1.2.840.10045.4.3.2", 
			SignatureHashAlgorithm.SHA384 => "1.2.840.10045.4.3.3", 
			SignatureHashAlgorithm.SHA512 => "1.2.840.10045.4.3.4", 
			_ => throw hifyx.nztrs("signatureHashAlgorithm", p1, "Hash algorithm cannot be used with the specified key algorithm."), 
		};
		byte[] parameters = new ObjectIdentifier(p0).ToArray(useDer: true);
		return new AlgorithmIdentifier(text, parameters);
	}

	internal static AlgorithmIdentifier heubo(HashingAlgorithmId p0)
	{
		string text = null;
		switch (p0)
		{
		case HashingAlgorithmId.MD4:
			text = "1.2.840.113549.2.4";
			break;
		case HashingAlgorithmId.MD5:
			text = "1.2.840.113549.2.5";
			break;
		case HashingAlgorithmId.SHA1:
			text = "1.3.14.3.2.26";
			break;
		case HashingAlgorithmId.SHA224:
			text = "2.16.840.1.101.3.4.2.4";
			break;
		case HashingAlgorithmId.SHA256:
			text = "2.16.840.1.101.3.4.2.1";
			break;
		case HashingAlgorithmId.SHA384:
			text = "2.16.840.1.101.3.4.2.2";
			break;
		case HashingAlgorithmId.SHA512:
			text = "2.16.840.1.101.3.4.2.3";
			break;
		default:
			return null;
		}
		return new AlgorithmIdentifier(text, new mdvaz().ionjf());
	}

	internal SignatureHashAlgorithm ldvqi()
	{
		string value;
		if ((value = yfhkt.scakm.Value) != null && 0 == 0)
		{
			if (fnfqw.yabhs == null || 1 == 0)
			{
				fnfqw.yabhs = new Dictionary<string, int>(22)
				{
					{ "1.2.840.113549.2.4", 0 },
					{ "1.2.840.113549.1.1.3", 1 },
					{ "1.2.840.113549.2.5", 2 },
					{ "1.2.840.113549.1.1.4", 3 },
					{ "1.3.14.3.2.26", 4 },
					{ "1.2.840.113549.1.1.5", 5 },
					{ "1.2.840.10040.4.3", 6 },
					{ "1.2.840.10045.4.1", 7 },
					{ "2.16.840.1.101.3.4.2.4", 8 },
					{ "1.2.840.113549.1.1.14", 9 },
					{ "1.2.840.10045.4.3.1", 10 },
					{ "2.16.840.1.101.3.4.2.1", 11 },
					{ "1.2.840.113549.1.1.11", 12 },
					{ "1.2.840.10045.4.3.2", 13 },
					{ "2.16.840.1.101.3.4.2.2", 14 },
					{ "1.2.840.113549.1.1.12", 15 },
					{ "1.2.840.10045.4.3.3", 16 },
					{ "2.16.840.1.101.3.4.2.3", 17 },
					{ "1.2.840.113549.1.1.13", 18 },
					{ "1.2.840.10045.4.3.4", 19 },
					{ "1.3.101.112", 20 },
					{ "1.2.840.113549.1.1.10", 21 }
				};
			}
			if (fnfqw.yabhs.TryGetValue(value, out var value2) && 0 == 0)
			{
				switch (value2)
				{
				case 0:
				case 1:
					return SignatureHashAlgorithm.MD4;
				case 2:
				case 3:
					return SignatureHashAlgorithm.MD5;
				case 4:
				case 5:
				case 6:
				case 7:
					return SignatureHashAlgorithm.SHA1;
				case 8:
				case 9:
				case 10:
					return SignatureHashAlgorithm.SHA224;
				case 11:
				case 12:
				case 13:
					return SignatureHashAlgorithm.SHA256;
				case 14:
				case 15:
				case 16:
					return SignatureHashAlgorithm.SHA384;
				case 17:
				case 18:
				case 19:
				case 20:
					return SignatureHashAlgorithm.SHA512;
				case 21:
				{
					bbiry bbiry = hbsvy();
					if (bbiry == null || 1 == 0)
					{
						return SignatureHashAlgorithm.SHA1;
					}
					return bpkgq.vfyof(bbiry.rjpev().HashAlgorithm);
				}
				}
			}
		}
		return SignatureHashAlgorithm.Unsupported;
	}

	internal KeyAlgorithm qlesd()
	{
		string value;
		if ((value = yfhkt.scakm.Value) != null && 0 == 0)
		{
			if (fnfqw.puokz == null || 1 == 0)
			{
				fnfqw.puokz = new Dictionary<string, int>(22)
				{
					{ "1.2.840.113549.2.4", 0 },
					{ "1.2.840.113549.1.1.3", 1 },
					{ "1.2.840.113549.2.5", 2 },
					{ "1.2.840.113549.1.1.4", 3 },
					{ "1.2.840.113549.1.1.1", 4 },
					{ "1.2.840.113549.1.1.5", 5 },
					{ "1.2.840.113549.1.1.14", 6 },
					{ "1.2.840.113549.1.1.11", 7 },
					{ "1.2.840.113549.1.1.12", 8 },
					{ "1.2.840.113549.1.1.13", 9 },
					{ "1.2.840.113549.1.1.10", 10 },
					{ "1.2.840.113549.1.1.7", 11 },
					{ "1.2.840.10040.4.1", 12 },
					{ "1.2.840.10040.4.3", 13 },
					{ "1.3.101.112", 14 },
					{ "1.3.101.110", 15 },
					{ "1.2.840.10045.2.1", 16 },
					{ "1.2.840.10045.4.1", 17 },
					{ "1.2.840.10045.4.3.1", 18 },
					{ "1.2.840.10045.4.3.2", 19 },
					{ "1.2.840.10045.4.3.3", 20 },
					{ "1.2.840.10045.4.3.4", 21 }
				};
			}
			if (fnfqw.puokz.TryGetValue(value, out var value2) && 0 == 0)
			{
				switch (value2)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
					return KeyAlgorithm.RSA;
				case 12:
				case 13:
					return KeyAlgorithm.DSA;
				case 14:
					return KeyAlgorithm.ED25519;
				case 15:
					return KeyAlgorithm.ECDsa;
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 21:
					return KeyAlgorithm.ECDsa;
				}
			}
		}
		return KeyAlgorithm.Unsupported;
	}

	internal AsymmetricKeyAlgorithmId edeag()
	{
		string value;
		if ((value = yfhkt.scakm.Value) != null && 0 == 0)
		{
			if (fnfqw.obfhn == null || 1 == 0)
			{
				fnfqw.obfhn = new Dictionary<string, int>(18)
				{
					{ "1.2.840.113549.1.1.1", 0 },
					{ "1.2.840.113549.1.1.2", 1 },
					{ "1.2.840.113549.1.1.3", 2 },
					{ "1.2.840.113549.1.1.4", 3 },
					{ "1.2.840.113549.1.1.5", 4 },
					{ "1.2.840.113549.1.1.14", 5 },
					{ "1.2.840.113549.1.1.11", 6 },
					{ "1.2.840.113549.1.1.12", 7 },
					{ "1.2.840.113549.1.1.13", 8 },
					{ "1.2.840.10040.4.1", 9 },
					{ "1.2.840.10040.4.3", 10 },
					{ "1.3.101.112", 11 },
					{ "1.2.840.10045.2.1", 12 },
					{ "1.2.840.10045.4.3.1", 13 },
					{ "1.2.840.10045.4.3.2", 14 },
					{ "1.2.840.10045.4.3.3", 15 },
					{ "1.2.840.10045.4.3.4", 16 },
					{ "1.3.101.110", 17 }
				};
			}
			if (fnfqw.obfhn.TryGetValue(value, out var value2) && 0 == 0)
			{
				switch (value2)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
					return AsymmetricKeyAlgorithmId.RSA;
				case 9:
				case 10:
					return AsymmetricKeyAlgorithmId.DSA;
				case 11:
					return AsymmetricKeyAlgorithmId.EdDsa;
				case 12:
				case 13:
				case 14:
				case 15:
				case 16:
					return AsymmetricKeyAlgorithmId.ECDsa;
				case 17:
					return AsymmetricKeyAlgorithmId.ECDH;
				}
			}
		}
		return (AsymmetricKeyAlgorithmId)(-1);
	}

	internal SignatureParameters jwptd(HashingAlgorithmId p0)
	{
		SignaturePaddingScheme signaturePaddingScheme;
		if (yfhkt.scakm.Value == "1.2.840.113549.1.1.10" && 0 == 0)
		{
			signaturePaddingScheme = SignaturePaddingScheme.Pss;
			if (signaturePaddingScheme != SignaturePaddingScheme.Default)
			{
				goto IL_004f;
			}
		}
		switch (qlesd())
		{
		case KeyAlgorithm.RSA:
			signaturePaddingScheme = SignaturePaddingScheme.Pkcs1;
			if (signaturePaddingScheme != SignaturePaddingScheme.Default)
			{
				break;
			}
			goto case KeyAlgorithm.DSA;
		case KeyAlgorithm.DSA:
			signaturePaddingScheme = SignaturePaddingScheme.Default;
			if (signaturePaddingScheme == SignaturePaddingScheme.Default)
			{
				break;
			}
			goto default;
		default:
			return null;
		}
		goto IL_004f;
		IL_004f:
		SignatureParameters signatureParameters;
		if (signaturePaddingScheme == SignaturePaddingScheme.Pss)
		{
			bbiry bbiry = hbsvy();
			if (bbiry != null && 0 == 0)
			{
				signatureParameters = bbiry.rjpev();
				if (signatureParameters.HashAlgorithm != p0)
				{
					throw new CryptographicException("RSA/PSS hash algorithm does not match the message digest algorithm.");
				}
			}
			else
			{
				signatureParameters = new bbiry().rjpev();
				signatureParameters.HashAlgorithm = p0;
			}
		}
		else
		{
			signatureParameters = new SignatureParameters();
			signatureParameters.HashAlgorithm = p0;
			signatureParameters.Format = SignatureFormat.Pkcs;
			signatureParameters.PaddingScheme = signaturePaddingScheme;
		}
		return signatureParameters;
	}

	private bbiry hbsvy()
	{
		bbiry bbiry = new bbiry();
		byte[] lktyp = zpgoz.lktyp;
		if (lktyp != null && 0 == 0 && lktyp.Length > 0)
		{
			if (lktyp[0] == 5)
			{
				return null;
			}
			hfnnn.qnzgo(bbiry, zpgoz.lktyp);
		}
		return bbiry;
	}

	internal EncryptionParameters kwisk()
	{
		string value;
		if ((value = yfhkt.scakm.Value) != null && 0 == 0)
		{
			if (value == "1.2.840.113549.1.1.7")
			{
				uchfa uchfa = new uchfa();
				if (zpgoz != null && 0 == 0)
				{
					hfnnn.qnzgo(uchfa, zpgoz.lktyp);
				}
				return uchfa.vsuvv();
			}
			if (value == "1.2.840.113549.1.1.1")
			{
				EncryptionParameters encryptionParameters = new EncryptionParameters();
				encryptionParameters.PaddingScheme = EncryptionPaddingScheme.Pkcs1;
				return encryptionParameters;
			}
		}
		return null;
	}

	internal HashingAlgorithmId vvmoi(bool p0)
	{
		string value = yfhkt.scakm.Value;
		string key;
		if ((key = value) != null && 0 == 0)
		{
			if (fnfqw.xpbip == null || 1 == 0)
			{
				fnfqw.xpbip = new Dictionary<string, int>(22)
				{
					{ "1.2.840.113549.2.4", 0 },
					{ "1.2.840.113549.1.1.3", 1 },
					{ "1.2.840.113549.2.5", 2 },
					{ "1.2.840.113549.1.1.4", 3 },
					{ "1.3.14.3.2.26", 4 },
					{ "1.2.840.10040.4.1", 5 },
					{ "1.2.840.10040.4.3", 6 },
					{ "1.2.840.113549.1.1.5", 7 },
					{ "1.2.840.10045.4.1", 8 },
					{ "2.16.840.1.101.3.4.2.4", 9 },
					{ "1.2.840.113549.1.1.14", 10 },
					{ "1.2.840.10045.4.3.1", 11 },
					{ "2.16.840.1.101.3.4.2.1", 12 },
					{ "1.2.840.113549.1.1.11", 13 },
					{ "1.2.840.10045.4.3.2", 14 },
					{ "2.16.840.1.101.3.4.2.2", 15 },
					{ "1.2.840.113549.1.1.12", 16 },
					{ "1.2.840.10045.4.3.3", 17 },
					{ "2.16.840.1.101.3.4.2.3", 18 },
					{ "1.2.840.113549.1.1.13", 19 },
					{ "1.2.840.10045.4.3.4", 20 },
					{ "1.3.101.112", 21 }
				};
			}
			if (fnfqw.xpbip.TryGetValue(key, out var value2) && 0 == 0)
			{
				switch (value2)
				{
				case 0:
				case 1:
					return HashingAlgorithmId.MD4;
				case 2:
				case 3:
					return HashingAlgorithmId.MD5;
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
					return HashingAlgorithmId.SHA1;
				case 9:
				case 10:
				case 11:
					return HashingAlgorithmId.SHA224;
				case 12:
				case 13:
				case 14:
					return HashingAlgorithmId.SHA256;
				case 15:
				case 16:
				case 17:
					return HashingAlgorithmId.SHA384;
				case 18:
				case 19:
				case 20:
				case 21:
					return HashingAlgorithmId.SHA512;
				}
			}
		}
		if (p0 && 0 == 0)
		{
			throw new CryptographicException("Unknown hash algorithm '" + value + "'.");
		}
		return (HashingAlgorithmId)0;
	}

	internal string aejbk()
	{
		string value;
		if ((value = yfhkt.scakm.Value) != null && 0 == 0)
		{
			if (fnfqw.mymce == null || 1 == 0)
			{
				fnfqw.mymce = new Dictionary<string, int>(21)
				{
					{ "1.2.840.113549.2.4", 0 },
					{ "1.2.840.113549.1.1.3", 1 },
					{ "1.2.840.113549.2.5", 2 },
					{ "1.2.840.113549.1.1.4", 3 },
					{ "1.3.14.3.2.26", 4 },
					{ "1.2.840.10040.4.1", 5 },
					{ "1.2.840.10040.4.3", 6 },
					{ "1.2.840.113549.1.1.5", 7 },
					{ "1.2.840.10045.4.1", 8 },
					{ "2.16.840.1.101.3.4.2.4", 9 },
					{ "1.2.840.113549.1.1.14", 10 },
					{ "1.2.840.10045.4.3.1", 11 },
					{ "2.16.840.1.101.3.4.2.1", 12 },
					{ "1.2.840.113549.1.1.11", 13 },
					{ "1.2.840.10045.4.3.2", 14 },
					{ "2.16.840.1.101.3.4.2.2", 15 },
					{ "1.2.840.113549.1.1.12", 16 },
					{ "1.2.840.10045.4.3.3", 17 },
					{ "2.16.840.1.101.3.4.2.3", 18 },
					{ "1.2.840.113549.1.1.13", 19 },
					{ "1.2.840.10045.4.3.4", 20 }
				};
			}
			if (fnfqw.mymce.TryGetValue(value, out var value2) && 0 == 0)
			{
				switch (value2)
				{
				case 0:
				case 1:
					return "MD4";
				case 2:
				case 3:
					return "MD5";
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
					return "SHA1";
				case 9:
				case 10:
				case 11:
					return "SHA224";
				case 12:
				case 13:
				case 14:
					return "SHA256";
				case 15:
				case 16:
				case 17:
					return "SHA384";
				case 18:
				case 19:
				case 20:
					return "SHA512";
				}
			}
		}
		return null;
	}

	internal SymmetricKeyAlgorithm jatvn()
	{
		string value = yfhkt.scakm.Value;
		SymmetricKeyAlgorithm symmetricKeyAlgorithm = new SymmetricKeyAlgorithm(value);
		if (symmetricKeyAlgorithm.Algorithm == SymmetricKeyAlgorithmId.ArcTwo)
		{
			ctyia ctyia = new ctyia();
			hfnnn.qnzgo(ctyia, zpgoz.lktyp);
			symmetricKeyAlgorithm.EffectiveKeySize = ctyia.pgahg;
			symmetricKeyAlgorithm.SetIV(ctyia.wpttm);
		}
		else
		{
			rwolq rwolq = new rwolq();
			hfnnn.qnzgo(rwolq, zpgoz.lktyp);
			symmetricKeyAlgorithm.SetIV(rwolq.rtrhq);
		}
		return symmetricKeyAlgorithm;
	}

	internal DSAParameters cwuvb()
	{
		if (yfhkt.scakm.Value != "1.2.840.10040.4.1" && 0 == 0)
		{
			throw new CryptographicException("Not DSA public or private key.");
		}
		byte[] parameters = Parameters;
		if (parameters.Length == 0 || 1 == 0)
		{
			throw new CryptographicException("Parameters are missing.");
		}
		return ocawh.rgahm(parameters);
	}

	private void xtrdh(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xtrdh
		this.xtrdh(p0, p1, p2);
	}

	private lnabj kkhhn(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			yfhkt = new wyjqw();
			return yfhkt;
		case 1:
			zpgoz = new nnzwd();
			return zpgoz;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in kkhhn
		return this.kkhhn(p0, p1, p2);
	}

	private void ygpwv(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ygpwv
		this.ygpwv(p0, p1, p2);
	}

	private void ctsmq()
	{
		if (yfhkt == null || 1 == 0)
		{
			throw new CryptographicException("Invalid algorithm identifier encountered.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ctsmq
		this.ctsmq();
	}

	private void kfrcp(fxakl p0)
	{
		p0.afwyb();
		if (zpgoz == null || 1 == 0)
		{
			p0.suudj(yfhkt);
		}
		else
		{
			p0.suudj(yfhkt, zpgoz);
		}
		p0.xljze();
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in kfrcp
		this.kfrcp(p0);
	}

	private static int topzg(int p0, params int[] p1)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0012;
		IL_0006:
		if (p0 == p1[num])
		{
			return num;
		}
		num++;
		goto IL_0012;
		IL_0012:
		if (num < p1.Length)
		{
			goto IL_0006;
		}
		return -1;
	}

	internal AlgorithmIdentifier(SymmetricKeyAlgorithm algorithm)
	{
		string text = null;
		switch (algorithm.Algorithm)
		{
		case SymmetricKeyAlgorithmId.TripleDES:
			if ((topzg(algorithm.BlockSize, 64) == 0 || 1 == 0) && (topzg(algorithm.KeySize, 192) == 0 || 1 == 0))
			{
				text = "1.2.840.113549.3.7";
			}
			break;
		case SymmetricKeyAlgorithmId.DES:
			if ((topzg(algorithm.BlockSize, 64) == 0 || 1 == 0) && (topzg(algorithm.KeySize, 64) == 0 || 1 == 0))
			{
				text = "1.3.14.3.2.7";
			}
			break;
		case SymmetricKeyAlgorithmId.AES:
			if (topzg(algorithm.BlockSize, 128) == 0 || 1 == 0)
			{
				switch (topzg(algorithm.KeySize, 128, 192, 256))
				{
				case 0:
					text = "2.16.840.1.101.3.4.1.2";
					break;
				case 1:
					text = "2.16.840.1.101.3.4.1.22";
					break;
				case 2:
					text = "2.16.840.1.101.3.4.1.42";
					break;
				}
			}
			break;
		case SymmetricKeyAlgorithmId.ArcTwo:
			if (topzg(algorithm.BlockSize, 64) == 0 || 1 == 0)
			{
				text = "1.2.840.113549.3.2";
			}
			break;
		}
		if (text == null || 1 == 0)
		{
			throw new CryptographicException("Unsupported algorithm/keysize/blocksize combination.");
		}
		string text2;
		byte[] p;
		if ((text2 = text) != null && 0 == 0 && text2 == "1.2.840.113549.3.2")
		{
			int num = algorithm.EffectiveKeySize;
			if (num == 0 || 1 == 0)
			{
				num = algorithm.KeySize;
			}
			p = fxakl.kncuz(new ctyia(num, algorithm.GetIV()));
		}
		else
		{
			p = new rwolq(algorithm.GetIV()).ionjf();
		}
		yfhkt = new wyjqw(text);
		zpgoz = new nnzwd();
		hfnnn.qnzgo(zpgoz, p);
	}
}
