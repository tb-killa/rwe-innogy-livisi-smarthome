using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class PublicKeyInfo : PkcsBase, lnabj
{
	[NonSerialized]
	private AlgorithmIdentifier qlfpx;

	[NonSerialized]
	private AlgorithmIdentifier mquyt;

	[NonSerialized]
	private htykq lvbmj = new htykq();

	[NonSerialized]
	private bool udhgv;

	public AlgorithmIdentifier KeyAlgorithm
	{
		get
		{
			if (qlfpx == null || 1 == 0)
			{
				return null;
			}
			if (mquyt == null || 1 == 0)
			{
				mquyt = qlfpx.evxkk();
			}
			return mquyt;
		}
	}

	public PublicKeyInfo()
	{
	}

	public PublicKeyInfo(RSAParameters parameters)
		: this(new rcqtb(parameters))
	{
	}

	private PublicKeyInfo(rcqtb rsaPublicKey)
	{
		qlfpx = new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.113549.1.1.1"), new mdvaz().ionjf());
		lvbmj = new htykq(fxakl.kncuz(rsaPublicKey), 0);
	}

	public PublicKeyInfo(DSAParameters parameters)
	{
		zjcch p = new zjcch(parameters.P, allowNegative: false);
		zjcch q = new zjcch(parameters.Q, allowNegative: false);
		zjcch g = new zjcch(parameters.G, allowNegative: false);
		zjcch p2 = new zjcch(parameters.Y, allowNegative: false);
		ocawh p3 = new ocawh(p, q, g);
		qlfpx = new AlgorithmIdentifier(new ObjectIdentifier("1.2.840.10040.4.1"), fxakl.kncuz(p3));
		lvbmj = new htykq(fxakl.kncuz(p2), 0);
	}

	internal PublicKeyInfo(bgosr parameters)
	{
		string text = bpkgq.mjwcm(parameters.iztaf);
		if (text == null || 1 == 0)
		{
			throw new CryptographicException("Unsupported curve.");
		}
		byte[] data = lpcge.spbnp(parameters, text);
		qlfpx = AlgorithmIdentifier.xhnfa("1.2.840.10045.2.1", text);
		lvbmj = new htykq(data, 0);
	}

	internal PublicKeyInfo(AlgorithmIdentifier algorithm, byte[] publicKey)
	{
		qlfpx = algorithm;
		lvbmj = new htykq(publicKey, 0);
	}

	internal PublicKeyInfo(AlgorithmIdentifier algorithm, htykq publicKey)
	{
		qlfpx = algorithm;
		lvbmj = publicKey;
	}

	private void zjjgs()
	{
		if (qlfpx == null || false || lvbmj == null)
		{
			throw new CryptographicException("Key not loaded or set yet.");
		}
	}

	public byte[] ToBytes()
	{
		zjjgs();
		return (byte[])lvbmj.lssxa.Clone();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("The GetPublicKey method has been deprecated. Please use ToBytes() method instead.", false)]
	[wptwl(false)]
	public byte[] GetPublicKey()
	{
		return ToBytes();
	}

	public byte[] Encode()
	{
		return fxakl.kncuz(this);
	}

	public DSAParameters GetDSAParameters()
	{
		zjjgs();
		if (qlfpx.Oid.Value != "1.2.840.10040.4.1" && 0 == 0)
		{
			throw new CryptographicException("Not DSA public key.");
		}
		DSAParameters p = qlfpx.cwuvb();
		zjcch zjcch = new zjcch();
		hfnnn.qnzgo(zjcch, lvbmj.lssxa);
		p.Y = zjcch.rtrhq;
		return DSAManaged.busby(p);
	}

	public RSAParameters GetRSAParameters()
	{
		zjjgs();
		if (qlfpx.Oid.Value != "1.2.840.113549.1.1.1" && 0 == 0)
		{
			throw new CryptographicException("Not RSA public key.");
		}
		rcqtb rcqtb = new rcqtb();
		hfnnn.qnzgo(rcqtb, lvbmj.lssxa);
		return rcqtb.hqyyf();
	}

	internal bgosr vmjzm()
	{
		zjjgs();
		switch (qlfpx.edeag())
		{
		default:
			throw new CryptographicException("Not an EC public key.");
		case AsymmetricKeyAlgorithmId.ECDsa:
		case AsymmetricKeyAlgorithmId.ECDH:
		{
			if (qlfpx.Oid.Value == "1.3.101.110" && 0 == 0)
			{
				throw new CryptographicException("Not a classic EC public key.");
			}
			byte[] lssxa = lvbmj.lssxa;
			bgosr result = lpcge.kkyit(lssxa);
			result.iztaf = bpkgq.wmvaf(qlfpx);
			return result;
		}
		}
	}

	public int GetKeySize()
	{
		zjjgs();
		if (qlfpx.Oid.Value == "1.2.840.113549.1.1.1" && 0 == 0)
		{
			return bdjih.foxoi(GetRSAParameters().Modulus).jaioo();
		}
		if (qlfpx.Oid.Value == "1.2.840.10040.4.1" && 0 == 0)
		{
			return bdjih.foxoi(GetDSAParameters().P).jaioo();
		}
		string text = bpkgq.cafoz(qlfpx);
		if (text == null || 1 == 0)
		{
			throw new CryptographicException("Unsupported curve.");
		}
		return bpkgq.kgsco(text);
	}

	public KeyAlgorithm GetKeyAlgorithm()
	{
		zjjgs();
		return qlfpx.qlesd();
	}

	internal string dtrjf()
	{
		zjjgs();
		AsymmetricKeyAlgorithmId asymmetricKeyAlgorithmId = qlfpx.edeag();
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
			byte[] p2 = GetDSAParameters().P;
			int p3 = bdjih.foxoi(p2).jaioo();
			return bpkgq.pguks(asymmetricKeyAlgorithmId, null, p3, p3: true);
		}
		case AsymmetricKeyAlgorithmId.ECDsa:
		case AsymmetricKeyAlgorithmId.ECDH:
		{
			object obj = bpkgq.cafoz(qlfpx);
			if (obj == null || 1 == 0)
			{
				obj = "1.2.840.10045.3.1.7";
			}
			string text = (string)obj;
			if (text == null || 1 == 0)
			{
				throw new CryptographicException("Unsupported curve.");
			}
			return bpkgq.pguks(asymmetricKeyAlgorithmId, text, 0, p3: true);
		}
		case AsymmetricKeyAlgorithmId.EdDsa:
			return "ed25519-sha512";
		default:
			throw new CryptographicException("Unsupported key algorithm.");
		}
	}

	private void sqjhy(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in sqjhy
		this.sqjhy(p0, p1, p2);
	}

	private lnabj ggcki(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			qlfpx = new AlgorithmIdentifier();
			return qlfpx;
		case 1:
			lvbmj = new htykq();
			return lvbmj;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ggcki
		return this.ggcki(p0, p1, p2);
	}

	private void bqbzm(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in bqbzm
		this.bqbzm(p0, p1, p2);
	}

	private void whndx()
	{
		if (qlfpx == null || 1 == 0)
		{
			throw new CryptographicException("Algorithm not found in public key info.");
		}
		if (lvbmj == null || 1 == 0)
		{
			throw new CryptographicException("Public key not found in public key info.");
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in whndx
		this.whndx();
	}

	private void iucpu(fxakl p0)
	{
		zjjgs();
		p0.suudj(qlfpx, lvbmj);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in iucpu
		this.iucpu(p0);
	}

	public void Save(Stream output)
	{
		if (output == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		byte[] array = fxakl.kncuz(this);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.dlvlk("-----{0} PUBLIC KEY-----\r\n", "BEGIN");
		int num = 0;
		if (num != 0)
		{
			goto IL_003b;
		}
		goto IL_0060;
		IL_0060:
		if (num >= array.Length)
		{
			stringBuilder.dlvlk("-----{0} PUBLIC KEY-----\r\n", "END");
			array = EncodingTools.ASCII.GetBytes(stringBuilder.ToString());
			output.Write(array, 0, array.Length);
			return;
		}
		goto IL_003b;
		IL_003b:
		string p = Convert.ToBase64String(array, num, Math.Min(48, array.Length - num));
		stringBuilder.dlvlk("{0}\r\n", p);
		num += 48;
		goto IL_0060;
	}

	public void Save(string fileName)
	{
		if (fileName == null || 1 == 0)
		{
			throw new ArgumentNullException("fileName");
		}
		Stream stream = vtdxm.bolpl(fileName);
		try
		{
			Save(stream);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	public void Load(Stream input)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		if (udhgv && 0 == 0)
		{
			throw new CryptographicException("The public key is read-only.");
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
			throw new CryptographicException("Not a public key.");
		}
		PrivateKeyInfo.hhmob p;
		KeyAlgorithm p2;
		string p3;
		if (array[0] != 48)
		{
			array = PrivateKeyInfo.kvyav(array, num, out p, out p2, out p3);
			num = array.Length;
		}
		else if (!cjvew.mqmde(array, num, out p, out p2, out p3) || 1 == 0)
		{
			throw new CryptographicException("Unknown public key format.");
		}
		PublicKeyInfo publicKeyInfo;
		switch (p)
		{
		case PrivateKeyInfo.hhmob.uemtk:
			publicKeyInfo = new PublicKeyInfo();
			hfnnn.oalpn(publicKeyInfo, array, 0, num);
			break;
		case PrivateKeyInfo.hhmob.knpoz:
			if (p2 == Rebex.Security.Certificates.KeyAlgorithm.RSA)
			{
				rcqtb rcqtb = new rcqtb();
				hfnnn.oalpn(rcqtb, array, 0, num);
				publicKeyInfo = new PublicKeyInfo(rcqtb);
				break;
			}
			throw new CryptographicException("Unsupported key format / algorithm combination.");
		default:
			throw new CryptographicException("Not a public key.");
		}
		if (publicKeyInfo.qlfpx.Oid.Value == "1.3.6.1.4.1.11591.15.1" && 0 == 0)
		{
			qlfpx = new AlgorithmIdentifier("1.3.101.112", null);
		}
		else
		{
			qlfpx = publicKeyInfo.qlfpx;
		}
		mquyt = null;
		lvbmj = publicKeyInfo.lvbmj;
	}

	public void Load(string fileName)
	{
		if (fileName == null || 1 == 0)
		{
			throw new ArgumentNullException("fileName");
		}
		Stream stream = vtdxm.prsfm(fileName);
		try
		{
			Load(stream);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
	}

	internal void wymnt()
	{
		udhgv = true;
	}
}
