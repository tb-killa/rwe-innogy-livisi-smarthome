using System.Collections.Generic;
using System.Text;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;
using onrkn;

namespace Rebex.Net;

public class SshCipher
{
	private SshMacAlgorithm zcsvv;

	private string gbwwz;

	private SshMacAlgorithm jfsvu;

	private string bqkqq;

	private SshEncryptionAlgorithm eldjl;

	private SshEncryptionMode mwexi;

	private string usurd;

	private SshEncryptionAlgorithm hltji;

	private SshEncryptionMode jfcnz;

	private string gpbov;

	private bool rprjw;

	private bool obgbr;

	private SshKeyExchangeAlgorithm jqeuw;

	private string nctoj;

	private SshHostKeyAlgorithm utamw;

	private string huxfc;

	private HashingAlgorithmId klawg;

	internal static readonly SshCipher vqarl = new SshCipher();

	public SshMacAlgorithm OutgoingMacAlgorithm => zcsvv;

	public string OutgoingMacCipherId => gbwwz;

	public SshEncryptionAlgorithm OutgoingEncryptionAlgorithm => eldjl;

	public SshEncryptionMode OutgoingEncryptionMode => mwexi;

	public string OutgoingEncryptionCipherId => usurd;

	public SshMacAlgorithm IncomingMacAlgorithm => jfsvu;

	public string IncomingMacCipherId => bqkqq;

	public SshEncryptionAlgorithm IncomingEncryptionAlgorithm => hltji;

	public SshEncryptionMode IncomingEncryptionMode => jfcnz;

	public string IncomingEncryptionCipherId => gpbov;

	public SshKeyExchangeAlgorithm KeyExchangeAlgorithm => jqeuw;

	public string KeyExchangeCipherId => nctoj;

	public SshHostKeyAlgorithm HostKeyAlgorithm => utamw;

	public string HostKeyCipherId => huxfc;

	public SignatureHashAlgorithm SignatureHashAlgorithm => bpkgq.vfyof(klawg);

	public bool OutgoingCompressionEnabled => rprjw;

