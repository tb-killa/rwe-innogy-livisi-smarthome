using System;
using System.IO;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography.Pkcs;
using onrkn;

namespace Rebex.Security.Cryptography;

public class AsymmetricKeyAlgorithm : IDisposable
{
	private eatps gyfbn;

	private bool gtwdn;

	private bool tciys;

	private bool rgzeo;

	private heegp hevuu;

	private xtsej mgigk;

	private static Func<string, Exception> hsnfq;

	public int KeySize
	{
		get
		{
			eatps eatps = gyfbn;
			if (eatps == null || 1 == 0)
			{
				return 0;
			}
			return eatps.KeySize;
		}
	}

	public AsymmetricKeyAlgorithmId Algorithm
	{
		get
		{
			eatps eatps = gyfbn;
			if (eatps == null || 1 == 0)
			{
				return (AsymmetricKeyAlgorithmId)(-1);
			}
			return eatps.bptsq;
		}
	}

	internal string dxkah
	{
		get
		{
			eatps eatps = gyfbn;
			if (eatps == null || 1 == 0)
			{
				return null;
			}
			return eatps.janem;
		}
	}

	public bool PublicOnly => rgzeo;

	internal xtsej ipaig
	{
		get
		{
			return mgigk;
		}
		set
		{
			mgigk = value;
		}
	}

	public static void Register(Func<string, object> algFactory)
	{
		rmnyn.nkkuu(algFactory);
	}

	internal int lbcti()
	{
		return (KeySize + 7) / 8;
	}

	public CspParameters GetCspParameters()
	{
		if (gyfbn is lncnv lncnv && 0 == 0)
		{
			return lncnv.iqqfj();
		}
		return null;
	}

	public AsymmetricKeyAlgorithm()
	{
	}

	internal AsymmetricKeyAlgorithm(eatps asymmetric, bool publicOnly, bool ownsAlgorithm)
	{
		gyfbn = asymmetric;
		gtwdn = true;
		rgzeo = publicOnly;
	}

	[rbjhl("windows")]
	internal void zceob(bool p0)
	{
		if (hevuu != null && 0 == 0)
		{
			hevuu.rikva(p0);
		}
		else if (p0 ? true : false)
		{
			CspParameters cspParameters = GetCspParameters();
			if (cspParameters != null && 0 == 0)
			{
				hevuu = new heegp(cspParameters);
			}
		}
	}

	private void xqzei()
	{
		if (gyfbn != null && 0 == 0 && gtwdn && 0 == 0 && gyfbn is IDisposable disposable && 0 == 0)
		{
			disposable.Dispose();
		}
		gtwdn = false;
		gyfbn = null;
		if (hevuu != null && 0 == 0)
		{
			hevuu.Dispose();
			hevuu = null;
		}
	}

	public void Dispose()
	{
		if (!tciys || 1 == 0)
		{
			xqzei();
			tciys = true;
		}
	}

	private void pggev(bool p0)
	{
		if (tciys && 0 == 0)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		if (p0 && 0 == 0 && (gyfbn == null || 1 == 0))
		{
			throw new CryptographicException("No key has been generated yet.");
		}
	}

	public static AsymmetricKeyAlgorithm CreateFrom(AsymmetricAlgorithm algorithm, bool ownsAlgorithm)
	{
		if (algorithm == null || 1 == 0)
		{
			throw new ArgumentNullException("algorithm");
		}
		eatps eatps = algorithm as eatps;
		if (eatps == null || 1 == 0)
		{
			eatps = fxnlx.ghyhn(algorithm, ownsAlgorithm);
		}
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = new AsymmetricKeyAlgorithm();
		asymmetricKeyAlgorithm.gyfbn = eatps;
		asymmetricKeyAlgorithm.gtwdn = ownsAlgorithm;
		asymmetricKeyAlgorithm.rgzeo = CryptoHelper.srzqu(algorithm);
		return asymmetricKeyAlgorithm;
	}

	private imfrk kjixx(string p0)
	{
		try
		{
			return rmnyn.vuzke(p0, ipaig);
		}
		catch (NotSupportedException inner)
		{
			throw new CryptographicException("Unsupported algorithm or key size (" + p0 + ").", inner);
		}
	}

