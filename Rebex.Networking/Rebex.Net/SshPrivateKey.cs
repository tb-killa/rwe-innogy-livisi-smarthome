using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;
using onrkn;

namespace Rebex.Net;

public class SshPrivateKey : SshPublicKey
{
	public Certificate Certificate => pluuw;

	internal override bool dttop => pluuw != null;

	public static SshPrivateKey Generate()
	{
		return Generate(SshHostKeyAlgorithm.RSA, 1024);
	}

	public static SshPrivateKey Generate(SshHostKeyAlgorithm algorithm)
	{
		return Generate(algorithm, 0);
	}

	public static SshPrivateKey Generate(SshHostKeyAlgorithm algorithm, int keySize)
	{
		AsymmetricKeyAlgorithmId p;
		string p2;
		switch (algorithm)
		{
		case SshHostKeyAlgorithm.RSA:
			p = AsymmetricKeyAlgorithmId.RSA;
			p2 = null;
			break;
		case SshHostKeyAlgorithm.DSS:
			p = AsymmetricKeyAlgorithmId.DSA;
			p2 = null;
			break;
		case SshHostKeyAlgorithm.ED25519:
			p = AsymmetricKeyAlgorithmId.EdDsa;
			p2 = "1.3.101.112";
			break;
		case SshHostKeyAlgorithm.ECDsaNistP256:
			p = AsymmetricKeyAlgorithmId.ECDsa;
			p2 = "1.2.840.10045.3.1.7";
			break;
		case SshHostKeyAlgorithm.ECDsaNistP384:
			p = AsymmetricKeyAlgorithmId.ECDsa;
			p2 = "1.3.132.0.34";
			break;
		case SshHostKeyAlgorithm.ECDsaNistP521:
			p = AsymmetricKeyAlgorithmId.ECDsa;
			p2 = "1.3.132.0.35";
			break;
		default:
			throw new CryptographicException("Key generation for this algorithm is not supported.");
		}
		SshPrivateKey sshPrivateKey = new SshPrivateKey();
		sshPrivateKey.KeyAlgorithmId = ikgnw.ovsfz(algorithm);
		sshPrivateKey.rdbcs = new AsymmetricKeyAlgorithm();
		sshPrivateKey.rdbcs.kvrol(p, p2, keySize);
		return sshPrivateKey;
	}

	public static SshPrivateKey CreateFrom(RSAParameters parameters)
	{
		if (parameters.Modulus == null || false || parameters.Exponent == null || false || parameters.D == null || false || parameters.P == null || false || parameters.Q == null || false || parameters.DP == null || false || parameters.DQ == null || false || parameters.InverseQ == null)
		{
			throw new CryptographicException("One or more parameters is missing.");
		}
		SshPrivateKey sshPrivateKey = new SshPrivateKey();
		sshPrivateKey.rdbcs = new AsymmetricKeyAlgorithm();
		sshPrivateKey.rdbcs.ImportKey(parameters);
		sshPrivateKey.KeyAlgorithmId = "ssh-rsa";
		return sshPrivateKey;
	}

	public static SshPrivateKey CreateFrom(DSAParameters parameters)
	{
		if (parameters.P == null || false || parameters.Q == null || false || parameters.G == null || false || parameters.X == null)
		{
			throw new CryptographicException("One or more parameters is missing.");
		}
		SshPrivateKey sshPrivateKey = new SshPrivateKey();
		sshPrivateKey.rdbcs = new AsymmetricKeyAlgorithm();
		sshPrivateKey.rdbcs.ImportKey(parameters);
		sshPrivateKey.KeyAlgorithmId = "ssh-dss";
		return sshPrivateKey;
	}

	private SshPrivateKey()
	{
	}

	public SshPrivateKey(Certificate certificate)
	{
		if (certificate == null || 1 == 0)
		{
			throw new ArgumentNullException("certificate");
		}
		pluuw = certificate;
		lwlrh = CertificateChain.BuildFrom(certificate);
		base.KeyAlgorithmId = SshPublicKey.wusno(certificate);
		base.Comment = pluuw.GetSubjectName();
	}

