using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;
using onrkn;

namespace Rebex.Net;

public class SshPublicKey
{
	private const string rnqyw = "---- BEGIN SSH2 PUBLIC KEY ----";

	private const string bonbd = "---- END SSH2 PUBLIC KEY ----";

	private const string eclbn = "Comment:";

	private string xsfil;

	private SshFingerprint eehhl;

	private byte[] ffsjx;

	internal AsymmetricKeyAlgorithm rdbcs;

	internal Certificate pluuw;

	internal CertificateChain lwlrh;

	private string qxujl;

	public SshHostKeyAlgorithm KeyAlgorithm => snagl(KeyAlgorithmId);

	public string KeyAlgorithmId
	{
		get
		{
			return qxujl;
		}
		internal set
		{
			qxujl = value;
		}
	}

	public int KeySize
	{
		get
		{
			if (rdbcs != null && 0 == 0)
			{
				return rdbcs.KeySize;
			}
			if (pluuw != null && 0 == 0)
			{
				return pluuw.GetKeySize();
			}
			throw new InvalidOperationException("KeySize property is not supported for this key type.");
		}
	}

	public string Comment
	{
		get
		{
			return xsfil;
		}
		set
		{
			xsfil = value;
		}
	}

	public SshFingerprint Fingerprint
	{
		get
		{
			if (eehhl == null || 1 == 0)
			{
				byte[] publicKey = GetPublicKey();
				eehhl = SshFingerprint.Compute(publicKey);
			}
			return eehhl;
		}
	}

	internal virtual bool dttop => false;

	internal SshPublicKey()
	{
	}