	private imfrk uqbtb(string p0)
	{
		imfrk imfrk = null;
		if (ipaig == xtsej.dipgs || 1 == 0)
		{
			imfrk = smhps.rzowl(p0);
		}
		imfrk imfrk2 = imfrk;
		if (imfrk2 == null || 1 == 0)
		{
			imfrk2 = kjixx(p0);
		}
		return imfrk2;
	}

	internal AlgorithmIdentifier xdrvo(SignatureHashAlgorithm? p0)
	{
		pggev(p0: true);
		string janem = gyfbn.janem;
		return bpkgq.zvown(janem, p0);
	}

	public PublicKeyInfo GetPublicKey()
	{
		pggev(p0: true);
		if (gyfbn is lncnv lncnv && 0 == 0)
		{
			return lncnv.kptoi();
		}
		throw new CryptographicException("Not supported for this key algorithm.");
	}

	internal byte[] zimkk()
	{
		pggev(p0: true);
		if (gyfbn is ibhso ibhso && 0 == 0)
		{
			return ibhso.craet();
		}
		return GetPublicKey().ToBytes();
	}

	public PrivateKeyInfo GetPrivateKey()
	{
		pggev(p0: true);
		if (rgzeo && 0 == 0)
		{
			throw new CryptographicException("Private key is not available.");
		}
		if (gyfbn is lncnv lncnv && 0 == 0)
		{
			return lncnv.jbbgs(p0: true);
		}
		throw new CryptographicException("Not supported for this key algorithm.");
	}

	public void ImportKey(PublicKeyInfo key)
	{
		pggev(p0: false);
		xqzei();
		string p = key.dtrjf();
		imfrk imfrk = uqbtb(p);
		gyfbn = imfrk.neqkn(key);
		gtwdn = true;
		rgzeo = true;
	}

	public void ImportKey(PrivateKeyInfo key)
	{
		pggev(p0: false);
		xqzei();
		string p = key.bdgxx();
		imfrk imfrk = uqbtb(p);
		gyfbn = imfrk.xaunu(key);
		gtwdn = true;
		rgzeo = false;
	}

	public void ImportKey(RSAParameters key)
	{
		if (CryptoHelper.viqaa(key) && 0 == 0)
		{
			PrivateKeyInfo key2 = new PrivateKeyInfo(key);
			ImportKey(key2);
		}
		else
		{
			PublicKeyInfo key3 = new PublicKeyInfo(key);
			ImportKey(key3);
		}
	}

	public void ImportKey(DSAParameters key)
	{
		if (CryptoHelper.xkcuh(key) && 0 == 0)
		{
			PrivateKeyInfo key2 = new PrivateKeyInfo(key);
			ImportKey(key2);
		}
		else
		{
			PublicKeyInfo key3 = new PublicKeyInfo(key);
			ImportKey(key3);
		}
	}

	public void ImportKey(DiffieHellmanParameters key)
	{
		pggev(p0: false);
		xqzei();
		if (!CryptoHelper.fsfdo(key) || 1 == 0)
		{
			key.Y = null;
		}
		gyfbn = CryptoHelper.qgafy(key, ipaig);
		gtwdn = true;
		rgzeo = false;
	}

	public void ImportKey(AsymmetricKeyAlgorithmId algorithm, string curve, byte[] key, AsymmetricKeyFormat format)
	{
		pggev(p0: false);
		xqzei();
		switch (algorithm)
		{
		case AsymmetricKeyAlgorithmId.RSA:
		case AsymmetricKeyAlgorithmId.DSA:
		{
			if (format != AsymmetricKeyFormat.RawPrivateKey)
			{
				throw new CryptographicException("Format not supported for this algorithm.");
			}
			if (curve != null && 0 == 0)
			{
				throw new CryptographicException("Curve not supported for this algorithm.");
			}
			PrivateKeyInfo privateKeyInfo = new PrivateKeyInfo();
			privateKeyInfo.Load(new MemoryStream(key, writable: false), null);
			ImportKey(privateKeyInfo);
			return;
		}
		}
		switch (format)
		{
		case AsymmetricKeyFormat.ECPrivateKey:
		{
			if (algorithm != AsymmetricKeyAlgorithmId.ECDsa && algorithm != AsymmetricKeyAlgorithmId.ECDH)
			{
				throw new CryptographicException("Format not supported for this algorithm.");
			}
			tsnbe privateKey = tsnbe.mpano(key, curve);
			PrivateKeyInfo privateKeyInfo = new PrivateKeyInfo(privateKey, algorithm);
			ImportKey(privateKeyInfo);
			break;
		}
		case AsymmetricKeyFormat.RawPrivateKey:
		{
			if (algorithm != AsymmetricKeyAlgorithmId.EdDsa)
			{
				throw new CryptographicException("Format not supported for this algorithm.");
			}
			PrivateKeyInfo privateKeyInfo = PrivateKeyInfo.aypnd(key);
			ImportKey(privateKeyInfo);
			break;
		}
		case AsymmetricKeyFormat.RawPublicKey:
		{
			string p = bpkgq.pguks(algorithm, curve, 0, p3: true);
			PublicKeyInfo key2 = new PublicKeyInfo(bpkgq.aykug(p), key);
			ImportKey(key2);
			break;
		}
		default:
			throw hifyx.nztrs("format", format, "Unknown key format.");
		}
	}