	public SshPrivateKey(CertificateChain chain)
	{
		if (chain == null || 1 == 0)
		{
			throw new ArgumentNullException("chain");
		}
		Certificate leafCertificate = chain.LeafCertificate;
		if (chain.LeafCertificate == null || 1 == 0)
		{
			throw new InvalidOperationException("Leaf certificate is missing.");
		}
		pluuw = leafCertificate;
		lwlrh = chain;
		base.KeyAlgorithmId = SshPublicKey.wusno(leafCertificate);
		base.Comment = pluuw.GetSubjectName();
	}

	public SshPrivateKey(string path, string password)
	{
		if (path == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		Stream stream = vtdxm.prsfm(path);
		try
		{
			yutoh(stream, password);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public SshPrivateKey(Stream input, string password)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		byte[] array = new byte[32767];
		int num = input.Read(array, 0, array.Length);
		if (num >= 32767)
		{
			throw new CryptographicException("File is too long.");
		}
		input = new MemoryStream(array, 0, num, writable: false, publiclyVisible: false);
		yutoh(input, password);
	}

	public SshPrivateKey(byte[] data, string password)
	{
		if (data == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		yutoh(new MemoryStream(data, 0, data.Length, writable: false, publiclyVisible: false), password);
	}

	public SshPrivateKey(AsymmetricAlgorithm algorithm)
	{
		rdbcs = AsymmetricKeyAlgorithm.CreateFrom(algorithm, ownsAlgorithm: false);
		switch (rdbcs.Algorithm)
		{
		case AsymmetricKeyAlgorithmId.RSA:
			base.KeyAlgorithmId = "ssh-rsa";
			break;
		case AsymmetricKeyAlgorithmId.DSA:
			base.KeyAlgorithmId = "ssh-dss";
			break;
		default:
			throw new ArgumentException("Unsupported algorithm.", "algorithm");
		}
	}

	private void yutoh(Stream p0, string p1)
	{
		try
		{
			PrivateKeyInfo privateKeyInfo = new PrivateKeyInfo();
			privateKeyInfo.Load(p0, p1);
			AlgorithmIdentifier keyAlgorithm = privateKeyInfo.KeyAlgorithm;
			string value;
			if ((value = keyAlgorithm.Oid.Value) != null && 0 == 0)
			{
				if (awprl.ldsvq == null || 1 == 0)
				{
					awprl.ldsvq = new Dictionary<string, int>(7)
					{
						{ "1.2.840.113549.1.1.1", 0 },
						{ "1.2.840.10040.4.1", 1 },
						{ "1.3.101.112", 2 },
						{ "1.2.840.10045.4.3.2", 3 },
						{ "1.2.840.10045.4.3.3", 4 },
						{ "1.2.840.10045.4.3.4", 5 },
						{ "1.2.840.10045.2.1", 6 }
					};
				}
				if (awprl.ldsvq.TryGetValue(value, out var value2) && 0 == 0)
				{
					switch (value2)
					{
					case 0:
						base.KeyAlgorithmId = "ssh-rsa";
						goto IL_01f9;
					case 1:
						base.KeyAlgorithmId = "ssh-dss";
						goto IL_01f9;
					case 2:
						base.KeyAlgorithmId = "ssh-ed25519";
						goto IL_01f9;
					case 3:
						base.KeyAlgorithmId = "ecdsa-sha2-nistp256";
						goto IL_01f9;
					case 4:
						base.KeyAlgorithmId = "ecdsa-sha2-nistp384";
						goto IL_01f9;
					case 5:
						base.KeyAlgorithmId = "ecdsa-sha2-nistp521";
						goto IL_01f9;
					case 6:
						{
							string text;
							if ((text = bpkgq.cafoz(keyAlgorithm)) == null)
							{
								goto IL_01c5;
							}
							if (!(text == "1.2.840.10045.3.1.7") || 1 == 0)
							{
								if (!(text == "1.3.132.0.34") || 1 == 0)
								{
									if (!(text == "1.3.132.0.35") || 1 == 0)
									{
										goto IL_01c5;
									}
									base.KeyAlgorithmId = "ecdsa-sha2-nistp521";
								}
								else
								{
									base.KeyAlgorithmId = "ecdsa-sha2-nistp384";
								}
							}
							else
							{
								base.KeyAlgorithmId = "ecdsa-sha2-nistp256";
							}
							goto IL_01f9;
						}
						IL_01f9:
						rdbcs = new AsymmetricKeyAlgorithm();
						rdbcs.ImportKey(privateKeyInfo);
						base.Comment = privateKeyInfo.Comment;
						return;
						IL_01c5:
						throw new CryptographicException("Unsupported curve.");
					}
				}
			}
			throw new CryptographicException(brgjd.edcru("Unsupported key algorithm ({0}).", keyAlgorithm.Oid.Value));
		}
		finally
		{
			p0.Close();
		}
	}

	public PrivateKeyInfo GetPrivateKeyInfo()
	{
		if (pluuw != null && 0 == 0)
		{
			return pluuw.GetPrivateKeyInfo(silent: true);
		}
		string keyAlgorithmId;
		if ((keyAlgorithmId = base.KeyAlgorithmId) != null && 0 == 0)
		{
			if (awprl.yzkre == null || 1 == 0)
			{
				awprl.yzkre = new Dictionary<string, int>(6)
				{
					{ "ssh-rsa", 0 },
					{ "ssh-dss", 1 },
					{ "ssh-ed25519", 2 },
					{ "ecdsa-sha2-nistp256", 3 },
					{ "ecdsa-sha2-nistp384", 4 },
					{ "ecdsa-sha2-nistp521", 5 }
				};
			}
			if (awprl.yzkre.TryGetValue(keyAlgorithmId, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				{
					PrivateKeyInfo privateKey = rdbcs.GetPrivateKey();
					privateKey.Comment = base.Comment;
					return privateKey;
				}
				}
			}
		}
		throw new InvalidOperationException("Unknown key algorithm.");
	}

	[wptwl(false)]
	[Obsolete("This method has been deprecated. Please use Save(Stream, string, SshPrivateKeyFormat) instead.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void Save(Stream output, string password, object encryptionAlgorithm)
	{
		if (output == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		PrivateKeyFormat p;
		string oid = brqbd(encryptionAlgorithm, out p);
		PrivateKeyInfo privateKeyInfo = GetPrivateKeyInfo();
		if (p == PrivateKeyFormat.Base64Pkcs8 || 1 == 0)
		{
			privateKeyInfo.Save(output, password, new ObjectIdentifier(oid));
		}
		else
		{
			privateKeyInfo.Save(output, password, p);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	[Obsolete("This method has been deprecated. Please use Save(string, string, SshPrivateKeyFormat) instead.", false)]
	public void Save(string path, string password, object encryptionAlgorithm)
	{
		if (path == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		PrivateKeyFormat p;
		string oid = brqbd(encryptionAlgorithm, out p);
		PrivateKeyInfo privateKeyInfo = GetPrivateKeyInfo();
		Stream stream = vtdxm.bolpl(path);
		try
		{
			if (p == PrivateKeyFormat.Base64Pkcs8 || 1 == 0)
			{
				privateKeyInfo.Save(stream, password, new ObjectIdentifier(oid));
			}
			else
			{
				privateKeyInfo.Save(stream, password, p);
			}
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public void Save(Stream output, string password, SshPrivateKeyFormat format)
	{
		if (output == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		PrivateKeyInfo privateKeyInfo = GetPrivateKeyInfo();
		privateKeyInfo.Save(output, password, (PrivateKeyFormat)format);
	}

	public void Save(string path, string password, SshPrivateKeyFormat format)
	{
		if (path == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		Stream stream = vtdxm.bolpl(path);
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

	private static string brqbd(object p0, out PrivateKeyFormat p1)
	{
		if (p0 == null || 1 == 0)
		{
			p0 = "3DES";
		}
		if (p0 is Type && 0 == 0)
		{
			if (p0.GetType().IsSubclassOf(typeof(DES)) && 0 == 0)
			{
				p0 = "DES";
			}
			else if (p0.GetType().IsSubclassOf(typeof(TripleDES)) && 0 == 0)
			{
				p0 = "3DES";
			}
			else if (p0.GetType().IsSubclassOf(typeof(RC2)) && 0 == 0)
			{
				p0 = "RC2";
			}
		}
		string text = p0 as string;
		if (text != null && 0 == 0)
		{
			text = text.ToLower(CultureInfo.InvariantCulture);
		}
		string key;
		if ((key = text) != null && 0 == 0)
		{
			if (awprl.khemb == null || 1 == 0)
			{
				awprl.khemb = new Dictionary<string, int>(10)
				{
					{ "putty", 0 },
					{ "ssleay", 1 },
					{ "openssh", 2 },
					{ "tripledes", 3 },
					{ "3des", 4 },
					{ "1.2.840.113549.3.7", 5 },
					{ "des", 6 },
					{ "1.3.14.3.2.7", 7 },
					{ "rc2", 8 },
					{ "1.2.840.113549.3.2", 9 }
				};
			}
			if (awprl.khemb.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
					p1 = PrivateKeyFormat.Putty;
					return null;
				case 1:
				case 2:
					p1 = PrivateKeyFormat.OpenSsh;
					return null;
				case 3:
				case 4:
				case 5:
					p1 = PrivateKeyFormat.Base64Pkcs8;
					return "1.2.840.113549.3.7";
				case 6:
				case 7:
					p1 = PrivateKeyFormat.Base64Pkcs8;
					return "1.3.14.3.2.7";
				case 8:
				case 9:
					p1 = PrivateKeyFormat.Base64Pkcs8;
					return "1.2.840.113549.3.2";
				}
			}
		}
		throw new ArgumentException("Unsupported algorithm.", "encryptionAlgorithm");
	}

	public byte[] GetPrivateKey()
	{
		PrivateKeyInfo privateKeyInfo = GetPrivateKeyInfo();
		return privateKeyInfo.hsjue();
	}

	public new DSAParameters GetDSAParameters()
	{
		return wvjny(p0: true);
	}

	public new RSAParameters GetRSAParameters()
	{
		return zmisf(p0: true);
	}

	internal override DSAParameters wvjny(bool p0)
	{
		if (pluuw != null && 0 == 0)
		{
			return pluuw.GetDSAParameters(p0, silent: true);
		}
		if (rdbcs.Algorithm != AsymmetricKeyAlgorithmId.DSA)
		{
			throw new CryptographicException("Not a DSA key.");
		}
		try
		{
			if (p0 && 0 == 0)
			{
				return rdbcs.GetPrivateKey().GetDSAParameters();
			}
		}
		catch (CryptographicException inner)
		{
			throw new CryptographicException("Unable to export private key parameters.", inner);
		}
		return rdbcs.GetPublicKey().GetDSAParameters();
	}

	internal override RSAParameters zmisf(bool p0)
	{
		if (pluuw != null && 0 == 0)
		{
			return pluuw.GetRSAParameters(p0, silent: true);
		}
		if (rdbcs == null || 1 == 0)
		{
			throw new InvalidOperationException("This method is only supported for keys, not for certificates.");
		}
		if (rdbcs.Algorithm != AsymmetricKeyAlgorithmId.RSA && 0 == 0)
		{
			throw new CryptographicException("Not an RSA key.");
		}
		try
		{
			if (p0 && 0 == 0)
			{
				return rdbcs.GetPrivateKey().GetRSAParameters();
			}
		}
		catch (CryptographicException inner)
		{
			throw new CryptographicException("Unable to export private key parameters.", inner);
		}
		return rdbcs.GetPublicKey().GetRSAParameters();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	[Obsolete("This method has been deprecated and will be removed.", true)]
	public byte[] CreateSignature(byte[] hash, SignatureHashAlgorithm algorithm)
	{
		if (hash == null || 1 == 0)
		{
			throw new ArgumentNullException("hash");
		}
		string p = mqelq(algorithm);
		return ogvve(hash, p, p2: false);
	}

	internal byte[] ogvve(byte[] p0, string p1, bool p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		HashingAlgorithmId hashingAlgorithmId = SshPublicKey.bvjzt(p1);
		if (hashingAlgorithmId == (HashingAlgorithmId)0 || 1 == 0)
		{
			throw hifyx.nztrs("keyAlgorithmId", p1, "Unsupported key algorithtm.");
		}
		SignatureParameters signatureParameters = new SignatureParameters();
		signatureParameters.Silent = true;
		signatureParameters.Format = SignatureFormat.Raw;
		signatureParameters.HashAlgorithm = hashingAlgorithmId;
		string key;
		if ((key = p1) != null && 0 == 0)
		{
			if (awprl.zptbi == null || 1 == 0)
			{
				awprl.zptbi = new Dictionary<string, int>(13)
				{
					{ "ssh-rsa", 0 },
					{ "rsa-sha2-256", 1 },
					{ "rsa-sha2-512", 2 },
					{ "ssh-rsa-sha256@ssh.com", 3 },
					{ "ssh-dss", 4 },
					{ "ecdsa-sha2-nistp256", 5 },
					{ "ecdsa-sha2-nistp384", 6 },
					{ "ecdsa-sha2-nistp521", 7 },
					{ "ssh-ed25519", 8 },
					{ "x509v3-rsa2048-sha256", 9 },
					{ "x509v3-sign-dss", 10 },
					{ "x509v3-sign-rsa", 11 },
					{ "x509v3-sign-rsa-sha256@ssh.com", 12 }
				};
			}
			if (awprl.zptbi.TryGetValue(key, out var value) && 0 == 0)
			{
				byte[] array;
				tndeg tndeg;
				switch (value)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					if (rdbcs == null)
					{
						break;
					}
					array = rdbcs.SignMessage(p0, signatureParameters);
					if (p2 && 0 == 0)
					{
						array = kumym.yejxo(array);
					}
					goto IL_02c7;
				case 4:
					if (rdbcs == null)
					{
						break;
					}
					array = rdbcs.SignMessage(p0, signatureParameters);
					goto IL_02c7;
				case 5:
				case 6:
				case 7:
					if (rdbcs == null)
					{
						break;
					}
					array = rdbcs.SignMessage(p0, signatureParameters);
					array = ikgnw.ctjzt(array, rdbcs.lbcti());
					goto IL_02c7;
				case 8:
					if (rdbcs == null)
					{
						break;
					}
					array = rdbcs.SignMessage(p0);
					goto IL_02c7;
				case 9:
					if (pluuw == null)
					{
						break;
					}
					array = pluuw.SignMessage(p0, signatureParameters);
					p1 = p1.Substring("x509v3-".Length);
					goto IL_02c7;
				case 10:
					if (pluuw == null)
					{
						break;
					}
					array = pluuw.SignMessage(p0, signatureParameters);
					goto IL_02c7;
				case 11:
				case 12:
					{
						if (pluuw == null)
						{
							break;
						}
						array = pluuw.SignMessage(p0, signatureParameters);
						goto IL_02c7;
					}
					IL_02c7:
					tndeg = new tndeg(EncodingTools.ASCII);
					mkuxt.excko(tndeg, p1);
					mkuxt.lcbhj(tndeg, array, p2: false);
					return tndeg.ToArray();
				}
			}
		}
		throw new CryptographicException("Unsupported key algorithm.");
	}

	internal override byte[] marpi()
	{
		return hsdwk();
	}
}