	public bool IncomingCompressionEnabled => obgbr;

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("SSH 2.0, ");
		stringBuilder.Append(nctoj);
		stringBuilder.Append(", ");
		stringBuilder.Append(huxfc);
		stringBuilder.Append(", ");
		stringBuilder.Append(usurd);
		stringBuilder.Append("/");
		stringBuilder.Append(gpbov);
		if (zcsvv != SshMacAlgorithm.None || jfsvu != SshMacAlgorithm.None)
		{
			stringBuilder.Append(", ");
			string text = gbwwz;
			if (text == null || 1 == 0)
			{
				text = usurd;
			}
			stringBuilder.Append(text);
			stringBuilder.Append("/");
			string text2 = bqkqq;
			if (text2 == null || 1 == 0)
			{
				text2 = gpbov;
			}
			stringBuilder.Append(text2);
		}
		if ((rprjw ? true : false) || obgbr)
		{
			stringBuilder.Append(", ");
			stringBuilder.Append((rprjw ? true : false) ? "zlib" : "none");
			stringBuilder.Append("/");
			stringBuilder.Append((obgbr ? true : false) ? "zlib" : "none");
		}
		return stringBuilder.ToString();
	}

	internal static SshKeyExchangeAlgorithm xqnae(string p0, out HashingAlgorithmId p1, out int p2, out int p3, out string p4)
	{
		p2 = 0;
		p3 = 0;
		p4 = null;
		string key;
		SshKeyExchangeAlgorithm result;
		if ((key = p0) != null && 0 == 0)
		{
			if (awprl.xnbze == null || 1 == 0)
			{
				awprl.xnbze = new Dictionary<string, int>(14)
				{
					{ "diffie-hellman-group1-sha1", 0 },
					{ "diffie-hellman-group14-sha1", 1 },
					{ "diffie-hellman-group14-sha256", 2 },
					{ "diffie-hellman-group15-sha512", 3 },
					{ "diffie-hellman-group16-sha512", 4 },
					{ "diffie-hellman-group17-sha512", 5 },
					{ "diffie-hellman-group18-sha512", 6 },
					{ "diffie-hellman-group-exchange-sha1", 7 },
					{ "diffie-hellman-group-exchange-sha256", 8 },
					{ "ecdh-sha2-nistp256", 9 },
					{ "ecdh-sha2-nistp384", 10 },
					{ "ecdh-sha2-nistp521", 11 },
					{ "curve25519-sha256", 12 },
					{ "curve25519-sha256@libssh.org", 13 }
				};
			}
			if (awprl.xnbze.TryGetValue(key, out var value))
			{
				switch (value)
				{
				case 0:
					break;
				case 1:
					goto IL_0159;
				case 2:
					goto IL_0167;
				case 3:
					goto IL_0179;
				case 4:
					goto IL_018b;
				case 5:
					goto IL_019d;
				case 6:
					goto IL_01af;
				case 7:
					goto IL_01c1;
				case 8:
					goto IL_01cb;
				case 9:
					goto IL_01d5;
				case 10:
					goto IL_01ef;
				case 11:
					goto IL_0209;
				case 12:
				case 13:
					goto IL_0223;
				default:
					goto IL_0240;
				}
				result = SshKeyExchangeAlgorithm.DiffieHellmanGroup1SHA1;
				p1 = HashingAlgorithmId.SHA1;
				p2 = 2;
				goto IL_024d;
			}
		}
		goto IL_0240;
		IL_01af:
		result = SshKeyExchangeAlgorithm.DiffieHellmanOakleyGroupSHA512;
		p1 = HashingAlgorithmId.SHA512;
		p2 = 18;
		goto IL_024d;
		IL_01c1:
		result = SshKeyExchangeAlgorithm.DiffieHellmanGroupExchangeSHA1;
		p1 = HashingAlgorithmId.SHA1;
		goto IL_024d;
		IL_01ef:
		result = SshKeyExchangeAlgorithm.ECDiffieHellmanNistP384;
		p1 = HashingAlgorithmId.SHA384;
		p4 = "1.3.132.0.34";
		p3 = 384;
		p2 = -1;
		goto IL_024d;
		IL_01d5:
		result = SshKeyExchangeAlgorithm.ECDiffieHellmanNistP256;
		p1 = HashingAlgorithmId.SHA256;
		p4 = "1.2.840.10045.3.1.7";
		p3 = 256;
		p2 = -1;
		goto IL_024d;
		IL_01cb:
		result = SshKeyExchangeAlgorithm.DiffieHellmanGroupExchangeSHA256;
		p1 = HashingAlgorithmId.SHA256;
		goto IL_024d;
		IL_0209:
		result = SshKeyExchangeAlgorithm.ECDiffieHellmanNistP521;
		p1 = HashingAlgorithmId.SHA512;
		p4 = "1.3.132.0.35";
		p3 = 521;
		p2 = -1;
		goto IL_024d;
		IL_0223:
		result = SshKeyExchangeAlgorithm.Curve25519;
		p1 = HashingAlgorithmId.SHA256;
		p4 = "1.3.101.110";
		p3 = 256;
		p2 = -1;
		goto IL_024d;
		IL_0240:
		result = SshKeyExchangeAlgorithm.None;
		p1 = (HashingAlgorithmId)0;
		goto IL_024d;
		IL_024d:
		return result;
		IL_0159:
		result = SshKeyExchangeAlgorithm.DiffieHellmanGroup14SHA1;
		p1 = HashingAlgorithmId.SHA1;
		p2 = 14;
		goto IL_024d;
		IL_0167:
		result = SshKeyExchangeAlgorithm.DiffieHellmanOakleyGroupSHA256;
		p1 = HashingAlgorithmId.SHA256;
		p2 = 14;
		goto IL_024d;
		IL_0179:
		result = SshKeyExchangeAlgorithm.DiffieHellmanOakleyGroupSHA512;
		p1 = HashingAlgorithmId.SHA512;
		p2 = 15;
		goto IL_024d;
		IL_018b:
		result = SshKeyExchangeAlgorithm.DiffieHellmanOakleyGroupSHA512;
		p1 = HashingAlgorithmId.SHA512;
		p2 = 16;
		goto IL_024d;
		IL_019d:
		result = SshKeyExchangeAlgorithm.DiffieHellmanOakleyGroupSHA512;
		p1 = HashingAlgorithmId.SHA512;
		p2 = 17;
		goto IL_024d;
	}

	internal static SshHostKeyAlgorithm uxprc(string p0)
	{
		string key;
		if ((key = p0) != null && 0 == 0)
		{
			if (awprl.gekxx == null || 1 == 0)
			{
				awprl.gekxx = new Dictionary<string, int>(13)
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
			if (awprl.gekxx.TryGetValue(key, out var value) && 0 == 0)
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

	private SshCipher()
	{
	}

	internal SshCipher(eswpb sendMac, eswpb recvMac, ovpxz sendCipher, ovpxz recvCipher, SshKeyExchangeAlgorithm kex, string kexAlg, SshHostKeyAlgorithm hostKey, string hostKeyAlg, bool sendZlib, bool recvZlib)
	{
		if (sendMac != null && 0 == 0)
		{
			zcsvv = sendMac.hcwod;
			gbwwz = sendMac.hluhi;
		}
		else
		{
			zcsvv = SshMacAlgorithm.None;
		}
		eldjl = sendCipher.ohfaq;
		mwexi = sendCipher.ujdcx();
		usurd = sendCipher.hzzec;
		if (recvMac != null && 0 == 0)
		{
			jfsvu = recvMac.hcwod;
			bqkqq = recvMac.hluhi;
		}
		else
		{
			jfsvu = SshMacAlgorithm.None;
		}
		hltji = recvCipher.ohfaq;
		jfcnz = recvCipher.ujdcx();
		gpbov = recvCipher.hzzec;
		jqeuw = kex;
		nctoj = kexAlg;
		utamw = hostKey;
		huxfc = hostKeyAlg;
		klawg = SshPublicKey.bvjzt(hostKeyAlg);
		rprjw = sendZlib;
		obgbr = recvZlib;
	}
}