	public static bool IsSupported(AsymmetricKeyAlgorithmId algorithm, string curve, int keySize)
	{
		return iexxf(algorithm, curve, keySize) == zxjln.iuckt;
	}

	internal static bool fvglf(AsymmetricKeyAlgorithmId p0, string p1, int p2, bool p3)
	{
		return iexxf(p0, p1, p2) switch
		{
			zxjln.iuckt => true, 
			zxjln.dwzpe => p3, 
			_ => false, 
		};
	}

	internal static zxjln iexxf(AsymmetricKeyAlgorithmId p0, string p1, int p2)
	{
		string text = bpkgq.pguks(p0, p1, p2, p3: false);
		string text2;
		if ((text2 = text) != null)
		{
			if (text2 == "dh" && 0 == 0)
			{
				if (p2 == 0 || 1 == 0)
				{
					return zxjln.iuckt;
				}
				if (p2 >= 512 && p2 <= CryptoHelper.jtnnl())
				{
					return zxjln.iuckt;
				}
				return zxjln.mcbds;
			}
			return rmnyn.zbhkd(text);
		}
		return zxjln.mcbds;
	}

	internal static bool ckgxm()
	{
		return rmnyn.zbhkd("ecdh-sha2-nistp256") == zxjln.iuckt;
	}

	public static DiffieHellmanParameters GenerateDiffieHellmanParameters(int keySize)
	{
		return lcdzw(keySize, xtsej.dipgs);
	}

	internal static DiffieHellmanParameters lcdzw(int p0, xtsej p1)
	{
		DiffieHellman diffieHellman = ((p1 == xtsej.ocahi || !DiffieHellmanCryptoServiceProvider.fqzsi(p0)) ? rmnyn.hnofn() : new DiffieHellmanCryptoServiceProvider(p0));
		try
		{
			diffieHellman.ekwvc(p0);
			return diffieHellman.ExportParameters(includePrivateParameters: false);
		}
		finally
		{
			diffieHellman.Clear();
		}
	}

	internal static DiffieHellmanParameters zknnz(int p0)
	{
		return fuipf.paitk(p0);
	}

	public void GenerateKey(AsymmetricKeyAlgorithmId algorithm, int keySize)
	{
		kvrol(algorithm, null, keySize);
	}

	public void GenerateKey(AsymmetricKeyAlgorithmId algorithm, string curve)
	{
		kvrol(algorithm, curve, 0);
	}

	internal void kvrol(AsymmetricKeyAlgorithmId p0, string p1, int p2)
	{
		pggev(p0: false);
		xqzei();
		switch (p0)
		{
		case AsymmetricKeyAlgorithmId.RSA:
			p2 = (((p2 != 0) ? true : false) ? p2 : 2048);
			break;
		case AsymmetricKeyAlgorithmId.DSA:
			p2 = (((p2 != 0) ? true : false) ? p2 : 1024);
			break;
		case AsymmetricKeyAlgorithmId.ECDsa:
		case AsymmetricKeyAlgorithmId.ECDH:
		{
			object obj2 = p1;
			if (obj2 == null || 1 == 0)
			{
				obj2 = "1.2.840.10045.3.1.7";
			}
			p1 = (string)obj2;
			break;
		}
		case AsymmetricKeyAlgorithmId.EdDsa:
		{
			object obj = p1;
			if (obj == null || 1 == 0)
			{
				obj = "1.3.101.112";
			}
			p1 = (string)obj;
			break;
		}
		case AsymmetricKeyAlgorithmId.DiffieHellman:
			throw new CryptographicException("Key and parameters generation is not supported for this algorithm yet.");
		}
		string p3 = bpkgq.pguks(p0, p1, p2, p3: true);
		imfrk imfrk = kjixx(p3);
		try
		{
			gyfbn = imfrk.poerm();
		}
		catch (NotSupportedException)
		{
			throw new CryptographicException("Key generation is not supported for this algorithm or key size.");
		}
		gtwdn = true;
		rgzeo = false;
	}