	public SshPublicKey(byte[] data)
	{
		if (data == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		if (data[0] != 45)
		{
			aqklf(data);
		}
		else
		{
			lmksk(new MemoryStream(data, writable: false));
		}
	}

	public SshPublicKey(string path)
	{
		if (path == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		Stream stream = vtdxm.prsfm(path);
		try
		{
			lmksk(stream);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public SshPublicKey(Stream input)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		lmksk(input);
	}

	public SshPublicKey(Certificate certificate)
	{
		if (certificate == null || 1 == 0)
		{
			throw new ArgumentNullException("certificate");
		}
		KeyAlgorithmId = wusno(certificate);
		pluuw = certificate;
		lwlrh = CertificateChain.BuildFrom(certificate);
		ffsjx = certificate.GetRawCertData();
	}

	public SshPublicKey(CertificateChain chain)
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
		KeyAlgorithmId = wusno(leafCertificate);
		pluuw = leafCertificate;
		lwlrh = chain;
		ffsjx = leafCertificate.GetRawCertData();
	}

	public SshPublicKey(PublicKeyInfo publicKeyInfo)
	{
		if (publicKeyInfo == null || 1 == 0)
		{
			throw new ArgumentNullException("publicKeyInfo");
		}
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = new AsymmetricKeyAlgorithm();
		asymmetricKeyAlgorithm.ImportKey(publicKeyInfo);
		dooof(asymmetricKeyAlgorithm);
	}

	public SshPublicKey(AsymmetricAlgorithm algorithm)
	{
		if (algorithm == null || 1 == 0)
		{
			throw new ArgumentNullException("algorithm");
		}
		dooof(AsymmetricKeyAlgorithm.CreateFrom(algorithm, ownsAlgorithm: false));
	}

	private void dooof(AsymmetricKeyAlgorithm p0)
	{
		KeyAlgorithmId = kvquc(p0.Algorithm);
		rdbcs = p0;
		ffsjx = hsdwk();
	}

	private void aqklf(byte[] p0)
	{
		ffsjx = p0;
		KeyAlgorithmId = bzkgr(p0, out rdbcs, out pluuw);
	}

	private void lmksk(Stream p0)
	{
		byte[] array = new byte[32767];
		int num = zrwmt.ewhcy(p0, array, 0, array.Length);
		if (num >= 32767)
		{
			throw new CryptographicException("File is too long.");
		}
		if (num == 0 || 1 == 0)
		{
			throw new CryptographicException("Not a public key.");
		}
		string text = EncodingTools.Default.GetString(array, 0, num);
		text = text.Replace("\r", "").Trim().Trim('\n')
			.Trim();
		int num2;
		if (text.StartsWith("-") && 0 == 0)
		{
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_009e;
			}
			goto IL_00b7;
		}
		byte[] array2 = new byte[num];
		Array.Copy(array, array2, num);
		goto IL_00dc;
		IL_00b7:
		if (num2 < num)
		{
			goto IL_009e;
		}
		array2 = mqpsi(text);
		goto IL_00dc;
		IL_009e:
		if (array[num2] >= 127)
		{
			throw new CryptographicException("Invalid key format.");
		}
		num2++;
		goto IL_00b7;
		IL_00dc:
		aqklf(array2);
	}

	private static byte[] mqpsi(string p0)
	{
		if (!p0.StartsWith("---- BEGIN SSH2 PUBLIC KEY ----", StringComparison.Ordinal) || 1 == 0)
		{
			throw new CryptographicException("Invalid key format.");
		}
		if (!p0.EndsWith("---- END SSH2 PUBLIC KEY ----", StringComparison.Ordinal) || 1 == 0)
		{
			throw new CryptographicException("Invalid key format.");
		}
		p0 = p0.Substring("---- BEGIN SSH2 PUBLIC KEY ----".Length, p0.Length - "---- BEGIN SSH2 PUBLIC KEY ----".Length - "---- END SSH2 PUBLIC KEY ----".Length - 1);
		p0 = p0.Trim().Trim('\n').Trim();
		if (p0.StartsWith("Comment:") && 0 == 0)
		{
			int num = p0.IndexOf('\n');
			p0 = p0.Substring(num + 1);
			p0 = p0.Trim().Trim('\n').Trim();
		}
		try
		{
			return Convert.FromBase64String(p0);
		}
		catch (FormatException inner)
		{
			throw new CryptographicException("Invalid Base-64 encoding of a key.", inner);
		}
	}

	internal static HashingAlgorithmId bvjzt(string p0)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (awprl.bgbrk == null || 1 == 0)
			{
				awprl.bgbrk = new Dictionary<string, int>(15)
				{
					{ "ssh-rsa", 0 },
					{ "ssh-dss", 1 },
					{ "x509v3-sign-rsa", 2 },
					{ "x509v3-sign-dss", 3 },
					{ "rsa-sha2-256", 4 },
					{ "ecdsa-sha2-nistp256", 5 },
					{ "ssh-rsa-sha256@ssh.com", 6 },
					{ "x509v3-rsa2048-sha256", 7 },
					{ "x509v3-sign-rsa-sha256@ssh.com", 8 },
					{ "ssh-rsa-sha384@ssh.com", 9 },
					{ "ecdsa-sha2-nistp384", 10 },
					{ "ssh-rsa-sha512@ssh.com", 11 },
					{ "rsa-sha2-512", 12 },
					{ "ecdsa-sha2-nistp521", 13 },
					{ "ssh-ed25519", 14 }
				};
			}
			if (awprl.bgbrk.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					return HashingAlgorithmId.SHA1;
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
					return HashingAlgorithmId.SHA256;
				case 9:
				case 10:
					return HashingAlgorithmId.SHA384;
				case 11:
				case 12:
				case 13:
				case 14:
					return HashingAlgorithmId.SHA512;
				}
			}
		}
		return (HashingAlgorithmId)0;
	}

	internal static SshHostKeyAlgorithm snagl(string p0)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (awprl.iynko == null || 1 == 0)
			{
				awprl.iynko = new Dictionary<string, int>(13)
				{
					{ "ssh-rsa", 0 },
					{ "rsa-sha2-256", 1 },
					{ "rsa-sha2-512", 2 },
					{ "ssh-rsa-sha256@ssh.com", 3 },
					{ "ssh-dss", 4 },
					{ "x509v3-rsa2048-sha256", 5 },
					{ "x509v3-sign-rsa", 6 },
					{ "x509v3-sign-dss", 7 },
					{ "x509v3-sign-rsa-sha256@ssh.com", 8 },
					{ "ssh-ed25519", 9 },
					{ "ecdsa-sha2-nistp256", 10 },
					{ "ecdsa-sha2-nistp384", 11 },
					{ "ecdsa-sha2-nistp521", 12 }
				};
			}
			if (awprl.iynko.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					return SshHostKeyAlgorithm.RSA;
				case 4:
					return SshHostKeyAlgorithm.DSS;
				case 5:
				case 6:
				case 7:
				case 8:
					return SshHostKeyAlgorithm.Certificate;
				case 9:
					return SshHostKeyAlgorithm.ED25519;
				case 10:
					return SshHostKeyAlgorithm.ECDsaNistP256;
				case 11:
					return SshHostKeyAlgorithm.ECDsaNistP384;
				case 12:
					return SshHostKeyAlgorithm.ECDsaNistP521;
				}
			}
		}
		return SshHostKeyAlgorithm.None;
	}

	internal static string kvquc(AsymmetricKeyAlgorithmId p0)
	{
		return p0 switch
		{
			AsymmetricKeyAlgorithmId.RSA => "ssh-rsa", 
			AsymmetricKeyAlgorithmId.DSA => "ssh-dss", 
			_ => throw new ArgumentException("Unsupported algorithm.", "algorithmId"), 
		};
	}

	internal static string wusno(Certificate p0)
	{
		return p0.KeyAlgorithm switch
		{
			Rebex.Security.Certificates.KeyAlgorithm.RSA => "x509v3-sign-rsa", 
			Rebex.Security.Certificates.KeyAlgorithm.DSA => "x509v3-sign-dss", 
			_ => throw new InvalidOperationException(brgjd.edcru("Unsupported key algorithm ({0}).", p0.KeyAlgorithm)), 
		};
	}

	public Certificate GetCertificate()
	{
		return pluuw;
	}

	public CertificateChain GetCertificateChain()
	{
		if (pluuw == null || 1 == 0)
		{
			return null;
		}
		if (lwlrh == null || 1 == 0)
		{
			return CertificateChain.BuildFrom(pluuw);
		}
		return lwlrh;
	}

	public DSAParameters GetDSAParameters()
	{
		return wvjny(p0: false);
	}

	public RSAParameters GetRSAParameters()
	{
		return zmisf(p0: false);
	}

	internal virtual RSAParameters zmisf(bool p0)
	{
		if (pluuw != null && 0 == 0)
		{
			return pluuw.GetRSAParameters(p0, silent: true);
		}
		if (rdbcs.Algorithm != AsymmetricKeyAlgorithmId.RSA && 0 == 0)
		{
			throw new CryptographicException("Not an RSA key.");
		}
		if (p0 && 0 == 0)
		{
			throw new CryptographicException("Private key not available.");
		}
		return rdbcs.GetPublicKey().GetRSAParameters();
	}

	internal virtual DSAParameters wvjny(bool p0)
	{
		if (pluuw != null && 0 == 0)
		{
			return pluuw.GetDSAParameters(p0, silent: true);
		}
		if (rdbcs.Algorithm != AsymmetricKeyAlgorithmId.DSA)
		{
			throw new CryptographicException("Not a DSA key.");
		}
		if (p0 && 0 == 0)
		{
			throw new CryptographicException("Private key not available.");
		}
		return rdbcs.GetPublicKey().GetDSAParameters();
	}

	public PublicKeyInfo GetPublicKeyInfo()
	{
		if (pluuw != null && 0 == 0)
		{
			return pluuw.GetPublicKeyInfo();
		}
		return rdbcs.GetPublicKey();
	}

	public void SavePublicKey(string path)
	{
		SavePublicKey(path, SshPublicKeyFormat.Ssh2Base64);
	}

	public void SavePublicKey(string path, SshPublicKeyFormat format)
	{
		if (path == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		if (dttop && 0 == 0)
		{
			throw new InvalidOperationException("This method is only supported for keys, not for certificates.");
		}
		Stream stream = vtdxm.bolpl(path);
		try
		{
			SavePublicKey(stream, format);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public void SavePublicKey(Stream output)
	{
		SavePublicKey(output, SshPublicKeyFormat.Ssh2Base64);
	}

	public void SavePublicKey(Stream output, SshPublicKeyFormat format)
	{
		if (output == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		if (dttop && 0 == 0)
		{
			throw new InvalidOperationException("This method is only supported for keys, not for certificates.");
		}
		string text = Comment;
		if (string.IsNullOrEmpty(text) && 0 == 0)
		{
			text = "Saved by Rebex SSH";
		}
		StringBuilder stringBuilder;
		int num;
		byte[] publicKey;
		string p;
		switch (format)
		{
		case SshPublicKeyFormat.Ssh2Base64:
			publicKey = GetPublicKey();
			stringBuilder = new StringBuilder();
			stringBuilder.dlvlk("{0}\r\n", "---- BEGIN SSH2 PUBLIC KEY ----");
			stringBuilder.dlvlk("{0} \"" + text + "\"\r\n", "Comment:");
			num = 0;
			if (num != 0)
			{
				goto IL_00a9;
			}
			goto IL_00d0;
		case SshPublicKeyFormat.Ssh2Raw:
			publicKey = GetPublicKey();
			output.Write(publicKey, 0, publicKey.Length);
			break;
		case SshPublicKeyFormat.Pkcs8:
		{
			PublicKeyInfo publicKeyInfo = GetPublicKeyInfo();
			publicKeyInfo.Save(output);
			break;
		}
		default:
			{
				throw new ArgumentOutOfRangeException("format", brgjd.edcru("Unsupported key format '{0}'.", format));
			}
			IL_00d0:
			if (num >= publicKey.Length)
			{
				stringBuilder.dlvlk("{0}\r\n", "---- END SSH2 PUBLIC KEY ----");
				publicKey = EncodingTools.ASCII.GetBytes(stringBuilder.ToString());
				output.Write(publicKey, 0, publicKey.Length);
				break;
			}
			goto IL_00a9;
			IL_00a9:
			p = Convert.ToBase64String(publicKey, num, Math.Min(48, publicKey.Length - num));
			stringBuilder.dlvlk("{0}\r\n", p);
			num += 48;
			goto IL_00d0;
		}
	}

	public byte[] GetPublicKey()
	{
		return marpi();
	}

	internal byte[] cgplv(string p0)
	{
		if (!p0.StartsWith("x509v3-rsa2048-", StringComparison.Ordinal) || 1 == 0)
		{
			return GetPublicKey();
		}
		CertificateChain certificateChain = GetCertificateChain();
		if (certificateChain == null || false || certificateChain.LeafCertificate == null)
		{
			throw new CryptographicException("Certificate not available.");
		}
		int count = certificateChain.Count;
		tndeg tndeg = new tndeg(EncodingTools.ASCII);
		mkuxt.excko(tndeg, p0);
		mkuxt.ebmel(tndeg, (uint)count);
		int num = 0;
		if (num != 0)
		{
			goto IL_0072;
		}
		goto IL_008d;
		IL_008d:
		if (num >= count)
		{
			mkuxt.ebmel(tndeg, 0u);
			return tndeg.ToArray();
		}
		goto IL_0072;
		IL_0072:
		byte[] rawCertData = certificateChain[num].GetRawCertData();
		mkuxt.lcbhj(tndeg, rawCertData, p2: false);
		num++;
		goto IL_008d;
	}

	public static SshPublicKey[] LoadPublicKeys(string path)
	{
		List<SshPublicKey> list = new List<SshPublicKey>();
		int num = 0;
		StreamReader streamReader = new StreamReader(vtdxm.prsfm(path), EncodingTools.dmppd);
		try
		{
			while (true)
			{
				string text = streamReader.ReadLine();
				if (text == null || 1 == 0)
				{
					if (list.Count == 0 || 1 == 0)
					{
						throw new CryptographicException("No private key found.");
					}
					return list.ToArray();
				}
				num++;
				text = text.Trim();
				if (text.Length != 0 && 0 == 0)
				{
					int num2 = text.IndexOf(' ');
					if (num2 < 0)
					{
						break;
					}
					int num3 = text.IndexOf(" ", num2 + 5);
					if (num3 < 0)
					{
						num3 = text.Length;
					}
					byte[] data;
					try
					{
						string s = text.Substring(num2 + 1, num3 - num2 - 1).Trim();
						data = Convert.FromBase64String(s);
					}
					catch (FormatException inner)
					{
						throw new CryptographicException("Invalid key encoding at line " + num + ".", inner);
					}
					try
					{
						list.Add(new SshPublicKey(data));
					}
					catch (CryptographicException inner2)
					{
						throw new CryptographicException("Unable to load key at line " + num + ".", inner2);
					}
				}
			}
			throw new CryptographicException("Unsupported key at line at line " + num + ".");
		}
		finally
		{
			if (streamReader != null && 0 == 0)
			{
				((IDisposable)streamReader).Dispose();
			}
		}
	}

	private byte[] lxajk()
	{
		if (pluuw != null && 0 == 0)
		{
			throw new InvalidOperationException("This method is only supported for keys, not for certificates.");
		}
		switch (rdbcs.Algorithm)
		{
		default:
			throw new CryptographicException("Not supported for this key.");
		case AsymmetricKeyAlgorithmId.ECDsa:
		case AsymmetricKeyAlgorithmId.EdDsa:
			return rdbcs.zimkk();
		}
	}

	internal virtual byte[] marpi()
	{
		return ffsjx;
	}

	internal byte[] hsdwk()
	{
		tndeg tndeg = new tndeg(EncodingTools.ASCII);
		mkuxt.excko(tndeg, KeyAlgorithmId);
		string keyAlgorithmId;
		if ((keyAlgorithmId = KeyAlgorithmId) != null && 0 == 0)
		{
			if (awprl.zycpt == null || 1 == 0)
			{
				awprl.zycpt = new Dictionary<string, int>(8)
				{
					{ "ssh-rsa", 0 },
					{ "ssh-dss", 1 },
					{ "ecdsa-sha2-nistp256", 2 },
					{ "ecdsa-sha2-nistp384", 3 },
					{ "ecdsa-sha2-nistp521", 4 },
					{ "ssh-ed25519", 5 },
					{ "x509v3-sign-rsa", 6 },
					{ "x509v3-sign-dss", 7 }
				};
			}
			if (awprl.zycpt.TryGetValue(keyAlgorithmId, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				{
					RSAParameters rSAParameters = zmisf(p0: false);
					mkuxt.mkahh(tndeg, rSAParameters.Exponent);
					mkuxt.mkahh(tndeg, rSAParameters.Modulus);
					goto IL_01b3;
				}
				case 1:
				{
					DSAParameters dSAParameters = wvjny(p0: false);
					mkuxt.mkahh(tndeg, dSAParameters.P);
					mkuxt.mkahh(tndeg, dSAParameters.Q);
					mkuxt.mkahh(tndeg, dSAParameters.G);
					mkuxt.mkahh(tndeg, dSAParameters.Y);
					goto IL_01b3;
				}
				case 2:
				case 3:
				case 4:
				{
					byte[] p2 = lxajk();
					mkuxt.excko(tndeg, KeyAlgorithmId.Substring(11));
					mkuxt.lcbhj(tndeg, p2, p2: false);
					goto IL_01b3;
				}
				case 5:
				{
					byte[] p = lxajk();
					mkuxt.lcbhj(tndeg, p, p2: false);
					goto IL_01b3;
				}
				case 6:
				case 7:
					{
						return pluuw.GetRawCertData();
					}
					IL_01b3:
					return tndeg.ToArray();
				}
			}
		}
		throw new InvalidOperationException("Unknown key algorithm.");
	}

	private static bool yanll(AsymmetricKeyAlgorithm p0, byte[] p1, byte[] p2)
	{
		int num = p2.Length;
		if (num > 40)
		{
			return false;
		}
		if (num == 40)
		{
			return p0.VerifyHash(p1, SignatureHashAlgorithm.SHA1, p2);
		}
		int num2;
		int num3;
		if (num >= 20)
		{
			num2 = 40 - num;
			num3 = 0;
			if (num3 != 0)
			{
				goto IL_002d;
			}
			goto IL_0072;
		}
		goto IL_0076;
		IL_0072:
		if (num3 <= num2)
		{
			goto IL_002d;
		}
		goto IL_0076;
		IL_007d:
		byte[] array = new byte[40];
		int num4;
		Array.Copy(p2, 0, array, 20 - num + num4, num - num4);
		Array.Copy(p2, num - num4, array, 40 - num4, num4);
		if (p0.VerifyHash(p1, SignatureHashAlgorithm.SHA1, array) && 0 == 0)
		{
			return true;
		}
		num4++;
		goto IL_00c9;
		IL_0076:
		num4 = 0;
		if (num4 != 0)
		{
			goto IL_007d;
		}
		goto IL_00c9;
		IL_002d:
		byte[] array2 = new byte[40];
		Array.Copy(p2, 0, array2, num3, 20 - num3);
		Array.Copy(p2, 20 - num3, array2, 60 - num - num3, num - 20 + num3);
		if (p0.VerifyHash(p1, SignatureHashAlgorithm.SHA1, array2) && 0 == 0)
		{
			return true;
		}
		num3++;
		goto IL_0072;
		IL_00c9:
		if (num4 <= num)
		{
			goto IL_007d;
		}
		return false;
	}

	private static string bzkgr(byte[] p0, out AsymmetricKeyAlgorithm p1, out Certificate p2)
	{
		try
		{
			zyppx zyppx = new zyppx(p0, 0, p0.Length, EncodingTools.ASCII);
			string text = zyppx.mdsgo();
			string key;
			if ((key = text) != null && 0 == 0)
			{
				if (awprl.yazwy == null || 1 == 0)
				{
					awprl.yazwy = new Dictionary<string, int>(6)
					{
						{ "ssh-rsa", 0 },
						{ "ssh-dss", 1 },
						{ "ecdsa-sha2-nistp256", 2 },
						{ "ecdsa-sha2-nistp384", 3 },
						{ "ecdsa-sha2-nistp521", 4 },
						{ "ssh-ed25519", 5 }
					};
				}
				if (awprl.yazwy.TryGetValue(key, out var value) && 0 == 0)
				{
					switch (value)
					{
					case 0:
					{
						RSAParameters key4 = new RSAParameters
						{
							Exponent = zyppx.rypuc(p0: true),
							Modulus = zyppx.rypuc(p0: true)
						};
						p1 = new AsymmetricKeyAlgorithm();
						p1.ImportKey(key4);
						p2 = null;
						goto IL_01b2;
					}
					case 1:
					{
						DSAParameters key3 = new DSAParameters
						{
							P = zyppx.rypuc(p0: true),
							Q = zyppx.rypuc(p0: true),
							G = zyppx.rypuc(p0: true),
							Y = zyppx.rypuc(p0: true)
						};
						p1 = new AsymmetricKeyAlgorithm();
						p1.ImportKey(key3);
						p2 = null;
						goto IL_01b2;
					}
					case 2:
					case 3:
					case 4:
					{
						string p3 = bpkgq.mjwcm(text);
						p1 = ikgnw.hyvbq(AsymmetricKeyAlgorithmId.ECDsa, p3, zyppx);
						p2 = null;
						goto IL_01b2;
					}
					case 5:
						{
							byte[] key2 = zyppx.rypuc(p0: false);
							p1 = new AsymmetricKeyAlgorithm();
							p1.ImportKey(AsymmetricKeyAlgorithmId.EdDsa, "1.3.101.112", key2, AsymmetricKeyFormat.RawPublicKey);
							p2 = null;
							goto IL_01b2;
						}
						IL_01b2:
						return text;
					}
				}
			}
			throw new CryptographicException("Key algorithm is not supported.");
		}
		catch (InvalidOperationException inner)
		{
			throw new CryptographicException("Error while decoding key.", inner);
		}
	}

	internal string mqelq(SignatureHashAlgorithm p0)
	{
		string text = KeyAlgorithmId;
		if (text == "ssh-rsa" && 0 == 0)
		{
			switch (p0)
			{
			case SignatureHashAlgorithm.SHA256:
				text += "-sha256@ssh.com";
				break;
			case SignatureHashAlgorithm.SHA384:
				text += "-sha384@ssh.com";
				break;
			case SignatureHashAlgorithm.SHA512:
				text += "-sha512@ssh.com";
				break;
			default:
				throw hifyx.nztrs("algorithm", p0, "Unsupported algorithtm.");
			case SignatureHashAlgorithm.SHA1:
				break;
			}
		}
		return text;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This method has been deprecated and will be removed.", true)]
	[wptwl(false)]
	public bool VerifySignature(byte[] hash, byte[] signature, SignatureHashAlgorithm algorithm)
	{
		if (hash == null || 1 == 0)
		{
			throw new ArgumentNullException("hash");
		}
		if (signature == null || 1 == 0)
		{
			throw new ArgumentNullException("signature");
		}
		string p = mqelq(algorithm);
		return isjpb(hash, signature, p);
	}

	internal bool isjpb(byte[] p0, byte[] p1, string p2)
	{
		zyppx zyppx = new zyppx(p1, 0, p1.Length, EncodingTools.ASCII);
		string text = zyppx.mdsgo();
		p1 = zyppx.tebzf();
		if (p2 != text && 0 == 0)
		{
			if (p2 == "x509v3-rsa2048-sha256" && 0 == 0)
			{
				string text2 = p2.Substring("x509v3-".Length);
				if (text != text2 && 0 == 0)
				{
					return false;
				}
			}
			else if (p2 == "ssh-rsa" && 0 == 0 && text.StartsWith("rsa-sha2-", StringComparison.InvariantCulture) && 0 == 0)
			{
				p2 = text;
			}
		}
		HashingAlgorithmId hashingAlgorithmId = bvjzt(p2);
		if (hashingAlgorithmId == (HashingAlgorithmId)0 || 1 == 0)
		{
			throw hifyx.nztrs("keyAlgorithmId", p2, "Unsupported key algorithtm.");
		}
		SignatureParameters signatureParameters = new SignatureParameters();
		signatureParameters.Format = SignatureFormat.Raw;
		signatureParameters.HashAlgorithm = hashingAlgorithmId;
		string key;
		if ((key = p2) != null && 0 == 0)
		{
			if (awprl.kdzal == null || 1 == 0)
			{
				awprl.kdzal = new Dictionary<string, int>(13)
				{
					{ "ssh-rsa", 0 },
					{ "rsa-sha2-256", 1 },
					{ "rsa-sha2-512", 2 },
					{ "ssh-rsa-sha256@ssh.com", 3 },
					{ "ssh-dss", 4 },
					{ "ecdsa-sha2-nistp256", 5 },
					{ "ecdsa-sha2-nistp384", 6 },
					{ "ecdsa-sha2-nistp521", 7 },
					{ "x509v3-rsa2048-sha256", 8 },
					{ "ssh-ed25519", 9 },
					{ "x509v3-sign-rsa-sha256@ssh.com", 10 },
					{ "x509v3-sign-rsa", 11 },
					{ "x509v3-sign-dss", 12 }
				};
			}
			if (awprl.kdzal.TryGetValue(key, out var value) && 0 == 0)
			{
				switch (value)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					if (rdbcs == null || false || rdbcs.Algorithm != AsymmetricKeyAlgorithmId.RSA)
					{
						return false;
					}
					return rdbcs.VerifyMessage(p0, p1, signatureParameters);
				case 4:
				{
					if (rdbcs == null || false || rdbcs.Algorithm != AsymmetricKeyAlgorithmId.DSA)
					{
						return false;
					}
					byte[] p3 = HashingAlgorithm.ComputeHash(HashingAlgorithmId.SHA1, p0);
					return yanll(rdbcs, p3, p1);
				}
				case 5:
				case 6:
				case 7:
					if (rdbcs == null || false || rdbcs.Algorithm != AsymmetricKeyAlgorithmId.ECDsa)
					{
						return false;
					}
					p1 = ikgnw.xqfdb(p1, rdbcs.lbcti());
					return rdbcs.VerifyMessage(p0, p1, signatureParameters);
				case 8:
					if (pluuw == null || false || ((pluuw.KeyAlgorithm != Rebex.Security.Certificates.KeyAlgorithm.RSA) ? true : false) || pluuw.GetKeySize() < 2048)
					{
						return false;
					}
					return pluuw.VerifyMessage(p0, p1, signatureParameters);
				case 9:
					if (rdbcs == null || false || rdbcs.Algorithm != AsymmetricKeyAlgorithmId.EdDsa)
					{
						return false;
					}
					return rdbcs.VerifyMessage(p0, p1, signatureParameters);
				case 10:
				case 11:
					if (pluuw == null || false || pluuw.KeyAlgorithm != Rebex.Security.Certificates.KeyAlgorithm.RSA)
					{
						return false;
					}
					return pluuw.VerifyMessage(p0, p1, signatureParameters);
				case 12:
					if (pluuw == null || false || pluuw.KeyAlgorithm != Rebex.Security.Certificates.KeyAlgorithm.DSA)
					{
						return false;
					}
					return pluuw.VerifyMessage(p0, p1, signatureParameters);
				}
			}
		}
		return false;
	}

	internal static SshPublicKey gnwxo(byte[] p0, string p1)
	{
		if (!p1.StartsWith("x509v3-rsa2048-", StringComparison.Ordinal) || 1 == 0)
		{
			Certificate certificate = new Certificate(p0);
			if (p1.StartsWith("x509v3-sign-rsa", StringComparison.Ordinal) && 0 == 0)
			{
				if (certificate.KeyAlgorithm != Rebex.Security.Certificates.KeyAlgorithm.RSA && 0 == 0)
				{
					return null;
				}
			}
			else
			{
				if (!(p1 == "x509v3-sign-dss"))
				{
					return null;
				}
				if (certificate.KeyAlgorithm != Rebex.Security.Certificates.KeyAlgorithm.DSA)
				{
					return null;
				}
			}
			return new SshPublicKey(certificate);
		}
		zyppx zyppx = new zyppx(p0, 0, p0.Length, EncodingTools.ASCII);
		zyppx.mdsgo();
		int num = zyppx.rvfya();
		if (num == 0 || 1 == 0)
		{
			throw new SshException(tcpjq.ziezw, "Unexpected decoder error.");
		}
		CertificateChain certificateChain = new CertificateChain();
		while (num > 0)
		{
			byte[] data = zyppx.tebzf();
			Certificate certificate2 = new Certificate(data);
			certificateChain.Add(certificate2);
			num--;
		}
		Certificate leafCertificate = certificateChain.LeafCertificate;
		if (leafCertificate.KeyAlgorithm != nvynm(p1))
		{
			return null;
		}
		if (leafCertificate.KeyAlgorithm == Rebex.Security.Certificates.KeyAlgorithm.ECDsa)
		{
			return null;
		}
		return new SshPublicKey(certificateChain);
	}

	private static KeyAlgorithm nvynm(string p0)
	{
		string text;
		if ((text = p0) != null && 0 == 0)
		{
			if (text == "x509v3-sign-dss")
			{
				return Rebex.Security.Certificates.KeyAlgorithm.DSA;
			}
			if (text == "x509v3-sign-rsa" || text == "x509v3-sign-rsa-sha256@ssh.com" || text == "x509v3-rsa2048-sha256")
			{
				return Rebex.Security.Certificates.KeyAlgorithm.RSA;
			}
		}
		return Rebex.Security.Certificates.KeyAlgorithm.Unsupported;
	}

	public override bool Equals(object obj)
	{
		if (obj == this)
		{
			return true;
		}
		if (!(obj is SshPublicKey key) || 1 == 0)
		{
			return false;
		}
		return Equals(key);
	}

	public bool Equals(SshPublicKey key)
	{
		byte[] publicKey = GetPublicKey();
		byte[] publicKey2 = key.GetPublicKey();
		return jtxhe.hbsgb(publicKey, publicKey2);
	}

	public override int GetHashCode()
	{
		byte[] array = ffsjx;
		if (array == null || false || array.Length < 4)
		{
			return 0;
		}
		return BitConverter.ToInt32(array, 0);
	}
}
