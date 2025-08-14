using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class PrivateKeyInfo : PkcsBase, lnabj
{
	private abstract class umpeu
	{
		internal static void varqp(Stream p0, string p1)
		{
			byte[] bytes = EncodingTools.ASCII.GetBytes(p1);
			mzuxe(p0, bytes, 0, bytes.Length);
		}

		internal static void mzuxe(Stream p0, byte[] p1, int p2, int p3)
		{
			bfxse(p0, (uint)p3);
			p0.Write(p1, p2, p3);
		}

		public static void bfxse(Stream p0, uint p1)
		{
			p0.WriteByte((byte)((p1 >> 24) & 0xFF));
			p0.WriteByte((byte)((p1 >> 16) & 0xFF));
			p0.WriteByte((byte)((p1 >> 8) & 0xFF));
			p0.WriteByte((byte)(p1 & 0xFF));
		}

		public static void vctda(Stream p0, byte[] p1)
		{
			int num = p1.Length;
			if (num == 0 || false || p1[0] >= 128)
			{
				bfxse(p0, (uint)(num + 1));
				p0.WriteByte(0);
			}
			else
			{
				bfxse(p0, (uint)num);
			}
			if (p1 != null && 0 == 0)
			{
				p0.Write(p1, 0, p1.Length);
			}
		}
	}

	internal enum hhmob
	{
		uemtk,
		dpoma,
		ajugp,
		vtdze,
		xxczq,
		hsjtt,
		nqijj,
		knpoz
	}

	internal const string rkjek = "openssh-key-v1";

	[NonSerialized]
	private zjcch jmfxr = new zjcch(0);

	[NonSerialized]
	private AlgorithmIdentifier lohwj;

	[NonSerialized]
	private AlgorithmIdentifier kaeaj;

	[NonSerialized]
	private rwolq apmfs = new rwolq();

	[NonSerialized]
	private CryptographicAttributeCollection quysu;

	[NonSerialized]
	private htykq mmtss;

	[NonSerialized]
	private bool tazxq;

	[NonSerialized]
	private byte[] pktdj;

	[NonSerialized]
	private string pbhla;

	public AlgorithmIdentifier KeyAlgorithm
	{
		get
		{
			if (lohwj == null || 1 == 0)
			{
				return null;
			}
			if (kaeaj == null || 1 == 0)
			{
				kaeaj = lohwj.evxkk();
			}
			return kaeaj;
		}
	}

	public string Comment
	{
		get
		{
			return pbhla;
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
				goto IL_0045;
			}
			goto IL_0056;
			IL_0056:
			pbhla = value;
			return;
			IL_0045:
			if (num >= value.Length)
			{
				goto IL_0056;
			}
			goto IL_0010;
			IL_0010:
			if (value[num] < ' ')
			{
				throw new ArgumentException(brgjd.edcru("Value contains illegal character at position {0}.", num), "value");
			}
			num++;
			goto IL_0045;
		}
	}

	internal string jvnzi
	{
		get
		{
			switch (lohwj.edeag())
			{
			case AsymmetricKeyAlgorithmId.DSA:
				return "ssh-dss";
			case AsymmetricKeyAlgorithmId.RSA:
				return "ssh-rsa";
			case AsymmetricKeyAlgorithmId.EdDsa:
				return "ssh-ed25519";
			case AsymmetricKeyAlgorithmId.ECDsa:
			case AsymmetricKeyAlgorithmId.ECDH:
				if (lohwj.Oid.Value == "1.3.101.110" && 0 == 0)
				{
					return "curve25519-sha256";
				}
				return "ecdsa-sha2-" + bpkgq.wmvaf(lohwj);
			default:
				throw new CryptographicException("Specified format is not compatible with '" + lohwj.Oid.Value + "' key.");
			}
		}
	}

	public PrivateKeyInfo()
	{
	}

	internal PrivateKeyInfo(tsnbe privateKey, AsymmetricKeyAlgorithmId? algId)
	{
		if (privateKey.puxmw == null || 1 == 0)
		{
			throw new CryptographicException("Missing public key.");
		}
		if (privateKey.ckbjk == null || 1 == 0)
		{
			throw new CryptographicException("Missing curve.");
		}
		fitks(algId);
		lohwj = AlgorithmIdentifier.xhnfa("1.2.840.10045.2.1", privateKey.ckbjk.scakm);
		apmfs = new rwolq(fxakl.kncuz(privateKey));
	}

	internal PrivateKeyInfo(AlgorithmIdentifier algorithm, byte[] privateKey, byte[] publicKey, AsymmetricKeyAlgorithmId? algId)
	{
		if (algorithm.edeag() != AsymmetricKeyAlgorithmId.EdDsa)
		{
			lohwj = algorithm;
			apmfs = new rwolq(privateKey);
			if (algorithm.Oid.Value != "1.3.101.110" && 0 == 0)
			{
				fitks(algId);
			}
			if (publicKey != null && 0 == 0)
			{
				mmtss = new htykq(publicKey, 0);
			}
		}
		else
		{
			dteyh(privateKey);
		}
	}

	internal static PrivateKeyInfo aypnd(byte[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("privateKeyOrSeed");
		}
		PrivateKeyInfo privateKeyInfo = new PrivateKeyInfo();
		if (p0.Length == 32)
		{
			privateKeyInfo.bpcel(p0, null);
		}
		else
		{
			privateKeyInfo.dteyh(p0);
		}
		return privateKeyInfo;
	}

	public PrivateKeyInfo(RSAParameters parameters)
	{
		elzlx p = new elzlx(parameters);
		lohwj = new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.113549.1.1.1"), new mdvaz().ionjf());
		apmfs = new rwolq(fxakl.kncuz(p));
	}

	public PrivateKeyInfo(DSAParameters parameters)
	{
		zjcch p = new zjcch(parameters.P, allowNegative: false);
		zjcch q = new zjcch(parameters.Q, allowNegative: false);
		zjcch g = new zjcch(parameters.G, allowNegative: false);
		zjcch zjcch = new zjcch(parameters.X, allowNegative: false);
		ocawh p2 = new ocawh(p, q, g);
		lohwj = new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.10040.4.1"), fxakl.kncuz(p2));
		apmfs = new rwolq(zjcch.ionjf());
		pktdj = parameters.Y.aqhfc();
	}

	internal PrivateKeyInfo(bgosr parameters, AsymmetricKeyAlgorithmId? algId)
		: this(tsnbe.kusmi(parameters), algId)
	{
	}

	internal void fitks(AsymmetricKeyAlgorithmId? p0)
	{
		if (!p0.HasValue)
		{
			return;
		}
		AsymmetricKeyAlgorithmId valueOrDefault = p0.GetValueOrDefault();
		if (p0.HasValue && 0 == 0)
		{
			byte b;
			htykq htykq;
			switch (valueOrDefault)
			{
			case AsymmetricKeyAlgorithmId.ECDsa:
				b = 128;
				if (b == 0)
				{
					goto case AsymmetricKeyAlgorithmId.ECDH;
				}
				goto IL_0062;
			case AsymmetricKeyAlgorithmId.ECDH:
				b = 136;
				goto IL_0062;
			case AsymmetricKeyAlgorithmId.EdDsa:
				return;
				IL_0062:
				htykq = new htykq(new byte[1] { b }, 0);
				quysu = new CryptographicAttributeCollection();
				quysu.Add(new CryptographicAttributeNode("2.5.29.15", htykq.arcrw()));
				return;
			}
		}
		throw new CryptographicException("Unsupported key algorithm.");
	}

	internal KeyUses lkwqg()
	{
		if (quysu == null || 1 == 0)
		{
			return KeyUses.DigitalSignature;
		}
		CryptographicAttributeNode cryptographicAttributeNode = quysu["2.5.29.15"];
		if (cryptographicAttributeNode == null || 1 == 0)
		{
			return KeyUses.DigitalSignature;
		}
		if (cryptographicAttributeNode.Values.Count == 0 || 1 == 0)
		{
			return KeyUses.DigitalSignature;
		}
		htykq htykq = new htykq();
		hfnnn.qnzgo(htykq, cryptographicAttributeNode.Values[0]);
		return (KeyUses)htykq.xmojg();
	}

	public static PrivateKeyInfo Generate(KeyAlgorithm algorithm)
	{
		return Generate(algorithm, 0);
	}

	public static PrivateKeyInfo Generate(KeyAlgorithm algorithm, int keySize)
	{
		AsymmetricKeyAlgorithmId p;
		string p2;
		switch (algorithm)
		{
		case Rebex.Security.Certificates.KeyAlgorithm.DSA:
			p = AsymmetricKeyAlgorithmId.DSA;
			p2 = null;
			break;
		case Rebex.Security.Certificates.KeyAlgorithm.RSA:
			p = AsymmetricKeyAlgorithmId.RSA;
			p2 = null;
			break;
		case Rebex.Security.Certificates.KeyAlgorithm.ECDsa:
			p = AsymmetricKeyAlgorithmId.ECDsa;
			switch (keySize)
			{
			case 0:
			case 256:
				p2 = "1.2.840.10045.3.1.7";
				break;
			case 384:
				p2 = "1.3.132.0.34";
				break;
			case 512:
				p2 = "1.3.36.3.3.2.8.1.1.13";
				break;
			case 521:
				p2 = "1.3.132.0.35";
				break;
			default:
				throw new CryptographicException("Key size is not valid for this algorithm.");
			}
			break;
		case Rebex.Security.Certificates.KeyAlgorithm.ED25519:
			p = AsymmetricKeyAlgorithmId.EdDsa;
			if (keySize != 0 && 0 == 0 && keySize != 256)
			{
				throw new CryptographicException("Key size is not valid for this algorithm.");
			}
			p2 = "1.3.101.112";
			break;
		default:
			throw new CryptographicException("Key algorithm not supported.");
		}
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = new AsymmetricKeyAlgorithm();
		asymmetricKeyAlgorithm.kvrol(p, p2, keySize);
		return asymmetricKeyAlgorithm.GetPrivateKey();
	}

	private void kwtyo()
	{
		if (lohwj == null || false || apmfs == null)
		{
			throw new CryptographicException("Key not loaded or set yet.");
		}
	}

	internal string bdgxx()
	{
		kwtyo();
		AlgorithmIdentifier algorithmIdentifier = lohwj;
		AsymmetricKeyAlgorithmId asymmetricKeyAlgorithmId = algorithmIdentifier.edeag();
		switch (asymmetricKeyAlgorithmId)
		{
		case AsymmetricKeyAlgorithmId.RSA:
		{
			byte[] modulus = GetRSAParameters().Modulus;
			int p = bdjih.foxoi(modulus).jaioo();
			return bpkgq.pguks(asymmetricKeyAlgorithmId, null, p, p3: true);
		}
		case AsymmetricKeyAlgorithmId.DSA:
		{
			byte[] p2 = fwtfa().P;
			int p3 = bdjih.foxoi(p2).jaioo();
			return bpkgq.pguks(asymmetricKeyAlgorithmId, null, p3, p3: true);
		}
		case AsymmetricKeyAlgorithmId.ECDsa:
		case AsymmetricKeyAlgorithmId.ECDH:
		{
			string text = bpkgq.cafoz(algorithmIdentifier);
			if (text == null || 1 == 0)
			{
				throw new CryptographicException("Unsupported curve.");
			}
			if ((lkwqg() & KeyUses.KeyAgreement) != 0 && 0 == 0)
			{
				asymmetricKeyAlgorithmId = AsymmetricKeyAlgorithmId.ECDH;
			}
			return bpkgq.pguks(asymmetricKeyAlgorithmId, text, 0, p3: true);
		}
		case AsymmetricKeyAlgorithmId.EdDsa:
			return "ed25519-sha512";
		default:
			throw new CryptographicException("Unsupported key algorithm.");
		}
	}

	public byte[] ToBytes()
	{
		kwtyo();
		return apmfs.rtrhq.aqhfc();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("The GetPrivateKey method has been deprecated. Please use ToBytes() method instead.", false)]
	[wptwl(false)]
	public byte[] GetPrivateKey()
	{
		return ToBytes();
	}

	internal byte[] hsjue()
	{
		string value;
		if ((value = lohwj.Oid.Value) != null && 0 == 0 && value == "1.3.101.112" && 0 == 0)
		{
			byte[] p = kfvak();
			byte[] p2 = muxbx();
			return jlfbq.usqov(p, p2);
		}
		return apmfs.rtrhq.aqhfc();
	}

	internal byte[] kfvak()
	{
		if (lohwj.Oid.Value != "1.3.101.112" && 0 == 0)
		{
			throw new CryptographicException("Seed is only supported for EdDSA algorithms.");
		}
		rwolq rwolq = new rwolq();
		hfnnn.qnzgo(rwolq, apmfs.rtrhq);
		byte[] rtrhq = rwolq.rtrhq;
		if (rtrhq.Length != 32)
		{
			throw new CryptographicException("Unexpected seed.");
		}
		return rtrhq;
	}

	internal byte[] acfcx()
	{
		kwtyo();
		if (mmtss == null || 1 == 0)
		{
			return null;
		}
		return mmtss.lssxa.aqhfc();
	}

	internal byte[] muxbx()
	{
		kwtyo();
		string value = lohwj.Oid.Value;
		string text;
		if ((text = value) != null && 0 == 0 && (text == "1.3.101.112" || text == "1.3.101.110"))
		{
			if (mmtss != null && 0 == 0)
			{
				return mmtss.lssxa.aqhfc();
			}
			byte[] array;
			try
			{
				AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = new AsymmetricKeyAlgorithm();
				try
				{
					asymmetricKeyAlgorithm.ImportKey(this);
					array = asymmetricKeyAlgorithm.zimkk();
				}
				finally
				{
					if (asymmetricKeyAlgorithm != null && 0 == 0)
					{
						((IDisposable)asymmetricKeyAlgorithm).Dispose();
					}
				}
			}
			catch (CryptographicException inner)
			{
				throw new CryptographicException("Unable to calculate the public key.", inner);
			}
			if (array.Length != 32)
			{
				throw new CryptographicException("Unexpected public key.");
			}
			return array;
		}
		switch (lohwj.edeag())
		{
		case AsymmetricKeyAlgorithmId.RSA:
			return new PublicKeyInfo(GetRSAParameters()).ToBytes();
		case AsymmetricKeyAlgorithmId.DSA:
			return new PublicKeyInfo(GetDSAParameters()).ToBytes();
		case AsymmetricKeyAlgorithmId.ECDsa:
		case AsymmetricKeyAlgorithmId.ECDH:
		{
			tsnbe tsnbe = new tsnbe();
			hfnnn.qnzgo(tsnbe, apmfs.rtrhq);
			if (tsnbe.puxmw == null || 1 == 0)
			{
				throw new CryptographicException("Missing public key.");
			}
			return tsnbe.puxmw.lssxa.aqhfc();
		}
		default:
			throw new CryptographicException("Unsupported key algorithm '" + value + "'.");
		}
	}

	public PublicKeyInfo GetPublicKey()
	{
		kwtyo();
		string value;
		if ((value = lohwj.Oid.Value) != null && 0 == 0 && (value == "1.3.101.112" || value == "1.3.101.110"))
		{
			return new PublicKeyInfo(lohwj, muxbx());
		}
		switch (lohwj.qlesd())
		{
		case Rebex.Security.Certificates.KeyAlgorithm.RSA:
			return new PublicKeyInfo(GetRSAParameters());
		case Rebex.Security.Certificates.KeyAlgorithm.DSA:
			return new PublicKeyInfo(GetDSAParameters());
		case Rebex.Security.Certificates.KeyAlgorithm.ECDsa:
		{
			tsnbe.guhqz(apmfs.rtrhq, out var _, out var p2);
			return new PublicKeyInfo(lohwj, p2);
		}
		default:
			throw new CryptographicException("Unsupported key algorithm.");
		}
	}

	public DSAParameters GetDSAParameters()
	{
		kwtyo();
		DSAParameters dSAParameters = fwtfa();
		int num = dSAParameters.P.Length;
		if (pktdj != null && 0 == 0)
		{
			dSAParameters.Y = zjcch.bxisb(pktdj, num, num);
			return dSAParameters;
		}
		byte[] array = null;
		if ((array == null || 1 == 0) && (!CryptoHelper.UseFipsAlgorithmsOnly || 1 == 0))
		{
			DSAManaged dSAManaged = new DSAManaged();
			dSAManaged.ImportParameters(dSAParameters);
			array = dSAManaged.xchwk();
		}
		if (array == null || 1 == 0)
		{
			throw new CryptographicException("Unable to determine DSA public key.");
		}
		pktdj = array;
		dSAParameters.Y = zjcch.bxisb(array, num, num);
		return dSAParameters;
	}

	internal DSAParameters fwtfa()
	{
		kwtyo();
		DSAParameters p = lohwj.cwuvb();
		zjcch zjcch = new zjcch();
		hfnnn.qnzgo(zjcch, apmfs.rtrhq);
		p.X = zjcch.rtrhq;
		return DSAManaged.busby(p);
	}

	public RSAParameters GetRSAParameters()
	{
		kwtyo();
		if (lohwj.Oid.Value != "1.2.840.113549.1.1.1" && 0 == 0)
		{
			throw new CryptographicException("Not RSA private key.");
		}
		elzlx elzlx = new elzlx();
		hfnnn.qnzgo(elzlx, apmfs.rtrhq);
		return elzlx.wtkfr();
	}

	internal bgosr kdoob()
	{
		kwtyo();
		switch (lohwj.edeag())
		{
		default:
			throw new CryptographicException("Not an EC private key.");
		case AsymmetricKeyAlgorithmId.ECDsa:
		case AsymmetricKeyAlgorithmId.ECDH:
		{
			if (lohwj.Oid.Value == "1.3.101.110" && 0 == 0)
			{
				throw new CryptographicException("Not a classic EC private key.");
			}
			tsnbe.guhqz(apmfs.rtrhq, out var p, out var p2);
			bgosr result = lpcge.kkyit(p2);
			result.iztaf = bpkgq.kvrab(jvnzi);
			result.gwjuq = p;
			return result;
		}
		}
	}

	internal static byte[] kvyav(byte[] p0, int p1, out hhmob p2, out KeyAlgorithm p3, out string p4)
	{
		string text = EncodingTools.Default.GetString(p0, 0, p1);
		string text2 = text.Replace("\r", "");
		char[] trimChars = new char[1];
		text = text2.TrimEnd(trimChars).Trim();
		if (text.StartsWith("PuTTY-User-Key-File-", StringComparison.Ordinal) && 0 == 0)
		{
			p2 = hhmob.xxczq;
			p4 = text;
			p3 = Rebex.Security.Certificates.KeyAlgorithm.Unsupported;
			return new byte[0];
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0063;
		}
		goto IL_0079;
		IL_0079:
		if (num >= p1)
		{
			int num2 = text.IndexOf("-----BEGIN ", StringComparison.Ordinal);
			if (num2 < 0)
			{
				throw new CryptographicException("Invalid key format - no beginning.");
			}
			num2 += 11;
			int num3 = text.IndexOf('\n', num2, 64);
			if (num3 < 8)
			{
				throw new CryptographicException("Invalid key format - no end-of-line.");
			}
			string text3 = text.Substring(num2, num3 - num2);
			if (text3.StartsWith("EC PARAMETERS-----") && 0 == 0)
			{
				num2 = text.IndexOf("-----BEGIN EC PRIVATE ", StringComparison.Ordinal);
				if (num2 < 0)
				{
					throw new CryptographicException("Invalid key format - unknown EC key structure.");
				}
				num2 += 11;
				num3 = text.IndexOf('\n', num2, 64);
				if (num3 < 8)
				{
					throw new CryptographicException("Invalid key format - no end-of-line.");
				}
				text3 = text.Substring(num2, num3 - num2);
			}
			text3 = text3.TrimEnd();
			if (!text3.EndsWith(" KEY-----", StringComparison.Ordinal) || 1 == 0)
			{
				throw new CryptographicException("Invalid key format - unknown type.");
			}
			text3 = text3.Substring(0, text3.Length - 9);
			num2 += text3.Length + 9;
			string value = "-----END " + text3 + " KEY-----";
			int num4 = text.IndexOf(value, num2, StringComparison.Ordinal);
			if (num4 < 0)
			{
				throw new CryptographicException("Invalid key format - no ending.");
			}
			text = text.Substring(num2, num4 - num2);
			p3 = Rebex.Security.Certificates.KeyAlgorithm.Unsupported;
			string key;
			if ((key = text3) != null && 0 == 0)
			{
				if (fnfqw.ingtw == null || 1 == 0)
				{
					fnfqw.ingtw = new Dictionary<string, int>(9)
					{
						{ "RSA PRIVATE", 0 },
						{ "DSA PRIVATE", 1 },
						{ "RSA PUBLIC", 2 },
						{ "DSA PUBLIC", 3 },
						{ "PUBLIC", 4 },
						{ "PRIVATE", 5 },
						{ "EC PRIVATE", 6 },
						{ "OPENSSH PRIVATE", 7 },
						{ "ENCRYPTED PRIVATE", 8 }
					};
				}
				if (fnfqw.ingtw.TryGetValue(key, out var value2) && 0 == 0)
				{
					switch (value2)
					{
					case 0:
						p3 = Rebex.Security.Certificates.KeyAlgorithm.RSA;
						p2 = hhmob.dpoma;
						goto IL_02f4;
					case 1:
						p3 = Rebex.Security.Certificates.KeyAlgorithm.DSA;
						p2 = hhmob.dpoma;
						goto IL_02f4;
					case 2:
						p3 = Rebex.Security.Certificates.KeyAlgorithm.RSA;
						p2 = hhmob.knpoz;
						goto IL_02f4;
					case 3:
						p3 = Rebex.Security.Certificates.KeyAlgorithm.DSA;
						p2 = hhmob.knpoz;
						goto IL_02f4;
					case 4:
						p2 = hhmob.uemtk;
						goto IL_02f4;
					case 5:
						p2 = hhmob.ajugp;
						goto IL_02f4;
					case 6:
						if (text.IndexOf("DEK-Info:") >= 0)
						{
							p2 = hhmob.dpoma;
							p3 = Rebex.Security.Certificates.KeyAlgorithm.ECDsa;
						}
						else
						{
							p2 = hhmob.nqijj;
						}
						goto IL_02f4;
					case 7:
						p2 = hhmob.hsjtt;
						goto IL_02f4;
					case 8:
						{
							p2 = hhmob.vtdze;
							goto IL_02f4;
						}
						IL_02f4:
						if (p2 == hhmob.dpoma)
						{
							int num5 = text.IndexOf("\n\n", StringComparison.Ordinal);
							if (num5 > 0)
							{
								p4 = text.Substring(0, num5).Trim();
								text = text.Substring(num5 + 2);
							}
							else
							{
								p4 = "";
							}
						}
						else
						{
							p4 = null;
						}
						try
						{
							return Convert.FromBase64String(text);
						}
						catch (FormatException inner)
						{
							throw new CryptographicException("Invalid Base-64 encoding of a key.", inner);
						}
					}
				}
			}
			throw new CryptographicException("Unsupported key format.");
		}
		goto IL_0063;
		IL_0063:
		if (p0[num] >= 127)
		{
			throw new CryptographicException("Invalid key format - bad character.");
		}
		num++;
		goto IL_0079;
	}

	public byte[] Encode()
	{
		return fxakl.kncuz(this);
	}

	public void Save(Stream output, string password, PrivateKeyFormat format)
	{
		if (output == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		switch (format)
		{
		case PrivateKeyFormat.Base64Pkcs8:
			fdkfh(output, password, new ObjectIdentifier("2.16.840.1.101.3.4.1.42"), p3: true);
			break;
		case PrivateKeyFormat.OpenSsh:
			rklkv(output, password);
			break;
		case PrivateKeyFormat.NewOpenSsh:
		{
			byte[] array = new qfmgt().rsops(this, password);
			output.Write(array, 0, array.Length);
			break;
		}
		case PrivateKeyFormat.Putty:
			fqexb(output, password, 2);
			break;
		case PrivateKeyFormat.PPK3:
			fqexb(output, password, 3);
			break;
		case PrivateKeyFormat.RawPkcs8:
			fdkfh(output, password, new ObjectIdentifier("2.16.840.1.101.3.4.1.42"), p3: false);
			break;
		default:
			throw new NotSupportedException(string.Concat("Private key format ", format, " is not supported."));
		}
	}

	public void Save(string fileName, string password, PrivateKeyFormat format)
	{
		if (fileName == null || 1 == 0)
		{
			throw new ArgumentNullException("fileName");
		}
		Stream stream = vtdxm.bolpl(fileName);
		try
		{
			Save(stream, password, format);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public void Save(Stream output, string password, ObjectIdentifier encryptionAlgorithm)
	{
		if (output == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		fdkfh(output, password, encryptionAlgorithm, p3: true);
	}

	private void fdkfh(Stream p0, string p1, ObjectIdentifier p2, bool p3)
	{
		lnabj p4;
		string p5;
		if (p1 != null && 0 == 0)
		{
			if (p2 == null || 1 == 0)
			{
				throw new ArgumentNullException("encryptionAlgorithm");
			}
			p4 = new tbinn(this, p1, p2);
			p5 = "ENCRYPTED ";
		}
		else
		{
			p4 = this;
			p5 = "";
		}
		byte[] array = fxakl.kncuz(p4);
		StringBuilder stringBuilder;
		int num;
		if (p3 && 0 == 0)
		{
			stringBuilder = new StringBuilder();
			stringBuilder.mwigd("-----{0} {1}PRIVATE KEY-----\r\n", "BEGIN", p5);
			num = 0;
			if (num != 0)
			{
				goto IL_0071;
			}
			goto IL_009c;
		}
		goto IL_00d1;
		IL_0071:
		string p6 = Convert.ToBase64String(array, num, Math.Min(48, array.Length - num));
		stringBuilder.dlvlk("{0}\r\n", p6);
		num += 48;
		goto IL_009c;
		IL_009c:
		if (num < array.Length)
		{
			goto IL_0071;
		}
		stringBuilder.mwigd("-----{0} {1}PRIVATE KEY-----\r\n", "END", p5);
		array = EncodingTools.ASCII.GetBytes(stringBuilder.ToString());
		goto IL_00d1;
		IL_00d1:
		p0.Write(array, 0, array.Length);
	}

	private void rklkv(Stream p0, string p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		string arg;
		lnabj p2;
		switch (lohwj.edeag())
		{
		case AsymmetricKeyAlgorithmId.DSA:
		{
			arg = "DSA";
			DSAParameters dSAParameters = GetDSAParameters();
			p2 = new fnxie(dSAParameters);
			break;
		}
		case AsymmetricKeyAlgorithmId.RSA:
		{
			arg = "RSA";
			RSAParameters rSAParameters = GetRSAParameters();
			p2 = new elzlx(rSAParameters);
			break;
		}
		default:
			throw new CryptographicException("The specified format is only compatible with RSA and DSA keys.");
		}
		byte[] array = fxakl.kncuz(p2);
		byte[] p3;
		byte[] p4;
		if (p1 != null && 0 == 0)
		{
			p3 = yhkby(array, p1, out p4);
		}
		else
		{
			p3 = array;
			p4 = null;
		}
		StreamWriter streamWriter = new StreamWriter(p0, EncodingTools.Default);
		streamWriter.WriteLine("-----BEGIN {0} PRIVATE KEY-----", arg);
		if (p1 != null && 0 == 0)
		{
			streamWriter.WriteLine("Proc-Type: 4,ENCRYPTED");
			streamWriter.WriteLine("DEK-Info: DES-EDE3-CBC,{0}", BitConverter.ToString(p4).Replace("-", "").ToUpper(CultureInfo.InvariantCulture));
			streamWriter.WriteLine();
		}
		kjhmn.jsvbw(streamWriter, p3);
		streamWriter.WriteLine("-----END {0} PRIVATE KEY-----", arg);
		streamWriter.Flush();
	}

	private void fqexb(Stream p0, string p1, int p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		string text = jvnzi;
		string text2 = pbhla;
		if (text2 == null || false || text2.Length == 0 || 1 == 0)
		{
			text2 = brgjd.edcru("{0}-key-{1:yyyyMMdd}", text, DateTime.Now);
		}
		byte[] bytes = EncodingTools.Default.GetBytes(text2);
		byte[] p5;
		byte[] array;
		switch (lohwj.edeag())
		{
		case AsymmetricKeyAlgorithmId.ECDsa:
		case AsymmetricKeyAlgorithmId.ECDH:
			if (!(lohwj.Oid.Value == "1.3.101.110") || 1 == 0)
			{
				tsnbe.guhqz(apmfs.rtrhq, out var p3, out var p4);
				wmbjj wmbjj = new wmbjj();
				wmbjj.vokoa(text);
				wmbjj.vokoa(bpkgq.kvrab(text));
				wmbjj.qtrnf(p4);
				p5 = wmbjj.ihelo();
				wmbjj wmbjj2 = new wmbjj();
				wmbjj2.qtrnf(p3);
				array = wmbjj2.ihelo();
				break;
			}
			goto default;
		default:
			p5 = rssos(text);
			array = bfdep();
			break;
		}
		ffooh type = ffooh.vefjh;
		int num = 8192;
		int num2 = 13;
		int num3 = 1;
		byte[] array2 = null;
		byte[] array6;
		byte[] p7;
		string text3;
		if (p1 != null && 0 == 0 && p1.Length > 0)
		{
			byte[] bytes2 = EncodingTools.Default.GetBytes(p1);
			byte[] array4;
			byte[] array5;
			if (p2 >= 3)
			{
				byte[] array3 = new byte[80];
				array4 = new byte[32];
				array5 = new byte[16];
				array6 = new byte[32];
				byte[] array7 = new byte[0];
				array2 = CryptoHelper.aqljw(16);
				rkpix p6 = new rkpix(type, num, num2, num3);
				gpkne.zxiwe(bytes2, array7, array2, array7, p6, array3);
				Array.Copy(array3, 0, array4, 0, 32);
				Array.Copy(array3, 32, array5, 0, 16);
				Array.Copy(array3, 48, array6, 0, 32);
			}
			else
			{
				array4 = subce(bytes2);
				array5 = new byte[16];
				array6 = crzbn(bytes2);
			}
			int num4 = array.Length;
			num4 = ((num4 <= 0) ? 16 : (((num4 - 1) | 0xF) + 1));
			byte[] array8 = new byte[num4];
			byte[] randomBytes = CryptoHelper.GetRandomBytes(array8.Length - array.Length);
			array.CopyTo(array8, 0);
			randomBytes.CopyTo(array8, array.Length);
			array = array8;
			SymmetricKeyAlgorithm symmetricKeyAlgorithm = new SymmetricKeyAlgorithm(SymmetricKeyAlgorithmId.AES);
			try
			{
				symmetricKeyAlgorithm.Mode = CipherMode.CBC;
				symmetricKeyAlgorithm.Padding = PaddingMode.None;
				symmetricKeyAlgorithm.SetKey(array4);
				symmetricKeyAlgorithm.SetIV(array5);
				ICryptoTransform cryptoTransform = symmetricKeyAlgorithm.CreateEncryptor();
				try
				{
					p7 = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
					text3 = "aes256-cbc";
				}
				finally
				{
					if (cryptoTransform != null && 0 == 0)
					{
						cryptoTransform.Dispose();
					}
				}
			}
			finally
			{
				if (symmetricKeyAlgorithm != null && 0 == 0)
				{
					((IDisposable)symmetricKeyAlgorithm).Dispose();
				}
			}
		}
		else
		{
			p7 = array;
			text3 = "none";
			array6 = ((p2 < 3) ? crzbn(null) : new byte[0]);
		}
		StreamWriter streamWriter = new StreamWriter(p0, EncodingTools.Default);
		streamWriter.NewLine = "\r\n";
		streamWriter.WriteLine("PuTTY-User-Key-File-{0}: {1}", p2, text);
		streamWriter.WriteLine("Encryption: {0}", text3);
		streamWriter.WriteLine("Comment: {0}", text2);
		aqsrr(streamWriter, "Public", p5);
		if (p2 > 2 && array2 != null && 0 == 0)
		{
			streamWriter.WriteLine("Key-Derivation: Argon2id");
			streamWriter.WriteLine("Argon2-Memory: {0}", num);
			streamWriter.WriteLine("Argon2-Passes: {0}", num2);
			streamWriter.WriteLine("Argon2-Parallelism: {0}", num3);
			streamWriter.WriteLine("Argon2-Salt: " + brgjd.wlvqq(array2));
		}
		aqsrr(streamWriter, "Private", p7);
		HashingAlgorithmId p8 = ((p2 < 3) ? HashingAlgorithmId.SHA1 : HashingAlgorithmId.SHA256);
		byte[] array9 = krofs(array6, text, p5, array, text3, p8, bytes);
		streamWriter.WriteLine("Private-MAC: {0}", BitConverter.ToString(array9).Replace("-", "").ToLower(CultureInfo.InvariantCulture));
		streamWriter.Flush();
	}

	private static byte[] crzbn(byte[] p0)
	{
		IHashTransform hashTransform = new HashingAlgorithm(HashingAlgorithmId.SHA1).CreateTransform();
		qobrv(p0: false, hashTransform, EncodingTools.ASCII.GetBytes("putty-private-key-file-mac-key"));
		if (p0 != null && 0 == 0)
		{
			qobrv(p0: false, hashTransform, p0);
		}
		byte[] hash = hashTransform.GetHash();
		hashTransform.Reset();
		return hash;
	}

	private static byte[] krofs(byte[] p0, string p1, byte[] p2, byte[] p3, string p4, HashingAlgorithmId p5, byte[] p6)
	{
		HashingAlgorithm hashingAlgorithm = new HashingAlgorithm(p5);
		hashingAlgorithm.KeyMode = HashingAlgorithmKeyMode.HMAC;
		hashingAlgorithm.SetKey(p0);
		IHashTransform hashTransform = hashingAlgorithm.CreateTransform();
		qobrv(p0: true, hashTransform, EncodingTools.ASCII.GetBytes(p1));
		qobrv(p0: true, hashTransform, EncodingTools.ASCII.GetBytes(p4));
		qobrv(p0: true, hashTransform, p6);
		qobrv(p0: true, hashTransform, p2);
		qobrv(p0: true, hashTransform, p3);
		byte[] hash = hashTransform.GetHash();
		hashTransform.Reset();
		return hash;
	}

	private static byte[] subce(byte[] p0)
	{
		IHashTransform hashTransform = new HashingAlgorithm(HashingAlgorithmId.SHA1).CreateTransform();
		byte[] array = new byte[32];
		int num = 0;
		if (num != 0)
		{
			goto IL_001d;
		}
		goto IL_008b;
		IL_001d:
		byte[] bytes = BitConverter.GetBytes(num);
		if (BitConverter.IsLittleEndian && 0 == 0)
		{
			Array.Reverse(bytes, 0, bytes.Length);
		}
		hashTransform.Process(bytes, 0, bytes.Length);
		hashTransform.Process(p0, 0, p0.Length);
		byte[] hash = hashTransform.GetHash();
		hashTransform.Reset();
		int num2 = num * hash.Length;
		int length = Math.Min(hash.Length, array.Length - num2);
		Array.Copy(hash, 0, array, num2, length);
		num++;
		goto IL_008b;
		IL_008b:
		if (num < 2)
		{
			goto IL_001d;
		}
		return array;
	}

	private void aqsrr(StreamWriter p0, string p1, byte[] p2)
	{
		int num = (p2.Length + 2) / 3;
		if (num > 0)
		{
			num = (num - 1) / 16 + 1;
		}
		p0.WriteLine("{0}-Lines: {1}", p1, num);
		kjhmn.jsvbw(p0, p2);
	}

	private byte[] bfdep()
	{
		MemoryStream memoryStream = new MemoryStream();
		try
		{
			switch (lohwj.edeag())
			{
			case AsymmetricKeyAlgorithmId.RSA:
			{
				RSAParameters rSAParameters = GetRSAParameters();
				umpeu.vctda(memoryStream, rSAParameters.D);
				umpeu.vctda(memoryStream, rSAParameters.P);
				umpeu.vctda(memoryStream, rSAParameters.Q);
				umpeu.vctda(memoryStream, rSAParameters.InverseQ);
				break;
			}
			case AsymmetricKeyAlgorithmId.DSA:
				umpeu.vctda(memoryStream, fwtfa().X);
				break;
			case AsymmetricKeyAlgorithmId.ECDsa:
			case AsymmetricKeyAlgorithmId.ECDH:
			case AsymmetricKeyAlgorithmId.EdDsa:
			{
				byte[] p = hsjue();
				umpeu.mzuxe(memoryStream, p, 0, 32);
				break;
			}
			}
			return memoryStream.ToArray();
		}
		finally
		{
			if (memoryStream != null && 0 == 0)
			{
				((IDisposable)memoryStream).Dispose();
			}
		}
	}

	private byte[] rssos(string p0)
	{
		MemoryStream memoryStream = new MemoryStream();
		try
		{
			umpeu.varqp(memoryStream, p0);
			switch (lohwj.edeag())
			{
			case AsymmetricKeyAlgorithmId.RSA:
			{
				RSAParameters rSAParameters = GetRSAParameters();
				umpeu.vctda(memoryStream, rSAParameters.Exponent);
				umpeu.vctda(memoryStream, rSAParameters.Modulus);
				break;
			}
			case AsymmetricKeyAlgorithmId.DSA:
			{
				DSAParameters dSAParameters = GetDSAParameters();
				umpeu.vctda(memoryStream, dSAParameters.P);
				umpeu.vctda(memoryStream, dSAParameters.Q);
				umpeu.vctda(memoryStream, dSAParameters.G);
				umpeu.vctda(memoryStream, dSAParameters.Y);
				break;
			}
			case AsymmetricKeyAlgorithmId.ECDsa:
			case AsymmetricKeyAlgorithmId.ECDH:
			case AsymmetricKeyAlgorithmId.EdDsa:
			{
				byte[] array = muxbx();
				umpeu.mzuxe(memoryStream, array, 0, array.Length);
				break;
			}
			}
			return memoryStream.ToArray();
		}
		finally
		{
			if (memoryStream != null && 0 == 0)
			{
				((IDisposable)memoryStream).Dispose();
			}
		}
	}

	internal void ronzk(byte[] p0, string p1)
	{
		MemoryStream memoryStream = new MemoryStream(p0, writable: false);
		try
		{
			Load(memoryStream, p1);
		}
		finally
		{
			if (memoryStream != null && 0 == 0)
			{
				((IDisposable)memoryStream).Dispose();
			}
		}
	}

	public void Load(Stream input, string password)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		byte[] array = new byte[32767];
		int num = input.Read(array, 0, array.Length);
		input.Close();
		if (num >= 32767)
		{
			throw new CryptographicException("File is too long.");
		}
		if (num == 0 || 1 == 0)
		{
			throw new CryptographicException("Not a private key.");
		}
		hhmob p;
		KeyAlgorithm p2;
		string p3;
		if (array[0] != 48)
		{
			array = kvyav(array, num, out p, out p2, out p3);
			num = array.Length;
		}
		else if (!cjvew.mqmde(array, num, out p, out p2, out p3) || 1 == 0)
		{
			throw new CryptographicException("Unknown private key format.");
		}
		switch (p)
		{
		case hhmob.vtdze:
			if (password == null || 1 == 0)
			{
				throw new CryptographicException("Password required.");
			}
			break;
		default:
			throw new CryptographicException("Not a private key.");
		case hhmob.dpoma:
		case hhmob.ajugp:
		case hhmob.xxczq:
		case hhmob.hsjtt:
		case hhmob.nqijj:
			break;
		}
		byte[] array2 = null;
		byte[] array3 = null;
		SymmetricKeyAlgorithm symmetricKeyAlgorithm = null;
		if (p == hhmob.dpoma)
		{
			int num2 = 0;
			StringReader stringReader = new StringReader(p3);
			while (true)
			{
				string text = stringReader.ReadLine();
				if (text == null)
				{
					break;
				}
				if (text.StartsWith("Proc-Type: ", StringComparison.Ordinal) && 0 == 0)
				{
					num2++;
					if (!text.StartsWith("Proc-Type: 4,ENCRYPTED", StringComparison.Ordinal) || 1 == 0)
					{
						throw new CryptographicException("Unsupported private key type.");
					}
					continue;
				}
				if (!text.StartsWith("DEK-Info: ", StringComparison.Ordinal))
				{
					continue;
				}
				num2++;
				int num3 = text.IndexOf(',', 10);
				string p4 = null;
				if (num3 > 10)
				{
					p4 = text.Substring(10, num3 - 10).Trim();
				}
				symmetricKeyAlgorithm = SymmetricKeyAlgorithm.ciqvc(p4);
				string text2 = text.Substring(num3 + 1).Trim();
				array2 = new byte[text2.Length / 2];
				int num4 = 0;
				if (num4 != 0)
				{
					goto IL_01cf;
				}
				goto IL_0217;
				IL_01cf:
				try
				{
					array2[num4] = Convert.ToByte(text2.Substring(num4 * 2, 2), 16);
				}
				catch (FormatException)
				{
					throw new CryptographicException(brgjd.edcru("Unparsable IV: '{0}'.", text.Substring(10)));
				}
				num4++;
				goto IL_0217;
				IL_0217:
				if (num4 >= array2.Length)
				{
					if (array2.Length != symmetricKeyAlgorithm.BlockSize / 8)
					{
						throw new CryptographicException(brgjd.edcru("Unexpected IV length: '{0}'.", text.Substring(10)));
					}
					array3 = new byte[8];
					Array.Copy(array2, 0, array3, 0, 8);
					continue;
				}
				goto IL_01cf;
			}
			stringReader.Close();
			if ((array3 == null || 1 == 0) && num2 != 0 && 0 == 0)
			{
				throw new CryptographicException("Unsupported private key type.");
			}
		}
		switch (p)
		{
		case hhmob.dpoma:
			if (array3 != null && 0 == 0)
			{
				if (password == null || 1 == 0)
				{
					throw new CryptographicException("Password required.");
				}
				HashingAlgorithmId alg = (HashingAlgorithmId)65543;
				rbplv rbplv = new rbplv(alg, password, array3, 1);
				byte[] bytes = rbplv.GetBytes(symmetricKeyAlgorithm.KeySize / 8);
				array = euijk(array, 0, num, bytes, array2, symmetricKeyAlgorithm);
				array = dgros(array, num, symmetricKeyAlgorithm.BlockSize / 8);
				num = array.Length;
			}
			switch (p2)
			{
			case Rebex.Security.Certificates.KeyAlgorithm.DSA:
			{
				fnxie fnxie = new fnxie();
				hfnnn.oalpn(fnxie, array, 0, num);
				lohwj = fnxie.rswkn();
				kaeaj = null;
				apmfs = fnxie.oziuq();
				goto IL_03d3;
			}
			case Rebex.Security.Certificates.KeyAlgorithm.RSA:
			{
				elzlx p7 = new elzlx();
				hfnnn.oalpn(p7, array, 0, num);
				lohwj = new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.113549.1.1.1"), new mdvaz().ionjf());
				kaeaj = null;
				apmfs = new rwolq(array, 0, num);
				goto IL_03d3;
			}
			default:
				throw new CryptographicException("Unsupported key algorithm.");
			case Rebex.Security.Certificates.KeyAlgorithm.ECDsa:
				break;
				IL_03d3:
				jmfxr = new zjcch(0);
				quysu = null;
				mmtss = null;
				pbhla = null;
				pktdj = null;
				return;
			}
			goto case hhmob.nqijj;
		case hhmob.vtdze:
		{
			tbinn tbinn = new tbinn();
			hfnnn.oalpn(tbinn, array, 0, num);
			array = tbinn.nbero(password);
			num = array.Length;
			goto case hhmob.ajugp;
		}
		case hhmob.ajugp:
		{
			PrivateKeyInfo p6 = new PrivateKeyInfo();
			hfnnn.oalpn(p6, array, 0, num);
			ujnfb(p6);
			break;
		}
		case hhmob.hsjtt:
		{
			PrivateKeyInfo p5 = new qfmgt().ttubb(array, password);
			ujnfb(p5);
			break;
		}
		case hhmob.nqijj:
		{
			tsnbe tsnbe = new tsnbe();
			hfnnn.oalpn(tsnbe, array, 0, num);
			jmfxr = new zjcch(0);
			if (tsnbe.ckbjk == null || 1 == 0)
			{
				throw new CryptographicException("Missing curve.");
			}
			byte[] parameters = fxakl.kncuz(tsnbe.ckbjk);
			lohwj = new AlgorithmIdentifier("1.2.840.10045.2.1", parameters);
			kaeaj = null;
			apmfs = new rwolq(array, 0, num);
			quysu = null;
			mmtss = null;
			pbhla = null;
			pktdj = null;
			break;
		}
		case hhmob.xxczq:
			ojmtd(new StringReader(p3), password);
			break;
		default:
			throw new CryptographicException("Not a private key.");
		}
	}

	private void ujnfb(PrivateKeyInfo p0)
	{
		if (p0.lohwj.Oid.Value == "1.3.6.1.4.1.11591.15.1" && 0 == 0)
		{
			byte[] rtrhq = p0.apmfs.rtrhq;
			dteyh(rtrhq);
		}
		else
		{
			jmfxr = new zjcch(0);
			lohwj = p0.lohwj;
			apmfs = p0.apmfs;
			mmtss = p0.mmtss;
			kaeaj = null;
			pbhla = null;
			pktdj = null;
		}
		quysu = p0.quysu;
	}

	private void dteyh(byte[] p0)
	{
		if (p0.Length != 64)
		{
			throw new CryptographicException("Unexpected private key length.");
		}
		byte[] p1 = p0.wwots(0, 32);
		byte[] p2 = p0.wwots(32, 32);
		bpcel(p1, p2);
	}

	private void bpcel(byte[] p0, byte[] p1)
	{
		if (p0.Length != 32)
		{
			throw new CryptographicException("Unexpected seed length.");
		}
		if (p1 != null && 0 == 0 && p1.Length != 32)
		{
			throw new CryptographicException("Unexpected seed length.");
		}
		jmfxr = new zjcch(0);
		lohwj = new AlgorithmIdentifier("1.3.101.112", null);
		kaeaj = null;
		apmfs = new rwolq(new rwolq(p0).ionjf());
		mmtss = ((p1 == null) ? null : new htykq(p1, 0));
		quysu = null;
		pbhla = null;
		pktdj = null;
	}

	public void Load(string fileName, string password)
	{
		if (fileName == null || 1 == 0)
		{
			throw new ArgumentNullException("fileName");
		}
		Stream stream = vtdxm.prsfm(fileName);
		try
		{
			Load(stream, password);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	private byte[] fkwmd(TextReader p0, string p1)
	{
		if (p1.Length < 0 || p1.Length > 2)
		{
			throw new CryptographicException("Invalid private key format.");
		}
		int num;
		try
		{
			num = int.Parse(p1, CultureInfo.InvariantCulture);
		}
		catch
		{
			throw new CryptographicException("Invalid private key format.");
		}
		StringBuilder stringBuilder = new StringBuilder();
		while (num > 0)
		{
			string value = p0.ReadLine();
			stringBuilder.Append(value);
			num--;
		}
		try
		{
			return Convert.FromBase64String(stringBuilder.ToString());
		}
		catch (Exception inner)
		{
			throw new CryptographicException("Invalid private key format.", inner);
		}
	}

	private void ojmtd(TextReader p0, string p1)
	{
		jmfxr = new zjcch(0);
		quysu = null;
		mmtss = null;
		int num = 0;
		string text = null;
		string text2 = null;
		byte[] array = null;
		byte[] array2 = null;
		byte[] array3 = null;
		byte[] array4 = null;
		byte[] array5 = null;
		ffooh? ffooh = null;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		while (true)
		{
			string text3 = p0.ReadLine();
			if (text3 == null || 1 == 0)
			{
				p0.Close();
				break;
			}
			int num5 = text3.IndexOf(": ");
			if (num5 <= 0)
			{
				p0.Close();
				break;
			}
			string text4 = text3.Substring(0, num5);
			string text5 = text3.Substring(num5 + 2);
			try
			{
				string key;
				if ((key = text4) != null && 0 == 0)
				{
					if (fnfqw.cfzxv == null || 1 == 0)
					{
						fnfqw.cfzxv = new Dictionary<string, int>(12)
						{
							{ "PuTTY-User-Key-File-2", 0 },
							{ "PuTTY-User-Key-File-3", 1 },
							{ "Comment", 2 },
							{ "Encryption", 3 },
							{ "Public-Lines", 4 },
							{ "Private-Lines", 5 },
							{ "Private-MAC", 6 },
							{ "Key-Derivation", 7 },
							{ "Argon2-Memory", 8 },
							{ "Argon2-Passes", 9 },
							{ "Argon2-Parallelism", 10 },
							{ "Argon2-Salt", 11 }
						};
					}
					if (fnfqw.cfzxv.TryGetValue(key, out var value) && 0 == 0)
					{
						switch (value)
						{
						case 0:
							if ((num != 0) ? true : false)
							{
								break;
							}
							num = 2;
							text = text5;
							goto end_IL_0095;
						case 1:
							if ((num != 0) ? true : false)
							{
								break;
							}
							num = 3;
							text = text5;
							goto end_IL_0095;
						case 2:
							array4 = EncodingTools.Default.GetBytes(text5);
							goto end_IL_0095;
						case 3:
							text2 = text5;
							goto end_IL_0095;
						case 4:
							array = fkwmd(p0, text5);
							goto end_IL_0095;
						case 5:
							array2 = fkwmd(p0, text5);
							goto end_IL_0095;
						case 6:
							array3 = brgjd.qaycu(text5);
							goto end_IL_0095;
						case 7:
							if (num != 3)
							{
								break;
							}
							if (text5 == "Argon2id" && 0 == 0)
							{
								ffooh = onrkn.ffooh.vefjh;
								continue;
							}
							if (text5 == "Argon2i" && 0 == 0)
							{
								ffooh = onrkn.ffooh.gjqsj;
								continue;
							}
							if (text5 == "Argon2d")
							{
								ffooh = onrkn.ffooh.gzsyh;
								continue;
							}
							throw new CryptographicException("Unsupported PPK password-hashing function.");
						case 8:
							if (num != 3)
							{
								break;
							}
							num2 = int.Parse(text5, CultureInfo.InvariantCulture);
							goto end_IL_0095;
						case 9:
							if (num != 3)
							{
								break;
							}
							num3 = int.Parse(text5, CultureInfo.InvariantCulture);
							goto end_IL_0095;
						case 10:
							if (num != 3)
							{
								break;
							}
							num4 = int.Parse(text5, CultureInfo.InvariantCulture);
							goto end_IL_0095;
						case 11:
							if (num != 3)
							{
								break;
							}
							array5 = brgjd.qaycu(text5);
							goto end_IL_0095;
						}
					}
				}
				if (text4.StartsWith("PuTTY-User-Key-File-", StringComparison.Ordinal) && 0 == 0)
				{
					throw new CryptographicException("Unsupported PPK key file format.");
				}
				end_IL_0095:;
			}
			catch (FormatException inner)
			{
				throw new CryptographicException("Error while parsing PPK key file.", inner);
			}
			catch (OverflowException inner2)
			{
				throw new CryptographicException("Error while parsing PPK key file.", inner2);
			}
		}
		if (num == 0 || false || text == null || false || text2 == null || false || array == null || false || array2 == null || false || array3 == null || false || array4 == null)
		{
			throw new CryptographicException("Invalid private key format.");
		}
		byte[] array6;
		if (!(text2 != "none"))
		{
			array6 = ((num < 3) ? crzbn(null) : new byte[0]);
		}
		else
		{
			if (text2 != "aes256-cbc" && 0 == 0)
			{
				throw new CryptographicException("Unsupported private key encryption algorithm.");
			}
			byte[] array7 = ((p1 == null) ? new byte[0] : EncodingTools.Default.GetBytes(p1));
			byte[] array9;
			byte[] array10;
			if (num >= 3)
			{
				if (!ffooh.HasValue || false || num2 < 0 || num3 < 0 || num4 < 0)
				{
					throw new CryptographicException("Missing PPK key parameters.");
				}
				byte[] array8 = new byte[80];
				array9 = new byte[32];
				array10 = new byte[16];
				array6 = new byte[32];
				byte[] array11 = new byte[0];
				rkpix p2 = new rkpix(ffooh.Value, num2, num3, num4);
				gpkne.zxiwe(array7, array11, array5, array11, p2, array8);
				Array.Copy(array8, 0, array9, 0, 32);
				Array.Copy(array8, 32, array10, 0, 16);
				Array.Copy(array8, 48, array6, 0, 32);
			}
			else
			{
				array9 = subce(array7);
				array10 = new byte[16];
				array6 = crzbn(array7);
			}
			SymmetricKeyAlgorithm symmetricKeyAlgorithm = new SymmetricKeyAlgorithm(SymmetricKeyAlgorithmId.AES);
			try
			{
				symmetricKeyAlgorithm.Mode = CipherMode.CBC;
				symmetricKeyAlgorithm.Padding = PaddingMode.None;
				symmetricKeyAlgorithm.SetKey(array9);
				symmetricKeyAlgorithm.SetIV(array10);
				ICryptoTransform cryptoTransform = symmetricKeyAlgorithm.CreateDecryptor();
				try
				{
					array2 = cryptoTransform.TransformFinalBlock(array2, 0, array2.Length);
				}
				finally
				{
					if (cryptoTransform != null && 0 == 0)
					{
						cryptoTransform.Dispose();
					}
				}
			}
			finally
			{
				if (symmetricKeyAlgorithm != null && 0 == 0)
				{
					((IDisposable)symmetricKeyAlgorithm).Dispose();
				}
			}
		}
		HashingAlgorithmId p3 = ((num < 3) ? HashingAlgorithmId.SHA1 : HashingAlgorithmId.SHA256);
		byte[] p4 = krofs(array6, text, array, array2, text2, p3, array4);
		if (!zjcch.wduyr(p4, array3) || 1 == 0)
		{
			throw new CryptographicException("Invalid password or bad data.");
		}
		BinaryReader binaryReader = new BinaryReader(new MemoryStream(array, writable: false));
		BinaryReader binaryReader2 = new BinaryReader(new MemoryStream(array2, writable: false));
		try
		{
			string key2;
			if ((key2 = text) != null && 0 == 0)
			{
				if (fnfqw.oqctn == null || 1 == 0)
				{
					fnfqw.oqctn = new Dictionary<string, int>(6)
					{
						{ "ssh-rsa", 0 },
						{ "ssh-dss", 1 },
						{ "ssh-ed25519", 2 },
						{ "ecdsa-sha2-nistp256", 3 },
						{ "ecdsa-sha2-nistp384", 4 },
						{ "ecdsa-sha2-nistp521", 5 }
					};
				}
				if (fnfqw.oqctn.TryGetValue(key2, out var value2) && 0 == 0)
				{
					switch (value2)
					{
					case 0:
					{
						elzlx p5 = new elzlx(binaryReader, binaryReader2);
						lohwj = new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.113549.1.1.1"), new mdvaz().ionjf());
						kaeaj = null;
						byte[] array14 = fxakl.kncuz(p5);
						apmfs = new rwolq(array14, 0, array14.Length);
						goto end_IL_05ae;
					}
					case 1:
					{
						fnxie fnxie = new fnxie(binaryReader, binaryReader2);
						lohwj = fnxie.rswkn();
						kaeaj = null;
						apmfs = fnxie.oziuq();
						goto end_IL_05ae;
					}
					case 2:
					{
						byte[] p6 = ghzbc(binaryReader2);
						byte[] array12 = ghzbc(binaryReader);
						if (EncodingTools.ASCII.GetString(array12, 0, array12.Length) != text && 0 == 0)
						{
							throw new CryptographicException("Public key algorithm mismatch.");
						}
						byte[] p7 = ghzbc(binaryReader);
						bpcel(p6, p7);
						goto end_IL_05ae;
					}
					case 3:
					case 4:
					case 5:
					{
						byte[] array12 = ghzbc(binaryReader);
						byte[] array13 = ghzbc(binaryReader);
						if (EncodingTools.ASCII.GetString(array12, 0, array12.Length) != text && 0 == 0)
						{
							throw new CryptographicException("Public key algorithm mismatch.");
						}
						byte[] publicKey = ghzbc(binaryReader);
						byte[] privateKey = ghzbc(binaryReader2);
						string text6 = bpkgq.mjwcm(EncodingTools.ASCII.GetString(array13, 0, array13.Length));
						if (text6 == null || 1 == 0)
						{
							throw new CryptographicException("Unsupported curve.");
						}
						PrivateKeyInfo privateKeyInfo = new PrivateKeyInfo(new tsnbe(privateKey, publicKey, text6), AsymmetricKeyAlgorithmId.ECDsa);
						lohwj = privateKeyInfo.lohwj;
						kaeaj = null;
						apmfs = privateKeyInfo.apmfs;
						goto end_IL_05ae;
					}
					}
				}
			}
			throw new CryptographicException("Unsupported private key algorithm.");
			end_IL_05ae:;
		}
		finally
		{
			binaryReader.Close();
			binaryReader2.Close();
		}
		pbhla = EncodingTools.Default.GetString(array4, 0, array4.Length);
		pktdj = null;
	}

	internal static byte[] ghzbc(BinaryReader p0)
	{
		byte[] array = p0.ReadBytes(4);
		if (BitConverter.IsLittleEndian && 0 == 0)
		{
			Array.Reverse(array, 0, array.Length);
		}
		int count = BitConverter.ToInt32(array, 0);
		return p0.ReadBytes(count);
	}

	private static void qobrv(bool p0, IHashTransform p1, byte[] p2)
	{
		if (p0 && 0 == 0)
		{
			byte[] bytes = BitConverter.GetBytes(p2.Length);
			if (BitConverter.IsLittleEndian && 0 == 0)
			{
				Array.Reverse(bytes, 0, bytes.Length);
			}
			p1.Process(bytes, 0, bytes.Length);
		}
		p1.Process(p2, 0, p2.Length);
	}

	internal static byte[] dgros(byte[] p0, int p1, int p2)
	{
		int num = p0[p1 - 1];
		if (num == 0 || false || num > p2 || num > p1)
		{
			throw new CryptographicException("Invalid password or bad data.");
		}
		int num2 = 2;
		if (num2 == 0)
		{
			goto IL_002a;
		}
		goto IL_0041;
		IL_0041:
		if (num2 > num)
		{
			byte[] array = new byte[p1 - num];
			Array.Copy(p0, 0, array, 0, array.Length);
			return array;
		}
		goto IL_002a;
		IL_002a:
		if (p0[p1 - num2] != num)
		{
			throw new CryptographicException("Invalid password or bad data.");
		}
		num2++;
		goto IL_0041;
	}

	private static void xxpan(SymmetricKeyAlgorithm p0, byte[] p1, byte[] p2)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("key");
		}
		if (p2 == null || 1 == 0)
		{
			throw new ArgumentNullException("iv");
		}
		p0.SetKey(p1);
		p0.SetIV(p2);
	}

	internal static byte[] viqym(byte[] p0, byte[] p1, byte[] p2, SymmetricKeyAlgorithm p3)
	{
		return euijk(p0, 0, p0.Length, p1, p2, p3);
	}

	private static byte[] euijk(byte[] p0, int p1, int p2, byte[] p3, byte[] p4, SymmetricKeyAlgorithm p5)
	{
		if (p5 == null || 1 == 0)
		{
			return p0;
		}
		xxpan(p5, p3, p4);
		ICryptoTransform cryptoTransform = p5.CreateDecryptor();
		return cryptoTransform.TransformFinalBlock(p0, p1, p2);
	}

	internal static byte[] cntzn(byte[] p0, byte[] p1, byte[] p2, SymmetricKeyAlgorithm p3)
	{
		return jumwb(p0, 0, p0.Length, p1, p2, p3);
	}

	private static byte[] jumwb(byte[] p0, int p1, int p2, byte[] p3, byte[] p4, SymmetricKeyAlgorithm p5)
	{
		if (p5 == null || 1 == 0)
		{
			return p0;
		}
		xxpan(p5, p3, p4);
		ICryptoTransform cryptoTransform = p5.CreateEncryptor();
		return cryptoTransform.TransformFinalBlock(p0, p1, p2);
	}

	private static byte[] yhkby(byte[] p0, string p1, out byte[] p2)
	{
		SymmetricKeyAlgorithm symmetricKeyAlgorithm = new SymmetricKeyAlgorithm("3DES");
		try
		{
			symmetricKeyAlgorithm.Mode = CipherMode.CBC;
			symmetricKeyAlgorithm.Padding = PaddingMode.PKCS7;
			symmetricKeyAlgorithm.KeySize = 192;
			symmetricKeyAlgorithm.GenerateIV();
			p2 = symmetricKeyAlgorithm.GetIV();
			HashingAlgorithmId alg = (HashingAlgorithmId)65543;
			rbplv generator = new rbplv(alg, p1, p2, 1);
			symmetricKeyAlgorithm.DeriveKey(generator);
			ICryptoTransform cryptoTransform = symmetricKeyAlgorithm.CreateEncryptor();
			return cryptoTransform.TransformFinalBlock(p0, 0, p0.Length);
		}
		finally
		{
			if (symmetricKeyAlgorithm != null && 0 == 0)
			{
				((IDisposable)symmetricKeyAlgorithm).Dispose();
			}
		}
	}

	private void qeduz(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in qeduz
		this.qeduz(p0, p1, p2);
	}

	private lnabj xqfgn(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			return jmfxr;
		case 1:
			lohwj = new AlgorithmIdentifier();
			return lohwj;
		case 2:
			tazxq = true;
			return apmfs;
		case 65536:
			quysu = new CryptographicAttributeCollection();
			return new rwknq(quysu, 0, rmkkr.wguaf);
		case 65537:
			mmtss = new htykq();
			return new rwknq(mmtss, 1, rmkkr.ysphu);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xqfgn
		return this.xqfgn(p0, p1, p2);
	}

	private void kphhl(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in kphhl
		this.kphhl(p0, p1, p2);
	}

	private void wdfjv()
	{
		if (lohwj == null || 1 == 0)
		{
			throw new CryptographicException("Private key algorithm not found in PrivateKeyInfo.");
		}
		if (!tazxq || 1 == 0)
		{
			throw new CryptographicException("Private key not found in PrivateKeyInfo.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in wdfjv
		this.wdfjv();
	}

	private void xtbnn(fxakl p0)
	{
		kwtyo();
		List<lnabj> list = new List<lnabj>();
		list.Add(jmfxr);
		list.Add(lohwj);
		list.Add(apmfs);
		if (quysu != null && 0 == 0)
		{
			list.Add(new rwknq(quysu, 0, rmkkr.wguaf));
		}
		if (mmtss != null && 0 == 0 && lohwj.Oid.Value != "1.3.101.112" && 0 == 0)
		{
			list.Add(new rwknq(mmtss, 1, rmkkr.ysphu));
		}
		p0.qjrka(list);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xtbnn
		this.xtbnn(p0);
	}
}