	public byte[] SignHash(byte[] hash, SignatureHashAlgorithm hashAlgorithm)
	{
		if (hash == null || 1 == 0)
		{
			throw new ArgumentNullException("hash");
		}
		pggev(p0: true);
		if (gyfbn is dzjkq dzjkq && 0 == 0)
		{
			mrxvh p = mrxvh.vtcca(hashAlgorithm, gyfbn.bptsq);
			byte[] p2 = dzjkq.rypyi(hash, p);
			return zhjuy(p2);
		}
		throw new CryptographicException("Hash signing is not supported for this algorithm.");
	}

	internal byte[] sizlw(byte[] p0, SignatureParameters p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("hash");
		}
		pggev(p0: true);
		SignatureParameters.ffnfz(p1, gyfbn.bptsq, gyfbn.KeySize, out var p2, out var _, out var p4);
		if (gyfbn is dzjkq dzjkq && 0 == 0)
		{
			byte[] p5 = dzjkq.rypyi(p0, p4);
			p5 = zhjuy(p5);
			return nailt.zeuix(gyfbn, p5, p2);
		}
		throw new CryptographicException("Hash signing is not supported for this algorithm.");
	}

	public byte[] SignMessage(byte[] message)
	{
		return SignMessage(message, null);
	}

	public byte[] SignMessage(byte[] message, SignatureParameters parameters)
	{
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		pggev(p0: true);
		SignatureParameters.ffnfz(parameters, gyfbn.bptsq, gyfbn.KeySize, out var p, out var p2, out var p3);
		if (p3.hqtwc == goies.gbwxv && gyfbn is ijjlm ijjlm && 0 == 0)
		{
			byte[] p4 = ijjlm.vxuyd(message, p2);
			return nailt.zeuix(gyfbn, p4, p);
		}
		if (gyfbn is dzjkq dzjkq && 0 == 0)
		{
			byte[] p5 = HashingAlgorithm.ComputeHash(p2, message);
			byte[] p6 = dzjkq.rypyi(p5, p3);
			p6 = zhjuy(p6);
			return nailt.zeuix(gyfbn, p6, p);
		}
		throw new CryptographicException("Message signing is not supported for this algorithm.");
	}

	public bool VerifyHash(byte[] hash, SignatureHashAlgorithm hashAlgorithm, byte[] signature)
	{
		if (hash == null || 1 == 0)
		{
			throw new ArgumentNullException("hash");
		}
		if (signature == null || 1 == 0)
		{
			throw new ArgumentNullException("signature");
		}
		pggev(p0: true);
		if (gyfbn is dzjkq dzjkq && 0 == 0)
		{
			mrxvh p = mrxvh.vtcca(hashAlgorithm, gyfbn.bptsq);
			signature = zhjuy(signature);
			return dzjkq.cbzmp(hash, signature, p);
		}
		throw new CryptographicException("Hash signature verification is not supported for this algorithm.");
	}

	private byte[] zhjuy(byte[] p0)
	{
		if (Algorithm == AsymmetricKeyAlgorithmId.RSA || 1 == 0)
		{
			int p1 = lbcti();
			p0 = jlfbq.tykuz(p0, p1);
		}
		return p0;
	}

	internal bool hefzy(byte[] p0, byte[] p1, SignatureParameters p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("hash");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("signature");
		}
		pggev(p0: true);
		SignatureParameters.ffnfz(p2, gyfbn.bptsq, gyfbn.KeySize, out var p3, out var _, out var p5);
		if (gyfbn is dzjkq dzjkq && 0 == 0)
		{
			p1 = nailt.izcrh(gyfbn, p1, p3);
			p1 = zhjuy(p1);
			return dzjkq.cbzmp(p0, p1, p5);
		}
		throw new CryptographicException("Message signature verification is not supported for this algorithm.");
	}

	public bool VerifyMessage(byte[] message, byte[] signature)
	{
		return VerifyMessage(message, signature, null);
	}

	public bool VerifyMessage(byte[] message, byte[] signature, SignatureParameters parameters)
	{
		if (message == null || 1 == 0)
		{
			throw new ArgumentNullException("message");
		}
		if (signature == null || 1 == 0)
		{
			throw new ArgumentNullException("signature");
		}
		pggev(p0: true);
		SignatureParameters.ffnfz(parameters, gyfbn.bptsq, gyfbn.KeySize, out var p, out var p2, out var p3);
		if (p3.hqtwc == goies.gbwxv && gyfbn is ijjlm ijjlm && 0 == 0)
		{
			signature = nailt.izcrh(gyfbn, signature, p);
			return ijjlm.swsbt(message, p2, signature);
		}
		if (gyfbn is dzjkq dzjkq && 0 == 0)
		{
			signature = nailt.izcrh(gyfbn, signature, p);
			byte[] p4 = HashingAlgorithm.ComputeHash(p2, message);
			signature = zhjuy(signature);
			return dzjkq.cbzmp(p4, signature, p3);
		}
		throw new CryptographicException("Message signature verification is not supported for this algorithm.");
	}

	public byte[] Encrypt(byte[] rgb)
	{
		return Encrypt(rgb, null);
	}

	public byte[] Encrypt(byte[] rgb, EncryptionParameters parameters)
	{
		if (rgb == null || 1 == 0)
		{
			throw new ArgumentNullException("rgb");
		}
		EncryptionParameters.lkmtd(parameters, gyfbn.bptsq, gyfbn.KeySize, rgb.Length, out var p);
		pggev(p0: true);
		if (gyfbn is ntowq ntowq && 0 == 0)
		{
			return ntowq.sfbms(rgb, p);
		}
		if (gyfbn is ibhso ibhso && 0 == 0 && (rgb.Length == 0 || 1 == 0))
		{
			return ibhso.craet();
		}
		throw new CryptographicException("Encryption is not supported for this algorithm.");
	}

	public byte[] Decrypt(byte[] rgb)
	{
		return Decrypt(rgb, null);
	}

	public byte[] Decrypt(byte[] rgb, EncryptionParameters parameters)
	{
		if (rgb == null || 1 == 0)
		{
			throw new ArgumentNullException("rgb");
		}
		EncryptionParameters.lkmtd(parameters, gyfbn.bptsq, gyfbn.KeySize, null, out var p);
		pggev(p0: true);
		if (gyfbn is ntowq ntowq && 0 == 0)
		{
			return ntowq.lhhds(rgb, p);
		}
		if (gyfbn is hibhk hibhk && 0 == 0)
		{
			byte[] array = hibhk.ovrid(rgb);
			if (array != null && 0 == 0)
			{
				return array;
			}
		}
		throw new CryptographicException("Decryption is not supported for this algorithm.");
	}

	internal byte[] qpwqy(byte[] p0)
	{
		pggev(p0: true);
		byte[] array = null;
		if (gyfbn is hibhk hibhk && 0 == 0)
		{
			array = hibhk.ovrid(p0);
		}
		if (array != null && 0 == 0 && Algorithm == AsymmetricKeyAlgorithmId.ECDH)
		{
			int num = lbcti();
			byte[] p1 = array;
			if (hsnfq == null || 1 == 0)
			{
				hsnfq = fwdtd;
			}
			array = jlfbq.elzlr(p1, num, num, hsnfq);
		}
		return array;
	}

	internal byte[] fevai(byte[] p0)
	{
		byte[] array = qpwqy(p0);
		if (array == null || 1 == 0)
		{
			throw new CryptographicException("Shared key agreement is not supported for this algorithm.");
		}
		return array;
	}

	public KeyMaterialDeriver GetKeyMaterialDeriver(byte[] otherPublicKey)
	{
		pggev(p0: true);
		if (gyfbn is uumvy uumvy && 0 == 0)
		{
			return uumvy.mflnq(otherPublicKey);
		}
		byte[] array = qpwqy(otherPublicKey);
		if (array == null || 1 == 0)
		{
			throw new CryptographicException("Key material derivation is not supported for this algorithm.");
		}
		HashingAlgorithmId defaultHashAlgorithm = bpkgq.qivxr(dxkah);
		return new vbhtv(array, defaultHashAlgorithm);
	}

	private static Exception fwdtd(string p0)
	{
		return new CryptographicException(p0);
	}
}
